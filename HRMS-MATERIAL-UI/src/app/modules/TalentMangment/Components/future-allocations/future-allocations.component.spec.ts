import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FutureAllocationsComponent } from './future-allocations.component';

describe('FutureAllocationsComponent', () => {
  let component: FutureAllocationsComponent;
  let fixture: ComponentFixture<FutureAllocationsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FutureAllocationsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FutureAllocationsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
