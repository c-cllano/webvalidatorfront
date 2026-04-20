import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AtdpComponent } from './atdp.component';

describe('AtdpComponent', () => {
  let component: AtdpComponent;
  let fixture: ComponentFixture<AtdpComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AtdpComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AtdpComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
