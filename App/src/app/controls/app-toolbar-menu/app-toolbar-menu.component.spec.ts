import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AppToolbarMenuComponent } from './app-toolbar-menu.component';

describe('AppToolbarMenuComponent', () => {
  let component: AppToolbarMenuComponent;
  let fixture: ComponentFixture<AppToolbarMenuComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AppToolbarMenuComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AppToolbarMenuComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
