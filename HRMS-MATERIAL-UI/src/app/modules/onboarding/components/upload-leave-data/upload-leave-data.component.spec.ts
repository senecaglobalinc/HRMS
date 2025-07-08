import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UploadLeaveDataComponent } from './upload-leave-data.component';

describe('UploadLeaveDataComponent', () => {
  let component: UploadLeaveDataComponent;
  let fixture: ComponentFixture<UploadLeaveDataComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UploadLeaveDataComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UploadLeaveDataComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
