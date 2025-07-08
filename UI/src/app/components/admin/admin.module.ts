import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { AdminRoutingModule } from "./admin-routing.module";
import { FormsModule, ReactiveFormsModule  }   from '@angular/forms';

import { InternalBillingRolesFormComponent } from "./Internal Billing Roles/internal-billing-roles-form/internal-billing-roles-form.component";
import { InternalBillingRolestableComponent } from "./Internal Billing Roles/internal-billing-roles-table/internal-billing-roles-table.component";
import { InternalBillingRolesDirective } from "./Internal Billing Roles/internal-billing-roles.directive";
import { MapAssociateIdFormComponent } from "./Map Associate Id/map-associate-id-form/map-associate-id-form.component";

import { NotificationConfigurationFormComponent } from "./Notification Configuration/notification-configuration-form/notification-configuration-form.component";

import { AssignMenusTableComponent } from "./Assign Menus/assign-menus-table/assign-menus-table.component";
import { AssignMenusFormComponent } from "./Assign Menus/assign-menus-form/assign-menus-form.component";
import { AssignUserRolesFormComponent } from "./Assign User Roles/assign-user-roles-form/assign-user-roles-form.component";
import { AssignUserRolesTableComponent } from "./Assign User Roles/assign-user-roles-table/assign-user-roles-table.component";
import { ClientBillingRolesFormComponent } from "./Client Billing Roles/client-billing-roles-form/client-billing-roles-form.component";
import { ClientBillingRolesTableComponent } from "./Client Billing Roles/client-billing-roles-table/client-billing-roles-table.component";
import { ClientsFormComponent } from "./Clients/clients-form/clients-form.component";
import { ClientsTableComponent } from "./Clients/clients-table/clients-table.component";
import { CompetencyAreaFormComponent } from "./Competency Area/competency-area-form/competency-area-form.component";
import { CompetencyAreaTableComponent } from "./Competency Area/competency-area-table/competency-area-table.component";
import { DepartmentsTableComponent } from "./Departments/departments-table/departments-table.component";
import { DepartmentsFormComponent } from "./Departments/departments-form/departments-form.component";
import { DesignationsFormComponent } from "./Designations/designations-form/designations-form.component";
import { DesignationsTableComponent } from "./Designations/designations-table/designations-table.component";
import { DomainFormComponent } from "./Domain/domain-form/domain-form.component";
import { DomainTableComponent } from "./Domain/domain-table/domain-table.component";
import { GradesFormComponent } from "./Grades/grades-form/grades-form.component";
import { GradesTableComponent } from "./Grades/grades-table/grades-table.component";
import { NotificationTypesFormComponent } from "./Notification Types/notification-types-form/notification-types-form.component";
import { NotificationTypesTableComponent } from "./Notification Types/notification-types-table/notification-types-table.component";
import { PracticeAreaFormComponent } from "./Practice Area/practice-area-form/practice-area-form.component";
import { PracticeAreaTableComponent } from "./Practice Area/practice-area-table/practice-area-table.component";

import { ProficiencyLevelTableComponent } from "./Proficiency Level/proficiency-level-table/proficiency-level-table.component";
import { ProjectTypeFormComponent } from "./Project Type/project-type-form/project-type-form.component";
import { ProjectTypeTableComponent } from "./Project Type/project-type-table/project-type-table.component";
import { RolesFormComponent } from "./Roles/roles-form/roles-form.component";
import { RolesTableComponent } from "./Roles/roles-table/roles-table.component";
import { SkillGroupFormComponent } from "./Skill Group/skill-group-form/skill-group-form.component";
import { SkillGroupTableComponent } from "./Skill Group/skill-group-table/skill-group-table.component";
import { UpdateEmployeeStatusFormComponent } from "./Update Employee Status/update-employee-status-form/update-employee-status-form.component";
import { ProficiencyLevelFormComponent } from "./Proficiency Level/proficiency-level-form/proficiency-level-form.component";
import { SkillsFormComponent } from "./Skills/skills-form/skills-form.component";
import { SkillsTableComponent } from "./Skills/skills-table/skills-table.component";
import { AssignMenusDirective } from "./Assign Menus/assignMenus.directive";
import { AssignUserRolesDirective } from "./Assign User Roles/assignUserRoles.directive";
import { ClientBillingRolesDirective } from "./Client Billing Roles/clientBillingRoles.directive";
import { ClientsDirective } from "./Clients/clients.directive";
import { CompetencyAreaDirective } from "./Competency Area/competencyArea.directive";
import { DepartmentsDirective } from "./Departments/departments.directive";
import { DesignationsDirective } from "./Designations/designations.directive";
import { DomainDirective } from "./Domain/Domain.directive";
import { GradesDirective } from "./Grades/grades.directive";
import { NotificationTypesDirective } from "./Notification Types/notificationTypes.directive";
import { PracticeAreaDirective } from "./Practice Area/practiceAreadirective";
import { ProficiencyLevelDirective } from "./Proficiency Level/proficiencyLevel.directive";
import { ProjectTypeDirective } from "./Project Type/projectType.directive";
import { RolesDirective } from "./Roles/roles.directive";
//skill group components....
import { SkillGroupDirective } from "./Skill Group/skillGroup.directive";
import { SkillsDirective } from "./Skills/skills.directive";
import { AppPrimenNgModule } from "../shared/module/primeng.module";
import { AdminComponent } from "./admin.component";
import { WorkstationComponent } from './workstation/workstation.component';
import { DepartmentTypeDirective } from "./Department Type/department-type.directive";
import { DepartmentTypeTableComponent } from "./Department Type/department-type-table/department-type-table.component";
import { DepartmentTypeFormComponent } from "./Department Type/department-type-form/department-type-form.component";
import { CategoryFormComponent } from "./Category/Category-form/category-form.component";
import { CategoryTableComponent } from "./Category/Category-table/category-table.component";
import { CategoryDirective } from "./Category/Category.directive";
import { KeyFunctionFormComponent } from './KeyFunction/key-function-form/key-function-form.component';
import { KeyFunctionTableComponent } from './KeyFunction/key-function-table/key-function-table.component';
import { SeniorityTableComponent } from './Seniority/seniority-table/seniority-table.component';
import { SeniorityFormComponent } from './Seniority/seniority-form/seniority-form.component';
import { SpecialityFormComponent } from './Speciality/speciality-form/speciality-form.component';
import { SpecialityTableComponent } from './Speciality/speciality-table/speciality-table.component';
import { KeyFunctionDirective } from "./KeyFunction/key-function.directive";
import { SeniorityDirective } from "./Seniority/Seniority.directive";
import { SpecialityDirective } from "./Speciality/Speciality.directive";
import { RoleTypeComponent } from './Role Type/role-type.component';
import { AllAngularMaterialModule } from 'src/app/plugins/all-angular-material.module';

@NgModule({
  imports: [CommonModule, AdminRoutingModule,  AppPrimenNgModule, FormsModule, ReactiveFormsModule, AllAngularMaterialModule ],
  declarations: [
    AdminComponent,
    InternalBillingRolesDirective,
    AssignMenusDirective,
    AssignUserRolesDirective,
    CompetencyAreaDirective,
    DepartmentsDirective,
    CategoryDirective,
    ClientBillingRolesDirective,
    ClientsDirective,
    DepartmentTypeDirective,
    DesignationsDirective,
    DomainDirective,
    GradesDirective,
    KeyFunctionDirective,
    NotificationTypesDirective,
    PracticeAreaDirective,
    ProficiencyLevelDirective,
    ProjectTypeDirective,
    RolesDirective,
    SkillGroupDirective,
    SkillsDirective,
    SeniorityDirective,
    SpecialityDirective,
    InternalBillingRolesFormComponent,
    InternalBillingRolestableComponent,
    MapAssociateIdFormComponent,
    NotificationConfigurationFormComponent,
    AssignMenusTableComponent,
    AssignMenusFormComponent,
    AssignUserRolesFormComponent,
    AssignUserRolesTableComponent,
    CategoryFormComponent,
    CategoryTableComponent,
    ClientBillingRolesFormComponent,
    ClientBillingRolesTableComponent,
    ClientsFormComponent,
    ClientsTableComponent,
    CompetencyAreaFormComponent,
    CompetencyAreaTableComponent,
    DepartmentTypeFormComponent,
    DepartmentTypeTableComponent,
    DepartmentsTableComponent,
    DepartmentsFormComponent,
    DesignationsFormComponent,
    DesignationsTableComponent,
    DomainFormComponent,
    DomainTableComponent,
    GradesFormComponent,
    GradesTableComponent,
    NotificationTypesFormComponent,
    NotificationTypesTableComponent,
    ProficiencyLevelFormComponent,
    ProjectTypeFormComponent,
    PracticeAreaFormComponent,
    PracticeAreaTableComponent,
    ProficiencyLevelTableComponent,
    ProjectTypeFormComponent,
    ProjectTypeTableComponent,
    RolesFormComponent,
    RolesTableComponent,
    SkillGroupFormComponent,
    SkillGroupTableComponent,
    SkillsFormComponent,
    SkillsTableComponent,
    UpdateEmployeeStatusFormComponent,
    WorkstationComponent,
    KeyFunctionFormComponent,
    KeyFunctionTableComponent,
    SeniorityTableComponent,
    SeniorityFormComponent,
    SpecialityFormComponent,
    SpecialityTableComponent,
    RoleTypeComponent,   
  ],

  exports: []
})
export class AdminModule {}
