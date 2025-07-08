import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TalentManagementComponent } from './talentmanagement.component';

describe('TalentmanagementComponent', () => {
  let component: TalentManagementComponent;
  let fixture: ComponentFixture<TalentManagementComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TalentManagementComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TalentManagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
