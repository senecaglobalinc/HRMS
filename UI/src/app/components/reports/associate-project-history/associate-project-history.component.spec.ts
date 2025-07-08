import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AssociateProjectHistoryComponent } from './associate-project-history.component';

describe('AssociateProjectHistoryComponent', () => {
  let component: AssociateProjectHistoryComponent;
  let fixture: ComponentFixture<AssociateProjectHistoryComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AssociateProjectHistoryComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AssociateProjectHistoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
