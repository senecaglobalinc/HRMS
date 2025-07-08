import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AssociateEmploymentComponent } from './associate-employment.component';

describe('AssociateEmploymentComponent', () => {
  let component: AssociateEmploymentComponent;
  let fixture: ComponentFixture<AssociateEmploymentComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AssociateEmploymentComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AssociateEmploymentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
