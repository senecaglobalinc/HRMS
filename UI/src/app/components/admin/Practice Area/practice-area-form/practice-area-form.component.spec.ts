import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PracticeAreaFormComponent } from './practice-area-form.component';

describe('PracticeAreaFormComponent', () => {
  let component: PracticeAreaFormComponent;
  let fixture: ComponentFixture<PracticeAreaFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PracticeAreaFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PracticeAreaFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
