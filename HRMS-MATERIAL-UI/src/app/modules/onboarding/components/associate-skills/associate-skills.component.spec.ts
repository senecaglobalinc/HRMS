import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AssociateSkillsComponent } from './associate-skills.component';

describe('AssociateSkillsComponent', () => {
  let component: AssociateSkillsComponent;
  let fixture: ComponentFixture<AssociateSkillsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AssociateSkillsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AssociateSkillsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
