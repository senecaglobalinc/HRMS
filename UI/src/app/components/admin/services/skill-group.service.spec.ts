import { TestBed } from '@angular/core/testing';

import { SkillGroupService } from './skill-group.service';

describe('SkillGroupService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: SkillGroupService = TestBed.get(SkillGroupService);
    expect(service).toBeTruthy();
  });
});
