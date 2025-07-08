import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DomainReportComponent } from './domain-report.component';

describe('DomainReportComponent', () => {
  let component: DomainReportComponent;
  let fixture: ComponentFixture<DomainReportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DomainReportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DomainReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
