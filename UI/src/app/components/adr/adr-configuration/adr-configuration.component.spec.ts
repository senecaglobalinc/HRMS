import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdrConfigurationComponent } from './adr-configuration.component';

describe('AdrConfigurationComponent', () => {
  let component: AdrConfigurationComponent;
  let fixture: ComponentFixture<AdrConfigurationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdrConfigurationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdrConfigurationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
