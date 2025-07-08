import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectTypeFormComponent } from './project-type-form.component';

describe('ProjectTypeFormComponent', () => {
  let component: ProjectTypeFormComponent;
  let fixture: ComponentFixture<ProjectTypeFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProjectTypeFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectTypeFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
