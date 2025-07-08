import { TestBed } from '@angular/core/testing';

import { AdrKraService } from './adr-kra.service';

describe('AdrKraService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: AdrKraService = TestBed.get(AdrKraService);
    expect(service).toBeTruthy();
  });
});
