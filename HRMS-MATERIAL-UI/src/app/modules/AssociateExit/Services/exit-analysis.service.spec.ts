import { TestBed } from '@angular/core/testing';

import { ExitAnalysisService } from './exit-analysis.service';

describe('ExitAnalysisService', () => {
  let service: ExitAnalysisService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ExitAnalysisService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
