import { TestBed } from '@angular/core/testing';

import { AssociatekraService } from './associatekra.service';

describe('AssociatekraService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: AssociatekraService = TestBed.get(AssociatekraService);
    expect(service).toBeTruthy();
  });
});
