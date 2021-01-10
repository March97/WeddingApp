import { Component, OnInit } from '@angular/core';
import { UserM } from '../_models/userM';
import { AuthMicrosoftService } from '../_services/auth-microsoft.service';

@Component({
  selector: 'app-loginmicrosoft',
  templateUrl: './loginmicrosoft.component.html',
  styleUrls: ['./loginmicrosoft.component.css']
})
export class LoginmicrosoftComponent implements OnInit {
 // Is a user logged in?
 get authenticated(): boolean {
  return this.authService.authenticated;
}
// The user
get user(): UserM {
  return this.authService.user;
}

  constructor(private authService: AuthMicrosoftService) { }

  ngOnInit() {
  }

  async signIn(): Promise<void> {
    await this.authService.signIn();
  }

  signOut(): void {
    this.authService.signOut();
  }

}
