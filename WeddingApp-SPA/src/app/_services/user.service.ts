import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Message } from '../_models/message';
import { PaginatedResult } from '../_models/pagination';
import { Place } from '../_models/place';
import { Reservation } from '../_models/reservation';
import { User } from '../_models/user';

// const httpOptions = {
//   headers: new HttpHeaders({
//     'Authorization': 'Bearer ' + localStorage.getItem('token')
//   })
// };

@Injectable({
  providedIn: 'root'
})
export class UserService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getUsers(page?, itemsPerPage?, userParams?, likesParam?): Observable<PaginatedResult<User[]>> {
    const paginatedResult: PaginatedResult<User[]> = new PaginatedResult<User[]>();

    let params = new HttpParams();

    if (page != null && itemsPerPage != null) {
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemsPerPage);
    }

    if (userParams != null) {
      // params = params.append('minAge', userParams.minAge);
      // params = params.append('maxAge', userParams.maxAge);
      params = params.append('profession', userParams.profession);
      params = params.append('orderBy', userParams.orderBy);
    }

    if (likesParam === 'Likers') {
      params = params.append('likers', 'true');
    }

    if (likesParam === 'Likees') {
      params = params.append('likees', 'true');
    }

    return this.http.get<User[]>(this.baseUrl + 'users', { observe: 'response', params })
      .pipe(map(response => {
        paginatedResult.result = response.body;
        if (response.headers.get('Pagination') != null) {
          paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
        }
        return paginatedResult;
      })
      );
  }

  getUser(id): Observable<User> {
    return this.http.get<User>(this.baseUrl + 'users/' + id);
    // return this.http.get<User>(this.baseUrl + 'users/' + id, httpOptions);
  }

  updateUser(id: number, user: User) {
    return this.http.put(this.baseUrl + 'users/' + id, user);
  }

  setMainPhoto(userId: number, id: number) {
    return this.http.post(this.baseUrl + 'users/' + userId + '/photos/' + id + '/setMain', {});
  }

  deletePhoto(userId: number, id: number) {
    return this.http.delete(this.baseUrl + 'users/' + userId + '/photos/' + id);
  }

  sendLike(id: number, recipientId: number) {
    return this.http.post(this.baseUrl + 'users/' + id + '/like/' + recipientId, {});
  }

  getMessages(id: number, page?, itemsPerPage?, messageContainer?) {
    const paginatedResult: PaginatedResult<Message[]> = new PaginatedResult<Message[]>();

    let params = new HttpParams();

    params = params.append('MessageContainer', messageContainer);

    if (page != null && itemsPerPage != null) {
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemsPerPage);
    }

    return this.http.get<Message[]>(this.baseUrl + 'users/' + id + '/messages', { observe: 'response', params })
      .pipe(
        map(response => {
          paginatedResult.result = response.body;
          if (response.headers.get('Pagination') !== null) {
            paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
          }
          return paginatedResult;
        })
      );
  }

  getMessageThread(id: number, recipientId: number) {
    return this.http.get<Message[]>(this.baseUrl + 'users/' + id + '/messages/thread/' + recipientId);
  }

  sendMessage(id: number, message: Message) {
    return this.http.post(this.baseUrl + 'users/' + id + '/messages', message);
  }

  deleteMessage(id: number, userId: number) {
    return this.http.post(this.baseUrl + 'users/' + userId + '/messages/' + id, {});
  }

  markAsRead(userId: number, messageId: number) {
    return this.http.post(this.baseUrl + 'users/' + userId + '/messages/' + messageId + '/read', {})
      .subscribe();
  }

  createPlace(userId: number, place: Place) {
    return this.http.post(this.baseUrl + 'users/' + userId + '/places', place);
  }

  getPlacesForUser(userId: number) {
    return this.http.get(this.baseUrl + 'users/' + userId + '/places');
  }

  updatePlace(userId: number, placeId: number, place: Place) {
    return this.http.put(this.baseUrl + 'users/' + userId + '/places/' + placeId, place);
  }

  deletePlace(userId: number, placeId: number) {
    return this.http.delete(this.baseUrl + 'users/' + userId + '/places/' + placeId);
  }

  getPlace(userId: number, id): Observable<Place> {
    return this.http.get<Place>(this.baseUrl + 'users/' + userId + '/places/' + id);
    // return this.http.get<User>(this.baseUrl + 'users/' + id, httpOptions);
  }

  setMainPhotoForPlace(userId: number, placeId: number, id: number) {
    return this.http.post(this.baseUrl + 'users/' + userId + '/places/' + placeId + '/photos/' + id + '/setMain', {});
  }

  deletePhotoForPlace(userId: number, placeId: number, id: number) {
    return this.http.delete(this.baseUrl + 'users/' + userId + '/places/' + placeId +'/photos/' + id);
  }

  getPlaces(userId: number, page?, itemsPerPage?, placeParams?): Observable<PaginatedResult<Place[]>> {
    const paginatedResult: PaginatedResult<Place[]> = new PaginatedResult<Place[]>();

    let params = new HttpParams();

    if (page != null && itemsPerPage != null) {
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemsPerPage);
    }

    if (placeParams != null) {
      params = params.append('minPrice', placeParams.minPrice);
      params = params.append('maxPrice', placeParams.maxPrice);
      params = params.append('minCapacity', placeParams.minCapacity);
      params = params.append('maxCapacity', placeParams.maxCapacity);
      params = params.append('city', placeParams.city);
      params = params.append('orderBy', placeParams.orderBy);
    }

    return this.http.get<Place[]>(this.baseUrl + 'users/' + userId + '/places/list', { observe: 'response', params })
      .pipe(map(response => {
        paginatedResult.result = response.body;
        if (response.headers.get('Pagination') != null) {
          paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
        }
        return paginatedResult;
      })
      );
  }

  createReservation(userId: number, reservation: Reservation) {
    return this.http.post(this.baseUrl + 'users/' + userId + '/reservations', reservation);
  }

  getReservationsForUser(userId: number) {
    return this.http.get<Reservation[]>(this.baseUrl + 'users/' + userId + '/reservations');
  }

  getReservationsForPlace(userId: number, placeId: number) {
    return this.http.get<Reservation[]>(this.baseUrl + 'users/' + userId + '/reservations/places/' + placeId);
  }

  deleteReservation(userId: number, id: number) {
    return this.http.delete(this.baseUrl + 'users/' + userId + '/reservations/' + id);
  }

  payment(reservation: Reservation) {
    return this.http.post("http://localhost:5000/create-checkout-session", reservation);
  }

  pay(userId: number, id: number) {
    return this.http.put(this.baseUrl + 'users/' + userId + '/reservations/' + id, null);
  }
}
