import { TestBed } from '@angular/core/testing';

import { AdrcycleService } from './adrcycle.service';

describe('AdrcycleService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: AdrcycleService = TestBed.get(AdrcycleService);
    expect(service).toBeTruthy();
  });
});
