import { TestBed } from '@angular/core/testing';

import { ResourceReleaseService } from './resource-release.service';

describe('ResourceReleaseService', () => {
  let service: ResourceReleaseService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ResourceReleaseService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
