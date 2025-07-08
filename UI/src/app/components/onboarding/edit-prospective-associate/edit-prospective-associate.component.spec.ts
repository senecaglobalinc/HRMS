import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EditProspectiveAssociateComponent } from './edit-prospective-associate.component';

describe('EditProspectiveAssociateComponent', () => {
  let component: EditProspectiveAssociateComponent;
  let fixture: ComponentFixture<EditProspectiveAssociateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EditProspectiveAssociateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditProspectiveAssociateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
