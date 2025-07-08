import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewAnalysisFormComponent } from './view-analysis-form.component';

describe('ViewAnalysisFormComponent', () => {
  let component: ViewAnalysisFormComponent;
  let fixture: ComponentFixture<ViewAnalysisFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ViewAnalysisFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewAnalysisFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
