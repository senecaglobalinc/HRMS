import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PerformAuthloginComponent } from './perform-authlogin.component';

describe('PerformAuthloginComponent', () => {
  let component: PerformAuthloginComponent;
  let fixture: ComponentFixture<PerformAuthloginComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PerformAuthloginComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PerformAuthloginComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
