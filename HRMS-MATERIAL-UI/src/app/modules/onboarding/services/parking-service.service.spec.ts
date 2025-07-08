import { TestBed } from '@angular/core/testing';

import { ParkingServiceService } from './parking-service.service';

describe('ParkingServiceService', () => {
  let service: ParkingServiceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ParkingServiceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
