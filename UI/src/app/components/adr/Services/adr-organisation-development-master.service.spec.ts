import { TestBed } from '@angular/core/testing';

import { AdrOrganisationDevelopmentMasterService } from './adr-organisation-development-master.service';

describe('AdrOrganisationDevelopmentMasterService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: AdrOrganisationDevelopmentMasterService = TestBed.get(AdrOrganisationDevelopmentMasterService);
    expect(service).toBeTruthy();
  });
});
