import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AssignReportingManagerComponent } from './assign-reporting-manager.component';

describe('AssignReportingManagerComponent', () => {
  let component: AssignReportingManagerComponent;
  let fixture: ComponentFixture<AssignReportingManagerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AssignReportingManagerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AssignReportingManagerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
