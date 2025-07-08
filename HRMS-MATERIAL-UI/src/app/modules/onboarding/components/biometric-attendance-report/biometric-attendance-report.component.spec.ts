import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BiometricAttendanceReportComponent } from './biometric-attendance-report.component';

describe('BiometricAttendanceReportComponent', () => {
  let component: BiometricAttendanceReportComponent;
  let fixture: ComponentFixture<BiometricAttendanceReportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BiometricAttendanceReportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BiometricAttendanceReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
