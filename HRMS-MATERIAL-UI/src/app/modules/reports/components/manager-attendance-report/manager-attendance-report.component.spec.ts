import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ManagerAttendanceReportComponent } from './manager-attendance-report.component';

describe('ManagerAttendanceReportComponent', () => {
  let component: ManagerAttendanceReportComponent;
  let fixture: ComponentFixture<ManagerAttendanceReportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ManagerAttendanceReportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ManagerAttendanceReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
