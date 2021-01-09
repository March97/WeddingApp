import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import { AuthService } from '../_services/auth.service';
import { UserService } from '../_services/user.service';

@Component({
  selector: 'app-payment',
  templateUrl: './payment.component.html',
  styleUrls: ['./payment.component.css']
})
export class PaymentComponent implements OnInit {

  id: any;
  constructor(private userService: UserService, private authService: AuthService,
    private alertify: AlertifyService, private route: ActivatedRoute, private router: Router) { }

  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      this.id = params['id'];
      console.log(this.id);
      this.userService.pay(this.authService.decodedToken.nameid, this.id).subscribe(() => {
        this.alertify.success('Payment complete');
        this.router.navigate(['/reservations']);
      });
    }, error => {
      this.alertify.error(error);
    });
  }

  paymentResult() {
    this.userService.pay(this.authService.decodedToken.nameid, this.id).subscribe(() => {
      this.alertify.success('Payment complete');
    });
  }

}
