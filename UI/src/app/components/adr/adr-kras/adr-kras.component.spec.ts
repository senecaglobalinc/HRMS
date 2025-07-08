import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdrKrasComponent } from './adr-kras.component';

describe('AdrKrasComponent', () => {
  let component: AdrKrasComponent;
  let fixture: ComponentFixture<AdrKrasComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdrKrasComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdrKrasComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
