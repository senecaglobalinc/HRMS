import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SowComponent } from './components/sow/sow.component';
import { ClientBillingRoleComponent } from './components/client-billing-role/client-billing-role.component';
import { ProjectLifeCycleComponent } from './components/project-life-cycle/project-life-cycle.component';
import { ViewProjectComponent } from './components/view-project/view-project.component';
import { AllAngularMaterialModule } from '../plugins/all-angular-material/all-angular-material.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FlexLayoutModule } from '@angular/flex-layout';
import { ProjectLifeCycleRoutingModule } from './project-life-cycle-routing.module';
import { RouterModule } from '@angular/router';
import { ProjectComponent } from './components/project/project.component';
import { ProjectCreationComponent } from './components/project-creation/project-creation.component';
import { ProjectDashboardComponent } from './components/project-dashboard/project-dashboard.component';
import { DateDialogComponent } from './components/date-dialog/date-dialog.component';
import { ConfirmDialogComponent } from './components/confirm-dialog/confirm-dialog.component';
import { AssignManagerToProjectComponent } from './components/assign-manager-to-project/assign-manager-to-project.component';
import { AssignReportingManagerComponent } from './components/assign-reporting-manager/assign-reporting-manager.component';
import { CloseCbrDialogComponent } from './components/close-cbr-dialog/close-cbr-dialog.component';
import { MessageDialogComponent } from './components/message-dialog/message-dialog.component';
import { AllocatedResourcesDialogComponent } from './components/allocated-resources-dialog/allocated-resources-dialog.component';
import { AssociateskillsearchComponent } from './components/project/associateskillsearch/associateskillsearch.component';
import { DailogTagListComponent } from './components/project/dailog-tag-list/dailog-tag-list.component';
// import { ViewTagAssociateComponent } from './components/project/view-tag-associate/view-tag-associate.component';
import { SkillDataListComponent } from './components/project/skill-data-list/skill-data-list.component';
import { DeleteDialogComponent } from './components/delete-dialog/delete-dialog.component';
import { ProjectClosureReportComponent } from './components/project-closure-report/project-closure-report.component';
import { AngularEditorModule } from '@kolkov/angular-editor';
import { ViewProjectTeamleadComponent } from './components/view-project-teamlead/view-project-teamlead.component';
import { PMClosureDashboardComponent } from './components/pm-closure-dashboard/pm-closure-dashboard.component';
import { NgxSpinnerModule } from 'ngx-spinner';
import { ChecklistTabComponent } from './components/checklist-tab/checklist-tab.component';
import { ProjectDetailsComponent } from './components/project-details/project-details.component';
import { RemarksTeamleadComponent } from './components/remarks-teamlead.component';
import { DropDownSuggestionDirectiveModule } from 'src/app/drop-down-suggestion.directive';
import { RequirematchDirectiveModule } from 'src/app/requirematch.directive';
import { EmptySpacesValidationsDirectiveModule } from 'src/app/empty-spaces-validations.directive';
import { ProjectHistoryListComponent } from './components/project/project-history-list/project-history-list.component';


@NgModule({
  declarations: [ConfirmDialogComponent,AssignReportingManagerComponent,DateDialogComponent,ViewProjectComponent,ProjectDashboardComponent,ProjectCreationComponent, SowComponent, ClientBillingRoleComponent, ProjectLifeCycleComponent, ProjectComponent, AssignManagerToProjectComponent, CloseCbrDialogComponent, MessageDialogComponent, AllocatedResourcesDialogComponent, AssociateskillsearchComponent, DailogTagListComponent,  SkillDataListComponent,DeleteDialogComponent, ProjectClosureReportComponent, ViewProjectTeamleadComponent, PMClosureDashboardComponent, ChecklistTabComponent, ProjectDetailsComponent, RemarksTeamleadComponent, ProjectHistoryListComponent],
  imports: [
    RouterModule,
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    FlexLayoutModule,
    ProjectLifeCycleRoutingModule,
    AllAngularMaterialModule,
    AngularEditorModule,
    NgxSpinnerModule,
    DropDownSuggestionDirectiveModule,
    RequirematchDirectiveModule,
    EmptySpacesValidationsDirectiveModule    
  ],
  exports: [ProjectClosureReportComponent],
  
})
export class ProjectLifeCycleModule { }
