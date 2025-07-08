import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientBillingRolesFormComponent } from './client-billing-roles-form.component';

describe('ClientBillingRolesFormComponent', () => {
  let component: ClientBillingRolesFormComponent;
  let fixture: ComponentFixture<ClientBillingRolesFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClientBillingRolesFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClientBillingRolesFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
