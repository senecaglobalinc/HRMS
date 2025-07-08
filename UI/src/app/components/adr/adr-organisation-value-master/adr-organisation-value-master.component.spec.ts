import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdrOrganisationValueMasterComponent } from './adr-organisation-value-master.component';

describe('AdrOrganisationValueMasterComponent', () => {
  let component: AdrOrganisationValueMasterComponent;
  let fixture: ComponentFixture<AdrOrganisationValueMasterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdrOrganisationValueMasterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdrOrganisationValueMasterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
