import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DesignationsFormComponent } from './designations-form.component';

describe('DesignationsFormComponent', () => {
  let component: DesignationsFormComponent;
  let fixture: ComponentFixture<DesignationsFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DesignationsFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DesignationsFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
