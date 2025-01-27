import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PaginatedResult, Pagination } from 'src/app/_models/pagination';
import { User } from '../../_models/user';
import { AlertifyService } from '../../_services/alertify.service';
import { UserService } from '../../_services/user.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {
  users: User[];
  user: User = JSON.parse(localStorage.getItem('user'));
  genderList = [{value: 'male', display: 'Males'}, {value: 'female', display: 'Females'}];
  professionsList = [{value: 'Hairdresser', display: 'Hairdresser'}, {value: 'Make-up', display: 'Make-up'}, 
  {value: 'Stylist', display: 'Stylist'}, {value: 'Tailor', display: 'Tailor'}, {value: 'Musician', display: 'Musician'},
  {value: 'Chauffeur', display: 'Chauffeur'}, {value: 'Entertainer', display: 'Entertainer'}, 
  {value: 'Fireworks', display: 'Fireworks'}, {value: 'Barman', display: 'Barman'}, {value: 'User', display: 'User'}];
  userParams: any = {};
  pagination: Pagination;

  constructor(private userService: UserService, private alertify: AlertifyService, 
    private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.users = data['users'].result;
      this.pagination = data['users'].pagination;
    });

    this.userParams.profession = 'User';
    //this.userParams.gender = this.user.geneder === 'female' ? 'male' : 'male';
    // this.userParams.minAge = 18;
    // this.userParams.maxAge = 99;
    this.userParams.orderBy = 'lastActive';
    this.resetFilters();
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadUsers();
  }

  resetFilters(){
    //this.userParams.gender = this.user.geneder === 'female' ? 'male' : 'male';
    this.userParams.profession = 'User';
    // this.userParams.minAge = 18;
    // this.userParams.maxAge = 99;
    this.loadUsers();
  }

  loadUsers() {
    this.userService
    .getUsers(this.pagination.currentPage, this.pagination.itemsPerPage, this.userParams)
    .subscribe(
      (res: PaginatedResult<User[]>) => {
          this.users = res.result;
          this.pagination = res.pagination;
    }, error => {
      this.alertify.error(error);
    });
  }

}
