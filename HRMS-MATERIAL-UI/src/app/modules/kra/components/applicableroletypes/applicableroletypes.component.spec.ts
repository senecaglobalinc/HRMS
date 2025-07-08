import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ApplicableroletypesComponent } from './applicableroletypes.component';

describe('ApplicableroletypesComponent', () => {
  let component: ApplicableroletypesComponent;
  let fixture: ComponentFixture<ApplicableroletypesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ApplicableroletypesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ApplicableroletypesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
