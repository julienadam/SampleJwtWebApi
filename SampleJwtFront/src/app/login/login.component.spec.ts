import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AuthService } from '../_services/auth.service';
import { RouterTestingModule } from '@angular/router/testing';
import { LoginComponent } from './login.component';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { of, throwError } from 'rxjs';

describe('LoginComponent', () => {
  let component: LoginComponent;
  let fixture: ComponentFixture<LoginComponent>;
  let authMock = jasmine.createSpyObj('AuthService', ["login"]);
  let router: Router;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FormsModule, RouterTestingModule.withRoutes([])],
      declarations: [ LoginComponent ],
      providers: [{ provide: AuthService, useValue: authMock }]
    })
    .compileComponents();

    router = TestBed.get(Router);
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LoginComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should show an error if the auth service call fails', () => {
    // Remove the reload from our component, otherwise the test will loop indefinitely
    component.reloadPage = function () { };
    authMock.login.and.returnValue(throwError({ error: { message : "Foo"}}));

    component.form.username = "TheUser";
    component.form.password = "ThePassword";
    component.onSubmit();
    expect(component.isLoginFailed).toBeTrue();
    expect(component.errorMessage).toBeDefined();
  });

  it('should store the token if the call to the auth service succeeds', () => {
    // Remove the reload from our component, otherwise the test will loop indefinitely
    component.reloadPage = function () { };
    authMock.login.and.returnValue(of({ token: "TheToken", roles: ["Role1", "Role2"] }));

    component.form.username = "TheUser";
    component.form.password = "ThePassword";
    component.onSubmit();
    expect(component.isLoginFailed).toBeFalse();
    expect(component.isLoggedIn).toBeTrue();
    expect(component.roles).toEqual(["Role1", "Role2"]);
  });
});
