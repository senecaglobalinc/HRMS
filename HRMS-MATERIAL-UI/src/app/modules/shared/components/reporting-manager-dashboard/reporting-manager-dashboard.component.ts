import { Component, OnInit, ViewChild } from "@angular/core";
import * as servicePath from "../../../../core/service-paths";
import { Router } from "@angular/router";
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { NavService } from 'src/app/modules/master-layout/services/nav.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { NgxSpinnerService } from "ngx-spinner";
import { Associate } from 'src/app/modules/onboarding/models/associate.model';
import { SkillsService } from 'src/app/modules/onboarding/services/skills.service';

@Component({
  selector: "app-reporting-manager-dashboard",
  templateUrl: "./reporting-manager-dashboard.component.html",
  styleUrls: ["./reporting-manager-dashboard.component.scss"]
})
export class ReportingManagerDashboardComponent implements OnInit {
  projectstate:string;
  EmpId: number;
  roleName: string;
  dashboard: string = "RMDashboard";
  PageSize: number;
  PageDropDown: number[] = [];
  resources = servicePath.API.PagingConfigValue;
  
  @ViewChild('skillapprovalPaginator', { static: false }) skillapprovalPaginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  _dataSource = new MatTableDataSource<Associate>();
  EmployeesList : Associate[];

  displayedColumns: string[] = ['empcode', 'name', 'project', 'View'];
 
  constructor(
    private route: Router,
    private _snackBar: MatSnackBar,
    public navService: NavService,
    private spinner: NgxSpinnerService,
    private skillservice: SkillsService
  ) {
    this.navService.currentSearchBoxData.subscribe((responseData) => {
       this.applyFilter(responseData);
    });
  }
 
  ngOnInit() {
    this.spinner.show();     
    this.roleName = JSON.parse(
      sessionStorage["AssociatePortal_UserInformation"]
    ).roleName;
    this.EmpId = JSON.parse(
      sessionStorage["AssociatePortal_UserInformation"]
    ).employeeId;

    this.getSkillSubmittedEmployees();
      
  }

  getSkillSubmittedEmployees() {
    this.skillservice.getSkillSubmittedEmployees(this.EmpId).subscribe((res: Associate[]) => {
      this.EmployeesList = res;
      this._dataSource = new MatTableDataSource(this.EmployeesList);
      this._dataSource.paginator = this.skillapprovalPaginator;
      this._dataSource.sort = this.sort;
      this.spinner.hide();
    },
    (error)=>{
      if(error.error != "No Record is available")
      {
        this._snackBar.open(error.error, 'x', {
        duration: 3000,
        panelClass: ['error-alert'],
        horizontalPosition: 'right',
        verticalPosition: 'top'
      });
      }      
    });
    
  }

  applyFilter(event: Event) {
    if (event) {
      const filterValue = (event.target as HTMLInputElement).value;
      this._dataSource.filter = filterValue.trim().toLowerCase();
    } else {
      this._dataSource = new MatTableDataSource(this._dataSource.data);
    }
    if (this._dataSource.paginator) {
      this._dataSource.paginator.firstPage();
    }
    this._dataSource.paginator = this.skillapprovalPaginator;
    this._dataSource.sort = this.sort;
  }
  ViewSkills(element) {
    this.route.navigate(['/shared/submitted-skills/'+element.EmployeeId]);
  } 
}
