import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ResourceReportComponent } from './resource-report/resource-report.component';
import { ReportsRoutingModule } from './reports-routing.module';
import { AppPrimenNgModule } from '../shared/module/primeng.module';
import { MultiSelectModule } from 'primeng/primeng';
import { ResourceReportProjectComponent } from './resource-report-project/resource-report-project.component';
import { SkillReportComponent } from './skill-report/skill-report.component';
import { DomainReportComponent } from './domain-report/domain-report.component';
import { TalentpoolComponent } from './talentpool/talentpool.component';
import { ResourceReportMonthComponent } from './resource-report-month/resource-report-month.component';
import { FinanceReportComponent } from './finance-report/finance-report.component';
import { ResourceReportByTechnologyComponent } from './resource-report-by-technology/resource-report-by-technology.component';
import { ReportsComponent } from './reports.component';
import { RmgArchiveReportComponent } from './rmg-archive-report/rmg-archive-report.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AssociateProjectHistoryComponent } from './associate-project-history/associate-project-history.component';

@NgModule({
    imports: [
        CommonModule, ReportsRoutingModule,AppPrimenNgModule, MultiSelectModule,FormsModule, ReactiveFormsModule
    ],
    declarations: [ResourceReportComponent,ReportsComponent, ResourceReportProjectComponent, SkillReportComponent, DomainReportComponent, TalentpoolComponent, ResourceReportMonthComponent, FinanceReportComponent, ResourceReportByTechnologyComponent, RmgArchiveReportComponent, AssociateProjectHistoryComponent]
})

export class ReportsModule {

}
