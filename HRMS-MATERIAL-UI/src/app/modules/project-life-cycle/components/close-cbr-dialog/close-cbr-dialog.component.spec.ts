import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CloseCbrDialogComponent } from './close-cbr-dialog.component';

describe('CloseCbrDialogComponent', () => {
  let component: CloseCbrDialogComponent;
  let fixture: ComponentFixture<CloseCbrDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CloseCbrDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CloseCbrDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
