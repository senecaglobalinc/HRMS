import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectDashboardComponent } from './project-dashboard.component';
import { AppPrimenNgModule } from '../../shared/module/primeng.module';

describe('ProjectDashboardComponent', () => {
  let component: ProjectDashboardComponent;
  let fixture: ComponentFixture<ProjectDashboardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProjectDashboardComponent , AppPrimenNgModule ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectDashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
