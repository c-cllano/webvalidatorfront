import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OrientationBlockComponent } from './orientation-block.component';

describe('OrientationBlockComponent', () => {
  let component: OrientationBlockComponent;
  let fixture: ComponentFixture<OrientationBlockComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [OrientationBlockComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(OrientationBlockComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
