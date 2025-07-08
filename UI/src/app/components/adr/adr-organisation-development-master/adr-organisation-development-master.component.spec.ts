import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { AdrOrganisationDevelopmentMasterComponent } from './adr-organisation-development-master.component';


describe('AdrOrganisationDevelopmentComponent', () => {
  let component: AdrOrganisationDevelopmentMasterComponent;
  let fixture: ComponentFixture<AdrOrganisationDevelopmentMasterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdrOrganisationDevelopmentMasterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdrOrganisationDevelopmentMasterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
