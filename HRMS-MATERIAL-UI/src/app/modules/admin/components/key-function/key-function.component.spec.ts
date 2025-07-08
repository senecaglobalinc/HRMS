import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { KeyFunctionComponent } from './key-function.component';

describe('KeyFunctionComponent', () => {
  let component: KeyFunctionComponent;
  let fixture: ComponentFixture<KeyFunctionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ KeyFunctionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(KeyFunctionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
