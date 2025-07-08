import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientBillingRolesTableComponent } from './client-billing-roles-table.component';

describe('ClientBillingRolesTableComponent', () => {
  let component: ClientBillingRolesTableComponent;
  let fixture: ComponentFixture<ClientBillingRolesTableComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClientBillingRolesTableComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClientBillingRolesTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
