import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSnackBar, MatSnackBarHorizontalPosition, MatSnackBarVerticalPosition } from '@angular/material/snack-bar';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute, Router } from '@angular/router';
import * as moment from 'moment';
import { NgxSpinnerService } from 'ngx-spinner';
import { Observable } from 'rxjs';
import { FileExporterService } from 'src/app/core/services/file-exporter.service';
import { MasterDataService } from 'src/app/modules/master-layout/services/masterdata.service';
import { NavService } from 'src/app/modules/master-layout/services/nav.service';
import { AttendanceReportDialogComponent } from 'src/app/modules/reports/components/attendance-report-dialog/attendance-report-dialog.component';
import { AttendanceDetailReport, AttendanceDetailReportData, AttendanceReport, AttendanceReportFilter, SelectItem } from 'src/app/modules/reports/models/associate-attendance-report.model';
import { AssociateAttendanceReportService } from 'src/app/modules/reports/services/associate-attendance-report.service';
import { themeconfig } from 'src/themeconfig';
import * as servicePath from '../../../../core/service-paths';
import { BiometricAttendanceDialogComponent } from '../biometric-attendance-dialog/biometric-attendance-dialog.component';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-biometric-attendance-report',
  templateUrl: './biometric-attendance-report.component.html',
  styleUrls: ['./biometric-attendance-report.component.scss']
})
export class BiometricAttendanceReportComponent implements OnInit {

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
  displayedColumns: string[] = ['EmployeeCode','EmployeeName','ProjectName','ReportingManagerName','TotalDaysWorked','TotalWfoDays','TotalWfhDays', 'TotalLeavesTaken','Compliance%', "View"];
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
  BiometricAttendanceStartDate:Date;
  @ViewChild('sort') sort: MatSort;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;

  constructor(private associateAttendanceReportService: AssociateAttendanceReportService,
    private fileExporterService: FileExporterService,
    public navService: NavService,
    private spinner: NgxSpinnerService,
    private _snackBar: MatSnackBar,
    private fb: FormBuilder,
    public dialog: MatDialog,
    private masterDataService: MasterDataService,
    private router :  Router,
    private actRoute: ActivatedRoute
    ) {
this.searchData = new AttendanceReportFilter();
this.PageSize = this.resources.PageSize;
this.PageDropDown = this.resources.PageDropDown;

    this.BiometricAttendanceStartDate=environment.BiometricAttendanceStartDate;
    this.minDate=this.BiometricAttendanceStartDate;
    this.maxDate = new Date();

    this.empId = JSON.parse(
      sessionStorage.getItem("AssociatePortal_UserInformation")
    ).employeeId; 

    this.associateAttendanceReportService.IsDeliveryDepartmentforBiometric(this.empId).subscribe(
      (response: boolean) => {       
        this.isDeliveryDepartment = response;  
        if(this.isDeliveryDepartment === true)
this.displayedColumns = ['EmployeeCode','EmployeeName','ProjectName','ReportingManagerName','TotalDaysWorked','TotalWfoDays','TotalWfhDays','TotalLeavesTaken','Compliance%',"View"];
else
this.displayedColumns = ['EmployeeCode','EmployeeName','ReportingManagerName','TotalDaysWorked','TotalWfoDays','TotalWfhDays','TotalLeavesTaken','Compliance%',"View"]; 
        this.employeedataSource = new MatTableDataSource(this.lstEmployees);  
        this.paginator.pageIndex = 0;
        this.employeedataSource.paginator = this.paginator;     
        // this.employeedataSource.sort = this.sort       
                 
      },
      (error: any) => {  }
    );      

this.navService.currentSearchBoxData.subscribe(responseData => {
this.applyFilter(responseData);
});
}
FromDate:Date;
ToDate:Date;
AssociateName:string;
  ngOnInit(): void {  
    // this.router.navigate(['/associates/regularization'])
    this.actRoute.queryParams.subscribe(params => { this.FromDate = params.fromDate;this.ToDate=params.toDate });
   
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
this.associateAttendanceReportService.IsDeliveryDepartmentforBiometric(this.empId).subscribe(
  (response: boolean) => {       
    this.isDeliveryDepartment = response;               
  },
  (error: any) => {  }
);  

if(this.isDeliveryDepartment === true)
this.displayedColumns = ['EmployeeCode','EmployeeName','ProjectName','ReportingManagerName','TotalDaysWorked','TotalWfoDays','TotalWfhDays','TotalLeavesTaken','Compliance%',"View"];
else
this.displayedColumns = ['EmployeeCode','EmployeeName','ReportingManagerName','TotalDaysWorked','TotalWfoDays','TotalWfhDays','TotalLeavesTaken','Compliance%',"View"];

this.employeedataSource = new MatTableDataSource(this.lstEmployees);  
        this.paginator.pageIndex = 0;
        this.employeedataSource.paginator = this.paginator;     
        // this.employeedataSource.sort = this.sort       
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
      let date=new Date()
      fromdate=new Date(date.getFullYear(),date.getMonth(),1);
      if(fromdate<this.BiometricAttendanceStartDate)
      {
        fromdate=(this.BiometricAttendanceStartDate);
      }
    }
    this.myForm = this.fb.group({
      fromDate: [fromdate, [Validators.required]],
      toDate: [todate, [Validators.required]] ,          
    });   
    
    
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
    if(this.searchData.FromDate==undefined && this.searchData.ToDate==undefined)
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
    this.associateAttendanceReportService.GetBiometricAttendanceSummaryReport(filter).subscribe(
      (response: AttendanceReport[]) => {
        this.spinner.hide();
        this.lstEmployees = response;
        response.forEach((res) => {
          this.totalWorkingDays = res.TotalWorkingDays;
          this.totalHolidays = res.TotalHolidays;      
        });          
        this.employeedataSource = new MatTableDataSource(this.lstEmployees);  
        this.employeedataSource.paginator = this.paginator;     
        // this.employeedataSource.sort = this.sort       
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
        this.employeedataSource.paginator = this.paginator;
        // this.employeedataSource.sort = this.sort; 
      }
    } else {
      this.employeedataSource = new MatTableDataSource(this.lstEmployees);
      this.employeedataSource.paginator = this.paginator;
      // this.employeedataSource.sort = this.sort; 
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
    //  this.employeedataSource.sort = this.sort; 
     this.employeedataSource.paginator = this.paginator;
     this.totalRecordsCount = this.lstEmployees.length;  
     this.totalWorkingDays = 0;
     this.totalHolidays = 0;   
     }

     clearData() {     
      this.lstEmployees = [];
      this.employeedataSource = new MatTableDataSource(this.lstEmployees);  
      // this.employeedataSource.sort = this.sort; 
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
       'Total Days': x.TotalDaysWorked,
       'Total WFH Days' : x.TotalWFHDays,
       'Total WFO Days' : x.TotalWFODays,
       'Total Leaves' : x.TotalLeaves,
       'Compliance%':x.CompliancePrecentage

     }));
     this.fileExporterService.exportToExcel(columnsForExcel_delivery, "Associate Attendance Report");   
    }
     else
     {
      const columnsForExcel_delivery = this.employeedataSource.data.map(x => ({
        'ID': x.EmployeeCode,
        'Name': x.EmployeeName,                
        'Reporting Manager': x.ReportingManagerName,     
        'Total Days': x.TotalDaysWorked,
        'Total WFH Days' : x.TotalWFHDays,
        'Total WFO Days' : x.TotalWFODays,
        'Total Leaves' : x.TotalLeaves,
        'Compliance%':x.CompliancePrecentage  
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

    // this.spinner.show();
    this.lstAttendance = [];
    this.router.navigate(['associates/viewAttendance'])
    this.associateAttendanceReportService.GetBiometricAttendanceDetailReport(this.detailFilter).subscribe(
      (response: AttendanceDetailReport[]) => {
        // this.spinner.hide();
        this.lstAttendance = response;  
        this.dialogData = new AttendanceDetailReportData();
        this.dialogData.eventData =  this.lstAttendance;
        this.dialogData.filter = this.detailFilter; 
        this.dialogData.employeeName = employee.EmployeeName;
        this.dialogData.daysWorked = employee.TotalDaysWorked;
        this.associateAttendanceReportService.attendanceData.next(this.dialogData)   
        // const dialogRef = this.dialog.open(BiometricAttendanceDialogComponent, {
        //   width: '1200px',
        //   disableClose: true,
        //   data: this.dialogData
        // });         
      },
      (error: any) => { this.spinner.hide(); }
    );
    
   } 

}
