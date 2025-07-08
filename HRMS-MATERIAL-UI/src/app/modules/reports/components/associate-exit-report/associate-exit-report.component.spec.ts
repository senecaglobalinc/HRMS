import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AssociateExitReportComponent } from './associate-exit-report.component';

describe('AssociateExitReportComponent', () => {
  let component: AssociateExitReportComponent;
  let fixture: ComponentFixture<AssociateExitReportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AssociateExitReportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AssociateExitReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
