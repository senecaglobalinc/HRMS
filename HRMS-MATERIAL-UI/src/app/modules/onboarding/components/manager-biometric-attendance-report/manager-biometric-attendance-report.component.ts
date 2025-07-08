import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { AssociateAttendanceReportService } from '../../../reports/services/associate-attendance-report.service';
import {
  AttendanceReport,
  AttendanceReportFilter,
  AttendanceDetailReport, 
  SelectItem,
  AttendanceDetailReportData
} from '../../../reports/models/associate-attendance-report.model';
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
import { ReportsData } from '../../../reports/models/reportsdata.model';
import * as moment from 'moment';
import { AttendanceReportDialogComponent } from 'src/app/modules/reports/components/attendance-report-dialog/attendance-report-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { ProjectDetails } from '../../../onboarding/models/projects.model';
import { MasterDataService } from '../../../master-layout/services/masterdata.service';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { MatOption } from '@angular/material/core';
import { DropDownType, GenericType, GenericModel } from '../../../master-layout/models/dropdowntype.model';
import { BiometricAttendanceDialogComponent } from '../biometric-attendance-dialog/biometric-attendance-dialog.component';
import { ActivatedRoute, Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import saveAs from 'file-saver';


@Component({
  selector: 'app-manager-biometric-attendance-report',
  templateUrl: './manager-biometric-attendance-report.component.html',
  styleUrls: ['./manager-biometric-attendance-report.component.scss']
})
export class ManagerBiometricAttendanceReportComponent implements OnInit {

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
  displayedColumns: string[] = ['EmployeeCode','EmployeeName','ProjectName','ReportingManagerName','TotalDaysWorked','TotalWfoDays','TotalWfhDays','TotalLeavesTaken','Compliance%',"View"];
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
  FromDate:Date;
  ToDate:Date;
  BiometricAttendanceStartDate:Date;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  constructor(private associateAttendanceReportService: AssociateAttendanceReportService,
    private fileExporterService: FileExporterService,
    public navService: NavService,
    private spinner: NgxSpinnerService,
    private _snackBar: MatSnackBar,
    private fb: FormBuilder,
    public dialog: MatDialog,
    private masterDataService: MasterDataService,
    private router : Router,
    private actRoute: ActivatedRoute
    ) {      
this.searchData = new AttendanceReportFilter();
this.PageSize = this.resources.PageSize;
this.PageDropDown = this.resources.PageDropDown;
this.navService.currentSearchBoxData.subscribe(responseData => {
this.applyFilter(responseData);
});
   this.BiometricAttendanceStartDate=environment.BiometricAttendanceStartDate;
    this.minDate = this.BiometricAttendanceStartDate;
    this.maxDate = new Date();

    if (sessionStorage.getItem("AssociatePortal_UserInformation") != null) {
      this.roleName = JSON.parse(
        sessionStorage.getItem("AssociatePortal_UserInformation")
      ).roleName;
      
      this.empId = JSON.parse(
        sessionStorage.getItem("AssociatePortal_UserInformation")
      ).employeeId;  
}

    this.associateAttendanceReportService.IsDeliveryDepartmentforBiometric(this.empId).subscribe(
      (response: boolean) => {       
        this.isDeliveryDepartment = response;    
        if(this.roleName === "HRA" || this.roleName === "HRM")
        this.displayedColumns = ['EmployeeCode','EmployeeName','DepartmentName','ReportingManagerName','TotalDaysWorked','TotalWfoDays','TotalWfhDays','TotalLeavesTaken',"View"];
      else if(this.isDeliveryDepartment === true)
this.displayedColumns = ['EmployeeCode','EmployeeName','ProjectName','ReportingManagerName','TotalDaysWorked','TotalWfoDays','TotalWfhDays','TotalLeavesTaken',"View"];
else
this.displayedColumns = ['EmployeeCode','EmployeeName','ReportingManagerName','TotalDaysWorked','TotalWfoDays','TotalWfhDays','TotalLeavesTaken',"View"];           
this.employeedataSource = new MatTableDataSource(this.lstEmployees);  
        this.paginator.pageIndex = 0;
        this.employeedataSource.paginator = this.paginator;     
        this.employeedataSource.sort = this.sort       
      },
      (error: any) => {  }
    );    
    }

  ngOnInit(): void {
    this.actRoute.queryParams.subscribe(params => { this.FromDate = params.fromDate;this.ToDate=params.toDate });
  
    if (sessionStorage.getItem("AssociatePortal_UserInformation") != null) {
      this.roleName = JSON.parse(
        sessionStorage.getItem("AssociatePortal_UserInformation")
      ).roleName;
      
      this.empId = JSON.parse(
        sessionStorage.getItem("AssociatePortal_UserInformation")
      ).employeeId;  
}
this.associateAttendanceReportService.IsDeliveryDepartmentforBiometric(this.empId).subscribe(
  (response: boolean) => {       
    this.isDeliveryDepartment = response;  
    if(this.roleName === "HRA" || this.roleName === "HRM")
    this.displayedColumns = ['EmployeeCode','EmployeeName','DepartmentName','ReportingManagerName','TotalDaysWorked','TotalWfoDays','TotalWfhDays','TotalLeavesTaken','Compliance%',"View"];
  else if(this.isDeliveryDepartment === true)
this.displayedColumns = ['EmployeeCode','EmployeeName','ProjectName','ReportingManagerName','TotalDaysWorked','TotalWfoDays','TotalWfhDays','TotalLeavesTaken','Compliance%',"View"];
else
this.displayedColumns = ['EmployeeCode','EmployeeName','ReportingManagerName','TotalDaysWorked','TotalWfoDays','TotalWfhDays','TotalLeavesTaken','Compliance%',"View"];
this.employeedataSource = new MatTableDataSource(this.lstEmployees);  
this.paginator.pageIndex = 0;
this.employeedataSource.paginator = this.paginator;     
this.employeedataSource.sort = this.sort              
  },
  (error: any) => {  }
);  
  



    this.getProjects(this.empId, this.roleName);
    this.getAssociates(this.empId, this.roleName, this.projectId, this.IsLeadership);
    let fromdate=new Date();
    let todate=new Date();   
    if(this.FromDate!=null ||this.ToDate!=null)
    {
      fromdate=new Date(this.FromDate);
      todate=new Date(this.ToDate);
    }
    else
    {
      // fromdate.setDate(fromdate.getDate()-14);
      let date=new Date();
      fromdate=new Date(date.getFullYear(),date.getMonth(),1);
      if(fromdate<this.BiometricAttendanceStartDate)
      {
        fromdate=(this.BiometricAttendanceStartDate);
      }
    }
    this.myForm = this.fb.group({
      fromDate: [fromdate, [Validators.required]],
      toDate: [todate, [Validators.required]] ,
      ProjectId: ['', null] ,
      EmployeeName: ['', null] ,
      Leadership: [false, null],      
    });   

    this.totalWorkingDays = 0;
    this.totalHolidays = 0;
    this.fetchAttendanceReport();
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
    if(this.searchData.FromDate == undefined && this.searchData.ToDate == undefined)
    {
      this.searchData.FromDate=this.myForm.controls.fromDate.value;
      this.searchData.ToDate=this.myForm.controls.toDate.value;
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
    this.associateAttendanceReportService.GetBiometricAttendanceSummaryReport(filter).subscribe(
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
    this.associateAttendanceReportService.GetBiometricAttendanceDetailReport(filter).subscribe(
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
        'Total Days': x.TotalDaysWorked,
        'Total WFH Days' : x.TotalWFHDays,
       'Total WFO Days' : x.TotalWFODays,
       'Total Leaves' : x.TotalLeaves,
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
       'Total Days': x.TotalDaysWorked,
       'Total WFH Days' : x.TotalWFHDays,
       'Total WFO Days' : x.TotalWFODays,
       'Total Leaves' : x.TotalLeaves  
     }));   
     this.fileExporterService.exportToExcel(columnsForExcel, "Associate Attendance Report");    
    }
    else
    {
      const columnsForExcel = this.employeedataSource.data.map(x => ({
        'ID': x.EmployeeCode,
        'Name': x.EmployeeName,              
        'Reporting Manager': x.ReportingManagerName,     
        'Total Days': x.TotalDaysWorked,
        'Total WFH Days' : x.TotalWFHDays,
       'Total WFO Days' : x.TotalWFODays    
      }));    
      this.fileExporterService.exportToExcel(columnsForExcel, "Associate Attendance Report");   
    }
    
     this.spinner.hide();
   }

   exportAdvanceAsXLSX() {    
    this.spinner.show();
    this.lstAttendance = [];
      this.searchData.FromDate = moment(this.searchData.FromDate).format('YYYY-MM-DD');
      this.searchData.ToDate = moment(this.searchData.ToDate).format('YYYY-MM-DD');
      this.searchData.ManagerName = this.associateName;
      this.searchData.ManagerId = this.empId;
      this.searchData.RoleName = this.roleName;
      this.searchData.isLeadership = this.IsLeadership;
      this.associateAttendanceReportService.GetAdvanceAttendanceReport(this.searchData).subscribe(
      (response:any) => {
        const byteCharacters = atob(response.FileContents);
        const byteNumbers = new Array(byteCharacters.length);
        for (let i = 0; i < byteCharacters.length; i++) {
          byteNumbers[i] = byteCharacters.charCodeAt(i);
        }
        const byteArray = new Uint8Array(byteNumbers);
        
        const blob = new Blob([byteArray], { type: response.ContentType });
        saveAs(blob, response.FileDownloadName || "AttendanceReport.xls");
        this.spinner.hide();       
      },
      (error: any) => { this.spinner.hide(); }
    );

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

    this.router.navigate([],
      {
        queryParamsHandling: 'merge',
        replaceUrl: true
       }).then(
        () => {
          this.router.navigate(['associates/viewAttendance'], { queryParams: {'employeeName':employee.EmployeeName} });
        }
       );
    this.associateAttendanceReportService.GetBiometricAttendanceDetailReport(this.detailFilter).subscribe(
      (response: AttendanceDetailReport[]) => {
        this.spinner.hide();
        this.lstAttendance = response;  
        this.dialogData = new AttendanceDetailReportData();
        this.dialogData.eventData =  this.lstAttendance;
        this.dialogData.filter = this.detailFilter;      
        this.dialogData.employeeName = employee.EmployeeName;
        this.dialogData.daysWorked = employee.TotalDaysWorked;
        this.associateAttendanceReportService.attendanceData.next(this.dialogData)   
        // const dialogRef = this.dialog.open(BiometricAttendanceDialogComponent, {
        //   height: '700px',
        //   width: '1200px',
        //   disableClose: true,
        //   data: this.dialogData
        // });         
      },
      (error: any) => { this.spinner.hide(); }
    );
    
   }

   getProjects(employeeId:number, roleName:string): void {
    this.associateAttendanceReportService.GetProjectsForBiometricAttendance(employeeId,roleName).subscribe((res: GenericType[]) => {
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
    this.associateAttendanceReportService.GetAssociatesForBiometricAttendance(employeeId,roleName, projectId, isLeadership).subscribe((associateResponse: GenericType[]) => {
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
