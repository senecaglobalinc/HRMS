import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AttendanceReportDialogComponent } from './attendance-report-dialog.component';

describe('AttendanceReportDialogComponent', () => {
  let component: AttendanceReportDialogComponent;
  let fixture: ComponentFixture<AttendanceReportDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AttendanceReportDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AttendanceReportDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
