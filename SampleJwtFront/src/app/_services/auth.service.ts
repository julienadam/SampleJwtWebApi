import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
const API_URL = environment.backUrl + 'Security/';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};
@Injectable({
  providedIn: 'root'
})
export class AuthService {
  
  constructor(private http: HttpClient) { }
  login(username: string, password: string): Observable<any> {
    return this.http.post(API_URL + 'Login', {
      username,
      password
    }, httpOptions);
  }
  
  register(username: string, email: string, password: string, phoneNumber: string): Observable<any> {
    return this.http.post(API_URL + 'Register', {
      name: username,
      email,
      password,
      phoneNo: phoneNumber
    }, httpOptions);
  }

  resetPassword(username: string, token: string, password: string): Observable<any> {
    return this.http.post(API_URL + 'ResetPassword', {
      username,
      token,
      newpassword : password
    }, httpOptions);
  }

  requestResetPassword(email: string) {
    return this.http.post(API_URL + 'RequestPasswordResetEmail', {
      email
    }, httpOptions);
  }
}
