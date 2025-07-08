import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AssociateAttendanceReportComponent } from './associate-attendance-report.component';

describe('AssociateAttendanceReportComponent', () => {
  let component: AssociateAttendanceReportComponent;
  let fixture: ComponentFixture<AssociateAttendanceReportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AssociateAttendanceReportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AssociateAttendanceReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
