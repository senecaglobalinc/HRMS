import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { KraCommentsComponent } from './kra-comments.component';

describe('KraCommentsComponent', () => {
  let component: KraCommentsComponent;
  let fixture: ComponentFixture<KraCommentsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ KraCommentsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(KraCommentsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
