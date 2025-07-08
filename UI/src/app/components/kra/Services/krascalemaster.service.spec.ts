import { TestBed } from '@angular/core/testing';

import { KrascalemasterService } from './krascalemaster.service';

describe('KrascalemasterService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: KrascalemasterService = TestBed.get(KrascalemasterService);
    expect(service).toBeTruthy();
  });
});
