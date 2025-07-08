import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DesignationsTableComponent } from './designations-table.component';

describe('DesignationsTableComponent', () => {
  let component: DesignationsTableComponent;
  let fixture: ComponentFixture<DesignationsTableComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DesignationsTableComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DesignationsTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
