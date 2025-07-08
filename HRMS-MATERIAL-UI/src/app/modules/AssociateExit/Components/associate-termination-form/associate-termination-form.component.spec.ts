import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AssociateTerminationFormComponent } from './associate-termination-form.component';

describe('AssociateTerminationFormComponent', () => {
  let component: AssociateTerminationFormComponent;
  let fixture: ComponentFixture<AssociateTerminationFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AssociateTerminationFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AssociateTerminationFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
