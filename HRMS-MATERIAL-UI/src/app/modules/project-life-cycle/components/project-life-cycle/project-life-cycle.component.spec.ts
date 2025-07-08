import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectLifeCycleComponent } from './project-life-cycle.component';

describe('ProjectLifeCycleComponent', () => {
  let component: ProjectLifeCycleComponent;
  let fixture: ComponentFixture<ProjectLifeCycleComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProjectLifeCycleComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectLifeCycleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
