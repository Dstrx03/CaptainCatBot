import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { HolddownActionIconComponent } from './holddown-action-icon.component';

describe('HolddownActionIconComponent', () => {
  let component: HolddownActionIconComponent;
  let fixture: ComponentFixture<HolddownActionIconComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ HolddownActionIconComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HolddownActionIconComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
