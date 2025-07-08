import { TestBed } from '@angular/core/testing';

import { ProspectiveassociateService } from './prospectiveassociate.service';

describe('ProspectiveassociateService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ProspectiveassociateService = TestBed.get(ProspectiveassociateService);
    expect(service).toBeTruthy();
  });
});
