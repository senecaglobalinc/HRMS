import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RegulariationComponent } from './regulariation.component';

describe('RegulariationComponent', () => {
  let component: RegulariationComponent;
  let fixture: ComponentFixture<RegulariationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RegulariationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RegulariationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
