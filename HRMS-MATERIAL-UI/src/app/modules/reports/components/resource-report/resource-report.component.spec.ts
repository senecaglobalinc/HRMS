import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ResourceReportComponent } from './resource-report.component';

describe('ResourceReportComponent', () => {
  let component: ResourceReportComponent;
  let fixture: ComponentFixture<ResourceReportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ResourceReportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ResourceReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
