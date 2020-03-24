import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AppToolbarControlsComponent } from './app-toolbar-controls.component';

describe('AppToolbarControlsComponent', () => {
  let component: AppToolbarControlsComponent;
  let fixture: ComponentFixture<AppToolbarControlsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AppToolbarControlsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AppToolbarControlsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
