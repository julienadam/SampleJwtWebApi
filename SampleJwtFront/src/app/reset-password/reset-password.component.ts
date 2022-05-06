import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.css']
})
export class ResetPasswordComponent implements OnInit {
  hasErrors = false;
  isSuccess = false;
  userName= "";
  passwordResetToken="";
  errorMessage = "";
  form: any = {
    newPassword: null
  };
  constructor(private router: ActivatedRoute, private navigator: Router, private authService: AuthService) { }

  ngOnInit(): void {
    this.router.queryParams.subscribe(res => {
      this.userName = res['username'];
      this.passwordResetToken = res['token'];
    })
  }

  onSubmit(): void {
    this.authService
      .resetPassword(this.userName, this.passwordResetToken, this.form.newPassword)
      .subscribe(
        { 
          next: _ => {
            this.isSuccess = true;
            setTimeout(() => { 
              console.log("Redirecting to login page")
              this.navigator.navigate(["/login"]); }, 3000);
          },
          error: err => {
            this.errorMessage = err.error.message;
            this.hasErrors = true;
          }
        }
      );
  }
}
