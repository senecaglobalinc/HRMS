import { TestBed } from '@angular/core/testing';

import { TagAssociateService } from './tag-associate.service';

describe('TagAssociateService', () => {
  let service: TagAssociateService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(TagAssociateService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
