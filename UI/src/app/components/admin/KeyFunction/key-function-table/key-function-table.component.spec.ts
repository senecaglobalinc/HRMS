import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { KeyFunctionTableComponent } from './key-function-table.component';

describe('KeyFunctionTableComponent', () => {
  let component: KeyFunctionTableComponent;
  let fixture: ComponentFixture<KeyFunctionTableComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ KeyFunctionTableComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(KeyFunctionTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
