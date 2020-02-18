import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SystemLoggingComponent } from './system-logging.component';

describe('SystemLoggingComponent', () => {
  let component: SystemLoggingComponent;
  let fixture: ComponentFixture<SystemLoggingComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SystemLoggingComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SystemLoggingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
