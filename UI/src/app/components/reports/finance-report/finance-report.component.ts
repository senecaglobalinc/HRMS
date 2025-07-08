import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FinanceReportService } from '../../reports/services/finance-report.service';
import { MasterDataService } from '../../../services/masterdata.service';
import { ReportsData } from '../models/reportsdata.model';
import { ReportsFilterData } from '../models/reportsfilter.model';
import { FinanceReportFilterData } from '../models/reportsfilter.model';
import { ProjectDetails } from '../../../models/projects.model';
import { MessageService } from 'primeng/api';
import { Validators, FormGroup, FormBuilder } from '@angular/forms';
import * as servicePath from '../../../service-paths';
import * as moment from 'moment';
import { BooleanToStringPipe } from '../../../Pipes/BooleanToStringPipe';
import { ConfirmationService } from 'primeng/api';
import { GenericType } from "../../../models/dropdowntype.model";

//declare let alasql: any;
//declare var $: any;

@Component({
    selector: 'app-finance-report',
    templateUrl: './finance-report.component.html',
    styleUrls: ['./finance-report.component.scss'],
    providers: [FinanceReportService, MasterDataService, MessageService, BooleanToStringPipe, ConfirmationService]

})

export class FinanceReportComponent implements OnInit {

    searchData: FinanceReportFilterData;
    projectsList: any[] = [];
    financeReport: ReportsData[] = [];
    errorMessage: any[];
    searchFormSubmitted: boolean = false;
    componentName: string; myForm: FormGroup;
    @ViewChild('financeFilter') financeFilter: any;
    errorSummary: string = '';
    reportsFilterData: ReportsFilterData;
    totalRecordsCount: number;
    fromDate: Date;
    toDate: Date;
    maxDateValue: Date;
    daaa: any;
    cols: any[] = [];
    columnOptions: any[] = [];
    PageSize: number;
    PageDropDown: number[] = [];
    private resources = servicePath.API.PagingConfigValue;
    filterDisplay: boolean = false;
    selectedColumns: any[];
    loading: boolean;
    public reportStatus: boolean = false;
    constructor(private _financeReportService: FinanceReportService,
        private yesNoPipe: BooleanToStringPipe,
        private activatedRoute: ActivatedRoute, private messageService: MessageService,
        private masterDataService: MasterDataService, private fb: FormBuilder,
        private confirmationService: ConfirmationService) {
        this.searchData = new FinanceReportFilterData();
        // this.componentName = this.activatedRoute.routeConfig.component.name;
        this.reportsFilterData = new ReportsFilterData();
        this.PageSize = this.resources.PageSize;
        this.PageDropDown = this.resources.PageDropDown;

    }

    ngOnInit() {

        this.myForm = this.fb.group({
            fromDate: ['', [Validators.required]],
            toDate: ['', [Validators.required]],
            project: ['', null]
        });


        this.cols = [
            { field: 'EmployeeName', header: 'Associate Name' },
            { field: 'ProjectName', header: 'Project' },
            { field: 'Allocationpercentage', header: '(%) Allocation' },
            { field: 'FromDate', header: 'From Date' },
            { field: 'ToDate', header: 'To Date' },
            { field: 'IsBillableForExcel', header: 'Billable'},
            { field: 'IsCriticalForExcel', header: 'Critical' },
            { field: 'EmployeeCode', header: 'Associate ID' },
            // { field: 'EmployeeName', header:'Associate Name' },
            { field: 'DesignationName', header: 'Designation Name' },
            { field: 'GradeName', header: 'Grade' },
            // { field:'ProjectName', header:'Project' },
            { field: 'SkillCode', header: 'Skill' },
            { field: 'RoleName', header: 'Role' },
            { field: 'ClientName', header: 'Client' },
            // { field: 'Allocationpercentage', header: '(%) Allocation' },
            { field: 'ClientBillingPercentage', header: '(%) Client Billing' },
            { field: 'InternalBillingPercentage', header: '(%) Internal Billing' },
            { field: 'ClientBillingRoleCode', header: 'Client Billing Role' },
            { field: 'InternalBillingRoleCode', header: 'Internal Billing Role' },

            { field: 'LeadName', header: 'Lead' },
            { field: 'ReportingManagerName', header: 'Reporting Manager' },
            { field: 'ProgramManagerName', header: 'Program Manager' },

        ];

        this.columnOptions = [];

        for (let i = 0; i < this.cols.length; i++) {

            this.columnOptions.push({ label: this.cols[i].header, value: this.cols[i] });

        }
        this.selectedColumns = [

            { field: 'EmployeeName', header: 'Associate Name' },
            { field: 'ProjectName', header: 'Project' },
            { field: 'Allocationpercentage', header: '(%) Allocation' },
            { field: 'FromDate', header: 'From Date' },
            { field: 'ToDate', header: 'To Date' },
            // { field: 'IsBillable', header: 'Billable',type:this.yesNoPipe },
            // { field: 'IsCritical', header:'Critical', type:this.yesNoPipe }
            { field: 'IsBillableForExcel', header: 'Billable' },
            { field: 'IsCriticalForExcel', header: 'Critical' }

        ]

        this.maxDateValue = new Date();
    }



    getProjects(): void {
        this.masterDataService.GetProjectsForDropdown().subscribe((res: GenericType[]) => {            
            this.projectsList = [];
            this.projectsList.push({ label: '', value: null });
            res.forEach((element: GenericType) => {
                this.projectsList.push({ label: element.Name, value: element.Id });
            });            
        },
            (error: any) => {
                this.errorMessage = [];
                this.messageService.add({ severity: 'error', summary: 'Warn message', detail: 'Failed to Get Project List!' });
                // this.errorMessage.push({ severity: 'error', summary: 'Failed to Get Project List!' });
            }
        );
    }

    fetchResourceReport() {
        this.searchFormSubmitted = true;
        
        if (this.searchData.FromDate != null && this.searchData.ToDate != null) {
            this.errorSummary = '';
            if (moment(this.searchData.FromDate).isSameOrAfter(new Date())) {
                this.errorSummary = 'From date should be less than today';
                this.messageService.add({ severity: 'error', summary: 'Warn message', detail: this.errorSummary });

                return false;
            }
            if (moment(this.searchData.ToDate).isSameOrAfter(new Date())) {
                this.errorSummary = 'To date should be less than today';
                this.messageService.add({ severity: 'error', summary: 'Warn message', detail: this.errorSummary });

                return false;
            }
            if (moment(this.searchData.FromDate).isSameOrAfter(this.searchData.ToDate)) {
                this.errorSummary = 'From Date should be less than To Date';
                this.messageService.add({ severity: 'error', summary: 'Warn message', detail: this.errorSummary });

                return false;
            }
            this.searchData.RowsPerPage = 10;
            this.searchData.PageNumber = 1;
            this.reportsFilterData = new ReportsFilterData();
            this.reportsFilterData.financeReportFilterData = new FinanceReportFilterData();
            this.reportsFilterData.financeReportFilterData.FromDate = moment(this.searchData.FromDate).toDate();
            this.reportsFilterData.financeReportFilterData.ToDate = moment(this.searchData.ToDate).toDate();
            this.reportsFilterData.financeReportFilterData.ProjectId = this.searchData.ProjectId;
            // this.reportsFilterData.financeReportFilterData.PageNumber = this.searchData.PageNumber;
            // this.reportsFilterData.financeReportFilterData.RowsPerPage = this.searchData.RowsPerPage;
            this.reportStatus = true;
            this._financeReportService.GetFinanceReport(this.searchData).subscribe((reportsData: Array<ReportsData>) => {
                this.reportStatus = false;
                this.financeReport = reportsData;
                this.financeReport.forEach((record: ReportsData) => {
                    record.FromDate = moment(record.FromDate).format('DD-MM-YYYY');
                    record.ToDate = moment(record.ToDate).format('DD-MM-YYYY');
                    record.IsBillableForExcel = (record.IsBillable == true) ? 'Yes' : 'No';
                    record.IsCriticalForExcel = (record.IsCritical == true) ? 'Yes' : 'No';
                });
                // this.totalRecordsCount = ReportList.TotalCount;
                this.fromDate = moment(this.searchData.FromDate).toDate();
                this.toDate = moment(this.searchData.ToDate).toDate();
                this.filterDisplay = false;
            });
        }
    }



    // exportData = function () {
    //     if (this.fromDate != null && this.toDate != null) {
    //         let financeReportData: ReportsData[] = new Array<ReportsData>();
    //         this.reportsFilterData = new ReportsFilterData();
    //         this.reportsFilterData.financeReportFilterData = new FinanceReportFilterData();
    //         this.reportsFilterData.financeReportFilterData.RowsPerPage = this.totalRecordsCount;
    //         this.reportsFilterData.financeReportFilterData.PageNumber = 1;
    //         this.reportsFilterData.financeReportFilterData.FromDate = moment(this.fromDate).toDate();
    //         this.reportsFilterData.financeReportFilterData.ToDate = moment(this.toDate).toDate();
    //         this._financeReportService.GetFinanceReport(this.reportsFilterData).then((ReportList: ReportsFilterData) => {
    //             financeReportData = ReportList.reportsData;
    //             financeReportData.forEach((record: ReportsData) => {
    //                 record.FromDate = moment(record.FromDate).format('DD-MM-YYYY');
    //                 record.ToDate = moment(record.ToDate).format('DD-MM-YYYY');

    //             });
    //             if (financeReportData.length > 0)
    //                 alasql('SELECT EmployeeCode as [Associate ID],EmployeeName as [Associate Name],DesignationName as [Designation],GradeName as [Grade],ProjectName as Project,SkillCode as Skill,RoleName as [Role],ClientName as Client,Allocationpercentage as [Allocation Percentage],ClientBillingPercentage as [Client Billing Percentage],InternalBillingPercentage as [Internal Billing Percentage],ClientBillingRoleCode as [Client Billing Role],InternalBillingRoleCode as [Internal Billing Role],FromDate,ToDate,LeadName as [Lead],ReportingManagerName as [Reporting Manager],ProgramManagerName as [Program Manager],IsBillable as Billable,IsCritical as Critical INTO XLSX("Finance_Report.xlsx",{headers:true}) FROM ?', [financeReportData]);
    //         });
    //     }
    // };




    clearFilter = function () {
        this.myForm.reset();
        this.searchFormSubmitted = false;
        this.errorSummary = '';
        this.searchData = new FinanceReportFilterData();
    }
    OpenConfirmationDialog() {   // method to open dialog
        this.confirmationService.confirm({
            message: 'Do you want to clear ?',
            accept: () => {
                this.clearFilter()
            },
            reject: () => {

            }
        });
    }

    onFilter() {
        this.getProjects();
        this.filterDisplay = true;
        this.myForm.reset();
        this.searchFormSubmitted = false;
        this.errorSummary = '';
    }

    clearErrorSummary(event: any) {
        this.errorSummary = '';
    }
}
