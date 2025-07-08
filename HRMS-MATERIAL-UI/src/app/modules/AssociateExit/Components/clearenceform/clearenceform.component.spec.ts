import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClearenceformComponent } from './clearenceform.component';

describe('ClearenceformComponent', () => {
  let component: ClearenceformComponent;
  let fixture: ComponentFixture<ClearenceformComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClearenceformComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClearenceformComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
