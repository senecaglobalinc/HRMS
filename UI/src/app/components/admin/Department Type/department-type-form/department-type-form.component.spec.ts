import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DepartmentTypeFormComponent } from './department-type-form.component';

describe('DepartmentTypeFormComponent', () => {
  let component: DepartmentTypeFormComponent;
  let fixture: ComponentFixture<DepartmentTypeFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DepartmentTypeFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DepartmentTypeFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
