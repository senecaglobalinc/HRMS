import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SeniorityComponent } from './seniority.component';

describe('SeniorityComponent', () => {
  let component: SeniorityComponent;
  let fixture: ComponentFixture<SeniorityComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SeniorityComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SeniorityComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
