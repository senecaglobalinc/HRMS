import { Component, OnInit, ViewChild } from '@angular/core';
import {ProjectDetailsReportData} from '../../models/project-details-report';
import {ProjectdetailsService} from '../../services/projectdetails.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatTableDataSource } from '@angular/material/table';
import { MatTableExporterModule } from 'mat-table-exporter';
import * as servicePath from '../../../../core/service-paths';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { FileExporterService } from 'src/app/core/services/file-exporter.service';
import { NavService } from 'src/app/modules/master-layout/services/nav.service';

@Component({
  selector: 'app-projectdetails-report',
  templateUrl: './projectdetails-report.component.html',
  styleUrls: ['./projectdetails-report.component.scss']
})
export class ProjectdetailsReportComponent implements OnInit {

  projectDetailsReportList: ProjectDetailsReportData[] = [];
  dataSource: MatTableDataSource<ProjectDetailsReportData>;
  private resources = servicePath.API.PagingConfigValue;

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = ['ProjectCode','ProjectName','Technology', 'ProgramManager', 'Total', 'Billable','NonBillable', 'ClientName'];
  constructor(private projectDetailsService: ProjectdetailsService, private _snackBar: MatSnackBar, private fileExporterService:FileExporterService, public navService: NavService,) { 
    this.navService.currentSearchBoxData.subscribe((responseData) => {
      this.applyFilter(responseData);
    });
  }

  ngOnInit(): void {
    this.GetProjectDetails();
   
  }

  GetProjectDetails(): void{
    this.projectDetailsService.GetProjectDetailsForReport().subscribe(
      (res: ProjectDetailsReportData[]) => {
        if(res.length > 0 && res != null) {       
          this.projectDetailsReportList = res; 
          this.dataSource = new MatTableDataSource(this.projectDetailsReportList);
          this.dataSource.paginator = this.paginator;
          this.dataSource.sort = this.sort;
        }
        else {
          this._snackBar.open('No records found', 'x', {
            duration: 1000,
            panelClass:['success-alert'],
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
        }
      },
      (error: any) => {
        this._snackBar.open('Unable to get project details', 'x', {
          duration: 1000,
          panelClass:['error-alert'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
      }
    );
  }

  applyFilter(event: Event) {
    if (event) {
      const filterValue = (event.target as HTMLInputElement).value;
      if (filterValue){
        this.dataSource.filter = filterValue.trim().toLowerCase();
        }
        else{
        this.dataSource = new MatTableDataSource(this.projectDetailsReportList);
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
        this.dataSource.sortingDataAccessor = (data: any, sortHeaderId: string): string => {
          if (typeof data[sortHeaderId] === 'string') {
            return data[sortHeaderId].toLocaleLowerCase();
          }
        
          return data[sortHeaderId];
        };
        }
    } else {
      this.dataSource = new MatTableDataSource(this.projectDetailsReportList);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
      this.dataSource.sortingDataAccessor = (data: any, sortHeaderId: string): string => {
        if (typeof data[sortHeaderId] === 'string') {
          return data[sortHeaderId].toLocaleLowerCase();
        }
      
        return data[sortHeaderId];
      };
      }
  }

  exportAsXLSX(){
    this.fileExporterService.exportToExcel(this.dataSource.data, "Project Information Report");
  }
  

}
