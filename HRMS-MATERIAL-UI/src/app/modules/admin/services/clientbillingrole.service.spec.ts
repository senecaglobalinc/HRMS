import { TestBed } from '@angular/core/testing';

import { ClientbillingroleService } from './clientbillingrole.service';

describe('ClientbillingroleService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ClientbillingroleService = TestBed.get(ClientbillingroleService);
    expect(service).toBeTruthy();
  });
});
