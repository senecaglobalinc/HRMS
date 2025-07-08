import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewRegularizationAppliedAssociatesComponent } from './view-regularization-applied-associates.component';

describe('ViewRegularizationAppliedAssociatesComponent', () => {
  let component: ViewRegularizationAppliedAssociatesComponent;
  let fixture: ComponentFixture<ViewRegularizationAppliedAssociatesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ViewRegularizationAppliedAssociatesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewRegularizationAppliedAssociatesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
