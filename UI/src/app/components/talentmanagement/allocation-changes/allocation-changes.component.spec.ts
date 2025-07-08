import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AllocationChangesComponent } from './allocation-changes.component';

describe('AllocationChangesComponent', () => {
  let component: AllocationChangesComponent;
  let fixture: ComponentFixture<AllocationChangesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AllocationChangesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AllocationChangesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
