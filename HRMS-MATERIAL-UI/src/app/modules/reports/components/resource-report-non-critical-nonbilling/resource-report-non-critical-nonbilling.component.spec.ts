import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ResourceReportNonCriticalNonbillingComponent } from './resource-report-non-critical-nonbilling.component';

describe('ResourceReportNonCriticalNonbillingComponent', () => {
  let component: ResourceReportNonCriticalNonbillingComponent;
  let fixture: ComponentFixture<ResourceReportNonCriticalNonbillingComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ResourceReportNonCriticalNonbillingComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ResourceReportNonCriticalNonbillingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
