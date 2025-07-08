import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ResourceReportByProjectComponent } from './resource-report-by-project.component';

describe('ResourceReportByProjectComponent', () => {
  let component: ResourceReportByProjectComponent;
  let fixture: ComponentFixture<ResourceReportByProjectComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ResourceReportByProjectComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ResourceReportByProjectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
