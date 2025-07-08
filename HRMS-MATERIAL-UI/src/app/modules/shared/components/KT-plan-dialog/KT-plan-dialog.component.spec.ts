import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { KTPlanDialogComponent } from './KT-plan-dialog.component';

describe('KTPlanDialogComponent', () => {
  let component: KTPlanDialogComponent;
  let fixture: ComponentFixture<KTPlanDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ KTPlanDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(KTPlanDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
