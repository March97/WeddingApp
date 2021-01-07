import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Place } from 'src/app/_models/place';
import { Reservation } from 'src/app/_models/reservation';
import { User } from 'src/app/_models/user';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { AuthService } from 'src/app/_services/auth.service';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-reservations-list',
  templateUrl: './reservations-list.component.html',
  styleUrls: ['./reservations-list.component.css']
})
export class ReservationsListComponent implements OnInit {
  reservations: Reservation[];
  reservationsContainer: 'Your reservations';
  constructor(private userService: UserService, private authService: AuthService, 
    private alertify: AlertifyService, private route: ActivatedRoute) {}

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.reservations = data['reservations'];
      console.log('this.reservations', this.reservations);
    });
    this.loadAdditional();
  }

  loadAdditional() {
    this.reservations.forEach(r => {
      this.userService.getPlace(this.authService.decodedToken.nameid, r.placeId).subscribe((place: Place) => {
        r.placeName = place.name;
      }, error => {
        this.alertify.error(error);
      });
    });

    this.reservations.forEach(r => {
      this.userService.getUser(r.userId).subscribe((user: User) => {
        r.userName = user.knownAs;
      }, error => {
        this.alertify.error(error);
      });
    });
  }

  deleteReservation(id: number) {
    this.alertify.confirm('Are you sure you want to delete this reservation?', () => {
      this.userService.deleteReservation(this.authService.decodedToken.nameid, id).subscribe(() => {
        this.reservations.splice(this.reservations.findIndex(r => r.id === id), 1);
        this.alertify.success('Reservation has been deleted');
      }, error => {
        this.alertify.error('Failed to delete the Reservation');
      });
    });
  }

}
