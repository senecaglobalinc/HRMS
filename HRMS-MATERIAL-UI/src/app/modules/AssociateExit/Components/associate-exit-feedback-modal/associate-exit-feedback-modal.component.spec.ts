import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AssociateExitFeedbackModalComponent } from './associate-exit-feedback-modal.component';

describe('AssociateExitFeedbackModalComponent', () => {
  let component: AssociateExitFeedbackModalComponent;
  let fixture: ComponentFixture<AssociateExitFeedbackModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AssociateExitFeedbackModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AssociateExitFeedbackModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
