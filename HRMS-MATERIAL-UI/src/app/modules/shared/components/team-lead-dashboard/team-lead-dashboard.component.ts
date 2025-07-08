
import { Component, OnInit, ViewChild } from "@angular/core";
import { ProjectsData } from "src/app/modules/master-layout/models/projects.model";
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
import { ResignastionService } from "src/app/modules/AssociateExit/Services/resignastion.service";
import { AssociateExitDashboardService } from "../../services/associate-exit-dashboard.service";
import {KTPlanDialogComponent} from '../KT-plan-dialog/KT-plan-dialog.component';
import { MatDialog } from "@angular/material/dialog";

@Component({
  selector: 'app-team-lead-dashboard',
  templateUrl: './team-lead-dashboard.component.html',
  styleUrls: ['./team-lead-dashboard.component.scss']
})
export class TeamLeadDashboardComponent implements OnInit {
  resources = servicePath.API.PagingConfigValue;
  pageload: boolean = false;
  dashboard: string;
  EmpId: number;
  roleName: string;
  temroleName:string;
  ProjectsList: ProjectsData[];
  dataSource = new MatTableDataSource<ProjectsData>();

  UserRole : string;
  empId : number;
  loggedInEmpId: number;

  dataSource1: MatTableDataSource<any>;

  displayColumns = ['EmployeeCode','EmployeeName','Designation','ExitDate','StatusCode','TransitionPlan'];
  // content : tableContent[];
  departmentName: string;
  departmentId: number;

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild('exitPaginator', { static: false }) exitPaginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  
  displayedColumns: string[] = [
    'ProjectName',
    'ManagerName',
    'PracticeAreaCode',
    'ClientName',
    // 'ActualStartDate',
    'ProjectState',
    'View',
  ];
  constructor(private _router: Router, private _deliveryHeadService: DeliveryHeadService,
     private ProjectCreationServiceObj: ProjectCreationService,
     public dialog:MatDialog,
     public navService: NavService, private spinner: NgxSpinnerService,
     private _snackBar: MatSnackBar, private _resignationService : ResignastionService,
     private _associateExitDashboardService : AssociateExitDashboardService) {
      this.navService.currentSearchBoxData.subscribe((responseData) => {
      this.applyFilter(responseData);
    }); }

  ngOnInit(): void {
    this.spinner.show();
    this.roleName = JSON.parse(
      sessionStorage["AssociatePortal_UserInformation"]
    ).roleName;
    this.loggedInEmpId = JSON.parse(
      sessionStorage["AssociatePortal_UserInformation"]
    ).employeeId;
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
      this.GetProjectsList();
    }
    else{
      this.dashboard = "ServiceDashboard";
      this.displayColumns = ['EmployeeCode','EmployeeName','Designation','ExitDate','StatusCode','View', 'Action'];
    }
    //this.getResignedEmployees();
    this.getAssociateExitList()
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
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }
  

  GetProjectsList(): void {
    this.roleName = JSON.parse(
      sessionStorage["AssociatePortal_UserInformation"]
    ).roleName;
    
    this.EmpId = JSON.parse(
      sessionStorage["AssociatePortal_UserInformation"]
    ).employeeId;
    this._deliveryHeadService
      .GetProjectsList(this.roleName, this.EmpId, this.dashboard)
      .toPromise().then((res: ProjectsData[]) => {
        this.spinner.hide();
        this.ModifyDateFormat(res);
        this.dataSource.data = res;
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
      }).catch(error=>{
        this.spinner.hide();
        this.pageload = true;
        // this._snackBar.open('An error has occurred. Please contact administration.', 'x', {
        //   duration: 2000,
        //   horizontalPosition: 'right',
        //   verticalPosition: 'top',
        // });
     });;
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
    this.pageload = true;
  }

  ViewProject(ProjectObject: ProjectsData) {
    this.ProjectCreationServiceObj.SetProjectId(ProjectObject.ProjectId);
    this._router.navigate(["/project/viewteamlead/" + this.dashboard]);
  }

  getAssociateExitList():void{
    this.spinner.show();
    // let dashboardType = "DeliveryDashboard";
    this._associateExitDashboardService.getAssociateExitDashbaord(this.roleName, this.loggedInEmpId, this.dashboard).toPromise().then((res: any[]) => {
        res.forEach(e => { e.ExitDate = moment(e.ExitDate).format("MM/DD/YYYY"); });
        this.dataSource1 = new MatTableDataSource(res);
        this.dataSource1.sort = this.sort;
        this.dataSource1.paginator = this.exitPaginator;
        this.pageload = true;
        this.spinner.hide();
    }).catch(error => {
                    this.spinner.hide();
                    this.pageload = true;
                    // this._snackBar.open('An error has occurred. Please contact administration.', 'x', {
                    //   duration: 2000,
                    //   horizontalPosition: 'right',
                    //   verticalPosition: 'top',
                    // });
                });

  }
  

  viewResignationDets(selectedData : any){
    this._router.navigate(['/shared/KtForm/' + selectedData.empId]);
  }

  UpdateKTPLan(selectedEmpId)  {
   var dialogRef=  this.dialog.open(KTPlanDialogComponent, {    
      width:'500px',
      disableClose: true ,
      data: { EmployeeId: selectedEmpId },
    });
    dialogRef.afterClosed().subscribe(result => {
      this.getAssociateExitList();
    })

    
  }

  PMSubmissionScreen(selectedData){
    this._router.navigate(['/associateexit/PMSubmission/' + selectedData.EmployeeId]);
  }
  
}
