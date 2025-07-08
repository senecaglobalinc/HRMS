import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReportingManagerDashboardComponent } from './reporting-manager-dashboard.component';

describe('ReportingManagerDashboardComponent', () => {
  let component: ReportingManagerDashboardComponent;
  let fixture: ComponentFixture<ReportingManagerDashboardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ReportingManagerDashboardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReportingManagerDashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
