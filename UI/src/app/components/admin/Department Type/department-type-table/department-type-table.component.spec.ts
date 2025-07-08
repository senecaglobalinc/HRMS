import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DepartmentTypeTableComponent } from './department-type-table.component';

describe('DepartmentTypeTableComponent', () => {
  let component: DepartmentTypeTableComponent;
  let fixture: ComponentFixture<DepartmentTypeTableComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DepartmentTypeTableComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DepartmentTypeTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
