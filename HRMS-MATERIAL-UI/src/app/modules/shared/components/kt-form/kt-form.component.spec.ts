import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { KtFormComponent } from './kt-form.component';

describe('KtFormComponent', () => {
  let component: KtFormComponent;
  let fixture: ComponentFixture<KtFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ KtFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(KtFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
