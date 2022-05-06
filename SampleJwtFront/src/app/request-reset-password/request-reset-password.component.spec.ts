import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../_services/auth.service';

import { RequestResetPasswordComponent } from './request-reset-password.component';

describe('RequestResetPasswordComponent', () => {
  let component: RequestResetPasswordComponent;
  let fixture: ComponentFixture<RequestResetPasswordComponent>;
  let authMock = {};

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FormsModule],
      declarations: [ RequestResetPasswordComponent ],
      providers: [{ provide: AuthService, useValue: authMock }]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RequestResetPasswordComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
