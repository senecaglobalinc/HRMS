import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AssignUserRolesTableComponent } from './assign-user-roles-table.component';

describe('AssignUserRolesTableComponent', () => {
  let component: AssignUserRolesTableComponent;
  let fixture: ComponentFixture<AssignUserRolesTableComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AssignUserRolesTableComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AssignUserRolesTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
