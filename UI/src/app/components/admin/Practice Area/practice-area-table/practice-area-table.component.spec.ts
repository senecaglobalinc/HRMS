import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PracticeAreaTableComponent } from './practice-area-table.component';

describe('PracticeAreaTableComponent', () => {
  let component: PracticeAreaTableComponent;
  let fixture: ComponentFixture<PracticeAreaTableComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PracticeAreaTableComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PracticeAreaTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
