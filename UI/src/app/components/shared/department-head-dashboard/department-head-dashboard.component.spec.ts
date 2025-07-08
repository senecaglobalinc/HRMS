import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DepartmentHeadDashboardComponent } from './department-head-dashboard.component';

describe('DepartmentHeadDashboardComponent', () => {
  let component: DepartmentHeadDashboardComponent;
  let fixture: ComponentFixture<DepartmentHeadDashboardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DepartmentHeadDashboardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DepartmentHeadDashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
