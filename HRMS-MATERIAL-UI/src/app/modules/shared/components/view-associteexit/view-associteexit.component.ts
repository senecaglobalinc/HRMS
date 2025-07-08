import { THIS_EXPR } from '@angular/compiler/src/output/output_ast';
import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import * as moment from 'moment';
import { NgxSpinnerService } from 'ngx-spinner';
import { AssociateExit } from '../../../AssociateExit/Models/associateExit.model';
import { ChecklistService } from '../../../AssociateExit/Services/checklist.service';
import { PmApprovalService } from '../../../AssociateExit/Services/pm-approval.service';
import { ResignastionService } from '../../../AssociateExit/Services/resignastion.service';
import { NavService } from '../../../master-layout/services/nav.service';
import { CommonDialogComponent } from '../common-dialog/common-dialog.component';
import { AssociateExitDashboardService } from '../../services/associate-exit-dashboard.service';
import { MessageDialogComponent } from 'src/app/modules/project-life-cycle/components/message-dialog/message-dialog.component';
import { RevokeDialogComponent } from 'src/app/modules/AssociateExit/Components/revoke-dialog/revoke-dialog.component';
import { ViewDetailedStatusComponent } from '../view-detailed-status/view-detailed-status.component';

@Component({
  selector: 'app-view-associteexit',
  templateUrl: './view-associteexit.component.html',
  styleUrls: ['./view-associteexit.component.scss']
})
export class ViewAssociteexitComponent implements OnInit {

  EmpId: number;
  roleName: string;
  dashboard: string;
  allDashboards =
    {
      'Program Manager': 'PMDashboard',
       HRM: 'ActiveResignationsDashboard',
       HRA: 'HRADashboard',
      'Admin Manager': 'SHDashboard',
      'Finance Manager': 'SHDashboard',
      'IT Manager': 'SHDashboard',
      'Training Department Head': 'SHDashboard',
      'Quality and Information Security Manager': 'SHDashboard',
      'Corporate': 'CorporateDashboard'
      // "Team Lead":"TLDashboard"
    };

  @ViewChild('exitPaginator', { static: false }) exitPaginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  dataSourceAssociateExit: MatTableDataSource<any>;
  dialogResponse: boolean;
  displayColumnsAssociateExit = ['EmployeeCode', 'EmployeeName', 'Designation', 'ExitDate', 'StatusCode', 'RevokeStatus', 'View', 'Action'];
  pageload = false;
  isLoading = false;
  associateList: AssociateExit[];
  moment: any = moment;
  // currentDate: moment.Moment;
  currentDate:string;

  constructor(
    private route: Router,
    private _snackBar: MatSnackBar,
    public navService: NavService,
    private _resignationService: ResignastionService,
    private _pmApprovalService: PmApprovalService,
    private _associateExitDashboardService: AssociateExitDashboardService,
    private _checkListService: ChecklistService,
    public dialog: MatDialog,
    private spinner: NgxSpinnerService
  ) {
    this.navService.currentSearchBoxData.subscribe((responseData) => {
      this.applyFilter(responseData);
    });
    this.currentDate = moment(new Date()).format('MM/DD/YYYY');
  }

  ngOnInit() {
    this.spinner.show();
    this.roleName = JSON.parse(
      sessionStorage.AssociatePortal_UserInformation
    ).roleName;
    this.EmpId = JSON.parse(
      sessionStorage.AssociatePortal_UserInformation
    ).employeeId;
    this.dashboard = this.allDashboards[this.roleName];
    this.GetAssociateExitList();
  }

  applyFilter(event: Event) {
    if (event) {
      const filterValue = (event.target as HTMLInputElement).value;
      this.dataSourceAssociateExit.filter = filterValue.trim().toLowerCase();
    } else {
      this.dataSourceAssociateExit = new MatTableDataSource(this.associateList);
      this.dataSourceAssociateExit.paginator = this.exitPaginator;
      this.dataSourceAssociateExit.sort = this.sort;
    }
  }


  GetAssociateExitList(): void {
    this._associateExitDashboardService.getAssociateExitDashbaord(this.roleName, this.EmpId, this.dashboard).subscribe((res: any[]) => {
    res.forEach(e => {
        e.ExitDate = moment(e.ExitDate).format('MM/DD/YYYY');
        if (e.RevokeStatusCode === 'RevokeInitiated') {
          this._resignationService.getExitDetailsById(e.EmployeeId).subscribe((response: any) => {
            e.WithdrawReason = response.WithdrawReason;
          });
        }
        this.spinner.hide();
      });
      this.associateList = res;
      this.dataSourceAssociateExit = new MatTableDataSource(this.associateList);
      this.dataSourceAssociateExit.sort = this.sort;
      this.dataSourceAssociateExit.paginator = this.exitPaginator;
    });

  }
  revokeResignation(selectedData: any, SubmitType: string, Comment: string) {
    const dialogRef = this.dialog.open(RevokeDialogComponent, {
      height: '270px',
      width: '500px',
      disableClose: true,
      data: { value: SubmitType}
    });
    dialogRef.afterClosed().subscribe(result => {
      this.dialogResponse = result;
      if (this.dialogResponse) {


        const submitData = {
          EmployeeId: selectedData.EmployeeId,
          SubmitType,
          Comment,
          RevokeReason: result.RevokeReason
        };
        this.spinner.show();
        this._pmApprovalService.ApproveOrRejectRevoke(submitData).subscribe(res => {
          this.spinner.hide();
          if (res) {
            if (submitData.SubmitType === 'Approve') {
              this._snackBar.open('Revoke Approved successfully.', 'x', {
                duration: 1000,
                horizontalPosition: 'right',
                verticalPosition: 'top',
              });
            }
            else if (submitData.SubmitType === 'Reject') {
              this._snackBar.open('Revoke Rejected successfully.', 'x', {
                duration: 1000,
                horizontalPosition: 'right',
                verticalPosition: 'top',
              });

            }
            this.GetAssociateExitList();
          }
        });

      }
    });
    // if (SubmitType == "Approve")
    //   this.openDialog('Confirmation', 'Are you sure you want to Approve the Revoke?', selectedData, SubmitType);
    // else
    //   this.openDialog('Confirmation', 'Are you sure you want to Reject the Revoke?', selectedData, SubmitType);
  }
  openDialog(Heading, Message, selectedData, SubmitType): void {
    const dialogRef = this.dialog.open(CommonDialogComponent, {
      width: '300px',
      disableClose: true,
      data: { heading: Heading, message: Message }
    });

    dialogRef.afterClosed().subscribe(result => {
      this.dialogResponse = result;
      if (this.dialogResponse === true) {
        const submitData = {
          EmployeeId: selectedData.EmployeeId,
          SubmitType
        };
        this.spinner.show();
        this._pmApprovalService.ApproveOrRejectRevoke(submitData).subscribe(res => {
          this.spinner.hide();
          if (res) {
            if (submitData.SubmitType === 'Approve') {
              this._snackBar.open('Revoke Approved successfully.', 'x', {
                duration: 1000,
                horizontalPosition: 'right',
                verticalPosition: 'top',
              });
            }
            else if (submitData.SubmitType === 'Reject') {
              this._snackBar.open('Revoke Rejected successfully.', 'x', {
                duration: 1000,
                horizontalPosition: 'right',
                verticalPosition: 'top',
              });

            }
            this.GetAssociateExitList();
          }
        });

      }
    });
  }
  viewResignationDets(selectedData: any) {
    this.route.navigate(['/associateexit/pmApproval/' + selectedData.EmployeeId]);

  }
  abscondForm() {
    this.route.navigate(['/associateexit/associateabscondform']);

  }

  getExitChecklist(selectedData: any) {
    this.route.navigate(['/associateexit/view/' + selectedData.EmployeeId]);
  }

  getCleranceForm(selectedData: any) {
    this.route.navigateByUrl('/associateexit/deptchecklist/' + selectedData.EmployeeId, { state: selectedData.ExitDate });

  }
  Associateexitform(selectedData: any) {
    // this.route.navigate(["/associateexit/view/" + selectedData.EmployeeId]);

    this.route.navigate(['/associateexit/exitfeedback/' + selectedData.EmployeeId]);
  }

  onList(selectedData: any) {
    this.route.navigate(['/associateexit/deptchecklist/' + selectedData.EmployeeId]);

  }

  initiateactivities(selectedData: any) {
    this.spinner.show();
      this._checkListService.CreateActivityChecklist(selectedData.EmployeeId, this.EmpId).toPromise()
      .then(() => {
        this.spinner.hide();
        this._snackBar.open('Initiated Successfully', 'x', {
          duration: 1000,
          panelClass: ['success-alert'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
        this.GetAssociateExitList();
      }).catch(
    error => {
      this.spinner.hide();
          this._snackBar.open('Error initiating activities.', 'x', {
            duration: 1000,
            panelClass: ['error-alert'],
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
        });
    // }
  }
  PMSubmissionScreen(selectedData){
    this.route.navigate(['/associateexit/PMSubmission/' + selectedData.EmployeeId]);
  }
  ViewDetailedStatus(EmployeeId: number) {
    const dialogRef = this.dialog.open(ViewDetailedStatusComponent, {
      width: '500px',
      // disableClose: true,
      data: { value: EmployeeId}
    });
    dialogRef.afterClosed().subscribe(result => {
      this.dialogResponse = result;
    })
  }
  SendReminder(element){
    this._pmApprovalService.ReviewReminderNotification(element.EmployeeId).subscribe(res =>{
      if(res['IsSuccessful'] === true){
        this._snackBar.open('Sent reminder successfully', 'x', {
          duration: 1000,
          panelClass: ['success-alert'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
      }
      else{
        this._snackBar.open('Error while sending reminder.', 'x', {
          duration: 1000,
          panelClass: ['error-alert'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
      }
    })

  }
}

