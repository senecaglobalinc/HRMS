import { TestBed } from '@angular/core/testing';

import { ThemechangerService } from './themechanger.service';

describe('ThemechangerService', () => {
  let service: ThemechangerService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ThemechangerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
