import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AssignUserRolesComponent } from './components/assign-user-roles/assign-user-roles.component';
import { AdminComponent } from './components/admin/admin.component';
import { AssignMenusComponent } from './components/assign-menus/assign-menus.component';
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
import {ServiceTypeComponent} from './components/service-type/service-type.component';

const routes: Routes = [

  {
    path: '', component: AdminComponent, children: [
      { path: "userrole", component: AssignUserRolesComponent, data: { title: 'User Role' } },
      { path: "asignmenus", component: AssignMenusComponent, data: { title: 'Assign Menus' } },
      { path: "clientbillingrole", component: ClientBillingRolesComponent, data: { title: 'Client Billing Role' } },
      { path: "clients", component: ClientsComponent, data: { title: 'Clients' } },
      { path: "competencyarea", component: CompetencyAreaComponent, data: { title: 'Competency Area' } },
      { path: "department", component: DepartmentsComponent, data: { title: 'Department' } },
      { path: "departmenttype", component: DepartmentTypeComponent, data: { title: 'Department Type' } },
      { path: "designation", component: DesignationsComponent, data: { title: 'Designation' } },
      { path: "domain", component: DomainComponent, data: { title: 'Domain' } },
      { path: "grade", component: GradesComponent, data: { title: 'Grade' } },
      { path: "roletype", component: RoleTypeComponent, data: { title: 'Role Type' } },
      { path: "internalbillingrole", component: InternalBillingRolesComponent, data: { title: 'Internal Billing Role' } },
      { path: "keyfunction", component: KeyFunctionComponent, data: { title: 'Key Function' } },
      { path: "mapassociateid", component: MapAssociateIdComponent, data: { title: 'Map Associate ID' } },
      { path: "notificationconfiguration", component: NotificationConfigurationComponent, data: { title: 'Notification Configuration' } },
      { path: "notificationtype", component: NotificationTypesComponent, data: { title: 'Notification Type' } },
      { path: "practicearea", component: PracticeAreaComponent, data: { title: 'Practice Area' } },
      { path: "projecttype", component: ProjectTypeComponent, data: { title: 'Project Type' } },
      { path: "proficiencylevel", component: ProficiencyLevelComponent, data: { title: 'Proficiency Level' } },
      { path: "rolelist", component: RolesTableComponent, data: { title: 'Roles' } },
      { path: "addrole", component: RolesComponent, data: { title: 'Add Role' } },
      { path: "skillgroup", component: SkillGroupComponent, data: { title: 'Skill Group' } },
      { path: "skills", component: SkillsComponent, data: { title: 'Skills' } },
      { path: "seniority", component: SeniorityComponent, data: { title: 'Seniority' } },
      { path: "speciality", component: SpecialityComponent, data: { title: 'Speciality' } },
      { path: "employeestatus", component: UpdateEmployeeStatusComponent, data: { title: 'Deactivate Employee' } },
      { path: "workstation", component: WorkstationComponent, data: { title: 'Workstation' } },
      { path: "categorymaster", component: CategoryComponent, data: { title: 'Category Master' } },
      { path: "servicetype", component: ServiceTypeComponent, data: { title: 'Service Type' } },
    ]
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule { }
