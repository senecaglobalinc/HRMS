import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewBiometricAttendanceComponent } from './view-biometric-attendance.component';

describe('ViewBiometricAttendanceComponent', () => {
  let component: ViewBiometricAttendanceComponent;
  let fixture: ComponentFixture<ViewBiometricAttendanceComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ViewBiometricAttendanceComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewBiometricAttendanceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
