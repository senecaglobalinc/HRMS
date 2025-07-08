import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AssociateReleaseComponent } from './associate-release.component';

describe('AssociateReleaseComponent', () => {
  let component: AssociateReleaseComponent;
  let fixture: ComponentFixture<AssociateReleaseComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AssociateReleaseComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AssociateReleaseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
