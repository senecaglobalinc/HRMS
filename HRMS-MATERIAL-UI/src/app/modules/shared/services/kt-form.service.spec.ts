import { TestBed } from '@angular/core/testing';

import { KtFormService } from './kt-form.service';

describe('KtFormService', () => {
  let service: KtFormService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(KtFormService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
