import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ManagerBiometricAttendanceReportComponent } from './manager-biometric-attendance-report.component';

describe('ManagerBiometricAttendanceReportComponent', () => {
  let component: ManagerBiometricAttendanceReportComponent;
  let fixture: ComponentFixture<ManagerBiometricAttendanceReportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ManagerBiometricAttendanceReportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ManagerBiometricAttendanceReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
