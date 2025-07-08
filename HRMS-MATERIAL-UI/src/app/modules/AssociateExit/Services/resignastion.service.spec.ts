import { TestBed } from '@angular/core/testing';

import { ResignastionService } from './resignastion.service';

describe('ResignastionService', () => {
  let service: ResignastionService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ResignastionService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
