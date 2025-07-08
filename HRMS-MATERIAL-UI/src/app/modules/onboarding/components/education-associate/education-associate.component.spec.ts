import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EducationAssociateComponent } from './education-associate.component';

describe('EducationAssociateComponent', () => {
  let component: EducationAssociateComponent;
  let fixture: ComponentFixture<EducationAssociateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EducationAssociateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EducationAssociateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
