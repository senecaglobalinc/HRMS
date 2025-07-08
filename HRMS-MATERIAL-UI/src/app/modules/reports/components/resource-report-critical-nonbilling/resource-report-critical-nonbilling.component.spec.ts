import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ResourceReportCriticalNonbillingComponent } from './resource-report-critical-nonbilling.component';

describe('ResourceReportCriticalNonbillingComponent', () => {
  let component: ResourceReportCriticalNonbillingComponent;
  let fixture: ComponentFixture<ResourceReportCriticalNonbillingComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ResourceReportCriticalNonbillingComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ResourceReportCriticalNonbillingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
