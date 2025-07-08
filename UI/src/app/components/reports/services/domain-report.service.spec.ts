import { TestBed } from '@angular/core/testing';

import { DomainReportService } from './domain-report.service';

describe('DomainReportService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: DomainReportService = TestBed.get(DomainReportService);
    expect(service).toBeTruthy();
  });
});
