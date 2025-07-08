import { TestBed } from '@angular/core/testing';

import { ResourceReportService } from './resource-report.service';

describe('ResourceReportService', () => {
  let service: ResourceReportService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ResourceReportService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
