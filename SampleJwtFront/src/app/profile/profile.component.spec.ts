import { ComponentFixture, TestBed } from '@angular/core/testing';
import { TokenStorageService } from '../_services/token-storage.service';

import { ProfileComponent } from './profile.component';

describe('ProfileComponent', () => {
  let component: ProfileComponent;
  let fixture: ComponentFixture<ProfileComponent>;
  let tokenStorageMock = 
    { 
      getToken: () => "SomeTokenData",
      getUser: () => { username : "TheUser"}
    };

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProfileComponent ],
      providers: [{ provide: TokenStorageService, useValue: tokenStorageMock }]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProfileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
