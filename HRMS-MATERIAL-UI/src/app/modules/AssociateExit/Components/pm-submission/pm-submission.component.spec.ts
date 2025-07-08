import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PMSubmissionComponent } from './pm-submission.component';

describe('PMSubmissionComponent', () => {
  let component: PMSubmissionComponent;
  let fixture: ComponentFixture<PMSubmissionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PMSubmissionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PMSubmissionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
