import { Injectable } from '@angular/core';
import { Client } from '@microsoft/microsoft-graph-client';
import * as MicrosoftGraph from '@microsoft/microsoft-graph-types';

import { AlertifyService } from './alertify.service';
import { AuthMicrosoftService } from './auth-microsoft.service';

@Injectable({
  providedIn: 'root'
})

export class GraphService {

  private graphClient: Client;
  constructor(
    private authService: AuthMicrosoftService,
    private alertsService: AlertifyService) {

    // Initialize the Graph client
    this.graphClient = Client.init({
      authProvider: async (done) => {
        // Get the token from the auth service
        let token = await this.authService.getAccessToken()
          .catch((reason) => {
            done(reason, null);
          });

        if (token)
        {
          done(null, token);
        } else {
          done("Could not get an access token", null);
        }
      }
    });
  }

  async getCalendarView(start: string, end: string, timeZone: string): Promise<MicrosoftGraph.Event[]> {
    try {
      // GET /me/calendarview?startDateTime=''&endDateTime=''
      // &$select=subject,organizer,start,end
      // &$orderby=start/dateTime
      // &$top=50
      let result =  await this.graphClient
        .api('/me/calendarview')
        .header('Prefer', `outlook.timezone="${timeZone}"`)
        .query({
          startDateTime: start,
          endDateTime: end
        })
        .select('subject,organizer,start,end')
        .orderby('start/dateTime')
        .top(50)
        .get();

      return result.value;
    } catch (error) {
      this.alertsService.error('Could not get events' + JSON.stringify(error, null, 2));
    }
  }

  async addEventToCalendar(newEvent: MicrosoftGraph.Event): Promise<void> {
    try {
      // POST /me/events
      await this.graphClient
        .api('/me/events')
        .post(newEvent);
    } catch (error) {
      throw Error(JSON.stringify(error, null, 2));
    }
  }
}