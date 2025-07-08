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
import { FormGroupDirective } from "@angular/forms";

@Component({
  selector: 'app-service-manager-dashboard',
  templateUrl: './service-manager-dashboard.component.html',
  styleUrls: ['./service-manager-dashboard.component.scss']
})
export class ServiceManagerDashboardComponent implements OnInit {
  EmpId: number;
  roleName: string;
  dashboard: string = "SHDashboard";
  ProjectsList: ProjectsData[];
  PageSize: number;
  PageDropDown: number[] = [];
  resources = servicePath.API.PagingConfigValue;
  dataSource = new MatTableDataSource<ProjectsData>();
  pageload: boolean = false;
  EmployeeId: number;
  // dashboard: string = "ServiceMangerDashboard";
  isLoading: boolean = false;
  serviceDeptId : number = 0;

  @ViewChild(FormGroupDirective) formGroupDirective: FormGroupDirective;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
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
    // 'ActualStartDate',
    'ProjectState',
    'Action',
  ];


  constructor(
    private _deliveryHeadService: DeliveryHeadService,
    private route: Router,
    private ProjectCreationServiceObj: ProjectCreationService,
    private _snackBar: MatSnackBar,
    public navService: NavService,
    private spinner: NgxSpinnerService,
    private snackBar: MatSnackBar,
  ) {
    this.navService.currentSearchBoxData.subscribe((responseData) => {
      this.applyFilter(responseData);
    });
  }

  ngOnInit() {
    this.spinner.show();
    this.GetProjectsList();
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
      .GetProjectsList(this.roleName, this.EmpId, this.dashboard)//'Service Manager'
      .toPromise().then((res: ProjectsData[]) => {
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
    this.pageload = true;
    this.spinner.hide();
  }

  ViewChecklist(ProjectObject: ProjectsData){
    this.ProjectCreationServiceObj.SetProjectId(ProjectObject.ProjectId);
    this.route.navigate(['project/checklist']);
  }  
}
