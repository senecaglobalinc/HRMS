import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewAssociateExitDetailsComponent } from './view-associate-exit-details.component';

describe('ViewAssociateExitDetailsComponent', () => {
  let component: ViewAssociateExitDetailsComponent;
  let fixture: ComponentFixture<ViewAssociateExitDetailsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ViewAssociateExitDetailsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewAssociateExitDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
