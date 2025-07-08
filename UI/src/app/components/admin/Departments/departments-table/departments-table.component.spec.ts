import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DepartmentsTableComponent } from './departments-table.component';

describe('DepartmentsTableComponent', () => {
  let component: DepartmentsTableComponent;
  let fixture: ComponentFixture<DepartmentsTableComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DepartmentsTableComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DepartmentsTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
