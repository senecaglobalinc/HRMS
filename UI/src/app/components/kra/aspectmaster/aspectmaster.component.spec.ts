import { async, ComponentFixture, TestBed } from '@angular/core/testing';
// import { APP_BASE_HREF } from '@angular/common';
import { AspectmasterComponent } from './aspectmaster.component';
import { HttpClientModule } from '@angular/common/http';
import { ReactiveFormsModule } from '@angular/forms';
import { AppPrimenNgModule } from '../../shared/module/primeng.module';
import { By } from '@angular/platform-browser';
import { AspectMasterService } from "../Services/aspectmaster.service";
import { RouterTestingModule } from '@angular/router/testing';
import { MessageService } from 'primeng/api';
import { ActivatedRoute } from '@angular/router';
import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/compiler/src/core';

describe('AspectmasterComponent', () => {
  let component: AspectmasterComponent;
  let fixture: ComponentFixture<AspectmasterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [ ReactiveFormsModule, HttpClientModule, RouterTestingModule],
       providers: [ {
         provide: AspectMasterService,
         useValue: {}
       }, MessageService,
      {
        provide: ActivatedRoute,
        useValue: {
          routeConfig: {
            component: {
              name: "Iron man"
            }
          }
        }
      }],
      declarations: [ AspectmasterComponent ],
      schemas:[CUSTOM_ELEMENTS_SCHEMA]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AspectmasterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
