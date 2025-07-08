import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MapassociateroleComponent } from './mapassociaterole.component';

describe('MapassociateroleComponent', () => {
  let component: MapassociateroleComponent;
  let fixture: ComponentFixture<MapassociateroleComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MapassociateroleComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MapassociateroleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
