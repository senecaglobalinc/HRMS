import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GenericErrorsMsgsComponent } from './generic-errors-msgs.component';

describe('GenericErrorsMsgsComponent', () => {
  let component: GenericErrorsMsgsComponent;
  let fixture: ComponentFixture<GenericErrorsMsgsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GenericErrorsMsgsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GenericErrorsMsgsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
