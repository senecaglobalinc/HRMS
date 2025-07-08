import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CheckUsersGuard } from './components/projectLifeCycle/check-users.guard';
import { ToolbarComponent } from './components/shared/toolbar/toolbar.component';
import { LoginComponent } from './components/shared/login/login.component';
import { GenericErrorsMsgsComponent } from './error/generic-errors-msgs/generic-errors-msgs.component';
import { DashboardComponent } from './components/shared/dashboard/dashboard.component';
import { AccessDeniedComponent } from './components/shared/access-denied/access-denied.component';
import { AuthTokenComponent } from './components/shared/auth-token/auth-token.component';
// import { ChooseRoleDialogComponent } from './components/shared/choose-role-dialog/choose-role-dialog.component';
import { SharedComponent } from './components/shared/shared.component';
import { PerformAuthloginComponent } from './components/shared/perform-authlogin/perform-authlogin.component';
import { ChooseRoleComponent } from './components/shared/choose-role/choose-role.component';


const routes: Routes = [
  { path: "login", component: LoginComponent },
  // { path: "authLogin", redirectTo:'authLogin/' },
  { path: "authLogin", component: AuthTokenComponent },
  { path: "perform-auth-login", component: PerformAuthloginComponent },
  // { path: "role", component: ChooseRoleDialogComponent },
  { path: "role", component: ChooseRoleComponent },
  { path: "errorPage", component: GenericErrorsMsgsComponent },
  {
    path: "",
    component: ToolbarComponent,
    children: [
      { path: 'admin', loadChildren: './components/admin/admin.module#AdminModule' },
      { path: 'project', loadChildren: './components/projectLifeCycle/projectLifeCycle.module#ProjectLifeCycleModule', },
      { path: 'associates', loadChildren: './components/onboarding/onboarding.module#OnBoardingModule' },
      { path: 'talentmanagement', loadChildren: './components/talentmanagement/talentmanagement.module#TalentManagementModule',  },
      { path: 'reports', loadChildren: './components/reports/reports.module#ReportsModule' },
      { path: 'kra', loadChildren: './components/kra/kra.module#KraModule' },
      { path: 'adr', loadChildren: './components/adr/adr.module#AdrModule' },
      {
        path: 'shared', component:SharedComponent ,
        children: [
          { path: 'dashboard', component: DashboardComponent },
          { path: 'accessdenied', component: AccessDeniedComponent }
        ]
      },
    ]
  },
];
@NgModule({
  imports: [RouterModule.forRoot(routes)],
})
export class AppRoutingModule { }
