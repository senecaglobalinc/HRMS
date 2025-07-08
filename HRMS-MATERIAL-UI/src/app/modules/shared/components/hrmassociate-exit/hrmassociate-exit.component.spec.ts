import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { HrmassociateExitComponent } from './hrmassociate-exit.component';

describe('HrmassociateExitComponent', () => {
  let component: HrmassociateExitComponent;
  let fixture: ComponentFixture<HrmassociateExitComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ HrmassociateExitComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HrmassociateExitComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
