import { TestBed } from '@angular/core/testing';

import { WfhAttendanceService } from './wfh-attendance.service';

describe('WfhAttendanceService', () => {
  let service: WfhAttendanceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(WfhAttendanceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
