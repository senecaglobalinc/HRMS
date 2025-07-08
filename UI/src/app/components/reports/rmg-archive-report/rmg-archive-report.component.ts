import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { MessageService, SelectItem } from 'primeng/api';
import * as servicePath from '../../../service-paths';
import { BooleanToStringPipe } from '../../../Pipes/BooleanToStringPipe';
import { FinanceReportService } from '../services/finance-report.service';
import { ReportsFilterData, FinanceReportFilterData } from '../models/reportsfilter.model';
import { ReportsData } from '../models/reportsdata.model';
import * as moment from 'moment';
declare let alasql: any;

@Component({
  selector: 'app-rmg-archive-report',
  templateUrl: './rmg-archive-report.component.html',
  styleUrls: ['./rmg-archive-report.component.scss'],
  providers: [FinanceReportService,MessageService, BooleanToStringPipe, MessageService]
})
export class RmgArchiveReportComponent implements OnInit {

  rmgReportData : ReportsFilterData[] = [];
 viewRmgData : FormGroup;
 btnLabel : string = "";
 isEdit : boolean;
 formSubmitted = false;
 cols: any[] = [];
 columnOptions: any[] = [];
 selectedColumns: any[];
 maxDateValue: Date;
 PageSize: number;
 PageDropDown: number[] = [];
 months: any[] = [];
 MonthOptions: SelectItem[] = [];

 private resources = servicePath.API.PagingConfigValue;

  constructor(
    private yesNoPipe: BooleanToStringPipe,private _financeReportService: FinanceReportService,
    private messageService: MessageService
  ) { 
    this.PageSize = this.resources.PageSize;
    this.PageDropDown = this.resources.PageDropDown;
  }


   ngOnInit() {
    this.viewRmgData = new FormGroup({
      Month : new FormControl(null,[
        Validators.required,
        // Validators.pattern('^[a-zA-Z][a-zA-Z-&, ]*$'),
        Validators.maxLength(50)
      ]),
      Year : new FormControl(null,[
        Validators.required,
        // Validators.pattern('^[a-zA-Z][a-zA-Z-&, ]*$'),
        Validators.maxLength(100)
      ]),     
    });
    this.cols = [
      { field: 'EmployeeCode', header: 'Associate ID' },
      { field: 'EmployeeName', header: 'Associate Name' },
      { field: 'DesignationName', header: 'Designation Name' },
      { field: 'GradeName', header: 'Grade' },
      { field: 'ProjectName', header: 'Project' },
      { field: 'SkillCode', header: 'Skill' },
      { field: 'RoleName', header: 'Role' },
      { field: 'ClientName', header: 'Client' },
      { field: 'Allocationpercentage', header: '(%) Allocation' },
      { field: 'ClientBillingPercentage', header: '(%) Client Billing' },
      { field: 'InternalBillingPercentage', header: '(%) Internal Billing' },
      { field: 'ClientBillingRoleCode', header: 'Client Billing Role' },
      { field: 'InternalBillingRoleCode', header: 'Internal Billing Role' },
      { field: 'FromDate', header: 'From Date' },
      { field: 'ToDate', header: 'To Date' },
      { field: 'LeadName', header: 'Lead' },
      { field: 'ReportingManagerName', header: 'Reporting Manager' },
      { field: 'ProgramManagerName', header: 'Program Manager' },
      { field: 'IsBillable', header: 'Billablee',type: this.yesNoPipe },
      { field: 'IsCritical', header: 'Criticale',type: this.yesNoPipe }
     
  ];
  this.columnOptions = [];
        for (let i = 0; i < this.cols.length; i++) {
            this.columnOptions.push({ label: this.cols[i].header, value: this.cols[i] });
        }
        this.selectedColumns = this.cols;

        this.maxDateValue = new Date();

  this.months = [
      { Id: 1, Name: "January" },
      { Id: 2, Name: "February" },
      { Id: 3, Name: "March" },
      { Id: 4, Name: "April" },
      { Id: 5, Name: "May" },
      { Id: 6, Name: "June" },
      { Id: 7, Name: "July" },
      { Id: 8, Name: "August" },
      { Id: 9, Name: "September" },
      { Id: 10, Name: "October" },
      { Id: 11, Name: "November" },
      { Id: 12, Name: "December" },
    ];
    this.MonthOptions = [];
    for (let i = 0; i < this.months.length; i++) {
      this.MonthOptions.push({ label: this.months[i].Name, value: this.months[i].Id });
    }
   }
  viewRmgReportData(){
    this.formSubmitted = true;
    this.rmgReportData=[];
    if(this.viewRmgData.valid){
    this._financeReportService.GetRmgReportDataByMonthYear(this.viewRmgData.value).subscribe((ReportList: ReportsFilterData[]) => {
      console.log(ReportList)
      this.rmgReportData = ReportList;
      this.rmgReportData.forEach((record : any) => {
      record.FromDate = moment(record.FromDate).format('DD-MM-YYYY');
      record.ToDate = moment(record.ToDate).format('DD-MM-YYYY');
      record.IsCritical = record.IsCritical ? 'Yes' : 'No';
      record.IsBillable = record.IsBillable ? 'Yes' : 'No';
    });   
  },
  (error: any) => {
    this.messageService.add({
        severity: 'error',
        summary: 'Error message',
        detail: error.error
      });
    }
  
);}
  
  }
  GetRmgReportDataToFreez(): void {
    this._financeReportService.GetFinanceReportToFreez().subscribe((res: boolean) => {
        if(res == true){
            this.messageService.add({severity:'success', summary: 'Success Message', detail:'Rmg Data Successfully Archived'});
        }
        else{
            this.messageService.add({severity:'failed', summary: 'Failed Message', detail:'Rmg Data  Archival Failed'});
        }
    },
        (error: any) => {
            this.messageService.add({
                severity: 'error',
                summary: 'Error message',
                detail: error.error
              });

        }
    );
}
  cancel() : void{
    this.formSubmitted = false;
    this.viewRmgData.reset(); 
    this.rmgReportData=[];   
  }
}

