import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReviewKRAComponent } from './review-kra.component';

describe('ReviewKRAComponent', () => {
  let component: ReviewKRAComponent;
  let fixture: ComponentFixture<ReviewKRAComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ReviewKRAComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReviewKRAComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
