import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AssociateExitInformationComponent } from './associate-exit-information.component';

describe('AssociateExitInformationComponent', () => {
  let component: AssociateExitInformationComponent;
  let fixture: ComponentFixture<AssociateExitInformationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AssociateExitInformationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AssociateExitInformationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
