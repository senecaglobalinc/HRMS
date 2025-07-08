import { TestBed } from '@angular/core/testing';

import { RoleTypeService } from './role-type.service';

describe('RoleTypeService', () => {
  let service: RoleTypeService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(RoleTypeService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
