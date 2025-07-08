import { TestBed } from '@angular/core/testing';

import { KraAspectService } from './kra-aspect.service';



describe('KraAspectService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: KraAspectService = TestBed.get(KraAspectService);
    expect(service).toBeTruthy();
  });
});
