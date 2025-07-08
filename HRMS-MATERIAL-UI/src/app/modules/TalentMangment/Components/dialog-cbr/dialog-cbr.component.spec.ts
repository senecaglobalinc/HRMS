import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogCBRComponent } from './dialog-cbr.component';

describe('DialogCBRComponent', () => {
  let component: DialogCBRComponent;
  let fixture: ComponentFixture<DialogCBRComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogCBRComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogCBRComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
