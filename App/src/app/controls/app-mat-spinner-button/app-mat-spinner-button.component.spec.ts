import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AppMatSpinnerButtonComponent } from './app-mat-spinner-button.component';

describe('AppMatSpinnerButtonComponent', () => {
  let component: AppMatSpinnerButtonComponent;
  let fixture: ComponentFixture<AppMatSpinnerButtonComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AppMatSpinnerButtonComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AppMatSpinnerButtonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
