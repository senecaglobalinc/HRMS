import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { KraScaleMasterComponent } from './kra-scale-master.component';

describe('KraScaleMasterComponent', () => {
  let component: KraScaleMasterComponent;
  let fixture: ComponentFixture<KraScaleMasterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ KraScaleMasterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(KraScaleMasterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
