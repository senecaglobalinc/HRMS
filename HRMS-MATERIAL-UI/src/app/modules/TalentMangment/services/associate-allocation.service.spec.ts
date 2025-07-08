import { TestBed } from '@angular/core/testing';

import { AssociateAllocationService } from './associate-allocation.service';

describe('AssociateAllocationService', () => {
  let service: AssociateAllocationService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AssociateAllocationService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
