import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NonCriticalResourceLastBillingReportComponent } from './non-critical-resource-last-billing-report.component';

describe('NonCriticalResourceLastBillingReportComponent', () => {
  let component: NonCriticalResourceLastBillingReportComponent;
  let fixture: ComponentFixture<NonCriticalResourceLastBillingReportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NonCriticalResourceLastBillingReportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NonCriticalResourceLastBillingReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
