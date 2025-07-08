import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProspectiveAssociateComponent } from './prospective-associate.component';

describe('ProspectiveAssociateComponent', () => {
  let component: ProspectiveAssociateComponent;
  let fixture: ComponentFixture<ProspectiveAssociateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProspectiveAssociateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProspectiveAssociateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
