import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AssociateExitReviewScreenComponent } from './associate-exit-review-screen.component';

describe('AssociateExitReviewScreenComponent', () => {
  let component: AssociateExitReviewScreenComponent;
  let fixture: ComponentFixture<AssociateExitReviewScreenComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AssociateExitReviewScreenComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AssociateExitReviewScreenComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
