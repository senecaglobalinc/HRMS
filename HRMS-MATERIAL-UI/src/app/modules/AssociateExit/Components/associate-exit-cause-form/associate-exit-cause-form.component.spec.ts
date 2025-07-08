import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AssociateExitCauseFormComponent } from './associate-exit-cause-form.component';

describe('AssociateExitCauseFormComponent', () => {
  let component: AssociateExitCauseFormComponent;
  let fixture: ComponentFixture<AssociateExitCauseFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AssociateExitCauseFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AssociateExitCauseFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
