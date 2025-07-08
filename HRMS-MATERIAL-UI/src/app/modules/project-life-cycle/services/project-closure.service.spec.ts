import { TestBed } from '@angular/core/testing';

import { ProjectClosureService } from './project-closure.service';

describe('ProjectClosureService', () => {
  let service: ProjectClosureService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ProjectClosureService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
