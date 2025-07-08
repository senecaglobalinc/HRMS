import { TestBed } from '@angular/core/testing';

import { AdrConfigurationService } from './adr-configuration.service';

describe('AdrConfigurationService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: AdrConfigurationService = TestBed.get(AdrConfigurationService);
    expect(service).toBeTruthy();
  });
});
