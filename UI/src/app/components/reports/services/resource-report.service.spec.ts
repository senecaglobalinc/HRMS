import { TestBed } from '@angular/core/testing';

import { ResourceReportService } from './resource-report.service';

describe('ResourceReportService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ResourceReportService = TestBed.get(ResourceReportService);
    expect(service).toBeTruthy();
  });
});
