import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AssociateAbscondFormComponent } from './associate-abscond-form.component';

describe('AssociateAbscondFormComponent', () => {
  let component: AssociateAbscondFormComponent;
  let fixture: ComponentFixture<AssociateAbscondFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AssociateAbscondFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AssociateAbscondFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
