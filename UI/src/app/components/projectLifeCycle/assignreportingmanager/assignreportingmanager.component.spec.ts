import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AssignreportingmanagerComponent } from './assignreportingmanager.component';

describe('AssignreportingmanagerComponent', () => {
  let component: AssignreportingmanagerComponent;
  let fixture: ComponentFixture<AssignreportingmanagerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AssignreportingmanagerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AssignreportingmanagerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
