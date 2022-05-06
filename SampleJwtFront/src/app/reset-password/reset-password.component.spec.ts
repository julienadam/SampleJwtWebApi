import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ActivatedRoute } from '@angular/router';
import { of } from 'rxjs';
import { Router } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';
import { ResetPasswordComponent } from './reset-password.component';
import { AuthService } from '../_services/auth.service';
import { FormsModule } from '@angular/forms';

describe('ResetPasswordComponent', () => {
  let component: ResetPasswordComponent;
  let fixture: ComponentFixture<ResetPasswordComponent>;
  let router: Router;
  let authMock = {};

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FormsModule, RouterTestingModule.withRoutes([])],
      declarations: [ResetPasswordComponent],
      providers: [{
        provide: ActivatedRoute,
        useValue: {
          queryParams: of({ username: "TheUser", token: "TheToken" })
        }
      },
      {
        provide: AuthService, useValue: authMock
      }]
    })
      .compileComponents();

    router = TestBed.get(Router);
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ResetPasswordComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create with params from query string', () => {
    expect(component).toBeTruthy();
    expect(component.userName).toBe("TheUser");
    expect(component.passwordResetToken).toBe("TheToken");
  });
});
