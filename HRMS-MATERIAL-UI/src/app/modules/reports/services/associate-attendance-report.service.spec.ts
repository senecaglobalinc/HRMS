import { TestBed } from '@angular/core/testing';

import { AssociateAttendanceReportService } from './associate-attendance-report.service';

describe('AssociateAttendanceReportService', () => {
  let service: AssociateAttendanceReportService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AssociateAttendanceReportService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
