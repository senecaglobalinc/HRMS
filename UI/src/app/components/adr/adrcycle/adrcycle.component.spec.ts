import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdrcycleComponent } from './adrcycle.component';

describe('AdrcycleComponent', () => {
  let component: AdrcycleComponent;
  let fixture: ComponentFixture<AdrcycleComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdrcycleComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdrcycleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
