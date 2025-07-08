import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DefineKRAComponent } from './define-kra.component';

describe('DefineKRAComponent', () => {
  let component: DefineKRAComponent;
  let fixture: ComponentFixture<DefineKRAComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DefineKRAComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DefineKRAComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
