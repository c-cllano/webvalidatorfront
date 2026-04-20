import { TestBed } from '@angular/core/testing';

import { DocValidationService } from './doc-validation.service';

describe('DocValidationService', () => {
  let service: DocValidationService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DocValidationService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
