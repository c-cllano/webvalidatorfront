import { TestBed } from '@angular/core/testing';

import { UploadDocService } from './upload-doc.service';

describe('UploadDocService', () => {
  let service: UploadDocService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(UploadDocService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
