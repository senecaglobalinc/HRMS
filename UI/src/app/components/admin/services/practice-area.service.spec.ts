import { TestBed } from '@angular/core/testing';

import { PracticeAreaService } from './practice-area.service';

describe('PracticeAreaService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: PracticeAreaService = TestBed.get(PracticeAreaService);
    expect(service).toBeTruthy();
  });
});
