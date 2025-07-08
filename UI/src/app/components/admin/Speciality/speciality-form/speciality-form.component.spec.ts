import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SpecialityFormComponent } from './speciality-form.component';

describe('SpecialityFormComponent', () => {
  let component: SpecialityFormComponent;
  let fixture: ComponentFixture<SpecialityFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SpecialityFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SpecialityFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
