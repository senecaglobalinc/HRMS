import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SkillGroupTableComponent } from './skill-group-table.component';

describe('SkillGroupTableComponent', () => {
  let component: SkillGroupTableComponent;
  let fixture: ComponentFixture<SkillGroupTableComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SkillGroupTableComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SkillGroupTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
