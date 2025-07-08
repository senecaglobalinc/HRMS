import { TestBed } from '@angular/core/testing';

import { AssociateProjectHistoryService } from './associate-project-history.service';

describe('AssociateProjectHistoryService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: AssociateProjectHistoryService = TestBed.get(AssociateProjectHistoryService);
    expect(service).toBeTruthy();
  });
});
