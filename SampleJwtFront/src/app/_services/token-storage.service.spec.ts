import { TestBed } from '@angular/core/testing';

import { TokenStorageService } from './token-storage.service';

describe('TokenStorageService', () => {
  let service: TokenStorageService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(TokenStorageService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should return token after storing it', () => {
    service.saveToken("foo");
    expect(service.getToken()).toBe("foo");
  });

  it('should return user data after storing it', () => {
    var u = { name: "blah", roles: ["Admin", "User"] };
    service.saveUser(u);
    expect(service.getUser()).toEqual(u);
  });
});
