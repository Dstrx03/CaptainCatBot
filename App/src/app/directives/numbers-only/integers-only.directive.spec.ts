import { TestBed } from '@angular/core/testing';

import { NumbersOnlyDirective } from './numbers-only.directive';

describe('NumbersOnlyDirective', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const directive: NumbersOnlyDirective = TestBed.get(NumbersOnlyDirective);
    expect(directive).toBeTruthy();
  });
});