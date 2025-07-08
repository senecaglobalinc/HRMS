import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProgramManagerDashboardComponent } from './program-manager-dashboard.component';

describe('ProgramManagerDashboardComponent', () => {
  let component: ProgramManagerDashboardComponent;
  let fixture: ComponentFixture<ProgramManagerDashboardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProgramManagerDashboardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProgramManagerDashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
