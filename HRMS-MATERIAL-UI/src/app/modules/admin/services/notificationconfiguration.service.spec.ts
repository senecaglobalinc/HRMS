import { TestBed } from '@angular/core/testing';

import { NotificationConfigurationService } from './notificationconfiguration.service';

describe('NotificationconfigurationService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: NotificationConfigurationService = TestBed.get(NotificationConfigurationService);
    expect(service).toBeTruthy();
  });
});
