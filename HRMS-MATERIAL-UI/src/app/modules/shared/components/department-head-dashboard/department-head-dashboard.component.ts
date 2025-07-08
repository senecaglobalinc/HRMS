import { Component, OnInit, ViewChild } from '@angular/core';
import { DashboardService } from '../../services/dashboard.service';
// import { MessageService } from "primeng/components/common/messageservice";
import { DeliveryHeadService } from '../../services/delivery-head.service';
import { ProjectsData } from 'src/app/modules/master-layout/models/projects.model';
import * as moment from 'moment';
import * as servicePath from '../../../../core/service-paths';
import { ProjectCreationService } from "../../../project-life-cycle/services/project-creation.service";
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { NavService } from 'src/app/modules/master-layout/services/nav.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { NgxSpinnerService } from 'ngx-spinner';
import { AssociateExitDashboardService } from '../../services/associate-exit-dashboard.service';
import { ResignastionService } from 'src/app/modules/AssociateExit/Services/resignastion.service';
import { PmApprovalService } from 'src/app/modules/AssociateExit/Services/pm-approval.service';
import { MatDialog } from '@angular/material/dialog';
import { CommonDialogComponent } from '../common-dialog/common-dialog.component';

@Component({
  selector: 'app-department-head-dashboard',
  templateUrl: './department-head-dashboard.component.html',
  styleUrls: ['./department-head-dashboard.component.scss'],
  // providers: [MessageService]
})
export class DepartmentHeadDashboardComponent implements OnInit {
  EmpId: number;
  roleName: string;
  dashboard: string = 'DHDashboard';
  ProjectsList: ProjectsData[];
  PageSize: number;
  editMode: boolean;
  PageDropDown: number[] = [];
  resources = servicePath.API.PagingConfigValue;
  editModeSubscription: Subscription;
  dataSource = new MatTableDataSource<ProjectsData>();
  disablestyles = { 'opacity': '0.6', 'pointer-events': 'none'}

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild('exitPaginator', { static: true }) exitPaginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  cols = [
    { field: 'ProjectName', header: 'Project Name' },
    { field: 'ManagerName', header: 'Program Manager' },
    { field: 'PracticeAreaCode', header: 'Technology' },
    { field: 'ClientName', header: 'Client Name' },
    { field: 'ActualStartDate', header: 'Start Date' },
    { field: 'ProjectState', header: 'Status' },
  ];
  displayedColumns: string[] = [
    'ProjectName',
    'ManagerName',
    'PracticeAreaCode',
    'ClientName',
    'ActualStartDate',
    'ProjectState',
    'Edit',
    'View',
  ];
  columnsToDisplay: string[] = this.displayedColumns.slice();
  displayColumnsAssociateExit = ['EmployeeCode','EmployeeName','Designation','ExitDate','StatusCode','Action'];
  dataSourceAssociateExit: MatTableDataSource<any>;
  dialogResponse:boolean;
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
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }
  constructor(
    private _deliveryHeadService: DeliveryHeadService,
    // private messageService: MessageService,
    private route: Router,
    private ProjectCreationServiceObj: ProjectCreationService,
    private _snackBar: MatSnackBar,
    public navService: NavService,
    private spinner: NgxSpinnerService,
    private _associateExitDashboardService: AssociateExitDashboardService,
    public _resignationService: ResignastionService,
    private _pmApprovalService : PmApprovalService,
    public dialog: MatDialog,
  ) {
    this.navService.currentSearchBoxData.subscribe((responseData) => {
      this.applyFilter(responseData);
    });
  }

  ngOnInit() {
    this.spinner.show();
    this.GetProjectsList();
    // this.GetAssociateExitList();
    this.editModeSubscription = this.ProjectCreationServiceObj.GetEditMode().subscribe(
      (data) => {
        this.editMode = data;
      }
    );
  }

  GetProjectsList(): void {
    this.roleName = JSON.parse(
      sessionStorage['AssociatePortal_UserInformation']
    ).roleName;
    this.EmpId = JSON.parse(
      sessionStorage['AssociatePortal_UserInformation']
    ).employeeId;

    // this.EmpId = JSON.parse(sessionStorage['AssociatePortal_UserInformation']).employeeId;
    this._deliveryHeadService
      .GetProjectsList(this.roleName, this.EmpId, this.dashboard)
      .toPromise().then((res: ProjectsData[]) => {
        this.ModifyDateFormat(res);
        this.dataSource.data = res;
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
        this.spinner.hide();
      }).catch(error=>{
        this.spinner.hide();
        // this._snackBar.open('An error has occurred. Please contact administration.', 'x', {
        //   duration: 2000,
        //   horizontalPosition: 'right',
        //   verticalPosition: 'top',
        // });
     });;
  }

  ModifyDateFormat(data: ProjectsData[]): void {
    data.forEach((e) => {
      if (e.ActualStartDate != null) {
        e.ActualStartDate = moment(e.ActualStartDate).format('MM/DD/YYYY');
      }
      if (e.ActualEndDate != null) {
        e.ActualEndDate = moment(e.ActualEndDate).format('MM/DD/YYYY');
      }
      if (e.PlannedStartDate != null) {
        e.PlannedStartDate = moment(e.PlannedStartDate).format('MM/DD/YYYY');
      }
      if (e.PlannedEndDate != null) {
        e.PlannedEndDate = moment(e.PlannedEndDate).format('MM/DD/YYYY');
      }
    });
    this.ProjectsList = data;
  }

  EditProject(ProjectObject: ProjectsData): void {
    this.ProjectCreationServiceObj.SetSeletedTab(0);
    this.ProjectCreationServiceObj.SetEditMode(true);
    this.ProjectCreationServiceObj.SetProjectId(ProjectObject.ProjectId);
    this.route.navigate(['/project/addproject/' + this.dashboard]);
  }

  ViewProject(ProjectObject: ProjectsData) {
    if(ProjectObject.ProjectState==="SubmittedForClosureApproval"){
      this.ProjectCreationServiceObj.SetProjectId(ProjectObject.ProjectId);
      this.route.navigate(['project/projectclosure/' + this.dashboard]);
    } else{
      this.ProjectCreationServiceObj.SetProjectId(ProjectObject.ProjectId);
      this.route.navigate(['/project/view/' + this.dashboard]);
    }
    
  }

  canViewEdit(ProjectObject: ProjectsData){
    if(ProjectObject.ProjectState!=='SubmittedForApproval')
      return this.disablestyles;

    return { 'cursor':'pointer' };
    
  }

  ngOnDestroy() {
    this.editModeSubscription.unsubscribe();
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
  //       this.dataSourceAssociateExit.paginator = this.exitPaginator
  //       this.dataSourceAssociateExit.sort = this.sort;
  //   });
    
  // }

  viewResignationDets(selectedData : any){
    this.route.navigate(['/associateexit/pmApproval/' + selectedData.EmployeeId]);
  
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
                    // this.GetAssociateExitList();
                  }
                });
         
            }
          });
    }
}
