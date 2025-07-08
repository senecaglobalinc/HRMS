import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginComponent } from './modules/auth/components/login/login.component';
import { MasterLayoutComponent } from './modules/master-layout/components/master-layout/master-layout.component';
import { DEFAULT_ROUTES } from './routes/default-layout-routes';
import { Page404Component } from './modules/master-layout/components/page404/page404.component';
import { RolesComponent } from './modules/admin/components/roles/roles.component';
import { ChooseRoleComponent } from './modules/master-layout/components/choose-role/choose-role.component';
import { AuthCallbackComponent } from './modules/auth/components/auth-callback/auth-callback.component';


const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: '', component: MasterLayoutComponent, children: DEFAULT_ROUTES },
  { path: 'roles', component: ChooseRoleComponent },
  { path: 'auth-callback', component: AuthCallbackComponent },
  { path: '**', component: Page404Component },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
