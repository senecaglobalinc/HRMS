import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdrAppreciateAssociateComponent } from './adr-appreciate-associate.component';

describe('AdrAppreciateAssociateComponent', () => {
  let component: AdrAppreciateAssociateComponent;
  let fixture: ComponentFixture<AdrAppreciateAssociateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdrAppreciateAssociateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdrAppreciateAssociateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
