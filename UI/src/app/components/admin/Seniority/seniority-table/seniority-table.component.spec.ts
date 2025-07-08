import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SeniorityTableComponent } from './seniority-table.component';

describe('SeniorityTableComponent', () => {
  let component: SeniorityTableComponent;
  let fixture: ComponentFixture<SeniorityTableComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SeniorityTableComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SeniorityTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
