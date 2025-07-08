import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewAbscondDetailedStatusComponent } from './view-abscond-detailed-status.component';

describe('ViewAbscondDetailedStatusComponent', () => {
  let component: ViewAbscondDetailedStatusComponent;
  let fixture: ComponentFixture<ViewAbscondDetailedStatusComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ViewAbscondDetailedStatusComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewAbscondDetailedStatusComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
