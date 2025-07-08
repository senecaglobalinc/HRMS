import { TestBed } from '@angular/core/testing';

import { AssignRmForNonDeliveryService } from './assign-rm-for-non-delivery.service';

describe('AssignRmForNonDeliveryService', () => {
  let service: AssignRmForNonDeliveryService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AssignRmForNonDeliveryService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
