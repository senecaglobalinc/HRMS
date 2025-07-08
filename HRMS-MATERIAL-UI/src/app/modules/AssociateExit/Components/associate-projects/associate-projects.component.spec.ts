import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AssociateProjectsComponent } from './associate-projects.component';

describe('AssociateProjectsComponent', () => {
  let component: AssociateProjectsComponent;
  let fixture: ComponentFixture<AssociateProjectsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AssociateProjectsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AssociateProjectsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
