import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AssociateSkillSearchComponent } from './associateSkill-search.component';

describe('AssociateSkillSearchComponent', () => {
  let component: AssociateSkillSearchComponent;
  let fixture: ComponentFixture<AssociateSkillSearchComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AssociateSkillSearchComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AssociateSkillSearchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
