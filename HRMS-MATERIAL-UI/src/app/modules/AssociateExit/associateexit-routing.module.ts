import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AssociateExitComponent } from './Components/associate-exit/associate-exit.component';
import { AssociateResignationComponent } from './Components/associate-resignation/associate-resignation.component';
import { AssociateLongleaveComponent } from './Components/associate-longleave/associate-longleave.component';
import { AssociateAbscondFormComponent } from './Components/associate-abscond-form/associate-abscond-form.component';
import { KTPlanDashboardComponent } from './Components/ktplan-dashboard/ktplan-dashboard.component';
import { AssociateExitAnalysisComponent } from './Components/associate-exit-analysis/associate-exit-analysis.component';
import { AssociateExitInformationComponent } from './Components/associate-exit-information/associate-exit-information.component';
import { PmApprovalComponent } from './Components/pm-approval/pm-approval.component';
import { ClearenceformComponent } from './Components/clearenceform/clearenceform.component';
import { AssociateExitFeedbackFormComponent } from './Components/associate-exit-feedback-form/associate-exit-feedback-form.component';
import { ResignationComponent } from './Components/resignation/resignation.component';
import { ActivitylistComponent } from './Components/activitylist/activitylist.component';
import { AssociateExitByHrComponent } from './Components/associate-exit-by-hr/associate-exit-by-hr.component';
import { PMSubmissionComponent } from './Components/pm-submission/pm-submission.component';
import { AssociateExitFeedbackComponent } from './Components/associate-exit-feedback/associate-exit-feedback.component';
import { AbscondComponent } from './Components/abscond/abscond.component';
import { AbscondDashboardComponent } from './Components/abscond-dashboard/abscond-dashboard.component';
import { AbscondDetailsComponent } from './Components/abscond-details/abscond-details.component';

const routes: Routes = [

  {
    path: '', component: AssociateExitComponent, children: [
      { path: "resignation-legacy", component: AssociateResignationComponent, data: { title: 'Associate Resignation' } },
      { path: "longleave", component: AssociateLongleaveComponent, data: { title: 'Associate Long Leave' } },
      { path: 'resignation', component: ResignationComponent, data: { title: 'Resignation' }} ,
      { path: 'view/:id', component: ActivitylistComponent, data: { title: 'Associate Exit Checklist' }} ,
      { path: 'abscond/:id', component: ActivitylistComponent, data: { title: 'Associate Abscond Checklist' }} ,
      { path: 'exitfeedback/:id', component: AssociateExitFeedbackFormComponent, data: { title: 'Associate Exit Feedback' }} ,
      { path: 'deptchecklist/:id', component: ClearenceformComponent, data: { title: 'Associate Clearance ' } } ,
      { path: 'abscond-deptchecklist/:id', component: ClearenceformComponent, data: { title: 'Associate Department Checklist ' } } ,
      { path: 'pmApproval/:id', component: PmApprovalComponent, data: {title: 'Resignation Approval Screen'}},
      { path: 'PMSubmission/:id', component: PMSubmissionComponent, data: {title: 'Resignation Review'}},
      { path: 'reasonanalasis', component: AssociateExitAnalysisComponent, data: {title: 'Associate Exit Analysis'}},
      { path: 'associateexitinformation', component: AssociateExitInformationComponent, data: {title: 'Associate Exit Information'}},
      { path: "associateexitanalysis", component: AssociateExitAnalysisComponent, data: {title: 'Associate Exit Analysis'}},
      { path: "ktplan", component: KTPlanDashboardComponent, data: {title: 'Associate Exit Analysis'}},
      { path: "abscond-request", component: AbscondComponent, data: {title: 'Associate Abscond Form'}},
      { path: "abscond-dashboard", component: AbscondDashboardComponent, data: {title: 'Associate Abscond Requests'}},
      { path: "abscond-details/:id", component: AbscondDetailsComponent},
      { path: "seperationbyhr", component: AssociateAbscondFormComponent, data: {title: 'SBH'}},
      { path: "abscondform", component: AssociateAbscondFormComponent, data: {title: 'Abscond'}},
      { path: "termination", component: AssociateAbscondFormComponent, data: {title: 'Termination'}},
      { path: "associateexitbyhr", component:AssociateExitByHrComponent, data: {title: 'Associate Exit By HR'}},
      { path: "associateexitfeedback", component:AssociateExitFeedbackComponent, data: {title: 'Associate Exit Feedback Review'}},
    ]
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AssociateExitRoutingModule { }
