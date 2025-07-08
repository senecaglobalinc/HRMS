import { TestBed } from '@angular/core/testing';

import { DepartmentTypeService } from './department-type.service';

describe('DepartmentTypeService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: DepartmentTypeService = TestBed.get(DepartmentTypeService);
    expect(service).toBeTruthy();
  });
});
