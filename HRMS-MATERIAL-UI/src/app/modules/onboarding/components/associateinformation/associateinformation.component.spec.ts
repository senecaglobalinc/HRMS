import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AssociateinformationComponent } from './associateinformation.component';

describe('AssociateinformationComponent', () => {
  let component: AssociateinformationComponent;
  let fixture: ComponentFixture<AssociateinformationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AssociateinformationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AssociateinformationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
