import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
const AUTH_API = 'https://localhost:7212/api/Security/';
const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};
@Injectable({
  providedIn: 'root'
})
export class AuthService {
  constructor(private http: HttpClient) { }
  login(username: string, password: string): Observable<any> {
    return this.http.post(AUTH_API + 'Login', {
      username,
      password
    }, httpOptions);
  }
  
  register(username: string, email: string, password: string): Observable<any> {
    return this.http.post(AUTH_API + 'Register', {
      name: username,
      email,
      password,
      phoneNo: ""
    }, httpOptions);
  }
}
