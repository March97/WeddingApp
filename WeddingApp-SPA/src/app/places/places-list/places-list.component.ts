import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PaginatedResult, Pagination } from 'src/app/_models/pagination';
import { Place } from 'src/app/_models/place';
import { User } from 'src/app/_models/user';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { AuthService } from 'src/app/_services/auth.service';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-places-list',
  templateUrl: './places-list.component.html',
  styleUrls: ['./places-list.component.css']
})
export class PlacesListComponent implements OnInit {
  places: Place[];
  user: User = JSON.parse(localStorage.getItem('user'));
  cityList = [{value: 'Warszawa', display: 'Warszawa'}, {value: 'Poznań', display: 'Poznań'}, {value: 'Wrocław', display: 'Wrocław'}];
  placeParams: any = {};
  pagination: Pagination;

  constructor(private userService: UserService, private alertify: AlertifyService, private authService: AuthService, 
    private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.places = data['places'].result;
      this.pagination = data['places'].pagination;
    });

    this.placeParams.city = this.user.city;
    this.placeParams.minPrice = 0;
    this.placeParams.maxPrice = 100000;
    this.placeParams.minCapacity = 0;
    this.placeParams.maxCapacity = 100000;
    this.placeParams.orderBy = 'created';
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadPlaces();
  }

  resetFilters(){
    this.placeParams.city = null;
    this.placeParams.minPrice = 0;
    this.placeParams.maxPrice = 100000;
    this.placeParams.minCapacity = 0;
    this.placeParams.maxCapacity = 100000;
    this.placeParams.orderBy = 'created';
    this.loadPlaces();
  }

  loadPlaces() {
    this.userService
    .getPlaces(this.authService.decodedToken.nameid, this.pagination.currentPage, this.pagination.itemsPerPage, this.placeParams)
    .subscribe(
      (res: PaginatedResult<Place[]>) => {
          this.places = res.result;
          this.pagination = res.pagination;
    }, error => {
      this.alertify.error(error);
    });
  }

}
