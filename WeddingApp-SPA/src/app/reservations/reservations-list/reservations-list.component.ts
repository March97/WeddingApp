import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Place } from 'src/app/_models/place';
import { Reservation } from 'src/app/_models/reservation';
import { User } from 'src/app/_models/user';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { AuthService } from 'src/app/_services/auth.service';
import { UserService } from 'src/app/_services/user.service';
import { loadStripe } from "@stripe/stripe-js";
import { HttpClient } from '@angular/common/http';
import { analyzeAndValidateNgModules } from '@angular/compiler';

@Component({
  selector: 'app-reservations-list',
  templateUrl: './reservations-list.component.html',
  styleUrls: ['./reservations-list.component.css']
})
export class ReservationsListComponent implements OnInit {
  reservations: Reservation[];
  response: any;
  reservationsContainer: 'Your reservations';
  stripePromise = loadStripe("pk_test_51I7gdcKhTDjEMF3pCbMSERK9k0VlKifvGUiMYperqvOJycUcktKU21r1ogbLuXnl4lxlP3AHR5xQ2DQSjt6lxCp800k2868kLH");
  id: any;
  constructor(private userService: UserService, private authService: AuthService,
    private alertify: AlertifyService, private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.reservations = data['reservations'];
      console.log('this.reservations', this.reservations);
    });
    this.loadAdditional();
    // this.route.queryParams.subscribe(params => {
    //   this.id = params['id'];
    //   console.log(this.id);
    // }, error => {}, 

    
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

  async pay(res: Reservation) {
    console.log('pay');
    const stripe = await this.stripePromise;
    // const response = await this.http.post("http://localhost:5000/create-checkout-session", res);

    this.userService.payment(res).subscribe((response) => {
      this.response = response;
      const session = this.response;
      stripe.redirectToCheckout({
        sessionId: session.id,
      });
    });
  }

  loadReservations() {
    this.route.data.subscribe(data => {
      this.reservations = data['reservations'];
      console.log('this.reservations', this.reservations);
    });
    this.loadAdditional();
  
  }

  paymentComplete() {
    this.userService.pay(this.authService.decodedToken.nameid, this.id).subscribe(() => {
      this.alertify.success('Payment complete');
      this.route['reservations'];
    }, error => {
      this.alertify.error(error);
    }, () => {this.loadReservations();});
  }
}
