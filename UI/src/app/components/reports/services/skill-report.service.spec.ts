import { TestBed } from '@angular/core/testing';

import { SkillReportService } from './skill-report.service';

describe('SkillReportService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: SkillReportService = TestBed.get(SkillReportService);
    expect(service).toBeTruthy();
  });
});
