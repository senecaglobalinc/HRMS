import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AssociateSkillDialogComponent } from './associate-skill-dialog.component';

describe('AssociateSkillDialogComponent', () => {
  let component: AssociateSkillDialogComponent;
  let fixture: ComponentFixture<AssociateSkillDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AssociateSkillDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AssociateSkillDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
