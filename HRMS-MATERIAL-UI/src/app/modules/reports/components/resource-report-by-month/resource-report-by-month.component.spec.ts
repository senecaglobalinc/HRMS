import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ResourceReportByMonthComponent } from './resource-report-by-month.component';

describe('ResourceReportByMonthComponent', () => {
  let component: ResourceReportByMonthComponent;
  let fixture: ComponentFixture<ResourceReportByMonthComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ResourceReportByMonthComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ResourceReportByMonthComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
