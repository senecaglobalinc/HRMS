import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectassociateComponent } from './projectassociate.component';

describe('ProjectassociateComponent', () => {
  let component: ProjectassociateComponent;
  let fixture: ComponentFixture<ProjectassociateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProjectassociateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectassociateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
