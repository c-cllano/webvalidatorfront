import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FlowCompletionComponent } from './flow-completion.component';

describe('FlowCompletionComponent', () => {
  let component: FlowCompletionComponent;
  let fixture: ComponentFixture<FlowCompletionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FlowCompletionComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FlowCompletionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
