import { TestBed } from '@angular/core/testing';

import { TemporaryAllocationReleaseService } from './temporary-allocation-release.service';

describe('TemporaryAllocationReleaseService', () => {
  let service: TemporaryAllocationReleaseService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(TemporaryAllocationReleaseService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
