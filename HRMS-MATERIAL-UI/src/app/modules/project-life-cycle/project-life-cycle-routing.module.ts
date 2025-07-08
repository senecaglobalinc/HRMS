import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { from } from 'rxjs';
import { ProjectLifeCycleComponent } from './components/project-life-cycle/project-life-cycle.component';
import { SowComponent } from './components/sow/sow.component';
import { ViewProjectComponent } from './components/view-project/view-project.component';
import { ProjectComponent } from './components/project/project.component';
import { ProjectDashboardComponent } from './components/project-dashboard/project-dashboard.component';
import { CheckUsersGuard } from './check-users.guard';
import { AssignManagerToProjectComponent } from './components/assign-manager-to-project/assign-manager-to-project.component';
import { AssignReportingManagerComponent } from './components/assign-reporting-manager/assign-reporting-manager.component';
import { AssociateskillsearchComponent } from './components/project/associateskillsearch/associateskillsearch.component';
import { ProjectClosureReportComponent } from './components/project-closure-report/project-closure-report.component';
import { ViewProjectTeamleadComponent } from './components/view-project-teamlead/view-project-teamlead.component';
import { PMClosureDashboardComponent } from './components/pm-closure-dashboard/pm-closure-dashboard.component';
import { ChecklistTabComponent } from './components/checklist-tab/checklist-tab.component';

const routes: Routes = [
    {
        path: '', component: ProjectLifeCycleComponent, children: [
          { path: 'view/:id', component: ViewProjectComponent, data: { title: 'View' } } ,
          { path: 'addproject/:id', component: ProjectComponent ,data: { title:'Add Project'}, canActivate : [CheckUsersGuard] , },
          { path: 'dashboard', component: ProjectDashboardComponent ,  } ,  


          { path: "associateSkillSearch", component: AssociateskillsearchComponent, data: { title: 'Associate Skill Search' } },

          

          { path: "assignmanagertoproject", component: AssignManagerToProjectComponent, data: { title: 'Assign Manager To Project' } },  
          { path: "assignreportingmanager", component: AssignReportingManagerComponent, data: { title: 'Assign Reporting Manager' } },  
          { path: "projectclosurereport", component: ProjectClosureReportComponent, data: { title: 'Project Closure Report' } },  
          { path: 'viewteamlead/:id', component: ViewProjectTeamleadComponent, data: { title: 'Project Closure Report' } },
          { path: "projectclosure/:id", component: PMClosureDashboardComponent, data: { title: 'Project Closure Dashboard' } },
          { path: 'checklist', component: ChecklistTabComponent, data: { title: 'Department Activities Report' },}

        ],
        
    },
    
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class ProjectLifeCycleRoutingModule { }