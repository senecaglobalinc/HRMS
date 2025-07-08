import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { KradialogsComponent } from './kradialogs.component';

describe('KradialogsComponent', () => {
  let component: KradialogsComponent;
  let fixture: ComponentFixture<KradialogsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ KradialogsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(KradialogsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
