import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SkillGroupFormComponent } from './skill-group-form.component';

describe('SkillGroupFormComponent', () => {
  let component: SkillGroupFormComponent;
  let fixture: ComponentFixture<SkillGroupFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SkillGroupFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SkillGroupFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
