import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DepartmentTypeComponent } from './department-type.component';

describe('DepartmentTypeComponent', () => {
  let component: DepartmentTypeComponent;
  let fixture: ComponentFixture<DepartmentTypeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DepartmentTypeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DepartmentTypeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
