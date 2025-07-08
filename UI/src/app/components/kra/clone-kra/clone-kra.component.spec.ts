import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CloneKraComponent } from './clone-kra.component';

describe('CloneKraComponent', () => {
  let component: CloneKraComponent;
  let fixture: ComponentFixture<CloneKraComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CloneKraComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CloneKraComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
