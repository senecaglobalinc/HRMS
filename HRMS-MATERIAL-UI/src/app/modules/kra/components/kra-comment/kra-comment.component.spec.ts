import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { KraCommentComponent } from './kra-comment.component';

describe('KraCommentComponent', () => {
  let component: KraCommentComponent;
  let fixture: ComponentFixture<KraCommentComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ KraCommentComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(KraCommentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
