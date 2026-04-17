import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PwabannerComponent } from './pwabanner.component';

describe('PwabannerComponent', () => {
  let component: PwabannerComponent;
  let fixture: ComponentFixture<PwabannerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PwabannerComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PwabannerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
