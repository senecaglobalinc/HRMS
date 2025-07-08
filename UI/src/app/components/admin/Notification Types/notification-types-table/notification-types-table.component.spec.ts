import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NotificationTypesTableComponent } from './notification-types-table.component';

describe('NotificationTypesTableComponent', () => {
  let component: NotificationTypesTableComponent;
  let fixture: ComponentFixture<NotificationTypesTableComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NotificationTypesTableComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NotificationTypesTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
