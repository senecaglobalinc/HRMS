  import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { KraaspectComponent } from './kraaspect.component';

describe('KraaspectComponent', () => {
  let component: KraaspectComponent;
  let fixture: ComponentFixture<KraaspectComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ KraaspectComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(KraaspectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
