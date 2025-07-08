import { TestBed } from '@angular/core/testing';

import { DomainReportService } from './domain-report.service';

describe('DomainReportService', () => {
  let service: DomainReportService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DomainReportService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
