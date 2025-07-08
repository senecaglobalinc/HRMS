import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AssociateallocationComponent } from './associateallocation.component';

describe('AssociateallocationComponent', () => {
  let component: AssociateallocationComponent;
  let fixture: ComponentFixture<AssociateallocationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AssociateallocationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AssociateallocationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
