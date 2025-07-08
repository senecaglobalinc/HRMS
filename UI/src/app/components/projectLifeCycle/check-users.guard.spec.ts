import { TestBed, async, inject } from '@angular/core/testing';

import { CheckUsersGuard } from './check-users.guard';

describe('CheckUsersGuard', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [CheckUsersGuard]
    });
  });

  it('should ...', inject([CheckUsersGuard], (guard: CheckUsersGuard) => {
    expect(guard).toBeTruthy();
  }));
});
