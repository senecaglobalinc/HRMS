import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientModule } from '@angular/common/http';
import { ReactiveFormsModule } from '@angular/forms';
import { SkillsComponent } from './skills.component';
import { AppPrimenNgModule } from '../../shared/module/primeng.module';
import { By } from '@angular/platform-browser';
import { SkillsService } from '../services/skills.service';
import { MasterDataService } from '../../../services/masterdata.service';
import { CommonService } from '../../../services/common.service';
import {  ActivatedRoute } from '@angular/router';


describe('SkillsComponent', () => {
  let component: SkillsComponent;
  let fixture: ComponentFixture<SkillsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [AppPrimenNgModule, ReactiveFormsModule, HttpClientModule],
      providers: [ SkillsService, MasterDataService,CommonService,ActivatedRoute ],
      declarations: [ SkillsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SkillsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
