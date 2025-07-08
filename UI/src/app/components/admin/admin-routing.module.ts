import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { InternalBillingRolesDirective } from "./Internal Billing Roles/internal-billing-roles.directive";
import { AssignUserRolesDirective } from "./Assign User Roles/assignUserRoles.directive";
import { AssignMenusDirective } from "./Assign Menus/assignMenus.directive";
import { ClientBillingRolesDirective } from "./Client Billing Roles/clientBillingRoles.directive";
import { ClientsDirective } from "./Clients/clients.directive";
import { CompetencyAreaDirective } from "./Competency Area/competencyArea.directive";
import { DepartmentsDirective } from "./Departments/departments.directive";
import { DesignationsDirective } from "./Designations/designations.directive";
import { DomainDirective } from "./Domain/Domain.directive";
import { GradesDirective } from "./Grades/grades.directive";
import { MapAssociateIdFormComponent } from "./Map Associate Id/map-associate-id-form/map-associate-id-form.component";
import { NotificationConfigurationFormComponent } from "./Notification Configuration/notification-configuration-form/notification-configuration-form.component";
import { NotificationTypesDirective } from "./Notification Types/notificationTypes.directive";
import { PracticeAreaDirective } from "./Practice Area/practiceAreadirective";
import { ProjectTypeDirective } from "./Project Type/projectType.directive";
import { ProficiencyLevelDirective } from "./Proficiency Level/proficiencyLevel.directive";
import { RolesDirective } from "./Roles/roles.directive";
import { SkillGroupDirective } from "./Skill Group/skillGroup.directive";
import { UpdateEmployeeStatusFormComponent } from "./Update Employee Status/update-employee-status-form/update-employee-status-form.component";
import { SkillsDirective } from "./Skills/skills.directive";
import { WorkstationComponent } from "./workstation/workstation.component";
import { ToolbarComponent } from "../shared/toolbar/toolbar.component";
import { RolesTableComponent } from "./Roles/roles-table/roles-table.component";
import { RolesFormComponent } from "./Roles/roles-form/roles-form.component";
import { CheckLoggedUserIsAdminGuard } from "./check-logged-user-is-admin.guard";
import { AdminComponent } from "./admin.component";
import { DepartmentTypeDirective } from "./Department Type/department-type.directive";
import { CategoryDirective } from "./Category/Category.directive";
import { KeyFunctionDirective } from "./KeyFunction/key-function.directive";
import { SeniorityDirective } from "./Seniority/Seniority.directive";
import { SpecialityDirective } from "./Speciality/Speciality.directive";
import { RoleTypeComponent } from "./Role Type/role-type.component";
const adminRoutes: Routes = [
  {
    path: "",
    component: AdminComponent, 
    children: [
      {
        path: "userrole",
        component: AssignUserRolesDirective,
        canActivate: [CheckLoggedUserIsAdminGuard]
      },
      {
        path: "asignmenus",
        component: AssignMenusDirective,
        canActivate: [CheckLoggedUserIsAdminGuard]
      },
      {
        path: "clientbillingrole",
        component: ClientBillingRolesDirective,
        canActivate: [CheckLoggedUserIsAdminGuard]
      },
      {
        path: "clients",
        component: ClientsDirective,
        canActivate: [CheckLoggedUserIsAdminGuard]
      },
      {
        path: "competencyarea",
        component: CompetencyAreaDirective,
        canActivate: [CheckLoggedUserIsAdminGuard]
      },
      {
        path: "department",
        component: DepartmentsDirective,
        canActivate: [CheckLoggedUserIsAdminGuard]
      },
      {
        path: "departmenttype",
        component: DepartmentTypeDirective,
        canActivate: [CheckLoggedUserIsAdminGuard]
      },
      {
        path: "designation",
        component: DesignationsDirective,
        canActivate: [CheckLoggedUserIsAdminGuard]
      },
      {
        path: "domain",
        component: DomainDirective,
        canActivate: [CheckLoggedUserIsAdminGuard]
      },
      {
        path: "grade",
        component: GradesDirective,
        canActivate: [CheckLoggedUserIsAdminGuard]
      },
      {
        path: "roletype",
        component: RoleTypeComponent,
        canActivate: [CheckLoggedUserIsAdminGuard]
      },
      {
        path: "internalbillingrole",
        component: InternalBillingRolesDirective,
        canActivate: [CheckLoggedUserIsAdminGuard]
      },
      {
        path: "keyfunction",
        component: KeyFunctionDirective,
        canActivate: [CheckLoggedUserIsAdminGuard]
      },
      {
        path: "mapassociateid",
        component: MapAssociateIdFormComponent,
        canActivate: [CheckLoggedUserIsAdminGuard]
      },
      {
        path: "notificationconfiguration",
        component: NotificationConfigurationFormComponent,
        canActivate: [CheckLoggedUserIsAdminGuard]
      },
      {
        path: "notificationtype",
        component: NotificationTypesDirective,
        canActivate: [CheckLoggedUserIsAdminGuard]
      },
      {
        path: "practicearea",
        component: PracticeAreaDirective,
        canActivate: [CheckLoggedUserIsAdminGuard]
      },
      {
        path: "projecttype",
        component: ProjectTypeDirective,
        canActivate: [CheckLoggedUserIsAdminGuard]
      },
      {
        path: "proficiencylevel",
        component: ProficiencyLevelDirective,
        canActivate: [CheckLoggedUserIsAdminGuard]
      },
      {
        path: "rolelist",
        component: RolesTableComponent,
        canActivate: [CheckLoggedUserIsAdminGuard]
      },
      {
        path: "addrole",
        component: RolesFormComponent,
        canActivate: [CheckLoggedUserIsAdminGuard]
      },
      {
        path: "skillgroup",
        component: SkillGroupDirective,
        canActivate: [CheckLoggedUserIsAdminGuard]
      },
      {
        path: "skills",
        component: SkillsDirective,
        canActivate: [CheckLoggedUserIsAdminGuard]
      },
      {
        path: "seniority",
        component: SeniorityDirective,
        canActivate: [CheckLoggedUserIsAdminGuard]
      },
      {
        path: "speciality",
        component: SpecialityDirective,
        canActivate: [CheckLoggedUserIsAdminGuard]
      },
      {
        path: "employeestatus",
        component: UpdateEmployeeStatusFormComponent,
        canActivate: [CheckLoggedUserIsAdminGuard]
      },
      {
        path: "workstation",
        component: WorkstationComponent,
        canActivate: [CheckLoggedUserIsAdminGuard]
      },
      {
        path: "categorymaster",
        component: CategoryDirective,
        canActivate: [CheckLoggedUserIsAdminGuard]
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(adminRoutes)],
  exports: [RouterModule]
})
export class AdminRoutingModule {}
