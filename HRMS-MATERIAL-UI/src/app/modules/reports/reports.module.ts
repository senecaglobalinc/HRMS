import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { DatePipe } from '@angular/common'

import { ReportsRoutingModule } from './reports-routing.module';
import {ReportsComponent} from './components/reports/reports.component';
import { ResourceReportComponent } from './components/resource-report/resource-report.component';
import { ResourceReportProjectComponent } from './components/resource-report-project/resource-report-project.component';
import { RouterModule } from '@angular/router';
import { FlexLayoutModule } from '@angular/flex-layout';
import { AllAngularMaterialModule } from '../plugins/all-angular-material/all-angular-material.module';
import { TalentpoolComponent } from './components/talentpool/talentpool.component';
import {DomainReportComponent} from './components/domain-report/domain-report.component';

import { FinanceReportComponent } from './components/finance-report/finance-report.component';
import { MatTableExporterModule } from 'mat-table-exporter';
import { NgxMatSelectSearchModule } from 'ngx-mat-select-search';
import { SkillReportComponent } from './components/skill-report/skill-report.component';
import { ResourceReportByProjectComponent } from './components/resource-report-by-project/resource-report-by-project.component';
import { ResourceReportByMonthComponent } from './components/resource-report-by-month/resource-report-by-month.component';
import { ServicetypeReportComponent } from './components/servicetype-report/servicetype-report.component';
import { NgxSpinnerModule } from 'ngx-spinner';
import { ProjectsReportComponent } from './components/projects-report/projects-report.component';
import { ProjectdetailsReportComponent } from './components/projectdetails-report/projectdetails-report.component';
import { ResourceReportCriticalNonbillingComponent } from './components/resource-report-critical-nonbilling/resource-report-critical-nonbilling.component';
import { ResourceReportNonCriticalNonbillingComponent } from './components/resource-report-non-critical-nonbilling/resource-report-non-critical-nonbilling.component';
import { UtilizationReportComponent } from './components/utilization-report/utilization-report.component';
import { AssociateExitReportComponent } from './components/associate-exit-report/associate-exit-report.component';
import { AssociateExitSummaryReportComponent } from './components/associate-exit-summary-report/associate-exit-summary-report.component';
import { AssociateAttendanceReportComponent } from './components/associate-attendance-report/associate-attendance-report.component';
import { AttendanceReportDialogComponent } from './components/attendance-report-dialog/attendance-report-dialog.component';
import { FullCalendarModule } from '@fullcalendar/angular';
import dayGridPlugin from '@fullcalendar/daygrid';
import { ManagerAttendanceReportComponent } from './components/manager-attendance-report/manager-attendance-report.component';
import { NonCriticalResourceLastBillingReportComponent } from './components/non-critical-resource-last-billing-report/non-critical-resource-last-billing-report.component';

FullCalendarModule.registerPlugins([ 
  dayGridPlugin  
]);
@NgModule({
    declarations: [ReportsComponent,ResourceReportComponent,ResourceReportProjectComponent, TalentpoolComponent, FinanceReportComponent, 
        DomainReportComponent, SkillReportComponent, ResourceReportByProjectComponent, ResourceReportByMonthComponent, ServicetypeReportComponent, ProjectsReportComponent, ProjectdetailsReportComponent, ResourceReportCriticalNonbillingComponent, ResourceReportNonCriticalNonbillingComponent, UtilizationReportComponent, AssociateExitReportComponent, AssociateExitSummaryReportComponent, AssociateAttendanceReportComponent, AttendanceReportDialogComponent, ManagerAttendanceReportComponent, NonCriticalResourceLastBillingReportComponent      
      ],

    imports: [
        RouterModule,
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        FlexLayoutModule,
        ReportsRoutingModule,
        AllAngularMaterialModule,
        MatTableExporterModule,
        NgxMatSelectSearchModule,
        NgxSpinnerModule, 
        FullCalendarModule,       
      ],
      exports: [],
      providers:[DatePipe],      
    })

export class ReportsModule { }