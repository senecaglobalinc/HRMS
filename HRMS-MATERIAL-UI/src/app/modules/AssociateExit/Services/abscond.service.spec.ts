import { TestBed } from '@angular/core/testing';

import { AbscondService } from './abscond.service';

describe('AbscondService', () => {
  let service: AbscondService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AbscondService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
