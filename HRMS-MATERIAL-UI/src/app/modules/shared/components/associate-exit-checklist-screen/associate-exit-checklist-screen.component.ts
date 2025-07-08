import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import * as moment from 'moment';
import { NgxSpinnerService } from 'ngx-spinner';
import { AssociateExit } from 'src/app/modules/AssociateExit/Models/associateExit.model';
import { ChecklistService } from 'src/app/modules/AssociateExit/Services/checklist.service';
import { PmApprovalService } from 'src/app/modules/AssociateExit/Services/pm-approval.service';
import { ResignastionService } from 'src/app/modules/AssociateExit/Services/resignastion.service';
import { NavService } from 'src/app/modules/master-layout/services/nav.service';
import { AssociateExitDashboardService } from '../../services/associate-exit-dashboard.service';
import { CommonDialogComponent } from '../common-dialog/common-dialog.component';

@Component({
  selector: 'app-associate-exit-checklist-screen',
  templateUrl: './associate-exit-checklist-screen.component.html',
  styleUrls: ['./associate-exit-checklist-screen.component.scss']
})
export class AssociateExitChecklistScreenComponent implements OnInit {

  EmpId: number;
  roleName: string;
  dashboard: string;
  dashboards = {
    HRM: 'ServiceMangerDashboard'
  };

  @ViewChild('exitPaginator', { static: false }) exitPaginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  dataSourceAssociateExit: MatTableDataSource<any>;
  dialogResponse: boolean;
  displayColumnsAssociateExit = ['EmployeeCode', 'EmployeeName', 'Designation', 'ExitDate', 'StatusCode', 'View', 'Action'];
  pageload = false;
  isLoading = false;
  associateList: AssociateExit[];

  constructor(
    private route: Router,
    private _snackBar: MatSnackBar,
    public navService: NavService,
    private spinner: NgxSpinnerService,
    private _resignationService: ResignastionService,
    private _pmApprovalService: PmApprovalService,
    private _associateExitDashboardService: AssociateExitDashboardService,
    private _checkListService: ChecklistService,
    public dialog: MatDialog
  ) {
    this.navService.currentSearchBoxData.subscribe((responseData) => {
      this.applyFilter(responseData);
    });
  }

  ngOnInit() {
    this.spinner.show();
    this.roleName = JSON.parse(
      sessionStorage.AssociatePortal_UserInformation
    ).roleName;
    this.EmpId = JSON.parse(
      sessionStorage.AssociatePortal_UserInformation
    ).employeeId;
    this.dashboard = this.dashboards[this.roleName];
    this.GetAssociateExitList();
  }

  applyFilter(event: Event) {
    // if (event) {
    //   const filterValue = (event.target as HTMLInputElement).value;
    //   this.dataSourceAssociateExit.filter = filterValue.trim().toLowerCase();
    // } else {
    //   this.dataSourceAssociateExit = new MatTableDataSource(this.dataSourceAssociateExit.data);
    // }
    // if (this.dataSourceAssociateExit.paginator) {
    //   this.dataSourceAssociateExit.paginator.firstPage();
    // }
    if (event) {
      const filterValue = (event.target as HTMLInputElement).value;
      this.dataSourceAssociateExit.filter = filterValue.trim().toLowerCase();
    } else {
      this.dataSourceAssociateExit = new MatTableDataSource(this.associateList);
      this.dataSourceAssociateExit.paginator = this.exitPaginator;
      this.dataSourceAssociateExit.sort = this.sort;
    }
    // this.dataSourceAssociateExit.paginator = this.exitPaginator;
    // this.dataSourceAssociateExit.sort = this.sort;
  }


  GetAssociateExitList(): void {
    this._associateExitDashboardService.getAssociateExitDashbaord(this.roleName, this.EmpId, this.dashboard).subscribe((res: any[]) => {
      res.forEach(e => {
        e.ExitDate = moment(e.ExitDate).format('MM/DD/YYYY');
      });
      this.spinner.hide();
      this.associateList = res;
      this.dataSourceAssociateExit = new MatTableDataSource(this.associateList);
      this.dataSourceAssociateExit.sort = this.sort;
      this.dataSourceAssociateExit.paginator = this.exitPaginator;
    });

  }
  
  abscondForm() {
    this.route.navigate(['/associateexit/associateabscondform']);

  }

  getExitChecklist(selectedData: any) {
    this.route.navigate(['/associateexit/view/' + selectedData.EmployeeId]);
  }

}
