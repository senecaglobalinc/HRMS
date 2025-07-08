import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CeocommentComponent } from './ceocomment.component';

describe('CeocommentComponent', () => {
  let component: CeocommentComponent;
  let fixture: ComponentFixture<CeocommentComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CeocommentComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CeocommentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
