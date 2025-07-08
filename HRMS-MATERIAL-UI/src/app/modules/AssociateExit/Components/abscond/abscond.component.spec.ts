import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AbscondComponent } from './abscond.component';

describe('AbscondComponent', () => {
  let component: AbscondComponent;
  let fixture: ComponentFixture<AbscondComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AbscondComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AbscondComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
