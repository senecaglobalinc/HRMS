import { TestBed } from '@angular/core/testing';

import { FileExporterService } from './file-exporter.service';

describe('FileExporterService', () => {
  let service: FileExporterService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(FileExporterService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
