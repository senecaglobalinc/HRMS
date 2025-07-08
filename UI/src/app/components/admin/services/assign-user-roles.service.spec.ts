import { TestBed } from '@angular/core/testing';

import { AssignUserRolesService } from './assign-user-roles.service';

describe('AssignUserRolesService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: AssignUserRolesService = TestBed.get(AssignUserRolesService);
    expect(service).toBeTruthy();
  });
});
