import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { InternalBillingRolestableComponent } from './internal-billing-roles-table.component';

describe('InternalBillingRolesListComponent', () => {
  let component: InternalBillingRolestableComponent;
  let fixture: ComponentFixture<InternalBillingRolestableComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ InternalBillingRolestableComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(InternalBillingRolestableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
