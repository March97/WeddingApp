import { Component } from '@angular/core';
import { AuthService } from './_services/auth.service';
import {JwtHelperService} from '@auth0/angular-jwt';
import { OnInit } from '@angular/core';
import { User } from './_models/user';
import {loadStripe} from '@stripe/stripe-js';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'app';
  jwtHelper = new JwtHelperService();

  constructor(private authService: AuthService) {}

  ngOnInit() {
    const token = localStorage.getItem('token');
    const user: User = JSON.parse(localStorage.getItem('user'));
    if (token) {
      this.authService.decodedToken = this.jwtHelper.decodeToken(token);
    }
    if (user) {
      this.authService.currentUser = user;
      this.authService.changeMemberPhoto(user.photoUrl);
    }
  }

  // *** To Do #2 of 3: Add this
  async checkout() {
    
    // Replace with your own publishable key:    https://dashboard.stripe.com/test/apikeys
    const PUBLISHABLE_KEY = 'pk_test_51I7gdcKhTDjEMF3pCbMSERK9k0VlKifvGUiMYperqvOJycUcktKU21r1ogbLuXnl4lxlP3AHR5xQ2DQSjt6lxCp800k2868kLH';
    
    // Replace with the domain you want your users to be redirected back to after payment
    const DOMAIN = 'localhost:4200/reservations';
    
    // Replace with a SKU for your own product (created either in the Stripe Dashboard or with the API)
    const SUBSCRIPTION_BASIC_PLAN_ID = 'plan_1234';

    try {
      const stripe = await loadStripe(PUBLISHABLE_KEY);
      stripe.redirectToCheckout({
        items: [{plan: SUBSCRIPTION_BASIC_PLAN_ID, quantity: 1}],
        successUrl:
          'http://' +
          DOMAIN +
          '/success.html?session_id={CHECKOUT_SESSION_ID}',
        cancelUrl: 'https://' + DOMAIN + '/canceled.html'
      })
        .then(this.handleResult);
    } catch (error) {
      console.error('checkout() try catch error', error);
    }
    
  }

  // *** To Do #3 of 3: Add this
  handleResult(result) {
    console.log('handleResult()', result);
  }
}
