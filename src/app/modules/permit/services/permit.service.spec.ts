import { TestBed } from '@angular/core/testing';

import { PermitService } from './permit.service';

describe('permitService', () => {
  let service: PermitService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PermitService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
