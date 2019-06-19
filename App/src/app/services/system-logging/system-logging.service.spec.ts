import { TestBed } from '@angular/core/testing';

import { SystemLoggingService } from './system-logging.service';

describe('SystemLoggingService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: SystemLoggingService = TestBed.get(SystemLoggingService);
    expect(service).toBeTruthy();
  });
});
