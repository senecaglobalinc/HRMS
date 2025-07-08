import { TestBed } from '@angular/core/testing';

import { EmployeeStatusService } from './employeestatus.service';

describe('EmployeestatusService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: EmployeeStatusService = TestBed.get(EmployeeStatusService);
    expect(service).toBeTruthy();
  });
});
