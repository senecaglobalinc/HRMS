import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ResourceReportProjectComponent } from './resource-report-project.component';

describe('ResourceReportProjectComponent', () => {
  let component: ResourceReportProjectComponent;
  let fixture: ComponentFixture<ResourceReportProjectComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ResourceReportProjectComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ResourceReportProjectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
