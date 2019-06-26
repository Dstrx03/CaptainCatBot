import { TestBed } from '@angular/core/testing';

import { SignalrHubService } from './signalr-hub.service';

describe('SignalrHubService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: SignalrHubService = TestBed.get(SignalrHubService);
    expect(service).toBeTruthy();
  });
});
