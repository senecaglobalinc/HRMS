import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdrWorkplaceBehaviourComponent } from './adr-workplace-behaviour.component';

describe('AdrWorkplaceBehaviourComponent', () => {
  let component: AdrWorkplaceBehaviourComponent;
  let fixture: ComponentFixture<AdrWorkplaceBehaviourComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdrWorkplaceBehaviourComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdrWorkplaceBehaviourComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
