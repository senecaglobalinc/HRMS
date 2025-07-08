import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RmgArchiveReportComponent } from './rmg-archive-report.component';

describe('RmgArchiveReportComponent', () => {
  let component: RmgArchiveReportComponent;
  let fixture: ComponentFixture<RmgArchiveReportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RmgArchiveReportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RmgArchiveReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
