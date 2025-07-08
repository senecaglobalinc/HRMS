import { TestBed } from '@angular/core/testing';

import { ResourceReportCriticalService } from './resource-report-critical.service';

describe('ResourceReportCriticalService', () => {
  let service: ResourceReportCriticalService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ResourceReportCriticalService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
