import { TestBed } from '@angular/core/testing';

import { AssociateExitDashboardService } from './associate-exit-dashboard.service';

describe('AssociateExitDashboardService', () => {
  let service: AssociateExitDashboardService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AssociateExitDashboardService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
