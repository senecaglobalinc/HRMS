import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AttendanceMusterReportComponent } from './attendance-muster-report.component';

describe('AttendanceMusterReportComponent', () => {
  let component: AttendanceMusterReportComponent;
  let fixture: ComponentFixture<AttendanceMusterReportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AttendanceMusterReportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AttendanceMusterReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
