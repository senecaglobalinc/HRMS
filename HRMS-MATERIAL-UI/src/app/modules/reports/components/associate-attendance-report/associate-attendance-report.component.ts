import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { AssociateAttendanceReportService } from '../../services/associate-attendance-report.service';
import {
  AttendanceReport,
  AttendanceReportFilter,
  AttendanceDetailReport, 
  SelectItem,
  AttendanceDetailReportData
} from '../../models/associate-attendance-report.model';
//import { GenericType, GenericModel } from '../../../../modules/master-layout/models/dropdowntype.model';
import { Validators, FormGroup, FormBuilder, FormGroupDirective } from '@angular/forms';
import { themeconfig } from '../../../../../themeconfig';
import {
  HttpClient,
  HttpHeaders,
  HttpErrorResponse,
} from '@angular/common/http';
import * as servicePath from '../../../../core/service-paths';
import { take } from 'rxjs/operators';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import {
  MatSnackBar,
  MatSnackBarHorizontalPosition,
  MatSnackBarVerticalPosition,
} from '@angular/material/snack-bar';

import { Chart } from 'chart.js';
import { MatTableDataSource } from '@angular/material/table';
import { AllColorCodes } from '../../../../core/color-codes';
import { NavService } from '../../../master-layout/services/nav.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { FileExporterService } from 'src/app/core/services/file-exporter.service';
import { ReportsData } from '../../models/reportsdata.model';
import * as moment from 'moment';
import { AttendanceReportDialogComponent } from 'src/app/modules/reports/components/attendance-report-dialog/attendance-report-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { ProjectDetails } from '../../../onboarding/models/projects.model';
import { MasterDataService } from '../../../master-layout/services/masterdata.service';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { MatOption } from '@angular/material/core';
import { DropDownType, GenericType, GenericModel } from '../../../master-layout/models/dropdowntype.model';
import { Console } from 'console';
@Component({
  selector: 'app-associate-attendance-report',
  templateUrl: './associate-attendance-report.component.html',
  styleUrls: ['./associate-attendance-report.component.scss']
})
export class AssociateAttendanceReportComponent implements OnInit { 
  
  empId:number;
  themeAppearence = themeconfig.formfieldappearances;
  filter:AttendanceReportFilter;
  detailFilter:AttendanceReportFilter;
  lstEmployees: AttendanceReport[] = []; 
  lstAttendance: AttendanceDetailReport[] = []; 
  componentName: string; myForm: FormGroup;
  totalRecordsCount: number;
  errorMessage: any[];
  searchFormSubmitted: boolean = false;  
  options: any;
  searchData: AttendanceReportFilter; 
  errorSummary: string = '';
  step=0;
  loading: boolean;
  public reportStatus: boolean = false;
  isLoading: boolean = false;
  firstDate: Date;
  lastDate: Date;
  PageSize: number;
  PageDropDown: number[] = [];
  roleName :string;
  associateName : string;
  employeeCode: string;
  private resources = servicePath.API.PagingConfigValue;
  displayedColumns: string[] = ['EmployeeCode','EmployeeName','ProjectName','ReportingManagerName','TotalDaysWorked',"View"];
  projectsList: any[] = [];
  filteredProject : Observable<any>;
  selectedProjectId:any;
  filteredAssociates: Observable<any>;
  associatesList: SelectItem[] = [];
  selectedEmployeeName: any;
  dialogData : AttendanceDetailReportData;
  horizontalPosition: MatSnackBarHorizontalPosition = 'right';
  verticalPosition: MatSnackBarVerticalPosition = 'top';
  employeedataSource: MatTableDataSource<AttendanceReport>;
  minDate : Date;
  maxDate : Date;
  totalWorkingDays : number;
  totalHolidays: number;
  isDeliveryDepartment = true;
  @ViewChild('sort') sort: MatSort;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;

  constructor(private associateAttendanceReportService: AssociateAttendanceReportService,
    private fileExporterService: FileExporterService,
    public navService: NavService,
    private spinner: NgxSpinnerService,
    private _snackBar: MatSnackBar,
    private fb: FormBuilder,
    public dialog: MatDialog,
    private masterDataService: MasterDataService
    ) {
this.searchData = new AttendanceReportFilter();
this.PageSize = this.resources.PageSize;
this.PageDropDown = this.resources.PageDropDown;

let date: Date = new Date();
date.setDate(date.getDate() - 7);
    this.minDate = new Date(2022, 6, 1);
    this.maxDate = date;

    this.associateAttendanceReportService.GetAttendanceMaxDate().subscribe(
      (response: Date) => {       
        this.maxDate = response;               
      },
      (error: any) => {  }
    );

    this.empId = JSON.parse(
      sessionStorage.getItem("AssociatePortal_UserInformation")
    ).employeeId; 

    this.associateAttendanceReportService.IsDeliveryDepartment(this.empId).subscribe(
      (response: boolean) => {       
        this.isDeliveryDepartment = response;  
        if(this.isDeliveryDepartment === true)
this.displayedColumns = ['EmployeeCode','EmployeeName','ProjectName','ReportingManagerName','TotalDaysWorked',"View"];
else
this.displayedColumns = ['EmployeeCode','EmployeeName','ReportingManagerName','TotalDaysWorked',"View"]; 
        this.employeedataSource = new MatTableDataSource(this.lstEmployees);  
        this.paginator.pageIndex = 0;
        this.employeedataSource.paginator = this.paginator;     
        this.employeedataSource.sort = this.sort       
                 
      },
      (error: any) => {  }
    );      

this.navService.currentSearchBoxData.subscribe(responseData => {
this.applyFilter(responseData);
});
}

  ngOnInit(): void {  
    
    if (sessionStorage.getItem("AssociatePortal_UserInformation") != null) {
      this.roleName = JSON.parse(
        sessionStorage.getItem("AssociatePortal_UserInformation")
      ).roleName;
      
      this.empId = JSON.parse(
        sessionStorage.getItem("AssociatePortal_UserInformation")
      ).employeeId;   
      this.totalWorkingDays = 0;
      this.totalHolidays = 0;
}
this.associateAttendanceReportService.IsDeliveryDepartment(this.empId).subscribe(
  (response: boolean) => {       
    this.isDeliveryDepartment = response;               
  },
  (error: any) => {  }
);  

if(this.isDeliveryDepartment === true)
this.displayedColumns = ['EmployeeCode','EmployeeName','ProjectName','ReportingManagerName','TotalDaysWorked',"View"];
else
this.displayedColumns = ['EmployeeCode','EmployeeName','ReportingManagerName','TotalDaysWorked',"View"];

this.employeedataSource = new MatTableDataSource(this.lstEmployees);  
        this.paginator.pageIndex = 0;
        this.employeedataSource.paginator = this.paginator;     
        this.employeedataSource.sort = this.sort       

    this.myForm = this.fb.group({
      fromDate: ['', [Validators.required]],
      toDate: ['', [Validators.required]] ,          
    });    
  }

  fetchAttendanceReport() {
    if (sessionStorage.getItem("AssociatePortal_UserInformation") != null) {
      this.roleName = JSON.parse(
        sessionStorage.getItem("AssociatePortal_UserInformation")
      ).roleName;
     
      this.associateName = JSON.parse(
        sessionStorage.getItem("AssociatePortal_UserInformation")
      ).fullName;

      this.employeeCode = JSON.parse(
        sessionStorage.getItem("AssociatePortal_UserInformation")
      ).employeeCode;
    }
    
    if(this.myForm.valid){
      this.step = 1;
    }   
    this.isLoading = true;
    this.searchFormSubmitted = true;
    if (this.myForm.controls.fromDate.value === '' || this.myForm.controls.toDate.value === '' || this.myForm.controls.fromDate.value === null || this.myForm.controls.toDate.value === null) {
      this.isLoading = false;     
      return;
    }    
    
    this.spinner.show()
    if (this.searchData.FromDate != null && this.searchData.ToDate != null) {
      this.errorSummary = '';
      if (moment(this.searchData.FromDate).isAfter(new Date())) {
        this.errorSummary = 'From date should be less than today';
        this.isLoading = false;
        this.spinner.hide()
        this._snackBar.open('From date should be less than today', 'x', {
          duration: 5000,
          panelClass: ['error-alert'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });

        return false;
      }
      if (moment(this.searchData.ToDate).isAfter(new Date())) {
        this.errorSummary = 'To date should be less than today';
        this.isLoading = false;
        this.spinner.hide()
        this._snackBar.open('To date should be less than today', 'x', {
          duration: 5000,
          panelClass: ['error-alert'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
        return false;
      }
      if (moment(this.searchData.FromDate).isAfter(this.searchData.ToDate)) {
        this.errorSummary = 'From Date should be less than To Date';
        this.isLoading = false;
        this.spinner.hide()
        this._snackBar.open('From Date should be less than To Date', 'x', {
          duration: 5000,
          panelClass: ['error-alert'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
        return false;
      }      
      
      this.searchData.FromDate = moment(this.searchData.FromDate).format('YYYY-MM-DD');
      this.searchData.ToDate = moment(this.searchData.ToDate).format('YYYY-MM-DD');      
      this.searchData.EmployeeName = this.associateName;
      this.searchData.EmployeeCode = this.employeeCode;
      this.searchData.EmployeeId = this.empId;
      this.searchData.ManagerName = this.associateName;
      this.searchData.RoleName = this.roleName;      
      this.getAttendanceSummary(this.searchData);      
    }
  }

  getAttendanceSummary(filter: AttendanceReportFilter) {
    this.spinner.show();
    
    this.lstEmployees = [];
    this.associateAttendanceReportService.GetAttendanceSummaryReport(filter).subscribe(
      (response: AttendanceReport[]) => {
        this.spinner.hide();
        this.lstEmployees = response;
        response.forEach((res) => {
          this.totalWorkingDays = res.TotalWorkingDays;
          this.totalHolidays = res.TotalHolidays;      
        });          
        this.employeedataSource = new MatTableDataSource(this.lstEmployees);  
        this.employeedataSource.paginator = this.paginator;     
        this.employeedataSource.sort = this.sort       
      },
      (error: any) => { this.spinner.hide(); }
    );
  }

  getAttendanceDetail(filter: AttendanceReportFilter) {
    this.spinner.show();
    this.lstAttendance = [];
    this.associateAttendanceReportService.GetAttendanceDetailReport(filter).subscribe(
      (response: AttendanceDetailReport[]) => {
        this.spinner.hide();
        this.lstAttendance = response;            
      },
      (error: any) => { this.spinner.hide(); }
    );
  }

  applyFilter(event: Event) {
    if (event) {
      const filterValue = (event.target as HTMLInputElement).value;
      if (filterValue) {
        this.employeedataSource.filter = filterValue.trim().toLowerCase();
      } else {
        this.employeedataSource = new MatTableDataSource(this.lstEmployees);
        this.employeedataSource.paginator = this.paginator;
        this.employeedataSource.sort = this.sort; 
      }
    } else {
      this.employeedataSource = new MatTableDataSource(this.lstEmployees);
      this.employeedataSource.paginator = this.paginator;
      this.employeedataSource.sort = this.sort; 
    }
  }

  clearInput(evt: any, fieldName): void {
    if(fieldName=='toDate'){
      evt.stopPropagation();
      this.myForm.get('toDate').reset();
      this.clearData();
    }
    if(fieldName=='fromDate'){
      evt.stopPropagation();
      this.myForm.get('fromDate').reset();
      this.clearData();
    }
  }

  clearFilter() {
    this.myForm.reset();
    this.myForm.clearValidators(); 
     this.searchFormSubmitted = false;
     this.errorSummary = '';    
     this.searchData = new AttendanceReportFilter();
     this.lstEmployees = [];
     this.employeedataSource = new MatTableDataSource(this.lstEmployees);  
     this.employeedataSource.sort = this.sort; 
     this.employeedataSource.paginator = this.paginator;
     this.totalRecordsCount = this.lstEmployees.length;  
     this.totalWorkingDays = 0;
     this.totalHolidays = 0;   
     }

     clearData() {     
      this.lstEmployees = [];
      this.employeedataSource = new MatTableDataSource(this.lstEmployees);  
      this.employeedataSource.sort = this.sort; 
      this.paginator.pageIndex = 0;
      this.employeedataSource.paginator = this.paginator;
      this.totalRecordsCount = this.lstEmployees.length; 
      this.totalWorkingDays = 0;
      this.totalHolidays = 0;               
      }
  
   exportAsXLSX() {    
     this.spinner.show();
     if(this.isDeliveryDepartment == true)
     {
     const columnsForExcel_delivery = this.employeedataSource.data.map(x => ({
       'ID': x.EmployeeCode,
       'Name': x.EmployeeName,          
       'Project': x.ProjectName,       
       'Reporting Manager': x.ReportingManagerName,     
       'Total Days': x.TotalDaysWorked     
     }));
     this.fileExporterService.exportToExcel(columnsForExcel_delivery, "Associate Attendance Report");   
    }
     else
     {
      const columnsForExcel_delivery = this.employeedataSource.data.map(x => ({
        'ID': x.EmployeeCode,
        'Name': x.EmployeeName,                
        'Reporting Manager': x.ReportingManagerName,     
        'Total Days': x.TotalDaysWorked     
      }));
      this.fileExporterService.exportToExcel(columnsForExcel_delivery, "Associate Attendance Report"); 
     } 
     this.spinner.hide();
    }

   viewAttendanceDetails(employee: AttendanceReport)
   {
    this.detailFilter = new AttendanceReportFilter();
    this.detailFilter.EmployeeName =employee.EmployeeName;
    this.detailFilter.EmployeeCode = employee.EmployeeCode;
    this.detailFilter.FromDate = this.searchData.FromDate;
    this.detailFilter.ToDate = this.searchData.ToDate;
    this.detailFilter.ProjectId = 0;

    this.spinner.show();
    this.lstAttendance = [];
    this.associateAttendanceReportService.GetAttendanceDetailReport(this.detailFilter).subscribe(
      (response: AttendanceDetailReport[]) => {
        this.spinner.hide();
        this.lstAttendance = response;  
        this.dialogData = new AttendanceDetailReportData();
        this.dialogData.eventData =  this.lstAttendance;
        this.dialogData.filter = this.detailFilter; 
        this.dialogData.employeeName = employee.EmployeeName;
        this.dialogData.daysWorked = employee.TotalDaysWorked;     
        const dialogRef = this.dialog.open(AttendanceReportDialogComponent, {
          width: '1100px',
          disableClose: true,
          data: this.dialogData
        });         
      },
      (error: any) => { this.spinner.hide(); }
    );
    
   } 
}
