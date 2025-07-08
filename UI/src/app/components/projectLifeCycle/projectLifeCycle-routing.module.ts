import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ProjectComponent } from './project/project.component';
import { AssociateSkillSearchComponent } from './associateSkill-search-search/associateSkill-search.component';
import { CheckUsersGuard } from './check-users.guard';
import { ProjectDashboardComponent } from './project-dashboard/project-dashboard.component';
import { ViewProjectComponent } from './view-project/view-project.component';
import { AssignmanagertoprojectComponent } from './assignmanagertoproject/assignmanagertoproject.component';
import { AssignreportingmanagerComponent } from 'src/app/components/projectLifeCycle/assignreportingmanager/assignreportingmanager.component';
import { TagAssociateComponent } from './tag-associate/tag-associate.component';
import { ProjectLifeCycleComponent } from './projectLifeCycle.component';


const routes: Routes = [
    {
      path: "", component: ProjectLifeCycleComponent ,  
      children: [
        { path: 'addproject/:id', component: ProjectComponent , canActivate : [CheckUsersGuard] , },
        { path: 'dashboard', component: ProjectDashboardComponent ,  } ,  
        { path: 'view/:id', component: ViewProjectComponent } ,  
        { path: 'associateSkillSearch', component: AssociateSkillSearchComponent, canActivate: [CheckUsersGuard] },
        { path: 'tagAssociate', component: TagAssociateComponent },
        { path: 'assignmanagertoproject', component: AssignmanagertoprojectComponent, canActivate: [CheckUsersGuard] , },
        { path: 'assignreportingmanager', component: AssignreportingmanagerComponent, canActivate: [CheckUsersGuard] , },
      ]
    }
  ];
  
  @NgModule({
    imports: [RouterModule.forChild(routes)],
  })
  export class ProjectLifeCycleRoutingModule { }
