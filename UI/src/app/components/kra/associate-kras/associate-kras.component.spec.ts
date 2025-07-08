import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AssociateKRAsComponent } from './associate-kras.component';

describe('AssociateKRAsComponent', () => {
  let component: AssociateKRAsComponent;
  let fixture: ComponentFixture<AssociateKRAsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AssociateKRAsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AssociateKRAsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
