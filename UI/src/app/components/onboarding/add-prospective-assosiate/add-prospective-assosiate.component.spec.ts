import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddProspectiveAssosiateComponent } from './add-prospective-assosiate.component';

describe('AddProspectiveAssosiateComponent', () => {
  let component: AddProspectiveAssosiateComponent;
  let fixture: ComponentFixture<AddProspectiveAssosiateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddProspectiveAssosiateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddProspectiveAssosiateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
