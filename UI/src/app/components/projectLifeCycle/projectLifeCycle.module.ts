import { NgModule } from '@angular/core';
import { ProjectComponent } from './project/project.component';
import { ProjectLifeCycleComponent } from './projectLifeCycle.component';
import { ProjectLifeCycleRoutingModule } from './projectLifeCycle-routing.module';
import { CommonModule } from '@angular/common';
import { ProjectCreationComponent } from './project-creation/project-creation.component';
import { SOWComponent } from './sow/sow.component';
import { ClientBillingRolesComponent } from './client-billing-roles/client-billing-roles.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ProjectDashboardComponent } from './project-dashboard/project-dashboard.component';
import { AppPrimenNgModule } from '../shared/module/primeng.module';
import { AssociateSkillSearchComponent } from './associateSkill-search-search/associateSkill-search.component';
import { ViewProjectComponent } from './view-project/view-project.component';
import { AssignmanagertoprojectComponent } from './assignmanagertoproject/assignmanagertoproject.component';
import { AssignreportingmanagerComponent } from './assignreportingmanager/assignreportingmanager.component';
import { TagAssociateComponent } from './tag-associate/tag-associate.component';



@NgModule({
    imports: [
        ProjectLifeCycleRoutingModule,
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        AppPrimenNgModule
    ],
    declarations: [
        ProjectComponent,
        ProjectLifeCycleComponent,
        ProjectCreationComponent,
        SOWComponent,
        TagAssociateComponent,
        ClientBillingRolesComponent,
        ProjectDashboardComponent,
        AssociateSkillSearchComponent,
        ViewProjectComponent,
        AssignmanagertoprojectComponent,
        AssignreportingmanagerComponent
    ],
    providers: []
})
export class ProjectLifeCycleModule { }