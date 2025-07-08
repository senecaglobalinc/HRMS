import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AssociateExitByHrComponent } from './associate-exit-by-hr.component';

describe('AssociateExitByHrComponent', () => {
  let component: AssociateExitByHrComponent;
  let fixture: ComponentFixture<AssociateExitByHrComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AssociateExitByHrComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AssociateExitByHrComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
