import { TestBed, async, inject } from '@angular/core/testing';

import { CheckLoggedUserIsAdminGuard } from './check-logged-user-is-admin.guard';

describe('CheckLoggedUserIsAdminGuard', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [CheckLoggedUserIsAdminGuard]
    });
  });

  it('should ...', inject([CheckLoggedUserIsAdminGuard], (guard: CheckLoggedUserIsAdminGuard) => {
    expect(guard).toBeTruthy();
  }));
});
