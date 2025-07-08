import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewDetailedStatusComponent } from './view-detailed-status.component';

describe('ViewDetailedStatusComponent', () => {
  let component: ViewDetailedStatusComponent;
  let fixture: ComponentFixture<ViewDetailedStatusComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ViewDetailedStatusComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewDetailedStatusComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
