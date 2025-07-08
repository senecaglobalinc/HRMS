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
  selector: 'app-manager-attendance-report',
  templateUrl: './manager-attendance-report.component.html',
  styleUrls: ['./manager-attendance-report.component.scss']
})
export class ManagerAttendanceReportComponent implements OnInit {
  
  themeAppearence = themeconfig.formfieldappearances;
  empId : number;  
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
  IsLeadership = false;
  projectId = 0;
  isDeliveryDepartment = true;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

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
this.navService.currentSearchBoxData.subscribe(responseData => {
this.applyFilter(responseData);
});
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
    if (sessionStorage.getItem("AssociatePortal_UserInformation") != null) {
      this.roleName = JSON.parse(
        sessionStorage.getItem("AssociatePortal_UserInformation")
      ).roleName;
      
      this.empId = JSON.parse(
        sessionStorage.getItem("AssociatePortal_UserInformation")
      ).employeeId;  
}

    this.associateAttendanceReportService.IsDeliveryDepartment(this.empId).subscribe(
      (response: boolean) => {       
        this.isDeliveryDepartment = response;    
        if(this.roleName === "HRA" || this.roleName === "HRM")
        this.displayedColumns = ['EmployeeCode','EmployeeName','DepartmentName','ReportingManagerName','TotalDaysWorked',"View"];
      else if(this.isDeliveryDepartment === true)
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
    }

  ngOnInit(): void {

    if (sessionStorage.getItem("AssociatePortal_UserInformation") != null) {
      this.roleName = JSON.parse(
        sessionStorage.getItem("AssociatePortal_UserInformation")
      ).roleName;
      
      this.empId = JSON.parse(
        sessionStorage.getItem("AssociatePortal_UserInformation")
      ).employeeId;  
}
this.associateAttendanceReportService.IsDeliveryDepartment(this.empId).subscribe(
  (response: boolean) => {       
    this.isDeliveryDepartment = response;  
    if(this.roleName === "HRA" || this.roleName === "HRM")
    this.displayedColumns = ['EmployeeCode','EmployeeName','DepartmentName','ReportingManagerName','TotalDaysWorked',"View"];
  else if(this.isDeliveryDepartment === true)
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
  



    this.getProjects(this.empId, this.roleName);
    this.getAssociates(this.empId, this.roleName, this.projectId, this.IsLeadership);
    this.myForm = this.fb.group({
      fromDate: ['', [Validators.required]],
      toDate: ['', [Validators.required]] ,
      ProjectId: ['', null] ,
      EmployeeName: ['', null] ,
      Leadership: [false, null],      
    });   

    this.totalWorkingDays = 0;
    this.totalHolidays = 0;
  }

  fetchAttendanceReport() {
    if (sessionStorage.getItem("AssociatePortal_UserInformation") != null) {
      this.roleName = JSON.parse(
        sessionStorage.getItem("AssociatePortal_UserInformation")
      ).roleName;
     
      this.associateName = JSON.parse(
        sessionStorage.getItem("AssociatePortal_UserInformation")
      ).fullName;
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
      this.searchData.ManagerName = this.associateName;
      this.searchData.ManagerId = this.empId;
    // this.searchData.EmployeeName = this.associateName;
      this.searchData.RoleName = this.roleName;
      this.searchData.isLeadership = this.IsLeadership;   
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
        this.paginator.pageIndex = 0;
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
        this.paginator.pageIndex = 0;
        this.employeedataSource.paginator = this.paginator;
        this.employeedataSource.sort = this.sort; 
      }
    } else {
      this.employeedataSource = new MatTableDataSource(this.lstEmployees);
      this.paginator.pageIndex = 0;
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
     this.paginator.pageIndex = 0;
     this.employeedataSource.paginator = this.paginator;
     this.totalRecordsCount = this.lstEmployees.length; 
     this.totalWorkingDays = 0;
     this.totalHolidays = 0;   
     this.projectId = 0;
     this.IsLeadership = false;
     this.getProjects(this.empId, this.roleName);
     this.getAssociates(this.empId, this.roleName, this.projectId, this.IsLeadership);
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

     if(this.roleName === "HRA" || this.roleName === "HRM")
     {
      const columnsForExcel = this.employeedataSource.data.map(x => ({
        'ID': x.EmployeeCode,
        'Name': x.EmployeeName,          
        'Department': x.DepartmentName,       
        'Reporting Manager': x.ReportingManagerName,     
        'Total Days': x.TotalDaysWorked     
      }));    
      this.fileExporterService.exportToExcel(columnsForExcel, "Associate Attendance Report"); 
     }
     else if(this.isDeliveryDepartment === true)
     {
     const columnsForExcel = this.employeedataSource.data.map(x => ({
       'ID': x.EmployeeCode,
       'Name': x.EmployeeName,          
       'Project': x.ProjectName,       
       'Reporting Manager': x.ReportingManagerName,     
       'Total Days': x.TotalDaysWorked     
     }));    
     this.fileExporterService.exportToExcel(columnsForExcel, "Associate Attendance Report");    
    }
    else
    {
      const columnsForExcel = this.employeedataSource.data.map(x => ({
        'ID': x.EmployeeCode,
        'Name': x.EmployeeName,              
        'Reporting Manager': x.ReportingManagerName,     
        'Total Days': x.TotalDaysWorked     
      }));    
      this.fileExporterService.exportToExcel(columnsForExcel, "Associate Attendance Report");   
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
          height: '700px',
          width: '1000px',
          disableClose: true,
          data: this.dialogData
        });         
      },
      (error: any) => { this.spinner.hide(); }
    );
    
   }

   getProjects(employeeId:number, roleName:string): void {
    this.associateAttendanceReportService.GetProjects(employeeId,roleName).subscribe((res: GenericType[]) => {
      let projList: any[] = [];
      this.projectsList = [];
     // projList.push({ label: '', value: null });
      res.forEach((element: GenericType) => {
        projList.push({ label: element.Name, value: element.Id });
      });
      this.projectsList = projList.filter(
        (project, index, arr) => arr.findIndex(t => t.value === project.value) === index);

        this.filteredProject = this.myForm.valueChanges.pipe(
          startWith(''),
          map((value) => this._filterProject(value))
          ); 
    },
      (error: any) => {
        this.errorMessage = [];
        this._snackBar.open('Failed to Get Project List!', 'x', {
          duration: 5000,
          horizontalPosition: this.horizontalPosition,
          verticalPosition: this.verticalPosition,
        });
      }
    );
  }

  private _filterProject(value) {   
    let filterValue;
    if (value && value.ProjectId) {
      filterValue = value.ProjectId.toLowerCase();
      return this.projectsList.filter((option) =>
        option.label.toLowerCase().includes(filterValue)
      );
    } else {
      return this.projectsList;
    }
  }

  clearField(fieldName) {    

    if (fieldName == 'EmployeeName') {
      this.myForm.controls.EmployeeName.setValue(null);       
      this.searchData.EmployeeName = null;
      this.searchData.EmployeeId = 0;
      this.clearData();
    }
    
    if (fieldName == 'ProjectId') {
      this.myForm.controls.ProjectId.setValue(null);
      this.searchData.ProjectId = null;      
      this.projectId = 0;
      this.getAssociates(this.empId, this.roleName, this.projectId, this.IsLeadership);
      this.clearData(); 
    }
  } 

  clearAssociateField()
  {
      this.myForm.controls.ProjectId.setValue(null);
      this.searchData.ProjectId = null;      
      this.projectId = 0;     
      this.clearData(); 
  }

  selectedChangeIds(frmCntrl, item) {
    if (frmCntrl == 'EmployeeName') {
      this.selectedEmployeeName = item.label;
      this.searchData.EmployeeName = this.selectedEmployeeName;
      this.searchData.EmployeeId = item.value;
      this.clearData(); 
    }
    
    else if (frmCntrl == 'ProjectId') {
      this.selectedProjectId = item.value;
      this.searchData.ProjectId = this.selectedProjectId;
      this.projectId = this.selectedProjectId;      
      this.getAssociates(this.empId, this.roleName, this.projectId, this.IsLeadership);
      this.clearData(); 
    }
    }

  private filterAssociates(value) {
    let filterValue;
    if (value && value.EmployeeName) {
      filterValue = value.EmployeeName.toLowerCase();
      return this.associatesList.filter((option) =>
        option.label.toLowerCase().includes(filterValue)
      );
    } else {
      return this.associatesList;
    }
  }  

  getAssociates(employeeId:number, roleName:string, projectId:number, isLeadership:boolean): void {
    this.associatesList = [];
    this.associateAttendanceReportService.GetAssociates(employeeId,roleName, projectId, isLeadership).subscribe((associateResponse: GenericType[]) => {
      associateResponse.forEach((associateResponse: GenericType) => {        
          this.associatesList.push({ label: associateResponse.Name, value: associateResponse.Id });
      });

      this.filteredAssociates = this.myForm.valueChanges.pipe(
        startWith(''),
        map((value) => this.filterAssociates(value))
      );
    }),
      (error: any) => {
        if (error._body != undefined && error._body != "")
          this._snackBar.open(error.error, 'x', {
            duration: 1000, panelClass: ['Failed to get Associate List'], horizontalPosition: 'right',
            verticalPosition: 'top'
          });
      };

  }

  CheckIsLeadership(event){
    this.IsLeadership = event.checked;
    this.clearAssociateField();
    this.clearField('EmployeeName');
    this.projectId = 0;
    this.getAssociates(this.empId, this.roleName, this.projectId, this.IsLeadership);
    this.searchData.isLeadership = this.IsLeadership;
  } 

}