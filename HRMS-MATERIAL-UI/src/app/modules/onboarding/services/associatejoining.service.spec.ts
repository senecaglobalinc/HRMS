import { TestBed } from '@angular/core/testing';

import { AssociatejoiningService } from './associatejoining.service';

describe('AssociatejoiningService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: AssociatejoiningService = TestBed.get(AssociatejoiningService);
    expect(service).toBeTruthy();
  });
});
