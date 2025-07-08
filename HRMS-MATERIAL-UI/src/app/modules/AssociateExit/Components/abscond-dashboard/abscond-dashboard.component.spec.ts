import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AbscondDashboardComponent } from './abscond-dashboard.component';

describe('AbscondDashboardComponent', () => {
  let component: AbscondDashboardComponent;
  let fixture: ComponentFixture<AbscondDashboardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AbscondDashboardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AbscondDashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
