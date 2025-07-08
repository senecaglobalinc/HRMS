import { TestBed } from '@angular/core/testing';

import { RolemasterService } from './rolemaster.service';

describe('RolemasterService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: RolemasterService = TestBed.get(RolemasterService);
    expect(service).toBeTruthy();
  });
});
