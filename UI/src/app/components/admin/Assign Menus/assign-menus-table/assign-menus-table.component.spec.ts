import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AssignMenusTableComponent } from './assign-menus-table.component';

describe('AssignMenusTableComponent', () => {
  let component: AssignMenusTableComponent;
  let fixture: ComponentFixture<AssignMenusTableComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AssignMenusTableComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AssignMenusTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
