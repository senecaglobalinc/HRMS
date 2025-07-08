import { TestBed } from '@angular/core/testing';

import { ProficiencyLevelService } from './proficiency-level.service';

describe('ProficiencyLevelService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ProficiencyLevelService = TestBed.get(ProficiencyLevelService);
    expect(service).toBeTruthy();
  });
});
