import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientBillingRolesComponent } from './client-billing-roles.component';

describe('ClientBillingRolesComponent', () => {
  let component: ClientBillingRolesComponent;
  let fixture: ComponentFixture<ClientBillingRolesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClientBillingRolesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClientBillingRolesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
