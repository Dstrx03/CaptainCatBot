import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TelegramStatusMonitorComponent } from './telegram-status-monitor.component';

describe('TelegramStatusMonitorComponent', () => {
  let component: TelegramStatusMonitorComponent;
  let fixture: ComponentFixture<TelegramStatusMonitorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TelegramStatusMonitorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TelegramStatusMonitorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
