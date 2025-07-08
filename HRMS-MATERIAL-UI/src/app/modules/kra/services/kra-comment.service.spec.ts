import { TestBed } from '@angular/core/testing';

import { KraCommentService } from './kra-comment.service';

describe('KraCommentService', () => {
  let service: KraCommentService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(KraCommentService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
