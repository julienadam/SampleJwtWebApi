import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AuthService } from '../_services/auth.service';
import { RouterTestingModule } from '@angular/router/testing';
import { LoginComponent } from './login.component';
import { Router } from '@angular/router';

describe('LoginComponent', () => {
  let component: LoginComponent;
  let fixture: ComponentFixture<LoginComponent>;
  let authMock = { };
  let router: Router;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RouterTestingModule.withRoutes([])],
      declarations: [ LoginComponent ],
      providers: [ { provide: AuthService, useValue: authMock }]
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
});
