import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddKradefinitionComponent } from './add-kradefinition.component';

describe('AddKradefinitionComponent', () => {
  let component: AddKradefinitionComponent;
  let fixture: ComponentFixture<AddKradefinitionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddKradefinitionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddKradefinitionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
