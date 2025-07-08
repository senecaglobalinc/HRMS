import { Component, OnInit, ViewChild, ViewChildren, QueryList } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ReasonForm, reasonData } from '../../models/reasonform.model';
// import { ReportsFilterData } from '../../models/reportsfilter.model';
// import { FinanceReportFilterData } from '../../models/reportsfilter.model';
import { Validators, FormGroup, FormBuilder, FormGroupDirective } from '@angular/forms';
import * as servicePath from '../../../../core/service-paths';
import * as moment from 'moment';
import { themeconfig } from '../../../../../themeconfig';
// import { MatTableExporterModule } from 'mat-table-exporter';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource, MatTable } from '@angular/material/table';
import {
  MatSnackBar,
  MatSnackBarHorizontalPosition,
  MatSnackBarVerticalPosition,
} from '@angular/material/snack-bar';

import { NavService } from '../../../master-layout/services/nav.service';
import { AssociateExitDashboardService } from '../../services/associate-exit-dashboard.service';

@Component({
  selector: 'app-reason-form',
  templateUrl: './reason-form.component.html',
  styleUrls: ['./reason-form.component.scss']
})
export class ReasonFormComponent implements OnInit {
  reasonlist : ReasonForm[];  
  EmployeeId: number;
  roleName: string;
  dashboard: string = "ServiceMangerDashboard";
  resources = servicePath.API.PagingConfigValue;
  dataSource = new MatTableDataSource();
causeForm: FormGroup;
  isLoading: boolean = false;
  searchFormSubmitted: boolean = false;
  searchData: reasonData;
  errorSummary: string = '';
  horizontalPosition: MatSnackBarHorizontalPosition = 'right';
  verticalPosition: MatSnackBarVerticalPosition = 'top';
  public reportStatus: boolean = false;
 reasonReport: reasonData[] = [];
 fromDate: Date;
 toDate: Date;
 themeConfigInput = themeconfig.formfieldappearances;




  @ViewChild(FormGroupDirective) formGroupDirective: FormGroupDirective;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
 
  displayedColumns: string[] = [
    'employeeId', 'employeeName', 'reason',
    'associatenumber', 'exitDate'
  ];

  constructor(
    public navService: NavService,
  
    private associateexitservice: AssociateExitDashboardService,

    private snackBar: MatSnackBar,


){
      this.navService.currentSearchBoxData.subscribe((responseData) => {
      });
     }

   ngOnInit() {
     this.getExitDashboard();
  }

  


  getExitDashboard() {

    this.roleName = JSON.parse(
        sessionStorage["AssociatePortal_UserInformation"]
    ).roleName;
    this.EmployeeId = JSON.parse(
        sessionStorage["AssociatePortal_UserInformation"]
    ).employeeId;
    this.associateexitservice
        .getAssociateExitDashbaord(this.roleName, this.EmployeeId, this.dashboard)//'Service Manager'
        .toPromise().then((res: reasonData[]) => {
        
            this.dataSource.paginator = this.paginator;
            this.dataSource.sort = this.sort;
        }).catch(error => {
        
        });
}

  apply() {
    this.isLoading = true;
    this.searchFormSubmitted = true;
    if (this.causeForm.controls.fromDate.value === '' || this.causeForm.controls.toDate.value === '' ){
      this.isLoading = false;
      return;
    }
    if (this.searchData.FromDate != null && this.searchData.ToDate != null) {
        this.errorSummary = '';
        if (moment(this.searchData.FromDate).isSameOrAfter(new Date())) {
            this.errorSummary = 'From date should be less than today';
           // this.messageService.add({ severity: 'error', summary: 'Warn message', detail: this.errorSummary });
            this.isLoading = false;
            this.snackBar.open('From date should be less than today', 'x', {
              duration: 5000,
              horizontalPosition: this.horizontalPosition,
              verticalPosition: this.verticalPosition,
          });

            return false;
        }
        if (moment(this.searchData.ToDate).isSameOrAfter(new Date())) {
            this.errorSummary = 'To date should be less than today';
           // this.messageService.add({ severity: 'error', summary: 'Warn message', detail: this.errorSummary });
            this.isLoading = false;
            this.snackBar.open('To date should be less than today', 'x', {
              duration: 5000,
              horizontalPosition: this.horizontalPosition,
              verticalPosition: this.verticalPosition,
          });
            return false;
        }
        if (moment(this.searchData.FromDate).isSameOrAfter(this.searchData.ToDate)) {
            this.errorSummary = 'From Date should be less than To Date';
           // this.messageService.add({ severity: 'error', summary: 'Warn message', detail: this.errorSummary });
            this.isLoading = false;
            this.snackBar.open('From Date should be less than To Date', 'x', {
              duration: 5000,
              horizontalPosition: this.horizontalPosition,
              verticalPosition: this.verticalPosition,
          });
            return false;
        }
       
        this.searchData = new reasonData();
        this.searchData.FromDate = moment(this.searchData.FromDate).toDate();
        this.searchData.ToDate = moment(this.searchData.ToDate).toDate();
        // this.reportsFilterData.financeReportFilterData.PageNumber = this.searchData.PageNumber;
        // this.reportsFilterData.financeReportFilterData.RowsPerPage = this.searchData.RowsPerPage;
        this.reportStatus = true;
      //  this.associateexitservice.GetFinanceReport(this.searchData).subscribe((reportsData: Array<reasonData>) => {
          //   if (reportsData){
          //   this.reportStatus = false;
          //   this.reasonReport = reportsData;
          //   this.reasonReport.forEach((record: ReasonList) => {
          //       record.FromDate = moment(record.FromDate).format('DD-MM-YYYY');
          //       record.ToDate = moment(record.ToDate).format('DD-MM-YYYY');
          //   });
          //   this.isLoading = false;
          //   this.dataSource = new MatTableDataSource<ReportsData>(this.financeReport);
          //   this.dataSource.paginator = this.paginator.toArray()[0];
          //   this.dataSource.sort = this.sort.toArray()[0];
          //   this.totalRecordsCount = this.financeReport.length;
          //   this.fromDate = moment(this.searchData.FromDate).toDate();
          //   this.toDate = moment(this.searchData.ToDate).toDate();
          //   this.filterDisplay = false;
          // }
      //     else{
      //       this.isLoading = false;
      //     }
      //   },
      //   (error: any) => {
      //     this.errorMessage = [];
      //     this.isLoading = false;
      //     this.snackBar.open('Project Resource Not Found', 'x', {
      //       duration: 5000,
      //       horizontalPosition: this.horizontalPosition,
      //       verticalPosition: this.verticalPosition,
      //   });
      // });
    }
  
}
clearFilter = function () {
  this.formGroupDirective.resetForm();
  this.myForm.controls.columnselect.setValue(this.selectedColumns);
  this.searchFormSubmitted = false;
  this.errorSummary = '';
  this.searchData = new reasonData();
}

}
