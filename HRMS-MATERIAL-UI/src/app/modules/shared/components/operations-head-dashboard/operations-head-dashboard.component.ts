import { Component, OnInit, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import * as moment from "moment";
import { AssociateExitDashboardService } from '../../services/associate-exit-dashboard.service';
import { NgxSpinnerService } from "ngx-spinner";
import { PmApprovalService } from '../../../AssociateExit/Services/pm-approval.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ResignastionService } from '../../../AssociateExit/Services/resignastion.service';
import { MatDialog } from '@angular/material/dialog';
import { CommonDialogComponent } from '../common-dialog/common-dialog.component';
import { MatPaginator } from '@angular/material/paginator';

@Component({
  selector: 'app-operations-head-dashboard',
  templateUrl: './operations-head-dashboard.component.html',
  styleUrls: ['./operations-head-dashboard.component.scss']
})
export class OperationsHeadDashboardComponent implements OnInit {

  EmpId: number;
  roleName: string;
  dashboard: string = "OpsHeadDashboard";
  pageload: boolean = false;
  dialogResponse:boolean;

  dataSourceAssociateExit: MatTableDataSource<any>;
  @ViewChild(MatSort, { static: true }) sortAssociateExit: MatSort;
  @ViewChild('exitPaginator', { static: true }) exitPaginator: MatPaginator;

  displayColumnsAssociateExit = ['EmployeeCode','EmployeeName','Designation','ExitDate','StatusCode','Action'];

  constructor(
    private _associateExitDashboardService: AssociateExitDashboardService,
    private route: Router,
    private spinner: NgxSpinnerService,
    private _pmApprovalService : PmApprovalService,
    private _snackBar: MatSnackBar,
    private _resignationService : ResignastionService,
    public dialog: MatDialog,
  ) { }

  ngOnInit(): void {
    this.spinner.show();

    this.roleName = JSON.parse(
      sessionStorage["AssociatePortal_UserInformation"]
    ).roleName;
    this.EmpId = JSON.parse(
      sessionStorage["AssociatePortal_UserInformation"]
    ).employeeId;

    this.GetAssociateExitList();

  }

  GetAssociateExitList():void{
    this._associateExitDashboardService.getAssociateExitDashbaord(this.roleName, this.EmpId, this.dashboard).subscribe((res: any[]) => {
        res.forEach(e => { e.ExitDate = moment(e.ExitDate).format("MM/DD/YYYY");
        if(e.StatusCode == "RevokeInitiated"){
          this._resignationService.getExitDetailsById(e.EmployeeId).subscribe((res:any)=>{ 
            e.WithdrawReason = res.WithdrawReason;
          });
        } });
        setTimeout(()=>{this.dataSourceAssociateExit = new MatTableDataSource(res);
        this.dataSourceAssociateExit.paginator = this.exitPaginator;
        this.dataSourceAssociateExit.sort = this.sortAssociateExit;
        this.spinner.hide();
        this.pageload = true;
        }, 2000);
    }),
      error => {
        this.spinner.hide();
        this.pageload = true;
      };
    }
    

  viewResignationDets(selectedData : any){
    this.route.navigate(['/associateexit/pmApproval/' + selectedData.EmployeeId]);
  
  }

  getclearanceForm(selectedData : any){
    this.route.navigate(["/associateexit/deptchecklist/" + selectedData.EmployeeId]);
  }

  revokeResignation(selectedData : any, SubmitType : string) {
    if(SubmitType == "Approve")
      this.openDialog('Confirmation','Are you sure you want to Approve the Revoke?',selectedData,SubmitType);
    else
      this.openDialog('Confirmation','Are you sure you want to Reject the Revoke?',selectedData,SubmitType);
    }


    openDialog(Heading,Message,selectedData,SubmitType): void {
      const dialogRef = this.dialog.open(CommonDialogComponent, {
        width: '300px',
        disableClose: true,
        data: {heading: Heading, message: Message}
      });
  
      dialogRef.afterClosed().subscribe(result => {
         this.dialogResponse = result;
         if(this.dialogResponse==true){

  
                let submitData = {
                  EmployeeId : selectedData.EmployeeId,
                  SubmitType : SubmitType
                } 
                this._pmApprovalService.ApproveOrRejectRevoke(submitData).subscribe(res=>{
                  if(res){
                    if(submitData.SubmitType == 'Approve'){
                    this._snackBar.open('Revoke Approved successfully.', 'x', {
                      duration: 1000,
                      horizontalPosition: 'right',
                      verticalPosition: 'top',
                    });
                  }
                  else if(submitData.SubmitType == 'Reject'){
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

}
