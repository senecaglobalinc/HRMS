import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectHistoryListComponent } from './project-history-list.component';

describe('ProjectHistoryListComponent', () => {
  let component: ProjectHistoryListComponent;
  let fixture: ComponentFixture<ProjectHistoryListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProjectHistoryListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectHistoryListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
