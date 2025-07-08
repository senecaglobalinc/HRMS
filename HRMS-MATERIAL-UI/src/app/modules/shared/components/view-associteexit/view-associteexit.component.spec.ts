import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewAssociteexitComponent } from './view-associteexit.component';

describe('ViewAssociteexitComponent', () => {
  let component: ViewAssociteexitComponent;
  let fixture: ComponentFixture<ViewAssociteexitComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ViewAssociteexitComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewAssociteexitComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
