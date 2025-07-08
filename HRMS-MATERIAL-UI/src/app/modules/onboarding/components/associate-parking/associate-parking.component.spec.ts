import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AssociateParkingComponent } from './associate-parking.component';

describe('AssociateParkingComponent', () => {
  let component: AssociateParkingComponent;
  let fixture: ComponentFixture<AssociateParkingComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AssociateParkingComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AssociateParkingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
