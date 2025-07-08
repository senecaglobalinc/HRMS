import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AssignManagerToProjectComponent } from './assign-manager-to-project.component';

describe('AssignManagerToProjectComponent', () => {
  let component: AssignManagerToProjectComponent;
  let fixture: ComponentFixture<AssignManagerToProjectComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AssignManagerToProjectComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AssignManagerToProjectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
