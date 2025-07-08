import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AssignRmToNonDeliveryComponent } from './assign-rm-to-non-delivery.component';

describe('AssignRmToNonDeliveryComponent', () => {
  let component: AssignRmToNonDeliveryComponent;
  let fixture: ComponentFixture<AssignRmToNonDeliveryComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AssignRmToNonDeliveryComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AssignRmToNonDeliveryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
