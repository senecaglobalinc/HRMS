import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { KtplanDashboardComponent } from './ktplan-dashboard.component';

describe('KtplanDashboardComponent', () => {
  let component: KtplanDashboardComponent;
  let fixture: ComponentFixture<KtplanDashboardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ KtplanDashboardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(KtplanDashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
