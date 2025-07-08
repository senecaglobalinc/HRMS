import { TestBed } from '@angular/core/testing';

import { PmApprovalService } from './pm-approval.service';

describe('PmApprovalService', () => {
  let service: PmApprovalService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PmApprovalService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
