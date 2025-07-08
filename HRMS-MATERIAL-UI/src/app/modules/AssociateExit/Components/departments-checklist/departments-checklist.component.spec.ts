import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DepartmentsChecklistComponent } from './departments-checklist.component';

describe('DepartmentsChecklistComponent', () => {
  let component: DepartmentsChecklistComponent;
  let fixture: ComponentFixture<DepartmentsChecklistComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DepartmentsChecklistComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DepartmentsChecklistComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
