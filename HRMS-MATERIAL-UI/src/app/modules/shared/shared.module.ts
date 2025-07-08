import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FlexLayoutModule } from '@angular/flex-layout';
// import { HttpModule } from '@angular/http';
import { SharedComponent } from './shared.component';
import { SharedRoutingModule } from './shared-routing.module';
import { RouterModule } from '@angular/router';
import { DepartmentHeadDashboardComponent } from './components/department-head-dashboard/department-head-dashboard.component';
import { AllAngularMaterialModule } from '../plugins/all-angular-material/all-angular-material.module';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { ProgramManagerDashboardComponent } from './components/program-manager-dashboard/program-manager-dashboard.component';
import { RejectDialogComponent } from './components/reject-dialog/reject-dialog.component';
import { CommonDialogComponent } from './components/common-dialog/common-dialog.component';
import { TeamLeadDashboardComponent } from './components/team-lead-dashboard/team-lead-dashboard.component';
import { ProjectLifeCycleModule } from '../project-life-cycle/project-life-cycle.module';
import { ServiceManagerDashboardComponent } from './components/service-manager-dashboard/service-manager-dashboard.component';
//import { ChecklistTabComponent } from '../project-life-cycle/components/checklist-tab/checklist-tab.component';
import { NgxSpinnerModule } from 'ngx-spinner';
import { SubmittedSkillsComponent } from '../shared/components/submitted-skills/submitted-skills.component';
import { EditSkillDialogComponent } from './components/edit-skill-dialog/edit-skill-dialog.component';
import { AssociateDashboardComponent } from './components/associate-dashboard/associate-dashboard.component';
import { HraDashboardComponent } from './components/hra-dashboard/hra-dashboard.component';
import { HrmDashboardComponent } from './components/hrm-dashboard/hrm-dashboard.component';
import { KtFormComponent } from './components/kt-form/kt-form.component';
import { KTPlanDashboardComponent } from './components/ktplan-dashboard/ktplan-dashboard.component';
import { OperationsHeadDashboardComponent } from './components/operations-head-dashboard/operations-head-dashboard.component';
import { ReasonFormComponent } from './components/reason-form/reason-form.component';
import { HrmassociateExitComponent } from './components/hrmassociate-exit/hrmassociate-exit.component';
import { ViewAssociteexitComponent } from './components/view-associteexit/view-associteexit.component';
import { AssociateExitReviewScreenComponent } from './components/associate-exit-review-screen/associate-exit-review-screen.component';
import { AssociateExitChecklistScreenComponent } from './components/associate-exit-checklist-screen/associate-exit-checklist-screen.component';
import { CorporateDashboardComponent } from './components/corporate-dashboard/corporate-dashboard.component';
import { ConfirmationDialogComponent } from './components/confirmation-dialog/confirmation-dialog.component';
import { ViewDetailedStatusComponent } from './components/view-detailed-status/view-detailed-status.component';
import { DropDownSuggestionDirectiveModule } from '../../../app/drop-down-suggestion.directive';
import { RequirematchDirectiveModule } from  '../../../app/requirematch.directive';
import { KTPlanDialogComponent } from './components/KT-plan-dialog/KT-plan-dialog.component';
import {ReportingManagerDashboardComponent} from './components/reporting-manager-dashboard/reporting-manager-dashboard.component';
import { KtWarningDialogComponent } from './components/kt-warning-dialog/kt-warning-dialog.component';
import { OnboardingModule } from '../onboarding/onboarding.module';

@NgModule({
  declarations: [RejectDialogComponent,
    SharedComponent, DepartmentHeadDashboardComponent,
    DashboardComponent,ProgramManagerDashboardComponent, 
    CommonDialogComponent, TeamLeadDashboardComponent,
    ServiceManagerDashboardComponent, SubmittedSkillsComponent, 
    EditSkillDialogComponent, AssociateDashboardComponent, 
    HraDashboardComponent, HrmDashboardComponent, 
    KtFormComponent, KTPlanDashboardComponent, 
    OperationsHeadDashboardComponent, ReasonFormComponent, 
    HrmassociateExitComponent, ViewAssociteexitComponent, 
    AssociateExitReviewScreenComponent, AssociateExitChecklistScreenComponent, 
    CorporateDashboardComponent, ConfirmationDialogComponent, ViewDetailedStatusComponent, 
    KTPlanDialogComponent, ReportingManagerDashboardComponent, KtWarningDialogComponent],

  imports: [
    RouterModule,
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    FlexLayoutModule,
    // HttpModule,
    SharedRoutingModule,
    AllAngularMaterialModule,
    NgxSpinnerModule,
    DropDownSuggestionDirectiveModule,
    RequirematchDirectiveModule,
    OnboardingModule
  ],
  exports: [ProjectLifeCycleModule],
})
export class SharedModule {}
