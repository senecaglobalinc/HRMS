import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AssociateExitChecklistScreenComponent } from './associate-exit-checklist-screen.component';

describe('AssociateExitChecklistScreenComponent', () => {
  let component: AssociateExitChecklistScreenComponent;
  let fixture: ComponentFixture<AssociateExitChecklistScreenComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AssociateExitChecklistScreenComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AssociateExitChecklistScreenComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
