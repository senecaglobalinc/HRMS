import { TestBed } from '@angular/core/testing';

import { AdrOrganisationValueService } from './adr-organisation-value.service';

describe('AdrOrganisationValueService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: AdrOrganisationValueService = TestBed.get(AdrOrganisationValueService);
    expect(service).toBeTruthy();
  });
});
