import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { AdminRoutingModule } from './admin-routing.module';
import { AdminComponent } from './components/admin/admin.component';
import { AssignMenusComponent } from './components/assign-menus/assign-menus.component';
import { AllAngularMaterialModule } from '../plugins/all-angular-material/all-angular-material.module';
import { AssignUserRolesComponent } from './components/assign-user-roles/assign-user-roles.component';
import { ClientBillingRolesComponent } from './components/client-billing-roles/client-billing-roles.component';
import { ClientsComponent } from './components/clients/clients.component';
import { CompetencyAreaComponent } from './components/competency-area/competency-area.component';
import { DepartmentsComponent } from './components/departments/departments.component';
import { DepartmentTypeComponent } from './components/department-type/department-type.component';
import { DesignationsComponent } from './components/designations/designations.component';
import { DomainComponent } from './components/domain/domain.component';
import { GradesComponent } from './components/grades/grades.component';
import { RoleTypeComponent } from './components/role-type/role-type.component';
import { InternalBillingRolesComponent } from './components/internal-billing-roles/internal-billing-roles.component';
import { KeyFunctionComponent } from './components/key-function/key-function.component';
import { MapAssociateIdComponent } from './components/map-associate-id/map-associate-id.component';
import { NotificationConfigurationComponent } from './components/notification-configuration/notification-configuration.component';
import { NotificationTypesComponent } from './components/notification-types/notification-types.component';
import { PracticeAreaComponent } from './components/practice-area/practice-area.component';
import { ProjectTypeComponent } from './components/project-type/project-type.component';
import { ProficiencyLevelComponent } from './components/proficiency-level/proficiency-level.component';
import { RolesTableComponent } from './components/roles-table/roles-table.component';
import { RolesComponent } from './components/roles/roles.component';
import { SkillGroupComponent } from './components/skill-group/skill-group.component';
import { SkillsComponent } from './components/skills/skills.component';
import { SeniorityComponent } from './components/seniority/seniority.component';
import { SpecialityComponent } from './components/speciality/speciality.component';
import { UpdateEmployeeStatusComponent } from './components/update-employee-status/update-employee-status.component';
import { WorkstationComponent } from './components/workstation/workstation.component';
import { CategoryComponent } from './components/category/category.component';

import { AngularEditorModule } from '@kolkov/angular-editor';
import { ServiceTypeComponent } from './components/service-type/service-type.component';
import { ValidationsDirectiveModule } from 'src/app/validations.directive';
import { MatSortModule } from '@angular/material/sort';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { GradesValidationsDirectiveModule } from 'src/app/grades-validations.directive';
import { PracticeAreaValidationsDirectiveModule } from 'src/app/practice-area-validations.directive';
import { DropDownSuggestionDirectiveModule } from 'src/app/drop-down-suggestion.directive';
import { RequirematchDirectiveModule } from 'src/app/requirematch.directive'
import { EmptySpacesValidationsDirectiveModule } from 'src/app/empty-spaces-validations.directive';
import { ValidationImpSpecialCharsDirectiveModule } from 'src/app/validation-imp-special-chars.directive';
import { NgxSpinnerModule } from 'ngx-spinner';

@NgModule({
  declarations: [AssignMenusComponent, AssignUserRolesComponent, ClientBillingRolesComponent, ClientsComponent, CompetencyAreaComponent, DepartmentsComponent, DepartmentTypeComponent, DesignationsComponent, DomainComponent, GradesComponent, RoleTypeComponent, InternalBillingRolesComponent, KeyFunctionComponent, MapAssociateIdComponent, NotificationConfigurationComponent, NotificationTypesComponent, PracticeAreaComponent, ProjectTypeComponent, ProficiencyLevelComponent, RolesTableComponent, RolesComponent, SkillGroupComponent, SkillsComponent, SeniorityComponent, SpecialityComponent, UpdateEmployeeStatusComponent, WorkstationComponent, CategoryComponent, AdminComponent, ServiceTypeComponent],
  imports: [
    CommonModule,
    AdminRoutingModule,
    AllAngularMaterialModule,
    FormsModule,
    ReactiveFormsModule,
    AngularEditorModule,
    ValidationsDirectiveModule,
    GradesValidationsDirectiveModule,
    PracticeAreaValidationsDirectiveModule,
    EmptySpacesValidationsDirectiveModule,
    ValidationImpSpecialCharsDirectiveModule,
    MatSortModule,
    MatProgressSpinnerModule,
    DropDownSuggestionDirectiveModule,
    RequirematchDirectiveModule,
    NgxSpinnerModule
  ]
})
export class AdminModule { }
