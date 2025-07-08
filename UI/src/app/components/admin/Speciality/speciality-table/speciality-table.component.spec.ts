import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SpecialityTableComponent } from './speciality-table.component';

describe('SpecialityTableComponent', () => {
  let component: SpecialityTableComponent;
  let fixture: ComponentFixture<SpecialityTableComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SpecialityTableComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SpecialityTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
