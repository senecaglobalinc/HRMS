import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ResourceReportByTechnologyComponent } from './resource-report-by-technology.component';

describe('ResourceReportByTechnologyComponent', () => {
  let component: ResourceReportByTechnologyComponent;
  let fixture: ComponentFixture<ResourceReportByTechnologyComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ResourceReportByTechnologyComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ResourceReportByTechnologyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
