import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddKRAdlgComponent } from './add-kradlg.component';

describe('AddKRAdlgComponent', () => {
  let component: AddKRAdlgComponent;
  let fixture: ComponentFixture<AddKRAdlgComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddKRAdlgComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddKRAdlgComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
