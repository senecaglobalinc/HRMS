import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TopNavComponent } from './components/top-nav/top-nav.component';
import { MenuListItemComponent, OrderByPipe } from './components/menu-list-item/menu-list-item.component';
import { MasterLayoutComponent } from './components/master-layout/master-layout.component';
import { AllAngularMaterialModule } from '../plugins/all-angular-material/all-angular-material.module';
import { FlexLayoutModule } from '@angular/flex-layout';
import {MasterRoutingModule} from './master-routing.module';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { Page404Component } from './components/page404/page404.component';
import { ChooseRoleComponent } from './components/choose-role/choose-role.component';
import { FormsModule } from '@angular/forms';
import { ConfirmCancelComponent } from './components/confirm-cancel/confirm-cancel.component';
import { ProfileDialogComponent } from './components/profile-dialog/profile-dialog.component';
import { NgxSpinnerModule } from 'ngx-spinner';
import  { SignoutDialogComponent } from './components/signout-dialog/signout-dialog.component'




@NgModule({
  declarations: [OrderByPipe, TopNavComponent, MenuListItemComponent, MasterLayoutComponent, DashboardComponent, Page404Component, ChooseRoleComponent, ConfirmCancelComponent, ProfileDialogComponent, SignoutDialogComponent],
  imports: [
    CommonModule,
	MasterRoutingModule,
    AllAngularMaterialModule,
    FlexLayoutModule,
    FormsModule,
    NgxSpinnerModule,
  ],
  exports:[
    MasterLayoutComponent,TopNavComponent,MenuListItemComponent,ConfirmCancelComponent
  ]
})
export class MasterLayoutModule { }
