import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AssociateExitAnalysisComponent } from './associate-exit-analysis.component';

describe('AssociateExitAnalysisComponent', () => {
  let component: AssociateExitAnalysisComponent;
  let fixture: ComponentFixture<AssociateExitAnalysisComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AssociateExitAnalysisComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AssociateExitAnalysisComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
