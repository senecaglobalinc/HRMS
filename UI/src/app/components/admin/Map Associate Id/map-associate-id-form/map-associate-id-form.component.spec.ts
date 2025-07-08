import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MapAssociateIdFormComponent } from './map-associate-id-form.component';

describe('MapAssociateIdFormComponent', () => {
  let component: MapAssociateIdFormComponent;
  let fixture: ComponentFixture<MapAssociateIdFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MapAssociateIdFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MapAssociateIdFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
