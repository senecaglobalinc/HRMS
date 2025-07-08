import { TestBed } from '@angular/core/testing';

import { FinanceReportService } from './finance-report.service';

describe('FinanceReportService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: FinanceReportService = TestBed.get(FinanceReportService);
    expect(service).toBeTruthy();
  });
});
