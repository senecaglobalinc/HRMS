import { TestBed } from '@angular/core/testing';

import { ResouceReleaseService } from './resouce-release.service';

describe('ResouceReleaseService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ResouceReleaseService = TestBed.get(ResouceReleaseService);
    expect(service).toBeTruthy();
  });
});
