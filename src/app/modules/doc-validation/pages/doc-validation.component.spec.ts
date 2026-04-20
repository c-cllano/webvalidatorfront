import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DocValidationComponent } from './doc-validation.component';

describe('DocValidationComponent', () => {
  let component: DocValidationComponent;
  let fixture: ComponentFixture<DocValidationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DocValidationComponent]
    })
      .compileComponents();

    fixture = TestBed.createComponent(DocValidationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
