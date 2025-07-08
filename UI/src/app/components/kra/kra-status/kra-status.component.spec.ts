import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { KraStatusComponent } from './kra-status.component';

describe('KraStatusComponent', () => {
  let component: KraStatusComponent;
  let fixture: ComponentFixture<KraStatusComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ KraStatusComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(KraStatusComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
