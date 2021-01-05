import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Place } from '../_models/place';
import { AlertifyService } from '../_services/alertify.service';
import { AuthService } from '../_services/auth.service';
import { UserService } from '../_services/user.service';


@Injectable()
export class PlaceDetailResolver implements Resolve<Place> {

    constructor(private userService: UserService, private authService: AuthService,
                private router: Router, private alertify: AlertifyService) {}

        resolve(route: ActivatedRouteSnapshot): Observable<Place> {
            return this.userService.getPlace(this.authService.decodedToken.nameid, route.params['id']).pipe(
                catchError(error => {
                    this.alertify.error('Problem retreiving your data');
                    this.router.navigate(['/places/list']);
                    return of(null);
                })
            );
        }
}