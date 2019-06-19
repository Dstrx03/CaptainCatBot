import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { InternalServicesComponent } from './internal-services.component';

describe('InternalServicesComponent', () => {
  let component: InternalServicesComponent;
  let fixture: ComponentFixture<InternalServicesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ InternalServicesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(InternalServicesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
