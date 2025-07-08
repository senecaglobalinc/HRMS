import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdrOrganizationDevelopmentComponent } from './adr-organization-development.component';

describe('AdrOrganizationDevelopmentComponent', () => {
  let component: AdrOrganizationDevelopmentComponent;
  let fixture: ComponentFixture<AdrOrganizationDevelopmentComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdrOrganizationDevelopmentComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdrOrganizationDevelopmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
