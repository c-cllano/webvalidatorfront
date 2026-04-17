import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PermitdeniedComponent } from './permitdenied.component';

describe('PermitdeniedComponent', () => {
  let component: PermitdeniedComponent;
  let fixture: ComponentFixture<PermitdeniedComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PermitdeniedComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PermitdeniedComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
