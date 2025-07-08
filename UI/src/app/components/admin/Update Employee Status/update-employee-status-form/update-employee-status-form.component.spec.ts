import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UpdateEmployeeStatusFormComponent } from './update-employee-status-form.component';

describe('UpdateEmployeeStatusFormComponent', () => {
  let component: UpdateEmployeeStatusFormComponent;
  let fixture: ComponentFixture<UpdateEmployeeStatusFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UpdateEmployeeStatusFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UpdateEmployeeStatusFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
