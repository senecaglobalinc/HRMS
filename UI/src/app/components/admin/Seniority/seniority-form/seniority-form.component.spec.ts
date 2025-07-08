import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SeniorityFormComponent } from './seniority-form.component';

describe('SeniorityFormComponent', () => {
  let component: SeniorityFormComponent;
  let fixture: ComponentFixture<SeniorityFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SeniorityFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SeniorityFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
