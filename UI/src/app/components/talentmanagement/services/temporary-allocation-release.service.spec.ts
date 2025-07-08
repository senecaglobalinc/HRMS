import { TestBed } from '@angular/core/testing';

import { TemporaryAllocationReleaseService } from './temporary-allocation-release.service';

describe('TemporaryAllocationReleaseService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: TemporaryAllocationReleaseService = TestBed.get(TemporaryAllocationReleaseService);
    expect(service).toBeTruthy();
  });
});
