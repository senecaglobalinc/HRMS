import { TestBed } from '@angular/core/testing';

import { NotificationTypeService } from './notificationtype.service';

describe('NotificationtypeService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: NotificationTypeService = TestBed.get(NotificationTypeService);
    expect(service).toBeTruthy();
  });
});
