import { TestBed } from '@angular/core/testing';

import { AssignReportingManagerService } from './assign-reporting-manager.service';

describe('AssignReportingManagerService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: AssignReportingManagerService = TestBed.get(AssignReportingManagerService);
    expect(service).toBeTruthy();
  });
});
