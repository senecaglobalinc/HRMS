import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BiometricAttendanceFoundDialogComponent } from './biometric-attendance-found-dialog.component';

describe('BiometricAttendanceFoundDialogComponent', () => {
  let component: BiometricAttendanceFoundDialogComponent;
  let fixture: ComponentFixture<BiometricAttendanceFoundDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BiometricAttendanceFoundDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BiometricAttendanceFoundDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
