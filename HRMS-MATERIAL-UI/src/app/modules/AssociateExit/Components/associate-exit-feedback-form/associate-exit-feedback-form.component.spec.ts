import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AssociateExitFeedbackFormComponent } from './associate-exit-feedback-form.component';

describe('AssociateExitFeedbackFormComponent', () => {
  let component: AssociateExitFeedbackFormComponent;
  let fixture: ComponentFixture<AssociateExitFeedbackFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AssociateExitFeedbackFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AssociateExitFeedbackFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
