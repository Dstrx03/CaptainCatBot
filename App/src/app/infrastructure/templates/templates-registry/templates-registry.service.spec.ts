import { TestBed } from '@angular/core/testing';

import { TemplatesRegistryService } from './templates-registry.service';

describe('TemplatesRegistryService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: TemplatesRegistryService = TestBed.get(TemplatesRegistryService);
    expect(service).toBeTruthy();
  });
});
