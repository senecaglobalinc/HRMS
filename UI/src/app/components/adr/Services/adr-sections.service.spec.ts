import { TestBed } from '@angular/core/testing';

import { AdrSectionsService } from './adr-sections.service';

describe('AdrSectionsService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: AdrSectionsService = TestBed.get(AdrSectionsService);
    expect(service).toBeTruthy();
  });
});
