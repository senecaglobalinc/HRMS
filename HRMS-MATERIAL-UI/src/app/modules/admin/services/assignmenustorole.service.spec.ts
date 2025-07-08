import { TestBed } from '@angular/core/testing';

import { AssignmenustoroleService } from './assignmenustorole.service';

describe('AssignmenustoroleService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: AssignmenustoroleService = TestBed.get(AssignmenustoroleService);
    expect(service).toBeTruthy();
  });
});
