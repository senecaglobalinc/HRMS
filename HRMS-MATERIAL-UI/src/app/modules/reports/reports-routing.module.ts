import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ReportsComponent } from './components/reports/reports.component';
import { ResourceReportComponent} from './components/resource-report/resource-report.component';
import { ResourceReportProjectComponent } from './components/resource-report-project/resource-report-project.component';
import {DomainReportComponent} from './components/domain-report/domain-report.component';
import { TalentpoolComponent } from './components/talentpool/talentpool.component';
import {FinanceReportComponent} from './components/finance-report/finance-report.component';
import { ResourceReportByProjectComponent } from './components/resource-report-by-project/resource-report-by-project.component';
import { ResourceReportByMonthComponent } from './components/resource-report-by-month/resource-report-by-month.component';
import { SkillReportComponent } from './components/skill-report/skill-report.component';
import { ServicetypeReportComponent } from './components/servicetype-report/servicetype-report.component';
import { ProjectsReportComponent } from './components/projects-report/projects-report.component';
import { ProjectdetailsReportComponent } from './components/projectdetails-report/projectdetails-report.component';
import { ResourceReportCriticalNonbillingComponent } from './components/resource-report-critical-nonbilling/resource-report-critical-nonbilling.component';
import { ResourceReportNonCriticalNonbillingComponent } from './components/resource-report-non-critical-nonbilling/resource-report-non-critical-nonbilling.component';
import { UtilizationReportComponent } from './components/utilization-report/utilization-report.component';
import { AssociateExitReportComponent } from './components/associate-exit-report/associate-exit-report.component';
import { AssociateExitSummaryReportComponent } from './components/associate-exit-summary-report/associate-exit-summary-report.component';
import { AssociateAttendanceReportComponent } from './components/associate-attendance-report/associate-attendance-report.component';
import { ManagerAttendanceReportComponent } from './components/manager-attendance-report/manager-attendance-report.component';
import { NonCriticalResourceLastBillingReportComponent } from './components/non-critical-resource-last-billing-report/non-critical-resource-last-billing-report.component';
const routes: Routes = [
    {
        path: '', component: ReportsComponent,
        children: [
            { path:'resourcereport', component: ResourceReportComponent, data: { title: 'Resource Report' } },
            { path:'resourcereportsbyproject', component: ResourceReportProjectComponent,  data: { title: 'Resource Report By Project' }},
            { path:'resourcereportsbyproject/:projectId', component: ResourceReportProjectComponent, data: { title: 'Allocation Details' }},
            { path:'domainreport', component: DomainReportComponent, data: { title: 'Domain Report' } },
            { path: 'talentpoolreport', component: TalentpoolComponent,  data: { title: 'Talentpool data Report' }  },
            { path: 'financereport', component: FinanceReportComponent,  data: { title: 'Finance Report' }  },

            { path: 'resourcereportsbyTechnology', component: ResourceReportByProjectComponent,  data: { title: 'Resource Report By Project' }  },
            { path: 'resourcereportbyMonth', component: ResourceReportByMonthComponent,  data: { title: 'Resource Report By Month' }  },
            { path: 'skillreport', component: SkillReportComponent,  data: { title: 'Finance Report' }  },
            { path: 'servicereport', component: ServicetypeReportComponent,  data: { title: 'Service Type Report' }  },
            { path: 'projectsreport', component: ProjectsReportComponent,  data: { title: 'Projects Report' }  },
            { path: 'projectdetails', component: ProjectdetailsReportComponent,  data: { title: 'Projects Information Report' }  },
            { path: 'resource-report-with-critical', component: ResourceReportCriticalNonbillingComponent,  data: { title: 'Critical Resource Report' } },
            { path: 'resource-report-with-noncritical', component: ResourceReportNonCriticalNonbillingComponent,  data: { title: 'Non critical Resource Report' } },
            { path: 'utilizationreport', component: UtilizationReportComponent,  data: { title: 'Utilization Report' }  },
            { path: 'resource-report-with-noncritical', component: ResourceReportNonCriticalNonbillingComponent,  data: { title: 'Non critical Resource Report (Available Talent)' } },
            { path: 'associateexitreport', component: AssociateExitReportComponent,  data: { title: 'Associate Exit Report' }  },
            { path: 'associateexitsummary', component: AssociateExitSummaryReportComponent,  data: { title: 'Associate Exit Summary' }  },
            { path: 'associateattendancereport', component: AssociateAttendanceReportComponent,  data: { title: 'Attendance Report' }  },
            { path: 'managerattendancereport', component: ManagerAttendanceReportComponent,  data: { title: 'Attendance Reports' }  },
            { path: 'noncritical-resource-with-last-billing-report', component: NonCriticalResourceLastBillingReportComponent,  data: { title: 'Non critical Resource Last Billing Report' } },

        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
  })
  export class ReportsRoutingModule { }