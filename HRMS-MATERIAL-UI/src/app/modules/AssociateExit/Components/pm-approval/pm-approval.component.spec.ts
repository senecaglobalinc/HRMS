import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PmApprovalComponent } from './pm-approval.component';

describe('PmApprovalComponent', () => {
  let component: PmApprovalComponent;
  let fixture: ComponentFixture<PmApprovalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PmApprovalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PmApprovalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
