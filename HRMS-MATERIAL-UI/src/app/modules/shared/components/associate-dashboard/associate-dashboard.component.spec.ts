import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AssociateDashboardComponent } from './associate-dashboard.component';

describe('AssociateDashboardComponent', () => {
  let component: AssociateDashboardComponent;
  let fixture: ComponentFixture<AssociateDashboardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AssociateDashboardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AssociateDashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
