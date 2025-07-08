import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AssociateJoiningComponent } from './associate-joining.component';

describe('AssociateJoiningComponent', () => {
  let component: AssociateJoiningComponent;
  let fixture: ComponentFixture<AssociateJoiningComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AssociateJoiningComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AssociateJoiningComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
