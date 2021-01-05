import { Component, HostListener, Input, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Place } from 'src/app/_models/place';
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
  @HostListener('window:beforeunload', ['$event'])
  unloadNotfication($event: any) {
    if (this.editForm.dirty) {
      $event.returnValue = true;
    }
  }
  
  constructor(private route: ActivatedRoute, private alertify: AlertifyService, private router: Router,
    private userService: UserService, private authService: AuthService) { }

  ngOnInit() {
    // this.route.data.subscribe(data => {
    //   this.place = data['place'];
    // });
    this.loadPlace();
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

}
