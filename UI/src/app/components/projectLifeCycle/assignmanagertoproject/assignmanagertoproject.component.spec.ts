import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AssignmanagertoprojectComponent } from './assignmanagertoproject.component';

describe('AssignmanagertoprojectComponent', () => {
  let component: AssignmanagertoprojectComponent;
  let fixture: ComponentFixture<AssignmanagertoprojectComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AssignmanagertoprojectComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AssignmanagertoprojectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
