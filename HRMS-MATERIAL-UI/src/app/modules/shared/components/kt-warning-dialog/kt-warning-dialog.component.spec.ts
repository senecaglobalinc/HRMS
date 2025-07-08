import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { KtWarningDialogComponent } from './kt-warning-dialog.component';

describe('KtWarningDialogComponent', () => {
  let component: KtWarningDialogComponent;
  let fixture: ComponentFixture<KtWarningDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ KtWarningDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(KtWarningDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
