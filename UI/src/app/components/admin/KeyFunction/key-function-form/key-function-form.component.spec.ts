import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { KeyFunctionFormComponent } from './key-function-form.component';

describe('KeyFunctionFormComponent', () => {
  let component: KeyFunctionFormComponent;
  let fixture: ComponentFixture<KeyFunctionFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ KeyFunctionFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(KeyFunctionFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
