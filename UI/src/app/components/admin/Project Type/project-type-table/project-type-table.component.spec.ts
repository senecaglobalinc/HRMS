import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectTypeTableComponent } from './project-type-table.component';

describe('ProjectTypeTableComponent', () => {
  let component: ProjectTypeTableComponent;
  let fixture: ComponentFixture<ProjectTypeTableComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProjectTypeTableComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectTypeTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
