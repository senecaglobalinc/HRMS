import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AbscondDetailsComponent } from './abscond-details.component';

describe('AbscondDetailsComponent', () => {
  let component: AbscondDetailsComponent;
  let fixture: ComponentFixture<AbscondDetailsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AbscondDetailsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AbscondDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
