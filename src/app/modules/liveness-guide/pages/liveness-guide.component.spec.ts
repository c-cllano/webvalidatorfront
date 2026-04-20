import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LivenessGuideComponent } from './liveness-guide.component';

describe('FaceCaptureInstructionComponent', () => {
  let component: LivenessGuideComponent;
  let fixture: ComponentFixture<LivenessGuideComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LivenessGuideComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LivenessGuideComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
