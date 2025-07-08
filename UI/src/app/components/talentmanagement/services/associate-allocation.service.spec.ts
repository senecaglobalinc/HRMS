import { TestBed } from '@angular/core/testing';

import { AssociateAllocationService } from './associate-allocation.service';

describe('AssociateAllocation.Service.TsService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: AssociateAllocationService = TestBed.get(AssociateAllocationService);
    expect(service).toBeTruthy();
  });
});
