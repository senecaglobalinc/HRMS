import { TestBed } from '@angular/core/testing';

import { ResourceReportNoncriticalService } from './resource-report-noncritical.service';

describe('ResourceReportNoncriticalService', () => {
  let service: ResourceReportNoncriticalService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ResourceReportNoncriticalService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
