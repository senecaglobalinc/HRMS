import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NotificationTypesFormComponent } from './notification-types-form.component';

describe('NotificationTypesFormComponent', () => {
  let component: NotificationTypesFormComponent;
  let fixture: ComponentFixture<NotificationTypesFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NotificationTypesFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NotificationTypesFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
