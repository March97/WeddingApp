import { Component, HostListener, Input, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Place } from 'src/app/_models/place';
import { Reservation } from 'src/app/_models/reservation';
import { User } from 'src/app/_models/user';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { AuthService } from 'src/app/_services/auth.service';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-place-edit',
  templateUrl: './place-edit.component.html',
  styleUrls: ['./place-edit.component.css']
})
export class PlaceEditComponent implements OnInit {

  @ViewChild('editForm') editForm: NgForm;
  place: Place;
  photoUrl: string;
  reservations: Reservation[];
  @HostListener('window:beforeunload', ['$event'])
  unloadNotfication($event: any) {
    if (this.editForm.dirty) {
      $event.returnValue = true;
    }
  }
  
  constructor(private route: ActivatedRoute, private alertify: AlertifyService, private router: Router,
    private userService: UserService, private authService: AuthService) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.place = data['place'];
    });
    this.updateMainPhoto();
    this.loadReservations();
    // this.loadPlace();
  }

  updatePlace() {
    this.userService.updatePlace(this.authService.decodedToken.nameid, +this.route.snapshot.params['id'], this.place).subscribe(next => {
      this.alertify.success('Place updated successful');
      this.editForm.reset(this.place);
    }, error => {
      this.alertify.error(error);
    });
  }

  updateMainPhoto(a?: any) {
    this.place.photoUrl = this.place.photos.find(p => p.isMain == true).url;
  }

  loadPlace() {
    this.userService.getPlace(this.authService.decodedToken.nameid, +this.route.snapshot.params['id'])
      .subscribe((place: Place) => {
        this.place = place;
        this.updateMainPhoto();
      }, error => {
        this.alertify.error(error);
      });
  }

  deletePlace() {
    this.alertify.confirm('Are you sure you want to delete this message?', () => {
      this.userService.deletePlace(this.authService.decodedToken.nameid, this.place.id).subscribe(() => {
        this.alertify.success('Place has been deleted');
        this.router.navigate(['/places']);
      }, error => {
        this.alertify.error('Failed to delte the message');
      });
    });
  }

  loadReservations() {
    this.userService.getReservationsForPlace(this.authService.decodedToken.nameid, this.place.id)
    .subscribe((reservations: Reservation[]) => {
      this.reservations = reservations;
    }, error => {
      this.alertify.error(error);
    },  () => this.loadAddons()
    );

    
  }

  loadAddons() {
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
