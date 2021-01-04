// import { Injectable } from '@angular/core';
// import { ActivatedRouteSnapshot, Resolve, Router } from '@angular/router';
// import { Observable, of } from 'rxjs';
// import { catchError } from 'rxjs/operators';
// import { Place } from '../_models/place';
// import { User } from '../_models/user';
// import { AlertifyService } from '../_services/alertify.service';
// import { AuthService } from '../_services/auth.service';
// import { UserService } from '../_services/user.service';


// @Injectable()
// export class PlacesListResolver implements Resolve<Place> {

//     pageNumber = 1;
//     pageSize = 5;

//     constructor(private userService: UserService, 
//                 private router: Router, private alertify: AlertifyService) {}

//         resolve(route: ActivatedRouteSnapshot): Observable<Place[]> {
//             return this.userService.getPlaces(this.pageNumber, this.pageSize).pipe(
//                 catchError(error => {
//                     this.alertify.error('Problem retreiving data');
//                     this.router.navigate(['/home']);
//                     return of(null);
//                 })
//             );
//         }
// }