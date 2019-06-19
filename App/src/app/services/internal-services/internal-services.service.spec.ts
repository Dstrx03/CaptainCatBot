import { TestBed } from '@angular/core/testing';

import { InternalServicesService } from './internal-services.service';

describe('InternalServicesService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: InternalServicesService = TestBed.get(InternalServicesService);
    expect(service).toBeTruthy();
  });
});
