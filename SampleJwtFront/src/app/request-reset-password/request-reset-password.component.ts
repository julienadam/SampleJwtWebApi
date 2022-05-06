import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-request-reset-password',
  templateUrl: './request-reset-password.component.html',
  styleUrls: ['./request-reset-password.component.css']
})
export class RequestResetPasswordComponent implements OnInit {

  isSuccess = false;
  errorMessage = "";
  hasErrors = false;
  form = {
    email : ""
  };

  constructor(private authService: AuthService) { }

  ngOnInit(): void {
  }

  onSubmit(): void {
    console.log("Asking password reset for email: " + this.form.email);
    this.authService.requestResetPassword(this.form.email).subscribe(
      {
        next: _ => {
            this.isSuccess = true;
        },
        error: err => {
          this.errorMessage = err.error.message;
          this.hasErrors = true;
        }
      }
    );
  }
}
