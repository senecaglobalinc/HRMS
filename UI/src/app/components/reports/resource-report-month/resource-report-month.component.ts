import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ResourceReportService } from '../services/resource-report.service';
import { MasterDataService } from '../../../services/masterdata.service';
import { CommonService } from '../../../services/common.service';
import { ReportsData } from '../models/reportsdata.model';
import { UtilizationReportFilterData, ReportsFilterData, utilizationReportByMonthFilterData } from '../models/reportsfilter.model';
import { GenericType } from '../../../models/dropdowntype.model';
import { SelectItem, MessageService } from 'primeng/api';
import * as servicePath from '../../../service-paths';
import {ConfirmationService} from 'primeng/api';

@Component({
  selector: 'app-resource-report-month',
  templateUrl: './resource-report-month.component.html',
  styleUrls: ['./resource-report-month.component.scss'],
  providers: [MessageService, ResourceReportService, CommonService, MasterDataService,ConfirmationService]
})
export class ResourceReportMonthComponent implements OnInit {
  cols: any[] = [];
  columnOptions: any[] = [];
  months: any[] = [];
  FrommonthOptions: any[] = [];
  //pattern: string;
  associateUtilizationReportList: ReportsData[] = [];
  private associateUtilizationReportExcel: ReportsData[] = [];
  reportsFilterData: ReportsFilterData;
  private resourceFilter: UtilizationReportFilterData;
  private componentName: string;
  filterDisplay: boolean = false;
  private technologyList: SelectItem[] = [];
  private associatesList: SelectItem[] = [];
  private projectsList: SelectItem[] = [];
  private gradesList: SelectItem[] = [];
  private experienceList: SelectItem[] = [];
  private clinentsList: SelectItem[] = [];
  private programManagersList: SelectItem[] = [];
  private designationsList: SelectItem[] = [];
  private percentageList: SelectItem[] = [];
  private expList: GenericType[] = [];
  totalRecordsCount: number = 0;
  afterSearchFilter: boolean = false;
  private allBillable: number = -1;
  private allCritical: number = -1;
  PageSize: number;
  PageDropDown: number[] = [];
  private utilizationPercentage: number = 0;
  isExportToExcel: boolean = false;
  PracticeAreaId: number = 0;
  private resources = servicePath.API.PagingConfigValue;
  constructor(private actRoute: ActivatedRoute, private confirmationService: ConfirmationService,
     private resourceReportService: ResourceReportService, private masterDataService: MasterDataService, 
     private messageService: MessageService, private commonService: CommonService) {
    this.reportsFilterData = new ReportsFilterData();
    this.reportsFilterData.utilizationReportFilterData = new UtilizationReportFilterData();
    this.PageSize = this.resources.PageSize;
    this.PageDropDown = this.resources.PageDropDown;
  }


  ngOnInit() {
    this.resetFilter();
    this.allColumns();
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
    this.FrommonthOptions = [];
    for (let i = 0; i < this.months.length; i++) {
      this.FrommonthOptions.push({ label: this.months[i].Name, value: this.cols[i].Id });
    }

    this.columnOptions = [];
    for (let i = 0; i < this.cols.length; i++) {
      this.columnOptions.push({ label: this.cols[i].Name, value: this.cols[i].Id });
    }
  }
  public allColumns() {
    this.cols = [
      // { field: 'EmployeeId', header: 'ID', value: 0 },
      { field: 'EmployeeCode', header: 'Employee Code', value: 0 },
      { field: 'EmployeeName', header: 'Name', value: 0 },
      { field: 'JoinDate', header: 'JoinDate' },
      { field: 'ReleaseDate', header: 'ReleaseDate' },
      { field: 'January', header: 'January', value: 1 },
      { field: 'February', header: 'February', value: 2 },
      { field: 'March', header: 'March', value: 3 },
      { field: 'April', header: 'April', value: 4 },
      { field: 'May', header: 'May', value: 5 },
      { field: 'June', header: 'June', value: 6 },
      { field: 'July', header: 'July', value: 7 },
      { field: 'August', header: 'August', value: 8 },
      { field: 'September', header: 'September', value: 9 },
      { field: 'October', header: 'October', value: 10 },
      { field: 'November', header: 'November', value: 11 },
      { field: 'December', header: 'December', value: 12 },
      { field: 'AverageUtilizationpercentage', header: 'Avg(%) Utilization', value: 0 },

    ];
  }
  OpenConfirmationDialog() {   // method to open dialog
    this.confirmationService.confirm({
      message: 'Do you want to clear ?',
      accept: () => {
        this.resetFilter()
      },
      reject: () => {

      }
    });
  }
resetFilter(): void {
  this.afterSearchFilter = false;
  this.reportsFilterData.utilizationReportByMonthFilterData = new utilizationReportByMonthFilterData();
  this.reportsFilterData.utilizationReportByMonthFilterData.isExportToExcel = false;
}

RequiredMonths(userInput){
  let date = new Date();
  let year = date.getFullYear();
  let month = date.getMonth() + 1; //The getMonth() method returns the month (from 0 to 11) for the specified date, according to local time.
  this.FrommonthOptions = [];
  if(userInput == year){    
    this.reportsFilterData.utilizationReportByMonthFilterData.FromMonth = 0;
    this.reportsFilterData.utilizationReportByMonthFilterData.ToMonth = 0; 
    for (let i = 0; i < month; i++) {
      this.FrommonthOptions.push({ label: this.months[i].Name, value: this.months[i].Id });
    }
  }
  else{    
    for (let i = 0; i < this.months.length; i++) {
      this.FrommonthOptions.push({ label: this.months[i].Name, value: this.months[i].Id });
    }
  }
}
  onFilter(): void {
    this.resetFilter();
    this.columnOptions = [];
    this.FrommonthOptions.splice(0, 0, { label: 'Select Months', value: 0 });
    this.filterDisplay = true;
    this.FrommonthOptions = [];
    for (let i = 0; i < this.months.length; i++) {
      this.FrommonthOptions.push({ label: this.months[i].Name, value: this.months[i].Id });
    }
  }


  requiredColumns() {
    this.allColumns();
    let cols1 = this.cols;
    this.cols = [];
    for (let i = 0; i < cols1.length; i++) {
      if (cols1[i].value == 0 || (cols1[i].value >= this.reportsFilterData.utilizationReportByMonthFilterData.FromMonth &&
        cols1[i].value <= this.reportsFilterData.utilizationReportByMonthFilterData.ToMonth)) {
        this.cols.push(cols1[i]);
      }
    }
  }
  onsearchByFilter(): void {
    this.associateUtilizationReportList = [];
    this.afterSearchFilter = true;
    // this.filterDisplay = false;
    this.reportsFilterData.utilizationReportByMonthFilterData.RowsPerPage = 20;
    this.reportsFilterData.utilizationReportByMonthFilterData.PageNumber = 1;
    this.requiredColumns();
    this.searchByFilter(false);
  }


  searchByFilter(isExportRequiered: boolean): void {
    this.reportsFilterData.utilizationReportByMonthFilterData.isExportToExcel = isExportRequiered;

    if(this.reportsFilterData.utilizationReportByMonthFilterData.FromMonth > this.reportsFilterData.utilizationReportByMonthFilterData.ToMonth){
      this.messageService.add({ severity: 'warn', summary: 'Warn message', detail: 'From Month should be less than to month' });
      return;
    }
    if (this.reportsFilterData.utilizationReportByMonthFilterData 
      && this.reportsFilterData.utilizationReportByMonthFilterData.FromMonth != 0
     && this.reportsFilterData.utilizationReportByMonthFilterData.ToMonth != 0
    )
      this.resourceReportService.GetUtilizationReportsByMonth(this.reportsFilterData).subscribe((resourceReportResponse: ReportsFilterData) => {
    
  
        
        if (resourceReportResponse.reportsData){
          this.associateUtilizationReportList = resourceReportResponse.reportsData;
          this.totalRecordsCount = resourceReportResponse.TotalCount;
          this.filterDisplay = false;
        }
      }),
        (error: any) => {
          if (error._body != undefined && error._body != "")
            // this.commonService.LogError(this.componentName, error._body).then((data: any) => {
            // });

            //  this.growlerrormessages("error", "Failed to get Resource Report By Month List", "");
            this.messageService.add({ severity: 'error', summary: 'Warn message', detail: 'Failed to get Resource Report By Month List' });
        };
        this.afterSearchFilter = true;
  }

  onlyNumbers(event: any) {
    this.commonService.onlyNumbers(event);
    //this.RequiredMonths();
};

checkYear(event){
 
  if( event > 200 )
  {
    this.RequiredMonths(event);
  }
}

 

  // private exportData(): void {
  //   this.reportsFilterData.utilizationReportByMonthFilterData.PageNumber = 1;
  //   this.exportToExcel(true);
  // };
  // private exportToExcel(isExportRequiered: boolean): void {

  //   this.reportsFilterData.utilizationReportByMonthFilterData.isExportToExcel = isExportRequiered;

  //   if (this.reportsFilterData.utilizationReportByMonthFilterData && this.reportsFilterData.utilizationReportByMonthFilterData.FromMonth != 0 && this.reportsFilterData.utilizationReportByMonthFilterData.ToMonth != 0)
  //     this.resourceReportService.GetUtilizationReportsByMonth(this.reportsFilterData).subscribe((resourceReportResponse: ReportsFilterData) => {
  //       this.associateUtilizationReportExcel = resourceReportResponse.reportsData;
  //       this.totalRecordsCount = resourceReportResponse.TotalCount;
  //       let requirecolumns: string;
  //       requirecolumns = '';
  //       for (let i = 0; i < this.cols.length; i++) {
  //         if (this.cols[i].value != 0) {
  //           if (requirecolumns == '')
  //             requirecolumns = this.cols[i].field;
  //           else
  //             requirecolumns = requirecolumns + ',' + this.cols[i].field;
  //         }
  //       }
  //       if (this.associateUtilizationReportExcel != null && this.associateUtilizationReportExcel.length > 0) {
  //         alasql('SELECT EmployeeId, EmployeeCode,EmployeeName,' + requirecolumns + ',AverageUtilizationpercentage INTO XLSX("ResourceReportByMonth_Report.xlsx",{headers:true}) FROM ?', [this.associateUtilizationReportExcel]);
  //       }
  //     }),
  //       (error: any) => {
  //         if (error._body != undefined && error._body != "")
  //           this.commonService.LogError(this.componentName, error._body).then((data: any) => {
  //           });

  //         this.growlerrormessages("error", "Failed to get Resource Report by Month List", "");
  //       };
  // }
}
