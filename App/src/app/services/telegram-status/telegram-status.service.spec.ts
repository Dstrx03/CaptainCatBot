import { TestBed } from '@angular/core/testing';

import { TelegramStatusService } from './telegram-status.service';

describe('TelegramStatusService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: TelegramStatusService = TestBed.get(TelegramStatusService);
    expect(service).toBeTruthy();
  });
});
