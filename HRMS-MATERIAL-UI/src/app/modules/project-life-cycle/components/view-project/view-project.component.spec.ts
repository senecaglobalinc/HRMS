import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewProjectComponent } from './view-project.component';
//import { AppPrimenNgModule } from '../../shared/module/primeng.module';
import { ProjectCreationService } from '../../services/project-creation.service';
import { ProjectsData } from '../../models/projects.model';
import { ClientBillingRoleService } from '../../services/client-billing-roles.service';
import * as moment from 'moment';
import { SowService } from '../../services/sow.service';
import { SOW } from '../../../admin/models/sow.model';

describe('ViewProjectComponent', () => {
  let component: ViewProjectComponent;
  let fixture: ComponentFixture<ViewProjectComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ViewProjectComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewProjectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
