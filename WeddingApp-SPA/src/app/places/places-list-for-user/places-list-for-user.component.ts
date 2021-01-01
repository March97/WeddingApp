import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Place } from 'src/app/_models/place';
import { User } from 'src/app/_models/user';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { AuthService } from 'src/app/_services/auth.service';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-places-list-for-user',
  templateUrl: './places-list-for-user.component.html',
  styleUrls: ['./places-list-for-user.component.css']
})
export class PlacesListForUserComponent implements OnInit {
  places: Place[];
  user: User = JSON.parse(localStorage.getItem('user'));

  constructor(private userService: UserService, private authService: AuthService, private alertify: AlertifyService, 
    private router: Router) { }

  ngOnInit() {
    this.loadUsers();
  }

  loadUsers() {
    this.userService.getPlacesForUser(this.authService.decodedToken.nameid).subscribe((places: Place[]) => {
      this.places = places;
    }, error => {
      this.alertify.error(error);
    });
  }

}
