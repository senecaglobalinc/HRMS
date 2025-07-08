import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DepartmentsFormComponent } from './departments-form.component';

describe('DepartmentsFormComponent', () => {
  let component: DepartmentsFormComponent;
  let fixture: ComponentFixture<DepartmentsFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DepartmentsFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DepartmentsFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
