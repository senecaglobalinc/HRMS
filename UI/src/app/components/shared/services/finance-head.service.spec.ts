import { TestBed } from '@angular/core/testing';

import { FinanceHeadService } from './finance-head.service';

describe('FinanceHeadService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: FinanceHeadService = TestBed.get(FinanceHeadService);
    expect(service).toBeTruthy();
  });
});
