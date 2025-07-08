import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import 'hammerjs';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AppPrimenNgModule } from './module/primeng.module';
import { AppMaterialModule } from './module/material.module';
import { LoginComponent } from './login/login.component';
import { FooterComponent } from './footer/footer.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { FlexLayoutModule } from '@angular/flex-layout';
import { HttpModule } from '@angular/http';
import { SharedComponent } from './shared.component';
import { MenuListComponent } from './menu-list/menu-list.component';
import { ToolbarComponent } from './toolbar/toolbar.component';
import { AccessDeniedComponent } from './access-denied/access-denied.component';
import { ProgramManagerDashboardComponent } from './program-manager-dashboard/program-manager-dashboard.component';
import { DepartmentHeadDashboardComponent } from './department-head-dashboard/department-head-dashboard.component';
import { AuthTokenComponent } from './auth-token/auth-token.component';
import { ChooseRoleDialogComponent } from './choose-role-dialog/choose-role-dialog.component';
import { ChooseRoleComponent } from './choose-role/choose-role.component';
import { PerformAuthloginComponent } from './perform-authlogin/perform-authlogin.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    AppPrimenNgModule,
    AppMaterialModule,
    HttpModule,
    FlexLayoutModule,
    HttpModule
  ],
  declarations: [LoginComponent, ChooseRoleComponent, FooterComponent,  DashboardComponent, SharedComponent, MenuListComponent,ToolbarComponent, AccessDeniedComponent, ProgramManagerDashboardComponent, DepartmentHeadDashboardComponent, AuthTokenComponent, ChooseRoleDialogComponent, PerformAuthloginComponent ],
  exports: [ FooterComponent,ChooseRoleComponent, AppPrimenNgModule, DashboardComponent, LoginComponent, MenuListComponent,ToolbarComponent,AuthTokenComponent]
})
export class SharedModule { }
