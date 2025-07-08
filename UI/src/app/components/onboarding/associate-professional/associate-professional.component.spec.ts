import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AssociateProfessionalComponent } from './associate-professional.component';

describe('AssociateProfessionalComponent', () => {
  let component: AssociateProfessionalComponent;
  let fixture: ComponentFixture<AssociateProfessionalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AssociateProfessionalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AssociateProfessionalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
