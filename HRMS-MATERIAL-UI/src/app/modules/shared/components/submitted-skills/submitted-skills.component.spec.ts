import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SubmittedSkillsComponent } from './submitted-skills.component';

describe('SubmittedSkillsComponent', () => {
  let component: SubmittedSkillsComponent;
  let fixture: ComponentFixture<SubmittedSkillsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SubmittedSkillsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SubmittedSkillsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
