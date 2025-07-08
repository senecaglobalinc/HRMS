import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WfhAttendanceComponent } from './wfh-attendance.component';

describe('WfhAttendanceComponent', () => {
  let component: WfhAttendanceComponent;
  let fixture: ComponentFixture<WfhAttendanceComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WfhAttendanceComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WfhAttendanceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
