import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ParkingSlotReportComponent } from './parking-slot-report.component';

describe('ParkingSlotReportComponent', () => {
  let component: ParkingSlotReportComponent;
  let fixture: ComponentFixture<ParkingSlotReportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ParkingSlotReportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ParkingSlotReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
