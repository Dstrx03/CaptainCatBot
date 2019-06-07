import { TestBed } from '@angular/core/testing';

import { GridSchemesService } from './grid-schemes.service';

describe('GridSchemesService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: GridSchemesService = TestBed.get(GridSchemesService);
    expect(service).toBeTruthy();
  });
});
