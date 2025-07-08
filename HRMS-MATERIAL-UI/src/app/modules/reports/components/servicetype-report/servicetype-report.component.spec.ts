import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ServicetypeReportComponent } from './servicetype-report.component';

describe('ServicetypeReportComponent', () => {
  let component: ServicetypeReportComponent;
  let fixture: ComponentFixture<ServicetypeReportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ServicetypeReportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ServicetypeReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
