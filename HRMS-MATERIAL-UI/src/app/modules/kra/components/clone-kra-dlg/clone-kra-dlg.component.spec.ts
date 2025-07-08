import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CloneKraDlgComponent } from './clone-kra-dlg.component';

describe('CloneKraDlgComponent', () => {
  let component: CloneKraDlgComponent;
  let fixture: ComponentFixture<CloneKraDlgComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CloneKraDlgComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CloneKraDlgComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
