import { TestBed } from '@angular/core/testing';

import { WorkstationService } from './workstation.service';

describe('WorkstationService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: WorkstationService = TestBed.get(WorkstationService);
    expect(service).toBeTruthy();
  });
});
