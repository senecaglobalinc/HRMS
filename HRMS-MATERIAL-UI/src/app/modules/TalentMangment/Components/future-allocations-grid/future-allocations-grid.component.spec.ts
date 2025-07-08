import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FutureAllocationsGridComponent } from './future-allocations-grid.component';

describe('FutureAllocationsGridComponent', () => {
  let component: FutureAllocationsGridComponent;
  let fixture: ComponentFixture<FutureAllocationsGridComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FutureAllocationsGridComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FutureAllocationsGridComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
