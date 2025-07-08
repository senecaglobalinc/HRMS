import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReasonFormComponent } from './reason-form.component';

describe('ReasonFormComponent', () => {
  let component: ReasonFormComponent;
  let fixture: ComponentFixture<ReasonFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ReasonFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReasonFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
