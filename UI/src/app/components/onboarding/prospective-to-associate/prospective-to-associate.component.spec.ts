import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProspectiveToAssociateComponent } from './prospective-to-associate.component';

describe('ProspectiveToAssociateComponent', () => {
  let component: ProspectiveToAssociateComponent;
  let fixture: ComponentFixture<ProspectiveToAssociateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProspectiveToAssociateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProspectiveToAssociateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
