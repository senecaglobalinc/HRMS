import { TestBed } from '@angular/core/testing';

import { SowService } from './sow.service';

describe('SowService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: SowService = TestBed.get(SowService);
    expect(service).toBeTruthy();
  });
});
