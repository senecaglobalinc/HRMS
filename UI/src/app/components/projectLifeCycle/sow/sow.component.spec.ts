import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientModule } from '@angular/common/http';
import { ReactiveFormsModule } from '@angular/forms';
import { SOWComponent } from './sow.component';
import { AppPrimenNgModule } from '../../shared/module/primeng.module';
import { By } from '@angular/platform-browser';
import { SowService } from "../services/sow.service";
import { ProjectCreationService } from "../services/project-creation.service";
import { CommonService } from "../../../services/common.service";
import { RouterTestingModule } from '@angular/router/testing';

fdescribe('SOWComponent', () => {
  let component: SOWComponent;
  let fixture: ComponentFixture<SOWComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [AppPrimenNgModule, ReactiveFormsModule, HttpClientModule],
      providers: [ {
        provide: SowService,
        useValue: {}
      }
    ],
      // providers: [ CommonService, ProjectCreationService,SowService,RouterTestingModule],
      declarations: [ SOWComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SOWComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
