import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AssociateExitSummaryReportComponent } from './associate-exit-summary-report.component';

describe('AssociateExitSummaryReportComponent', () => {
  let component: AssociateExitSummaryReportComponent;
  let fixture: ComponentFixture<AssociateExitSummaryReportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AssociateExitSummaryReportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AssociateExitSummaryReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
