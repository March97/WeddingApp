import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';
import { Place } from 'src/app/_models/place';
import { Reservation } from 'src/app/_models/reservation';
import { User } from 'src/app/_models/user';
import { ReservationCreateResolver } from 'src/app/_resolvers/reservation-create.resolver';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { AuthService } from 'src/app/_services/auth.service';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-reservation-create',
  templateUrl: './reservation-create.component.html',
  styleUrls: ['./reservation-create.component.css'],
})
export class ReservationCreateComponent implements OnInit {
  @Output() cancelCreate = new EventEmitter();
  user: User;
  place: Place;
  reservation: Reservation;
  reservationForm: FormGroup;
  bsConfig: Partial<BsDatepickerConfig>;

  constructor(
    private authService: AuthService,
    private alertify: AlertifyService,
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private userService: UserService
  ) {}

  ngOnInit() {
    this.bsConfig = {
      containerClass: 'theme-red',
    };
    this.route.data.subscribe((data) => {
      this.place = data['place'];
    });
    this.createReservationForm();
  }

  createReservationForm() {
    this.reservationForm = this.fb.group({
      amountOfGuests: [
        '',
        [
          Validators.required,
          Validators.max(this.place.capacity),
          Validators.min(1),
        ],
      ],
      date: [null, [Validators.required]],
      comments: [''],
    });
  }

  reserve() {
    if (this.reservationForm.valid) {
      this.reservation = Object.assign({}, this.reservationForm.value);
      this.reservation.userId = this.authService.decodedToken.nameid;
      this.reservation.placeId = this.place.id;
      this.userService
        .createReservation(
          this.authService.decodedToken.nameid,
          this.reservation
        )
        .subscribe(
          () => {
            this.alertify.success('Reservation successful');
          },
          (error) => {
            this.alertify.error(error);
          },
          () => {
            this.router.navigate(['places/detail/', this.place.id]);
          }
        );
    }
    console.log(this.reservationForm.value);
  }

  cancel() {
    this.cancelCreate.emit(false);
    this.alertify.message('Cancelled');
    this.router.navigate(['places/detail/', this.place.id]);
    console.log('Cancelled');
  }
}
