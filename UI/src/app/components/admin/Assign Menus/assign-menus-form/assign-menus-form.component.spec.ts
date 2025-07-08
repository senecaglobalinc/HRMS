import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AssignMenusFormComponent } from './assign-menus-form.component';

describe('AssignMenusFormComponent', () => {
  let component: AssignMenusFormComponent;
  let fixture: ComponentFixture<AssignMenusFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AssignMenusFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AssignMenusFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
