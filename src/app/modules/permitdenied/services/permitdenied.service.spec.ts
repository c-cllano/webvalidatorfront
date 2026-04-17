import { TestBed } from '@angular/core/testing';

import { PermitdeniedService } from './permitdenied.service';

describe('PermitdeniedService', () => {
  let service: PermitdeniedService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PermitdeniedService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
