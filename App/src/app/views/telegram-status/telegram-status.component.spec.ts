import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TelegramStatusComponent } from './telegram-status.component';

describe('TelegramStatusComponent', () => {
  let component: TelegramStatusComponent;
  let fixture: ComponentFixture<TelegramStatusComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TelegramStatusComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TelegramStatusComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
