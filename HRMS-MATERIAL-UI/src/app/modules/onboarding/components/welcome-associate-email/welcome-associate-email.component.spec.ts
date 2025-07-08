import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WelcomeAssociateEmailComponent } from './welcome-associate-email.component';

describe('WelcomeAssociateEmailComponent', () => {
  let component: WelcomeAssociateEmailComponent;
  let fixture: ComponentFixture<WelcomeAssociateEmailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WelcomeAssociateEmailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WelcomeAssociateEmailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
