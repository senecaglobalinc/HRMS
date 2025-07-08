import { TestBed } from '@angular/core/testing';

import { ProjectassosiateService } from './projectassosiate.service';

describe('ProjectassosiateService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ProjectassosiateService = TestBed.get(ProjectassosiateService);
    expect(service).toBeTruthy();
  });
});
