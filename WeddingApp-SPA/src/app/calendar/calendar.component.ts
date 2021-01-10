import { Component, OnInit } from '@angular/core';
import * as moment from 'moment-timezone';
import { findOneIana } from 'windows-iana';
import * as MicrosoftGraph from '@microsoft/microsoft-graph-types';

import { AuthMicrosoftService } from '../_services/auth-microsoft.service';
import { AlertifyService } from '../_services/alertify.service';
import { GraphService } from '../_services/graph.service';
import { NewEvent } from '../_models/new-event';
import { Reservation } from '../_models/reservation';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-calendar',
  templateUrl: './calendar.component.html',
  styleUrls: ['./calendar.component.css']
})
export class CalendarComponent implements OnInit {

  public events: MicrosoftGraph.Event[];
  model = new NewEvent();
  reservations: Reservation[];

  constructor(
    private authService: AuthMicrosoftService,
    private graphService: GraphService,
    private alertsService: AlertifyService,
    private route: ActivatedRoute) { }

    ngOnInit() {
      // Convert the user's timezone to IANA format
      const ianaName = findOneIana(this.authService.user.timeZone);
      const timeZone = "Europe/Warsaw";
      this.route.data.subscribe(data => {
        this.reservations = data['reservations'];
        console.log('this.reservations', this.reservations);
      });
      
      
    
      // Get midnight on the start of the current week in the user's timezone,
      // but in UTC. For example, for Pacific Standard Time, the time value would be
      // 07:00:00Z
      var startOfWeek = moment.tz(timeZone).startOf('week').utc();
      var endOfWeek = moment(startOfWeek).add(365, 'day');
    
      this.graphService.getCalendarView(
        startOfWeek.format(),
        endOfWeek.format(),
        this.authService.user.timeZone)
          .then((events) => {
            this.events = events;
          });
    }

    formatDateTimeTimeZone(dateTime: MicrosoftGraph.DateTimeTimeZone): string {
      try {
        return moment.tz(dateTime.dateTime, dateTime.timeZone).format();
      }
      catch(error) {
        this.alertsService.error('DateTimeTimeZone conversion error' + JSON.stringify(error));
      }
    }

    onSubmit(): void {
      const timeZone = "Europe/Warsaw";

      this.reservations.forEach(res => {
        this.model.body = res.comments;
        this.model.start = res.date.toString();
        this.model.end = res.date.toString();
        this.model.subject = "Rezerwacja nr: " + res.id;
        console.log(this.model);
        const graphEvent = this.model.getGraphEvent(timeZone);
        this.graphService.addEventToCalendar(graphEvent)
        .then(() => {
          this.alertsService.success('Event created.');
        }).catch(error => {
          this.alertsService.error('Error creating event.' + error.message);
        });
      })

      // this.model.body = this.reservations[1].comments;
      // this.model.start = this.reservations[1].date.toString();
      // this.model.end = this.reservations[1].date.toString();
      // this.model.subject = "Rezerwacja nr: " + this.reservations[1].id;
      //   console.log(this.model);
      //   const graphEvent = this.model.getGraphEvent(timeZone);
      //   this.graphService.addEventToCalendar(graphEvent)
      //   .then(() => {
      //     this.alertsService.success('Event created.');
      //   }).catch(error => {
      //     this.alertsService.error('Error creating event.' + error.message);
      //   });

      // const graphEvent = this.model.getGraphEvent(timeZone);
  
      // this.graphService.addEventToCalendar(graphEvent)
      //   .then(() => {
      //     this.alertsService.success('Event created.');
      //   }).catch(error => {
      //     this.alertsService.error('Error creating event.' + error.message);
      //   });
    }
}
