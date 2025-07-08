import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddProspectiveAssociateComponent } from './add-prospective-associate.component';

describe('AddProspectiveAssociateComponent', () => {
  let component: AddProspectiveAssociateComponent;
  let fixture: ComponentFixture<AddProspectiveAssociateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddProspectiveAssociateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddProspectiveAssociateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
