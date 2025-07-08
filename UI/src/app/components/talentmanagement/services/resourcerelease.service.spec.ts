import { TestBed } from '@angular/core/testing';

import { ResourceReleaseService } from './resourcerelease.service';

describe('ResourceReleaseService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ResourceReleaseService = TestBed.get(ResourceReleaseService);
    expect(service).toBeTruthy();
  });
});
