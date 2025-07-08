import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ChecklistTabComponent } from '../project-life-cycle/components/checklist-tab/checklist-tab.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { SharedComponent } from './shared.component';
import{TeamLeadDashboardComponent} from './components/team-lead-dashboard/team-lead-dashboard.component';
import {SubmittedSkillsComponent} from './components/submitted-skills/submitted-skills.component'
import { KtFormComponent } from './components/kt-form/kt-form.component';
import { AssociateExitReviewScreenComponent } from './components/associate-exit-review-screen/associate-exit-review-screen.component';

const routes: Routes = [
  {
    path: '',
    component: SharedComponent,
    children: [
      {
        path: 'dashboard',
        component: DashboardComponent,
        data: { title: 'Dashboard' },
      },
      {
        path: 'teamlead',
        component: TeamLeadDashboardComponent,
        data: { title: 'TeamLeadDashboard' },
      },
      {
        path: 'submitted-skills/:id', 
        component: SubmittedSkillsComponent,
        data: { title: 'Skill Review and Approval' },
      },
      { path: 'KtForm/TeamLead/:empid/:projectid',component: KtFormComponent, data: { title: 'Knowledge Transfer Handover Takeover' } },
      { path: 'KtForm/Associate/:empid/:projectid',component: KtFormComponent, data: { title: 'Knowledge Transfer Handover Takeover' } },
      { path: 'exit-actions', component:AssociateExitReviewScreenComponent},
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class SharedRoutingModule {}
