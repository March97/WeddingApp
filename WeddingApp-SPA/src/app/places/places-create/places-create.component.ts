import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Place } from 'src/app/_models/place';
import { User } from 'src/app/_models/user';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { AuthService } from 'src/app/_services/auth.service';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-places-create',
  templateUrl: './places-create.component.html',
  styleUrls: ['./places-create.component.css']
})
export class PlacesCreateComponent implements OnInit {
  @Output() cancelCreate = new EventEmitter();
  user: User;
  place: Place;
  placeForm: FormGroup;

  constructor(private userService: UserService, private authService: AuthService, private alertify: AlertifyService,
    private fb: FormBuilder, private router: Router) { }

  ngOnInit() {
    this.createPlaceForm();
  }

  createPlaceForm() {
    this.placeForm = this.fb.group({
      name: ['', Validators.required],
      country: ['Polska', Validators.required],
      city: ['', Validators.required],
      address: ['', Validators.required],
      facilities: [''],
      capacity: ['', Validators.required],
      price: ['', Validators.required],
      inPrice: [''],
      bonuses: [''],
      description: [''],});
  }

  create() {
    if(this.placeForm.valid) {
      this.place = Object.assign({}, this.placeForm.value);
      this.userService.createPlace(this.authService.decodedToken.nameid, this.place).subscribe(() => {
        this.alertify.success('New place created');
      }, error => {
        this.alertify.error(error);
      }, () => {
        this.router.navigate(['/members']);
      });
    }
    console.log(this.placeForm.value);
  }

  cancel() {
    this.cancelCreate.emit(false);
    this.alertify.message('Cancelled');
    console.log('Cancelled');
  }

}
