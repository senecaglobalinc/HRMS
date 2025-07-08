import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Route, Router } from '@angular/router';
import { NavService } from 'src/app/modules/master-layout/services/nav.service';
import { Associate } from '../../models/associate.model';
import { AttendanceRegularization } from '../../models/regularization.model';
import { RegularizationService } from '../../services/regularization.service';

@Component({
  selector: 'app-view-regularization-applied-associates',
  templateUrl: './view-regularization-applied-associates.component.html',
  styleUrls: ['./view-regularization-applied-associates.component.scss']
})
export class ViewRegularizationAppliedAssociatesComponent implements OnInit {

  _dataSource = new MatTableDataSource<AttendanceRegularization>();
  EmployeesList : AttendanceRegularization[] = [];
  displayedColumns: string[] = ['empcode', 'name', 'project', 'View', 'Days'];
  @ViewChild('regularizationsPaginator', { static: false }) regularizationsPaginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  EmpId : number;
  roleName : string;

  constructor(private router : Router,private service : RegularizationService,public navService: NavService) { }

  ngOnInit(): void {
    this.navService.currentSearchBoxData.subscribe(responseData => {
      this.applyFilter(responseData);
    })
    this.EmpId = JSON.parse(
      sessionStorage["AssociatePortal_UserInformation"]
    ).employeeId;
    this.roleName = JSON.parse(
      sessionStorage["AssociatePortal_UserInformation"]
    ).roleName;
    this.getRegularizationAppliedAssociates();
    

  }

  ViewAppliedRegularizationDays(element){
    this.router.navigate(['/associates/viewDays/'+element.SubmittedBy,element.AssociateName]);
  }

  getRegularizationAppliedAssociates(){
    this.EmployeesList = []; 
    this._dataSource = new MatTableDataSource(this.EmployeesList);
    this.service.GetAllAssociateSubmittedAttendanceRegularization(this.EmpId,this.roleName).subscribe((res : AttendanceRegularization[])=>{
    this.EmployeesList = res
    this._dataSource = new MatTableDataSource(this.EmployeesList);
    this._dataSource.paginator = this.regularizationsPaginator;
    this._dataSource.sort = this.sort;
    })
  }

  applyFilter(event: Event) {
    if (event) {
      const filterValue = (event.target as HTMLInputElement).value;
      if (filterValue) {
        this._dataSource.filter = filterValue.trim().toLowerCase();
      }
      else {
        this._dataSource = new MatTableDataSource(this.EmployeesList);
        this._dataSource.paginator = this.regularizationsPaginator;
        this._dataSource.sort = this.sort;
      }
    } else {
      this._dataSource = new MatTableDataSource(this.EmployeesList);
      this._dataSource.paginator = this.regularizationsPaginator;
      this._dataSource.sort = this.sort;
    }
  }

}
