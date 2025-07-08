import { Component, OnInit, ViewChild } from '@angular/core';
import { ResignastionService } from '../../../AssociateExit/Services/resignastion.service';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { RevokeDialogComponent } from '../../../AssociateExit/Components/revoke-dialog/revoke-dialog.component';
import { AssociateExitDashboardService } from '../../services/associate-exit-dashboard.service';
import * as moment from 'moment';
import { transition } from '@angular/animations';
import { KtFormService } from '../../services/kt-form.service';
import { KtFormWithSubStatus } from '../../models/kt-form.model';
import { RevokeResignation } from '../../../AssociateExit/Models/associateExit.model';
import { MatSnackBar } from '@angular/material/snack-bar';
import { NgxSpinnerService } from 'ngx-spinner';
import { ViewDetailedStatusComponent } from '../view-detailed-status/view-detailed-status.component';

@Component({
  selector: 'app-associate-dashboard',
  templateUrl: './associate-dashboard.component.html',
  styleUrls: ['./associate-dashboard.component.scss']
})
export class AssociateDashboardComponent implements OnInit {

  resignationSubmitted = false;
  showTransitionPlan = false;
  multipleKTPlans = false;
  UserRole: string;
  empId: number;
  dashboard: string;
  ProjectId: number;
  revokeResignationData: RevokeResignation;
  pageload = false;
  disableRevokeButton = false;
  dialogResponse: boolean;

  displayedColumns: string[] = ['Status', 'RevokeStatus', 'ExitDate', 'Action'];
  displayedColumnsKTPlan: string[] = ['Project', 'Status', 'TransitionPlan'];
  dataSource: MatTableDataSource<any>;
  dataSourceKTPlan: MatTableDataSource<any>;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  departmentName: string;
  departmentId: number;

  constructor(
    private spinner: NgxSpinnerService,
    private _associateExitDashboardService: AssociateExitDashboardService,
    private _KtFormService: KtFormService,
    private _resignationService: ResignastionService,
    private _snackBar: MatSnackBar,
    public dialog: MatDialog) { }

  ngOnInit(): void {
    this.spinner.show();
    if (sessionStorage.getItem('AssociatePortal_UserInformation') != null) {
      const currentRole = JSON.parse(
        sessionStorage.getItem('AssociatePortal_UserInformation')
      ).roleName;
      this.UserRole = currentRole;
      this.empId = JSON.parse(
        sessionStorage.getItem('AssociatePortal_UserInformation')
      ).employeeId;
    }
    this.departmentId = JSON.parse(
      sessionStorage["AssociatePortal_UserInformation"]
    ).departmentId;
    let RolesAndDepartments = JSON.parse(sessionStorage['RolesAndDepartments'])
    RolesAndDepartments.forEach(e => {
      if(this.departmentId === e.DepartmentId){
        this.departmentName = e.DepartmentCode
      }
    })
    if(this.departmentName === 'Delivery'){
      this.dashboard = "DeliveryDashboard";
    }
    else{
      this.dashboard = "ServiceDashboard";
    }
    this.getAssociateExitDetails();
  }

  getAssociateExitDetails() {
    if (this.empId) {
      this._associateExitDashboardService.getAssociateExitDashbaord(this.UserRole, this.empId, this.dashboard).toPromise().then(
        (res: any[]) => {
          if (res.length) {
            this.resignationSubmitted = true;

            res.forEach(e => {
              e.ExitDate = moment(e.ExitDate).format('MM/DD/YYYY');
              if (e.RevokeStatusCode === 'RevokeInitiated') {
                this.disableRevokeButton = true;
              }
              if (e.RevokeStatusCode === 'RevokeRejected') {
                this.displayedColumns = ['Status', 'RevokeStatus', 'RejectReason', 'ExitDate'];
              }
              if(e.RevokeStatusCode === null){
                this.displayedColumns = ['Status', 'ExitDate', 'Action']
              }
              this._KtFormService.getKtPlansForAssociate(this.empId).subscribe((resKTPlan: KtFormWithSubStatus[]) => {
                if (resKTPlan.length === 1) {
                  resKTPlan.forEach(item => {
                    if ((item.Status === 'KTPlanInProgress' || item.Status === 'KTPlanSubmitted')) {
                      this.showTransitionPlan = true;
                      this.ProjectId = item.ProjectId;
                      if(e.RevokeStatusCode === 'RevokeRejected'){
                        this.displayedColumns = ['Status', 'RevokeStatus', 'RejectReason', 'ExitDate', 'TransitionPlan'];
                      }
                      else if(e.RevokeStatusCode === null){
                        this.displayedColumns = ['Status', 'ExitDate', 'Action', 'TransitionPlan']
                      }
                      else{
                        this.displayedColumns = ['Status', 'RevokeStatus', 'ExitDate', 'Action', 'TransitionPlan'];
                      }
                    }
                  });

                }
                else if (resKTPlan.length > 1) {
                  resKTPlan.forEach(item => {
                    if ((e.SubStatusCode) != null && item.Status === 'KTPlanInProgress' || item.Status === 'KTPlanSubmitted') {
                      this.multipleKTPlans = true;
                      this.dataSourceKTPlan = new MatTableDataSource(resKTPlan);
                      this.showTransitionPlan = true;
                      if(e.RevokeStatusCode === 'RevokeRejected'){
                        this.displayedColumns = ['Status', 'RevokeStatus', 'ExitDate', 'TransitionPlan'];
                      }
                      else if(e.RevokeStatusCode === null){
                        this.displayedColumns = ['Status', 'ExitDate', 'Action', 'TransitionPlan']
                      }
                      else{
                        this.displayedColumns = ['Status', 'RevokeStatus', 'ExitDate', 'Action', 'TransitionPlan'];
                      }
                    }
                    if (e.SubStatusCode === null && item.Status === null) {
                      item.Status = 'NotYetStarted';
                      this.showTransitionPlan = false;
                      this.displayedColumns = ['Status', 'ExitDate', 'Action'];

                    }
                  });


                }

              });


            });



            this.dataSource = new MatTableDataSource(res);
            this.dataSource.sort = this.sort;
            this.spinner.hide();
            this.pageload = true;
          }
          else {
            this.spinner.hide();
            this.pageload = true;
          }
        }).catch(error => {
          this.spinner.hide();
          this.pageload = true;

        });
    }



  }

  revokeResignation(submitType: string) {
    const dialogRef = this.dialog.open(RevokeDialogComponent, {
      height: '270px',
      width: '500px',
      disableClose: true,
      data: { value: submitType }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result.RevokeReason) {
        this.revokeResignationData = new RevokeResignation();
        this.revokeResignationData.EmployeeId = this.empId;
        this.revokeResignationData.Comment = 'Revoke Initiated'
        this.revokeResignationData.RevokeReason = result.RevokeReason;
        this.spinner.show();
        this._resignationService.revokeResignation(this.revokeResignationData).subscribe(res => {
          this.spinner.hide();
          if (res) {
            this._snackBar.open('Resignation Revoke Request sent successfully.', 'x', {
              duration: 1000,
              panelClass: ['success-alert'],
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
            this.getAssociateExitDetails();
          }
        },
          (error) => {
            this.spinner.hide();
            this._snackBar.open('Unable to revoke resignation', 'x', {
              duration: 1000,
              panelClass: ['error-alert'],
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
          });
      }
    });
  }
  ViewDetailedStatus(EmployeeId: number) {
    const dialogRef = this.dialog.open(ViewDetailedStatusComponent, {
      width: '500px',
      disableClose: true,
      data: { value: EmployeeId}
    });
    dialogRef.afterClosed().subscribe(result => {
      this.dialogResponse = result;
    })
  }
}
