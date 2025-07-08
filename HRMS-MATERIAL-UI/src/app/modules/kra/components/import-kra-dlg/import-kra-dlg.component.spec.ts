import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ImportKraDlgComponent } from './import-kra-dlg.component';

describe('ImportKraDlgComponent', () => {
  let component: ImportKraDlgComponent;
  let fixture: ComponentFixture<ImportKraDlgComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ImportKraDlgComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ImportKraDlgComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
