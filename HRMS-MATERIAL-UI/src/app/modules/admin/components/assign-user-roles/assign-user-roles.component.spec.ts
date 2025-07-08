import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AssignUserRolesComponent } from './assign-user-roles.component';

describe('AssignUserRolesComponent', () => {
  let component: AssignUserRolesComponent;
  let fixture: ComponentFixture<AssignUserRolesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AssignUserRolesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AssignUserRolesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
