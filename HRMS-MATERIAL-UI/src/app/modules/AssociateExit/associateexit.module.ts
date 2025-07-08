import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AllAngularMaterialModule } from '../plugins/all-angular-material/all-angular-material.module';
import { AssociateResignationComponent } from './Components/associate-resignation/associate-resignation.component';
import { AssociateLongleaveComponent } from './Components/associate-longleave/associate-longleave.component';
import { AssociateExitComponent } from './Components/associate-exit/associate-exit.component';
import {AssociateExitRoutingModule} from './associateexit-routing.module';
import { RequirematchDirectiveModule } from '../../../../src/app/requirematch.directive'
import { NgxSpinnerModule } from 'ngx-spinner';
import { ActivitylistComponent } from './Components/activitylist/activitylist.component';
import { AssociateAbscondFormComponent } from './Components/associate-abscond-form/associate-abscond-form.component';
import { AssociateExitAnalysisComponent } from './Components/associate-exit-analysis/associate-exit-analysis.component';
import { AssociateExitByHrComponent } from './Components/associate-exit-by-hr/associate-exit-by-hr.component';
import { AssociateExitCauseFormComponent } from './Components/associate-exit-cause-form/associate-exit-cause-form.component';
import { AssociateExitFeedbackFormComponent } from './Components/associate-exit-feedback-form/associate-exit-feedback-form.component';
import { AssociateExitInformationComponent } from './Components/associate-exit-information/associate-exit-information.component';
import { AssociateProjectsComponent } from './Components/associate-projects/associate-projects.component';
import { AssociateTerminationFormComponent } from './Components/associate-termination-form/associate-termination-form.component';
import { ClearenceformComponent } from './Components/clearenceform/clearenceform.component';
import { DepartmentsChecklistComponent } from './Components/departments-checklist/departments-checklist.component';
import { KTPlanDashboardComponent } from './Components/ktplan-dashboard/ktplan-dashboard.component';
import { PmApprovalComponent } from './Components/pm-approval/pm-approval.component';
import { ResignationComponent } from './Components/resignation/resignation.component';
import { RevokeDialogComponent } from './Components/revoke-dialog/revoke-dialog.component';
import { ViewAnalysisFormComponent } from './Components/view-analysis-form/view-analysis-form.component';
import { ViewAssociateExitDetailsComponent } from './Components/view-associate-exit-details/view-associate-exit-details.component';
import { AngularEditorModule } from '@kolkov/angular-editor';
import { PMSubmissionComponent } from './Components/pm-submission/pm-submission.component';
import { AssociateExitFeedbackComponent } from './Components/associate-exit-feedback/associate-exit-feedback.component';
import { AssociateExitFeedbackModalComponent } from './Components/associate-exit-feedback-modal/associate-exit-feedback-modal.component';
import { AbscondComponent } from './Components/abscond/abscond.component';
import { AbscondDashboardComponent } from './Components/abscond-dashboard/abscond-dashboard.component';
import { AbscondDetailsComponent } from './Components/abscond-details/abscond-details.component';
import { ViewAbscondDetailedStatusComponent } from './Components/view-abscond-detailed-status/view-abscond-detailed-status.component';

@NgModule({
  declarations: [AssociateResignationComponent, AssociateLongleaveComponent, AssociateExitComponent, ActivitylistComponent, AssociateAbscondFormComponent, AssociateExitAnalysisComponent, AssociateExitByHrComponent, AssociateExitCauseFormComponent, AssociateExitFeedbackFormComponent, AssociateExitInformationComponent, AssociateProjectsComponent, AssociateTerminationFormComponent, ClearenceformComponent, DepartmentsChecklistComponent, KTPlanDashboardComponent, PmApprovalComponent, ResignationComponent, RevokeDialogComponent, ViewAnalysisFormComponent, ViewAssociateExitDetailsComponent, PMSubmissionComponent, AssociateExitFeedbackComponent, AssociateExitFeedbackModalComponent, AbscondComponent, AbscondDashboardComponent, AbscondDetailsComponent, ViewAbscondDetailedStatusComponent],
  imports: [
    CommonModule,
    AssociateExitRoutingModule,
    AllAngularMaterialModule,
    FormsModule,
    ReactiveFormsModule,
    RequirematchDirectiveModule,
    NgxSpinnerModule,
    AngularEditorModule
  ]
})
export class AssociateexitModule { }
