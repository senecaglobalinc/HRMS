import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AssociateuploadComponent } from './associateupload.component';

describe('AssociateuploadComponent', () => {
  let component: AssociateuploadComponent;
  let fixture: ComponentFixture<AssociateuploadComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AssociateuploadComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AssociateuploadComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
