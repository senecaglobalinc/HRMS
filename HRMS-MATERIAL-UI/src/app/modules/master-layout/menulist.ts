import { NavItem } from './models/nav-item';

export const navitems: NavItem[] = [
{ displayName: 'Admin', iconName: 'Admin.svg', route: 'admin', children: [
{ displayName: 'Assign User Roles', iconName: 'RightArrow.svg', route: 'admin/userrole' },
{ displayName: 'Assign Menus', iconName: 'RightArrow.svg', route: 'admin/asignmenus' },
{ displayName: 'Client Billing Roles', iconName: 'RightArrow.svg', route: 'admin/clientbillingrole' },
{ displayName: 'Clients', iconName: 'RightArrow.svg', route: 'admin/clients' },
{ displayName: 'Competency Area', iconName: 'RightArrow.svg', route: 'admin/competencyarea' },
{ displayName: 'Departments', iconName: 'RightArrow.svg', route: 'admin/department' },
{ displayName: 'Department Type', iconName: 'RightArrow.svg', route: 'admin/departmenttype' },
{ displayName: 'Designations', iconName: 'RightArrow.svg', route: 'admin/designation' },
{ displayName: 'Domain', iconName: 'RightArrow.svg', route: 'admin/domain' },
{ displayName: 'Grades', iconName: 'RightArrow.svg', route: 'admin/grade' },
{ displayName: 'RoleType', iconName: 'RightArrow.svg', route: 'admin/roletype' },
{ displayName: 'Internal Billing Roles', iconName: 'RightArrow.svg', route: 'admin/internalbillingrole' },
{ displayName: 'Key Function', iconName: 'RightArrow.svg', route: 'admin/keyfunction' },
{ displayName: 'Map Associate ID', iconName: 'RightArrow.svg', route: 'admin/mapassociateid' },
{ displayName: 'Notification Configuration', iconName: 'RightArrow.svg', route: 'admin/notificationconfiguration' },
{ displayName: 'Notification Types', iconName: 'RightArrow.svg', route: 'admin/notificationtype' },
{ displayName: 'Practice Area', iconName: 'RightArrow.svg', route: 'admin/practicearea' },
{ displayName: 'Project Type', iconName: 'RightArrow.svg', route: 'admin/projecttype' },
{ displayName: 'Proficiency Level', iconName: 'RightArrow.svg', route: 'admin/proficiencylevel' },
{ displayName: 'RolesTable', iconName: 'RightArrow.svg', route: 'admin/rolelist' },
{ displayName: 'Functional Roles', iconName: 'RightArrow.svg', route: 'admin/addrole' },
{ displayName: 'SkillGroup', iconName: 'RightArrow.svg', route: 'admin/skillgroup' },
{ displayName: 'Skills', iconName: 'RightArrow.svg', route: 'admin/skills' },
{ displayName: 'Seniority', iconName: 'RightArrow.svg', route: 'admin/seniority' },
{ displayName: 'Speciality', iconName: 'RightArrow.svg', route: 'admin/speciality' },
{ displayName: 'Update Employee Status', iconName: 'RightArrow.svg', route: 'admin/employeestatus' },
{ displayName: 'Workstation', iconName: 'RightArrow.svg', route: 'admin/workstation' },
{ displayName: 'Category', iconName: 'RightArrow.svg', route: 'admin/categorymaster' },
]
  },
  { displayName: 'ADR', iconName: 'ADR.svg', route: 'adr' },
  { displayName: 'Aspect Library', iconName: 'Aspect-Library.svg', route: 'aspectlibrary' },
  { displayName: 'Associates', iconName: 'Associates.svg', route: 'associates' },
  { displayName: 'Dashboard', iconName: 'Dashboard.svg', route: 'dashboard' },
  { displayName: 'Define', iconName: 'Define.svg', route: 'define' },
  {
    displayName: 'KRA', iconName: 'KRA.svg', route: 'kra', children: [
      { displayName: 'Applicable Role Types', iconName: 'ightArrow.svg', route: 'kra/applicableroletype' } ,
      { displayName: 'Measurement Types', iconName: 'ightArrow.svg', route: 'kra/krameasurementtype' },
      { displayName: 'Aspect', iconName: 'ightArrow.svg', route: 'kra/aspectmaster' },  
      { displayName: 'Scales', iconName: 'ightArrow.svg', route: 'kra/scaleMaster' },
      { displayName: 'Define', iconName: 'ightArrow.svg', route: 'kra/kradefinitions' } ,
      { displayName: 'Review', iconName: 'ightArrow.svg', route: 'kra/reviewkra' } ,
       { displayName: 'Status', iconName: 'Status.svg', route: 'kra/ceostatus' }     
    ]
  },
  { displayName: 'Project Details', iconName: 'Project-Details.svg', route: 'projectdetails' },
  { displayName: 'Prospective Associates', iconName: 'Prospective-Associates.svg', route: 'prospectiveassociates' },
  { displayName: 'Reports', iconName: 'Reports.svg', route: 'reports' },
  { displayName: 'Status', iconName: 'Status.svg', route: 'status' },
  { displayName: 'Talent Management', iconName: 'Talent-Management.svg', route: 'talentmanagement' }
  
];