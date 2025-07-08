import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewProjectTeamleadComponent } from './view-project-teamlead.component';

describe('ViewProjectTeamleadComponent', () => {
  let component: ViewProjectTeamleadComponent;
  let fixture: ComponentFixture<ViewProjectTeamleadComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ViewProjectTeamleadComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewProjectTeamleadComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
