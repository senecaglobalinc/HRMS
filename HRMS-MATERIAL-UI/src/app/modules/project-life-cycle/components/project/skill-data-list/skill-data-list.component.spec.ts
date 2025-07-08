import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SkillDataListComponent } from './skill-data-list.component';

describe('SkillDataListComponent', () => {
  let component: SkillDataListComponent;
  let fixture: ComponentFixture<SkillDataListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SkillDataListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SkillDataListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
