import { TestBed } from '@angular/core/testing';

import { AssociateskillsearchService } from './associateskillsearch.service';

describe('AssociateskillsearchService', () => {
  let service: AssociateskillsearchService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AssociateskillsearchService);
  });

  
  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
