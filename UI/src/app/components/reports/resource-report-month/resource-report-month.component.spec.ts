import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ResourceReportMonthComponent } from './resource-report-month.component';

describe('ResourceReportMonthComponent', () => {
  let component: ResourceReportMonthComponent;
  let fixture: ComponentFixture<ResourceReportMonthComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ResourceReportMonthComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ResourceReportMonthComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
