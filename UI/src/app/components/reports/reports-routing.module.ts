import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ResourceReportComponent } from './resource-report/resource-report.component';
import { ResourceReportProjectComponent } from './resource-report-project/resource-report-project.component';
import { FinanceReportComponent } from './finance-report/finance-report.component';
import { DomainReportComponent } from './domain-report/domain-report.component';
import { SkillReportComponent } from './skill-report/skill-report.component';
import { ResourceReportMonthComponent } from './resource-report-month/resource-report-month.component';
import { TalentpoolComponent } from './talentpool/talentpool.component';
import { ResourceReportByTechnologyComponent } from './resource-report-by-technology/resource-report-by-technology.component';
import { RmgArchiveReportComponent } from './rmg-archive-report/rmg-archive-report.component';
import { ReportsComponent } from './reports.component';

const routes: Routes = [
    {
        path: '', component: ReportsComponent,
        children: [
            { path: 'resourcereport', component: ResourceReportComponent },
            { path:'resourcereportsbyproject', component: ResourceReportProjectComponent},
            { path:'resourcereportsbyproject/:projectId', component: ResourceReportProjectComponent},
            { path: 'financereport', component: FinanceReportComponent },
            //{ path: 'importRMGreport', component: UploadRMGReportComponent },
            { path: 'domainreport', component: DomainReportComponent },
            { path: 'talentpoolreport', component: TalentpoolComponent },
            { path: 'skillreport', component: SkillReportComponent },
            { path: 'resourcereportsbyTechnology', component: ResourceReportByTechnologyComponent },
            { path: 'resourcereportbyMonth', component: ResourceReportMonthComponent },
            { path: 'rmgarchivereport', component: RmgArchiveReportComponent },
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
})
export class ReportsRoutingModule { }
