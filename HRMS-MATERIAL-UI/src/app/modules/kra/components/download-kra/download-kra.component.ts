import { Component, OnInit, ViewChild } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import {
  MatSnackBar,
  MatSnackBarHorizontalPosition,
  MatSnackBarVerticalPosition,
} from '@angular/material/snack-bar';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import * as servicePath from '../../../../core/service-paths';
import { FinancialYearRoleType } from 'src/app/modules/kra/models/financialyear-roletype.model';
import { FileDetail } from 'src/app/modules/kra/models/file-detail.model';
import { DownloadKraService } from 'src/app/modules/kra/services/download-kra.service';

@Component({
  selector: 'app-download-kra',
  templateUrl: './download-kra.component.html',
  styleUrls: ['./download-kra.component.scss']
})
export class DownloadKraComponent implements OnInit {
 loggedInEmployeeId:number;
 displayedColumns: string[] = [
    'FinancialYearName',
    'RoleTypeName',
    'DownloadKRA'
  ];
  length: number;
  PageSize: number;
  PageDropDown: number[] = [];
  pageSize = 5;
  pageSizeOptions: number[] = [5, 10, 20,30];
  dataSource = new MatTableDataSource<FinancialYearRoleType>(); 
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  resources = servicePath.API.PagingConfigValue;
  constructor(private _downloadKraService: DownloadKraService,  private _snackBar: MatSnackBar,
    private spinner: NgxSpinnerService,) {
      this.PageSize = this.resources.PageSize;
    this.PageDropDown = this.resources.PageDropDown;   
     }

  ngOnInit() {    
    this.loggedInEmployeeId = JSON.parse(sessionStorage["AssociatePortal_UserInformation"]).employeeId; 
    
    this.getRoleTypes(this.loggedInEmployeeId );  
 }

 public DownloadKRA(roleType :FinancialYearRoleType)
 { 

  this.spinner.show();
   this._downloadKraService.downloadKRA(roleType.EmployeeCode, roleType.FinancialYearName, roleType.RoleTypeName).subscribe((res: FileDetail) => {
    this.spinner.hide();
    if (res) {  
    const source = 'data:application/pdf;base64,' + res.Content;
    const link = document.createElement("a");
    link.href = source;
    link.download = res.FileName;
    link.click();    
    }    
   }, (error: any) => {
         this.spinner.hide();
         this._snackBar.open('Failed to get RoleType details.', '', {
           duration: 1000,
           horizontalPosition: 'right',
           verticalPosition: 'top',
         });        
       }
   );
 }
 
 private getRoleTypes(employeId: number): void {
   this.spinner.show();
   this._downloadKraService.getEmployeeRoleTypes(employeId).subscribe((res: FinancialYearRoleType[]) => {
    this.spinner.hide();
    if (res) {          
      this.length = res.length;
      this.dataSource = new MatTableDataSource(res);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
    }
    
   }, (error: any) => {
         this.spinner.hide();
         this._snackBar.open('Failed to get RoleType details.', '', {
           duration: 1000,
           horizontalPosition: 'right',
           verticalPosition: 'top',
         });        
       }
   );
 }

}
