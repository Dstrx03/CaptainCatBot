import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewSystemLogDialogComponent } from './view-system-log-dialog.component';

describe('ViewSystemLogDialogComponent', () => {
  let component: ViewSystemLogDialogComponent;
  let fixture: ComponentFixture<ViewSystemLogDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ViewSystemLogDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewSystemLogDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
