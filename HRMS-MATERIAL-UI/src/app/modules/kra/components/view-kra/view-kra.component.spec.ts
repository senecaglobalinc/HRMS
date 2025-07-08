import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewKraComponent } from './view-kra.component';

describe('ViewKraComponent', () => {
  let component: ViewKraComponent;
  let fixture: ComponentFixture<ViewKraComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ViewKraComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewKraComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
