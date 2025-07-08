import { Component, OnInit, ViewChild } from "@angular/core";
import { ProjectsData } from "src/app/modules/master-layout/models/projects.model";
import { AssociateExit } from '../../../AssociateExit/Models/associateExit.model';
import * as moment from "moment";
import { DeliveryHeadService } from "../../services/delivery-head.service";
import * as servicePath from "../../../../core/service-paths";
import { ProjectCreationService } from "../../../project-life-cycle/services/project-creation.service";
import { Router } from "@angular/router";
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { NavService } from 'src/app/modules/master-layout/services/nav.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { NgxSpinnerService } from "ngx-spinner";
import { Associate } from 'src/app/modules/onboarding/models/associate.model';
import { ResignastionService } from 'src/app/modules/AssociateExit/Services/resignastion.service';
import { AssociateExitDashboardService } from '../../services/associate-exit-dashboard.service';
import { CommonDialogComponent } from '../common-dialog/common-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { PmApprovalService } from 'src/app/modules/AssociateExit/Services/pm-approval.service';

@Component({
  selector: "app-program-manager-dashboard",
  templateUrl: "./program-manager-dashboard.component.html",
  styleUrls: ["./program-manager-dashboard.component.scss"]
})
export class ProgramManagerDashboardComponent implements OnInit {
  projectstate:string;
  EmpId: number;
  roleName: string;
  dashboard: string = "PMDashboard";
  ProjectsList: ProjectsData[];
  PageSize: number;
  PageDropDown: number[] = [];
  resources = servicePath.API.PagingConfigValue;
  dataSource = new MatTableDataSource<ProjectsData>();
  
  @ViewChild('projectdetailsPaginator', { static: false }) projectdetailsPaginator: MatPaginator;
  @ViewChild('exitPaginator', { static: false }) exitPaginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  EmployeesList : Associate[];

  cols = [
    { field: 'ProjectName', header: 'Project Name' },
    { field: 'ManagerName', header: 'Program Manager' },
    { field: 'PracticeAreaCode', header: 'Technology' },
    { field: 'ClientName', header: 'Client Name' },
    { field: 'ActualStartDate', header: 'Start Date' },
    { field: 'ProjectState', header: 'Status' },
  ];
  displayedColumns1: string[] = [
    'ProjectName',
    'ManagerName',
    'PracticeAreaCode',
    'ClientName',
    'ActualStartDate',
    'ProjectState',
    'View',
  ];
  dataSourceAssociateExit: MatTableDataSource<any>;
  dialogResponse:boolean;
  displayColumnsAssociateExit = ['EmployeeCode','EmployeeName','Designation','ExitDate','StatusCode','Action'];
  pageload: boolean = false;
  isLoading: boolean = false;
  associateList : AssociateExit[]; 

  constructor(
    private _deliveryHeadService: DeliveryHeadService,
    private route: Router,
    private ProjectCreationServiceObj: ProjectCreationService,
    private _snackBar: MatSnackBar,
    public navService: NavService,
    private spinner: NgxSpinnerService,
    private _resignationService: ResignastionService,
    private _pmApprovalService: PmApprovalService,
    private _associateExitDashboardService: AssociateExitDashboardService,
    public dialog: MatDialog
  ) {
    this.navService.currentSearchBoxData.subscribe((responseData) => {
      this.applyFilter(responseData);
    });
  }

  ngOnInit() {
    this.spinner.show();
    this.GetProjectsList();
    // this.GetAssociateExitList();
    this.roleName = JSON.parse(
      sessionStorage["AssociatePortal_UserInformation"]
    ).roleName;
    this.EmpId = JSON.parse(
      sessionStorage["AssociatePortal_UserInformation"]
    ).employeeId;
  }

  applyFilter(event: Event) {
    if (event) {
      const filterValue = (event.target as HTMLInputElement).value;
      this.dataSource.filter = filterValue.trim().toLowerCase();
    } else {
      this.dataSource = new MatTableDataSource(this.dataSource.data);
    }
    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
    this.dataSource.paginator = this.projectdetailsPaginator;
    this.dataSource.sort = this.sort;
  }
  

  GetProjectsList(): void {
    this.roleName = JSON.parse(
      sessionStorage["AssociatePortal_UserInformation"]
    ).roleName;
    this.EmpId = JSON.parse(
      sessionStorage["AssociatePortal_UserInformation"]
    ).employeeId;
    // this.EmpId = JSON.parse(sessionStorage['AssociatePortal_UserInformation']).employeeId;
    this._deliveryHeadService
      .GetProjectsList(this.roleName, this.EmpId, this.dashboard)
      .toPromise().then((res: ProjectsData[]) => {
        this.ModifyDateFormat(res);
        this.dataSource.data = res;
        this.dataSource.paginator = this.projectdetailsPaginator;
        this.dataSource.sort = this.sort;
        this.spinner.hide();
      }).catch(error=>{
        this.spinner.hide();
        // this._snackBar.open('An error has occurred. Please contact administration.', 'x', {
        //   duration: 2000,
        //   horizontalPosition: 'right',
        //   verticalPosition: 'top',
        // });
     });
  }

  ModifyDateFormat(data: ProjectsData[]): void {
    data.forEach(e => {
      if (e.ActualStartDate != null) {
        e.ActualStartDate = moment(e.ActualStartDate).format("MM/DD/YYYY");
      }
      if (e.ActualEndDate != null) {
        e.ActualEndDate = moment(e.ActualEndDate).format("MM/DD/YYYY");
      }
      if (e.PlannedStartDate != null) {
        e.PlannedStartDate = moment(e.PlannedStartDate).format("MM/DD/YYYY");
      }
      if (e.PlannedEndDate != null) {
        e.PlannedEndDate = moment(e.PlannedEndDate).format("MM/DD/YYYY");
      }
    });
    this.ProjectsList = data;
  }

  ViewProject(ProjectObject: ProjectsData) {
     this.projectstate =ProjectObject.ProjectState;
    if(this.projectstate == "ClosureInitiated" || this.projectstate === "SubmittedForClosureApproval"){
      this.ProjectCreationServiceObj.SetProjectId(ProjectObject.ProjectId);
      this.route.navigate(["/project/projectclosure/"+this.dashboard]);
    
   }
   else{
     
    this.ProjectCreationServiceObj.SetProjectId(ProjectObject.ProjectId);
    this.route.navigate(["/project/view/" + this.dashboard]);
   }
    


  }

  // GetAssociateExitList():void{
  //   this._associateExitDashboardService.getAssociateExitDashbaord(this.roleName, this.EmpId, this.dashboard).subscribe((res: any[]) => {
  //       res.forEach(e => { e.ExitDate = moment(e.ExitDate).format("MM/DD/YYYY"); 
  //       if(e.StatusCode == "RevokeInitiated"){
  //         this._resignationService.getExitDetailsById(e.EmployeeId).subscribe((res:any)=>{ 
  //           e.WithdrawReason = res.WithdrawReason;
  //         });
  //       } });
  //       this.dataSourceAssociateExit = new MatTableDataSource(res);
  //       this.dataSourceAssociateExit.sort = this.sort;
  //       this.dataSourceAssociateExit.paginator = this.exitPaginator;
  //   });
    
  // }
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
                  // this.GetAssociateExitList();
                }
              });
       
          }
        });
  }
  viewResignationDets(selectedData : any){
    this.route.navigate(['/associateexit/pmApproval/' + selectedData.EmployeeId]);
  
  }
  abscondForm(){
    this.route.navigate(['/associateexit/associateabscondform']);

 }
 
}
