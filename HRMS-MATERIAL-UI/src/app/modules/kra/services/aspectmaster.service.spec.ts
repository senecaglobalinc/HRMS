import { TestBed } from '@angular/core/testing';

import { AspectMasterService } from './aspectmaster.service';

describe('AspectmasterService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: AspectMasterService = TestBed.get(AspectMasterService);
    expect(service).toBeTruthy();
  });
});
