import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DailogTagListComponent } from './dailog-tag-list.component';

describe('DailogTagListComponent', () => {
  let component: DailogTagListComponent;
  let fixture: ComponentFixture<DailogTagListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DailogTagListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DailogTagListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
