import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AssociateskillsearchComponent } from './associateskillsearch.component';

describe('AssociateskillsearchComponent', () => {
  let component: AssociateskillsearchComponent;
  let fixture: ComponentFixture<AssociateskillsearchComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AssociateskillsearchComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AssociateskillsearchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
