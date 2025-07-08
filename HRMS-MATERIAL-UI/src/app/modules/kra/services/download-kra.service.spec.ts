import { TestBed } from '@angular/core/testing';

import { DownloadKraService } from './download-kra.service';

describe('DownloadKraService', () => {
  let service: DownloadKraService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DownloadKraService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
