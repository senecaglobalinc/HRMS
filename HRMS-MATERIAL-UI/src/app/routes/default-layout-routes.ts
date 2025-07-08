import { Routes } from '@angular/router';
import { RolesComponent } from '../modules/admin/components/roles/roles.component';
import { DashboardComponent } from '../modules/shared/components/dashboard/dashboard.component';

export const DEFAULT_ROUTES: Routes = [
  {
    path: 'associates',
    loadChildren: () =>
      import('../modules/onboarding/onboarding.module').then(
        (m) => m.OnboardingModule
      ),
  },
  {
    path: 'admin',
    loadChildren: () =>
      import('../modules/admin/admin.module').then((m) => m.AdminModule),
  },
  {
    path: 'talentmanagement', 
  loadChildren: () =>
   import('../modules/TalentMangment/talentmanagment.module').then(m => m.TalentManagModule)},
  {
    path: 'associateexit', 
    loadChildren: () =>
      import('../modules/AssociateExit/associateexit.module').then(m => m.AssociateexitModule)
  },
  {
    path: 'project',
    loadChildren: () =>
      import('../modules/project-life-cycle/project-life-cycle.module').then((m) => m.ProjectLifeCycleModule),
  },
  {
    path: 'shared',
    loadChildren: () =>
      import('../modules/shared/shared.module').then((m) => m.SharedModule),
  },
  {
    path: 'kra',
    loadChildren: () =>
      import('../modules/kra/kra.module').then((m) => m.KraModule),
  },
  {
    path: 'reports',
    loadChildren: () =>
      import('../modules/reports/reports.module').then((m) => m.ReportsModule),
  },
    { path: 'dashboard', component: DashboardComponent },
];
