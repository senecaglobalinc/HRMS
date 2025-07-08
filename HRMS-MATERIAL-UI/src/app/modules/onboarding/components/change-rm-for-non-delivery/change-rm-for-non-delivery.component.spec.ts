import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ChangeRmForNonDeliveryComponent } from './change-rm-for-non-delivery.component';

describe('ChangeRmForNonDeliveryComponent', () => {
  let component: ChangeRmForNonDeliveryComponent;
  let fixture: ComponentFixture<ChangeRmForNonDeliveryComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ChangeRmForNonDeliveryComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ChangeRmForNonDeliveryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
