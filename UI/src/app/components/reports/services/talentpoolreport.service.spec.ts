import { TestBed } from '@angular/core/testing';

import { TalentpoolreportService } from './talentpoolreport.service';

describe('TalentpoolreportService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: TalentpoolreportService = TestBed.get(TalentpoolreportService);
    expect(service).toBeTruthy();
  });
});
