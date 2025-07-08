import { TestBed } from '@angular/core/testing';

import { DeliveryHeadService } from './delivery-head.service';

describe('DeliveryHeadService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: DeliveryHeadService = TestBed.get(DeliveryHeadService);
    expect(service).toBeTruthy();
  });
});
