import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AllocatedResourcesDialogComponent } from './allocated-resources-dialog.component';

describe('AllocatedResourcesDialogComponent', () => {
  let component: AllocatedResourcesDialogComponent;
  let fixture: ComponentFixture<AllocatedResourcesDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AllocatedResourcesDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AllocatedResourcesDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
