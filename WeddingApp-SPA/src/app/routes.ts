import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { ListsComponent } from './lists/lists.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MessagesComponent } from './messages/messages.component';
import { PlacesCreateComponent } from './places/place-create/place-create.component';
import { PlaceDetailComponent } from './places/place-detail/place-detail.component';
import { PlaceEditComponent } from './places/place-edit/place-edit.component';
import { PlacesListForUserComponent } from './places/places-list-for-user/places-list-for-user.component';
import { PlacesListComponent } from './places/places-list/places-list.component';
import { AuthGuard } from './_guards/auth.guard';
import { PreventUnsavedChanges } from './_guards/prevent-unsaved-changes.guard';
import { ListsResolver } from './_resolvers/lists.resolver';
import { MemberDetailResolver } from './_resolvers/member-detail.resolver';
import { MemberEditResolver } from './_resolvers/member-edit.resolver';
import { MemberListResolver } from './_resolvers/member-list.resolver';
import { MessagesResolver } from './_resolvers/messages.resolver';
import { PlaceDetailResolver } from './_resolvers/place-detail.resolver';
import { PlaceEditResolver } from './_resolvers/place-edit.resolver';
import { PlacesCreateResolver } from './_resolvers/places-create.resolver';
import { PlacesListForUserResolver } from './_resolvers/places-list-for-user.resolver';
import { PlacesListResolver } from './_resolvers/places-list.resolver';

export const appRoutes: Routes = [
    { path: '', component: HomeComponent },
    {
        path: '',
        runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard],
        children: [
            { path: 'members', component: MemberListComponent,
                resolve: {users: MemberListResolver} },
            { path: 'members/:id', component: MemberDetailComponent, 
                resolve: {user: MemberDetailResolver} },
            { path: 'member/edit', component: MemberEditComponent, 
                resolve: {user: MemberEditResolver}, canDeactivate: [PreventUnsavedChanges]},
            { path: 'messages', component: MessagesComponent, resolve:  {messages: MessagesResolver}},
            { path: 'lists', component: ListsComponent, resolve: {users: ListsResolver}},
            { path: 'places/create', component: PlacesCreateComponent, resolve: {users: PlacesCreateResolver}},
            { path: 'places', component: PlacesListForUserComponent, resolve: {users: PlacesListForUserResolver}},
            { path: 'places/edit/:id', component: PlaceEditComponent, resolve: {users: PlaceEditResolver}},
            { path: 'places/list', component: PlacesListComponent, resolve: {places: PlacesListResolver}},
            { path: 'places/detail/:id', component: PlaceDetailComponent, resolve: {places: PlaceDetailResolver}}
        ]
    },
    { path: '**', redirectTo: '', pathMatch: 'full' }
];
