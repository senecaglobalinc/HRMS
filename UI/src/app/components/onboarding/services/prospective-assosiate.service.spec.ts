import { TestBed } from '@angular/core/testing';

import { ProspectiveAssosiateService } from './prospective-assosiate.service';

describe('ProspectiveAssosiateService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ProspectiveAssosiateService = TestBed.get(ProspectiveAssosiateService);
    expect(service).toBeTruthy();
  });
});
