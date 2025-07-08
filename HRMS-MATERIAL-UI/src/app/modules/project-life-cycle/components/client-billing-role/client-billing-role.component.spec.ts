import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientBillingRoleComponent } from './client-billing-role.component';

describe('ClientBillingRoleComponent', () => {
  let component: ClientBillingRoleComponent;
  let fixture: ComponentFixture<ClientBillingRoleComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClientBillingRoleComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClientBillingRoleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
