import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { AssociateExitReportRequestForPm, AssociateExitReportResponse } from '../../models/associate-exit-report.model';
import { MatTableDataSource, MatTable } from '@angular/material/table';
import { AssociateExitReportService } from '../../services/associate-exit-report.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatOption } from '@angular/material/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ReplaySubject, Subject } from 'rxjs';
import { themeconfig } from '../../../../../themeconfig';
import { takeUntil } from 'rxjs/operators';
import { DatePipe } from '@angular/common';
import * as moment from 'moment';
import { TableUtil } from "../../services/exportUtil";
import * as servicePath from '../../../../core/service-paths';
import { NavService } from 'src/app/modules/master-layout/services/nav.service';
import { FileExporterService } from 'src/app/core/services/file-exporter.service';

@Component({
  selector: 'app-associate-exit-report',
  templateUrl: './associate-exit-report.component.html',
  styleUrls: ['./associate-exit-report.component.scss']
})
export class AssociateExitReportComponent implements OnInit {
  reportDetails: AssociateExitReportResponse[] = [];
  dataSource: MatTableDataSource<AssociateExitReportResponse>;
  Role:string;
  exitReportForm:FormGroup;
  themeConfigInput = themeconfig.formfieldappearances;
  exitReportRequestObj:AssociateExitReportRequestForPm = new AssociateExitReportRequestForPm();
  isProgramManager:boolean = true;
  private resources = servicePath.API.PagingConfigValue;  

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = ['AssociateCode','AssociateName','Grade','Gender','Department','TechnologyGroup', 'JoinDate',"ExitDate", 'Project', 'ProgramManager','ReportingManager', 'ExitType', 'ExitCause', 'RehireEligibility', 'LegalExit','ImpactOnClientDelivery','ServiceTenure','ServiceTenureWithSG','ServiceTenurePriorToSG','ServiceTenureRange','ServiceTenureWithSGRange','FinancialYear','Quarter'];
  constructor(private associateExitReportService: AssociateExitReportService, 
              private _snackBar: MatSnackBar,
              public navService: NavService,
              private fileExporterService:FileExporterService,
              private spinner: NgxSpinnerService,private formBuilder:FormBuilder) { }

  ngOnInit(): void {
    this.Role = JSON.parse(
      sessionStorage.AssociatePortal_UserInformation
    ).roleName

    if(this.Role == 'Program Manager'){
      this.createAssociateExitReportForm();
      this.isProgramManager = true;
    }
    else{
    this.GetAssociateExitReportDetails();
    this.isProgramManager = false;
    }   
  }

  createAssociateExitReportForm(){
    this.exitReportForm = this.formBuilder.group({
      startDate: [null, Validators.required],
      endDate: [null, Validators.required],
    })
  }
  GetAssociateExitReportDetailsForPM(): void {
    if(this.exitReportForm.valid){
    this.exitReportRequestObj.FromDate = moment(this.exitReportForm.value.startDate).format('YYYY-MM-DD');
    this.exitReportRequestObj.ToDate = moment(this.exitReportForm.value.endDate).format('YYYY-MM-DD');

    this.spinner.show();
    this.associateExitReportService.GetAssociateExitReportForPM(this.exitReportRequestObj).subscribe(
      (res: AssociateExitReportResponse[]) => {
        if(res.length > 0 && res != null) {       
          this.reportDetails = res; 
          this.dataSource = new MatTableDataSource(this.reportDetails);
          this.dataSource.paginator = this.paginator;
          this.dataSource.sort = this.sort;
          this.spinner.hide();
        }
        else {  
          this.reportDetails = [];
          this.dataSource = new MatTableDataSource(this.reportDetails);
          this.dataSource.paginator = this.paginator;
          this.dataSource.sort = this.sort;
          this.spinner.hide();
          this._snackBar.open('No records found', 'x', {
            duration: 1000,
            panelClass:['success-alert'],
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
        }
      },
      (error: any) => {    
        this.reportDetails = [];
          this.dataSource = new MatTableDataSource(this.reportDetails);
          this.dataSource.paginator = this.paginator;
          this.dataSource.sort = this.sort;    
        this.spinner.hide();
        this._snackBar.open('Unable to get Associate Exit details', 'x', {
          duration: 1000,
          panelClass:['error-alert'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
      }
    );

    this.navService.currentSearchBoxData.subscribe(responseData => {
      this.applyFilter(responseData);
    });
  }
  else{
    this._snackBar.open('Please Select Start Date and End Date', 'x', {
      duration: 1000,
      panelClass:['error-alert'],
      horizontalPosition: 'right',
      verticalPosition: 'top',
    });
  }
  }
    GetAssociateExitReportDetails(): void{   
    this.spinner.show();
    this.associateExitReportService.GetAssociateExitReport().subscribe(
      (res: AssociateExitReportResponse[]) => {
        if(res.length > 0 && res != null) {       
          this.reportDetails = res; 
          this.reportDetails.forEach((record: AssociateExitReportResponse) => {            
            record.ImpactOnClientDeliveryStr = (record.ImpactOnClientDelivery == true) ? 'Yes' : 'No'; 
            record.RehireEligibilityStr = (record.RehireEligibility == true) ? 'Yes' : 'No';
            record.LegalExitStr = (record.LegalExit == true) ? 'Yes' : 'No';           
          });
          this.dataSource = new MatTableDataSource(this.reportDetails);
          this.dataSource.paginator = this.paginator;
          this.dataSource.sort = this.sort;
          this.spinner.hide();
        }
        else {  
          this.spinner.hide();
          this._snackBar.open('No records found', 'x', {
            duration: 1000,
            panelClass:['success-alert'],
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
        }
      },
      (error: any) => {        
        this.spinner.hide();
        this._snackBar.open('Unable to get Associate Exit details', 'x', {
          duration: 1000,
          panelClass:['error-alert'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
      }
    );

    this.navService.currentSearchBoxData.subscribe(responseData => {
      this.applyFilter(responseData);
    });
  } 

  applyFilter(event: Event) {
    if (event) {
      const filterValue = (event.target as HTMLInputElement).value;
      this.dataSource.filter = filterValue.trim().toLowerCase();
    } else {
      this.dataSource = new MatTableDataSource(this.reportDetails);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
    }
  }

  exportAsXLSX() {    
    this.spinner.show();
    if(this.Role == 'Program Manager'){
        this.exportDataForPM();
    }
    else{
      this.exportDataForCorporate();
    }

    this.spinner.hide();
  }

  clearInput(evt: any, fieldName): void {
    if(fieldName=='startDate'){
      evt.stopPropagation();
      this.exitReportForm.get('startDate').reset();
    }
    if(fieldName=='endDate'){
      evt.stopPropagation();
      this.exitReportForm.get('endDate').reset();
    }
  }

  exportDataForCorporate(){
    const columnsForExcel = this.dataSource.data.map(x => ({
      'Associate Code': x.AssociateCode,
      'Associate Name': x.AssociateName,
      'Grade': x.Grade,
      'Gender': x.Gender,
      'Department': x.Department,
      'Technology Group': x.TechnologyGroup,     
      'Project': x.Project,
      'Program Manager': x.ProgramManager,
      'Reporting Manager': x.ReportingManager, 
      'Join Date':  (moment(x.JoinDate).format('MM/DD/YYYY')),
      'Exit Date': (moment(x.ExitDate).format('MM/DD/YYYY')),
      'Exit Type': x.ExitType,
      'Exit Cause': x.ExitCause,
      'Eligible for Rehire': x.RehireEligibilityStr,
      'Impact On Client Delivery': x.ImpactOnClientDeliveryStr,
      'Legal Exit': x.LegalExitStr,
      'Service Tenure':x.ServiceTenure,
      'Service Tenure With SG':x.ServiceTenureWithSG,
      'Service Tenure Prior To SG':x.ServiceTenurePriorToSG,
      'Service Tenure Range':x.ServiceTenureRange,
      'Service Tenure With SG Range':x.ServiceTenureWithSGRange,
      'Financial Year': x.FinancialYear,
      'Quarter': x.Quarter,      
    }));
    this.fileExporterService.exportToExcel(columnsForExcel, "Associate Exit Report");  
  }

  exportDataForPM(){
    const columnsForExcel = this.dataSource.data.map(x => ({
      'Associate Name': x.AssociateName,
      'Grade': x.Grade,
      'Gender': x.Gender,
      'Department': x.Department,
      'Technology Group': x.TechnologyGroup,     
      'Program Manager': x.ProgramManager,
      'Reporting Manager': x.ReportingManager, 
      'Join Date':  (moment(x.JoinDate).format('MM/DD/YYYY')),
      'Exit Date': (moment(x.ExitDate).format('MM/DD/YYYY'))
    }));
    this.fileExporterService.exportToExcel(columnsForExcel, "Associate Exit Report");  
  }

  clearFilter(){
    this.exitReportForm.reset();
    this.reportDetails = [];
    this.dataSource = new MatTableDataSource(this.reportDetails);
  }

}

