import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewRegularizationAppliedDaysComponent } from './view-regularization-applied-days.component';

describe('ViewRegularizationAppliedDaysComponent', () => {
  let component: ViewRegularizationAppliedDaysComponent;
  let fixture: ComponentFixture<ViewRegularizationAppliedDaysComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ViewRegularizationAppliedDaysComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewRegularizationAppliedDaysComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
