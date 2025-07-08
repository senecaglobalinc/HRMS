import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AssignUserRolesFormComponent } from './assign-user-roles-form.component';

describe('AssignUserRolesFormComponent', () => {
  let component: AssignUserRolesFormComponent;
  let fixture: ComponentFixture<AssignUserRolesFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AssignUserRolesFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AssignUserRolesFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
