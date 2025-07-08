import { TestBed } from '@angular/core/testing';

import { DomainMasterService } from './domainmaster.service';

describe('DomainMasterService', () => {
  let service: DomainMasterService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DomainMasterService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
