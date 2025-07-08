import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FamilyAssociateComponent } from './family-associate.component';

describe('FamilyAssociateComponent', () => {
  let component: FamilyAssociateComponent;
  let fixture: ComponentFixture<FamilyAssociateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FamilyAssociateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FamilyAssociateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
