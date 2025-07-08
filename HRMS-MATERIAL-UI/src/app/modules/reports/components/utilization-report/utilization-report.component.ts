import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { UtilizationReportResponse } from '../../models/utilization-report.model';
import { MatTableDataSource, MatTable } from '@angular/material/table';
import { UtilizationReportService } from '../../services/utilization-report.service';
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
  selector: 'app-utilization-report',
  templateUrl: './utilization-report.component.html',
  styleUrls: ['./utilization-report.component.scss']
})
export class UtilizationReportComponent implements OnInit {
  reportDetails: UtilizationReportResponse[] = [];
  dataSource: MatTableDataSource<UtilizationReportResponse>;
  private resources = servicePath.API.PagingConfigValue;  

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = ['AssociateCode','AssociateName','DateOfJoining',"EmploymentStartDate", 'Fresher', 'LastWorkingDate', 'ProjectsWorked', 'TotalWorkedDays', 'TimeTakenForFirstBilling','LastBillingDate', 'TotalBillingDays','TotalNonBillingDays','BillingDaysPercentage','Experience','Technology','Skills'];
  constructor(private utilizationReportService: UtilizationReportService, 
              private _snackBar: MatSnackBar,
              public navService: NavService,
              private fileExporterService:FileExporterService,
              private spinner: NgxSpinnerService) { }

  ngOnInit(): void {
    this.GetUtiliztionReportDetails();   
  }
    
    GetUtiliztionReportDetails(): void{   
    this.spinner.show();
    this.utilizationReportService.GetUtilizationReport().subscribe(
      (res: UtilizationReportResponse[]) => {
        if(res.length > 0 && res != null) {       
          this.reportDetails = res; 
          this.reportDetails.forEach((record: UtilizationReportResponse) => {            
            record.Resigned = (record.Active == true) ? 'Yes' : ''; 
            record.IsFresher = (record.Fresher == true) ? 'Yes' : '';             
          });

          this.dataSource = new MatTableDataSource(this.reportDetails.filter(entry => entry.Active === true));
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
        this._snackBar.open('Unable to get utilization details', 'x', {
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

  public CheckResigned(event :any) { 
    if(event.checked)   
    {
      this.dataSource = new MatTableDataSource(this.reportDetails);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
    }
      else
      {
        this.dataSource = new MatTableDataSource(this.reportDetails.filter(entry => entry.Active === true));
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
      }      
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
    const columnsForExcel = this.dataSource.data.map(x => ({
      'Associate Code': x.AssociateCode,
      'Associate Name': x.AssociateName,
      'Date Of Joining':  new Date(moment(x.DateOfJoining).format('MM/DD/YYYY')),
      'Employment Start Date': (x.EmploymentStartDate != null ? new Date(moment(x.EmploymentStartDate).format('MM/DD/YYYY')) : ''),
      'Last Working Date': (x.LastWorkingDate != null ? new Date(moment(x.LastWorkingDate).format('MM/DD/YYYY')) : ''),
      'Projects Worked': x.ProjectsWorked,      
      'Total Worked Days': x.TotalWorkedDays,
      'Days Taken For First Billing': x.TimeTakenForBillable,
      'Last Billing Date': (x.LastBillingDate != null ? new Date(moment(x.LastBillingDate).format('MM/DD/YYYY')) : ''),
      'Total Billing Days': x.TotalBillingDays,
      'Total Non-Billing Days': x.TotalNonBillingDays,
      'Billing Days (%)': x.BillingDaysPercentage,
      'Experience': x.ExperienceExcludingCareerBreak,
      'Technology': x.CompetencyGroup,
      'Skills': x.Skills,
      'Fresher': x.IsFresher
    }));
    this.fileExporterService.exportToExcel(columnsForExcel, "Utilization Report");    
    this.spinner.hide();
  }
}
