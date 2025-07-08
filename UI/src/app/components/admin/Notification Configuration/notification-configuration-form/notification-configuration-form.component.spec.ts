import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NotificationConfigurationFormComponent } from './notification-configuration-form.component';

describe('NotificationConfigurationFormComponent', () => {
  let component: NotificationConfigurationFormComponent;
  let fixture: ComponentFixture<NotificationConfigurationFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NotificationConfigurationFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NotificationConfigurationFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
