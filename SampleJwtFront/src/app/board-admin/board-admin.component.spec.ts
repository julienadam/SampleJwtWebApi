import { ComponentFixture, TestBed } from '@angular/core/testing';
import { of } from 'rxjs'
import { UserService } from '../_services/user.service';
import { BoardAdminComponent } from './board-admin.component';


describe('BoardAdminComponent', () => {
  let component: BoardAdminComponent;
  let fixture: ComponentFixture<BoardAdminComponent>;
  let userMock = { getAdminBoard: () => of<any>("") };

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BoardAdminComponent ],
      providers: [
          {
            provide: UserService,
            useValue: userMock
          }
        ]  
    })
    .compileComponents();

  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BoardAdminComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
