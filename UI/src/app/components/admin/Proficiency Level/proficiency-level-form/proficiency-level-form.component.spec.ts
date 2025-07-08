import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProficiencyLevelFormComponent } from './proficiency-level-form.component';

describe('ProficiencyLevelFormComponent', () => {
  let component: ProficiencyLevelFormComponent;
  let fixture: ComponentFixture<ProficiencyLevelFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProficiencyLevelFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProficiencyLevelFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
