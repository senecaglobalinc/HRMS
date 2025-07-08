import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { InternalBillingRolesFormComponent } from './internal-billing-roles-form.component';

describe('InternalBillingRolesFormComponent', () => {
  let component: InternalBillingRolesFormComponent;
  let fixture: ComponentFixture<InternalBillingRolesFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ InternalBillingRolesFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(InternalBillingRolesFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
