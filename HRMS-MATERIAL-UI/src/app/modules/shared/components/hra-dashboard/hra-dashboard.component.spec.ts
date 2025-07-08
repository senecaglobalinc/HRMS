import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { HraDashboardComponent } from './hra-dashboard.component';

describe('HraDashboardComponent', () => {
  let component: HraDashboardComponent;
  let fixture: ComponentFixture<HraDashboardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ HraDashboardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HraDashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
