import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CeostatusComponent } from './ceostatus.component';

describe('CeostatusComponent', () => {
  let component: CeostatusComponent;
  let fixture: ComponentFixture<CeostatusComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CeostatusComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CeostatusComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
