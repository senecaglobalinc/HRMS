import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MapAssociateIdComponent } from './map-associate-id.component';

describe('MapAssociateIdComponent', () => {
  let component: MapAssociateIdComponent;
  let fixture: ComponentFixture<MapAssociateIdComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MapAssociateIdComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MapAssociateIdComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
