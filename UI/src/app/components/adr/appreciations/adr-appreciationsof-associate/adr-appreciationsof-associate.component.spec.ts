import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdrAppreciationsofAssociateComponent } from './adr-appreciationsof-associate.component';

describe('AdrAppreciationsofAssociateComponent', () => {
  let component: AdrAppreciationsofAssociateComponent;
  let fixture: ComponentFixture<AdrAppreciationsofAssociateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdrAppreciationsofAssociateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdrAppreciationsofAssociateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
