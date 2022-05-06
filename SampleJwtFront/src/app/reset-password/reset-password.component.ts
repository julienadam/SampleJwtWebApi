import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.css']
})
export class ResetPasswordComponent implements OnInit {
  hasErrors = false;
  userName= "";
  passwordResetToken="";
  errorMessage = "";
  form: any = {
    newPassword: null
  };
  constructor(private router: ActivatedRoute) { }

  ngOnInit(): void {
    this.router.queryParams.subscribe(res => {
      this.userName = res['username'];
      this.passwordResetToken = res['token'];
    })
  }

  onSubmit(): void {
    console.log("Doing the reset dance for user : " + this.userName + "\nwith token: \n" + this.passwordResetToken);
  }
}
