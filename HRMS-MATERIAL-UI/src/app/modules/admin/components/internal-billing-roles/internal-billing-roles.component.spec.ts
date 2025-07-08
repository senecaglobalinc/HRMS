import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { InternalBillingRolesComponent } from './internal-billing-roles.component';

describe('InternalBillingRolesComponent', () => {
  let component: InternalBillingRolesComponent;
  let fixture: ComponentFixture<InternalBillingRolesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ InternalBillingRolesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(InternalBillingRolesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
