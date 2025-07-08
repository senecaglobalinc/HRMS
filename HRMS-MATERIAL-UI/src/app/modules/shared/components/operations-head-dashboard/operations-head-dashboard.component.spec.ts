import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OperationsHeadDashboardComponent } from './operations-head-dashboard.component';

describe('OperationsHeadDashboardComponent', () => {
  let component: OperationsHeadDashboardComponent;
  let fixture: ComponentFixture<OperationsHeadDashboardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OperationsHeadDashboardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OperationsHeadDashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
