import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CompetencyAreaTableComponent } from './competency-area-table.component';

describe('CompetencyAreaTableComponent', () => {
  let component: CompetencyAreaTableComponent;
  let fixture: ComponentFixture<CompetencyAreaTableComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CompetencyAreaTableComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CompetencyAreaTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
