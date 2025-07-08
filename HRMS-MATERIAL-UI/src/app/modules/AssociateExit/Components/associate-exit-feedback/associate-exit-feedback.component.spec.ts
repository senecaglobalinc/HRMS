import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AssociateExitFeedbackComponent } from './associate-exit-feedback.component';

describe('AssociateExitFeedbackComponent', () => {
  let component: AssociateExitFeedbackComponent;
  let fixture: ComponentFixture<AssociateExitFeedbackComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AssociateExitFeedbackComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AssociateExitFeedbackComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
