import { Component, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { NgxSpinnerService } from 'ngx-spinner';
import { FileExporterService } from 'src/app/core/services/file-exporter.service';
import { themeconfig } from 'src/themeconfig';
import { parkingReportData, parkingReportObj, parkingReportResponse } from '../models/bookParking.model';
import { ParkingServiceService } from '../services/parking-service.service';

@Component({
  selector: 'app-parking-slot-report',
  templateUrl: './parking-slot-report.component.html',
  styleUrls: ['./parking-slot-report.component.scss']
})
export class ParkingSlotReportComponent implements OnInit {
  parkingFilterForm : FormGroup;
  themeConfigInput = themeconfig.formfieldappearances;
  today = new Date();
  parkingReportData : parkingReportData[] = [];
  displayColumns = ['Email','VehicleNumber','BookedDate', 'BookedTime','Location'];
  dataSource : MatTableDataSource<parkingReportData> = new MatTableDataSource([]);
  parkingReportObj:parkingReportObj;
  parkingLocations = ['GALAXY'];

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) matSort: MatSort;

  constructor(private fileExporterService:FileExporterService,private service:ParkingServiceService, private _snackBar : MatSnackBar,private spinner: NgxSpinnerService  ) { }

  ngOnInit(): void {
    this.createParkingFilter();
  }
  createParkingFilter(){
    this.parkingFilterForm = new FormGroup({
      startDate : new FormControl(null),
      endDate : new FormControl(null),
      location : new FormControl(null),
    })
  }

  applyFilter() {
    this.parkingReportObj  = new parkingReportObj();
    this.parkingReportObj.startDate = this.parkingFilterForm.value.startDate;
    this.parkingReportObj.enddate = this.parkingFilterForm.value.endDate;
    this.parkingReportObj.location = this.parkingFilterForm.value.location;
    this.spinner.show();
    this.service.getParkingSlotsReport(this.parkingReportObj).subscribe((res:parkingReportResponse)=>{
      this.parkingReportData = res.Items;
      this.dataSource = new MatTableDataSource(this.parkingReportData);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort  = this.matSort;
      this.spinner.hide();
    },(error)=>{
      this.spinner.hide();
      if(error.error.Items.length == 0){
        this._snackBar.open('No Records Found', 'x', {
          duration: 5000,
          panelClass: ['error-alert'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
      }
      else{
        this._snackBar.open('Failed to fetch the data', 'x', {
          duration: 5000,
          panelClass: ['error-alert'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
      }
      this.parkingReportData = [];
      this.dataSource = new MatTableDataSource(null);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort  = this.matSort;
    })
  }

  clearInput(){
    this.parkingFilterForm.reset();
    this.parkingReportData = [];
    this.dataSource = new MatTableDataSource(null);
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort  = this.matSort;
  }

  exportToExcel() {
    const columnsForExcel = this.parkingReportData.map(x => ({
      'Email': x.Email,
      'Vehicle Number': x.VehicleNumber,
      'Booked Date': x.BookedDate,
      'Booked Time': x.BookedTime,
      'Location': x.Location
    }));
    this.fileExporterService.exportToExcel(columnsForExcel, 'Parking Slots Report');
  }

}
