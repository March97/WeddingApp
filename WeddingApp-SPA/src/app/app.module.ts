import { BrowserModule} from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import {HttpClientModule} from '@angular/common/http';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';

import { AppComponent } from './app.component';
import { NavComponent } from './nav/nav.component';
import { AuthService } from './_services/auth.service';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { ErrorInterceptorProvider } from './_services/error.interceptor';
import { AlertifyService } from './_services/alertify.service';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MemberListComponent } from './members/member-list/member-list.component';
import { ListsComponent } from './lists/lists.component';
import { MessagesComponent } from './messages/messages.component';
import { RouterModule } from '@angular/router';
import { appRoutes } from './routes';
import { MemberCardComponent } from './members/member-card/member-card.component';
import { JwtModule } from '@auth0/angular-jwt';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { AuthGuard } from './_guards/auth.guard';
import { UserService } from './_services/user.service';
import { MemberDetailResolver } from './_resolvers/member-detail.resolver';
import { MemberListResolver } from './_resolvers/member-list.resolver';
import { NgxGalleryModule } from '@kolkov/ngx-gallery';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { MemberEditResolver } from './_resolvers/member-edit.resolver';
import { PreventUnsavedChanges } from './_guards/prevent-unsaved-changes.guard';
import { PhotoEditorComponent } from './members/photo-editor/photo-editor.component';
import { FileUploadModule } from 'ng2-file-upload';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { TimeagoModule } from 'ngx-timeago';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { ButtonsModule } from 'ngx-bootstrap/buttons';
import { ListsResolver } from './_resolvers/lists.resolver';
import { MessagesResolver } from './_resolvers/messages.resolver';
import { MemberMessagesComponent } from './members/member-messages/member-messages.component';
import { PlacesCreateComponent } from './places/place-create/place-create.component';
import { PlacesCreateResolver } from './_resolvers/places-create.resolver';
import { PlacesListForUserComponent } from './places/places-list-for-user/places-list-for-user.component';
import { PlacesListForUserResolver } from './_resolvers/places-list-for-user.resolver';
import { PlaceCardComponent } from './places/place-card/place-card.component';
import { PlaceEditComponent } from './places/place-edit/place-edit.component';
import { PlaceEditResolver } from './_resolvers/place-edit.resolver';
import { PlacePhotoEditorComponent } from './places/place-photo-editor/place-photo-editor.component';
import { PlacesListComponent } from './places/places-list/places-list.component';
import { PlacesListResolver } from './_resolvers/places-list.resolver';
import { PlaceListCardComponent } from './places/place-list-card/place-list-card.component';
import { PlaceDetailResolver } from './_resolvers/place-detail.resolver';
import { PlaceDetailComponent } from './places/place-detail/place-detail.component';
import { ReservationCreateComponent } from './reservations/reservation-create/reservation-create.component';
import { ReservationCreateResolver } from './_resolvers/reservation-create.resolver';

export function tokenGetter() {
  return localStorage.getItem('token');
}

// export class CustomHammerConfig extends HammerGestureConfig {
//   overrides = {
//     pinch: { enable: false},
//     rotate: { enable: false}
//   };
// }

@NgModule({
  declarations: [
      AppComponent,
      NavComponent,
      HomeComponent,
      RegisterComponent,
      MemberListComponent,
      ListsComponent,
      MessagesComponent,
      MemberCardComponent,
      MemberDetailComponent,
      MemberEditComponent,
      PhotoEditorComponent,
      MemberMessagesComponent,
      PlacesCreateComponent,
      PlacesListForUserComponent,
      PlaceCardComponent,
      PlaceEditComponent,
      PlacePhotoEditorComponent,
      PlacesListComponent,
      PlaceListCardComponent,
      PlaceDetailComponent,
      ReservationCreateComponent
   ],
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    BsDropdownModule.forRoot(),
    TabsModule.forRoot(),
    RouterModule.forRoot(appRoutes),
    NgxGalleryModule,
    FileUploadModule,
    ButtonsModule.forRoot(),
    TimeagoModule.forRoot(),
    BsDatepickerModule.forRoot(),
    PaginationModule.forRoot(),
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
        allowedDomains: ['localhost:5000'],
        disallowedRoutes: ['localhost:5000/api/auth']
      }
    })
  ],
  providers: [
    AuthService,
    ErrorInterceptorProvider,
    AlertifyService,
    AuthGuard,
    UserService,
    MemberDetailResolver,
    MemberListResolver,
    MemberEditResolver,
    PreventUnsavedChanges,
    ListsResolver,
    MessagesResolver,
    PlacesCreateResolver,
    PlacesListForUserResolver,
    PlaceEditResolver,
    PlacesListResolver,
    PlaceDetailResolver,
    ReservationCreateResolver
    // { provide: HAMMER_GESTURE_CONFIG, useClass: CustomHammerConfig }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
