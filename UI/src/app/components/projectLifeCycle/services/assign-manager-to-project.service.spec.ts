import { TestBed } from '@angular/core/testing';

import { AssignManagerToProjectService } from './assign-manager-to-project.service';

describe('AssignManagerToProjectService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: AssignManagerToProjectService = TestBed.get(AssignManagerToProjectService);
    expect(service).toBeTruthy();
  });
});
