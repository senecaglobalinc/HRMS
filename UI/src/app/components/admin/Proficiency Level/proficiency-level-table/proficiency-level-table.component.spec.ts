import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProficiencyLevelTableComponent } from './proficiency-level-table.component';

describe('ProficiencyLevelTableComponent', () => {
  let component: ProficiencyLevelTableComponent;
  let fixture: ComponentFixture<ProficiencyLevelTableComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProficiencyLevelTableComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProficiencyLevelTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
