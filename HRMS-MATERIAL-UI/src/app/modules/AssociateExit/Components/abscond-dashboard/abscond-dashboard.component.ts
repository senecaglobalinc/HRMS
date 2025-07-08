import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { AbscondService } from '../../Services/abscond.service';
import { MatDialog } from '@angular/material/dialog';
import { ViewAbscondDetailedStatusComponent } from '../view-abscond-detailed-status/view-abscond-detailed-status.component';

@Component({
  selector: 'app-abscond-dashboard',
  templateUrl: './abscond-dashboard.component.html',
  styleUrls: ['./abscond-dashboard.component.scss']
})
export class AbscondDashboardComponent implements OnInit {
  EmpId: number;
  roleName: string;
  DeptId: number;
  associateList= [];
  dialogResponse: boolean;
  @ViewChild('Paginator', { static: false }) Paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayColumns = ['EmployeeCode', 'EmployeeName', 'StatusCode', 'Acknowledge', 'Activity', 'Clearance'];
  dataSourceData: MatTableDataSource<any>;
  constructor(private route: Router, private abscondService: AbscondService,
    private _snackBar: MatSnackBar, private spinner: NgxSpinnerService,
    public dialog: MatDialog) { }

  ngOnInit(): void {
    this.roleName = JSON.parse(
      sessionStorage.AssociatePortal_UserInformation
    ).roleName;
    this.EmpId = JSON.parse(
      sessionStorage.AssociatePortal_UserInformation
    ).employeeId;
    this.DeptId = JSON.parse(
      sessionStorage.Department
    ).DepartmentId;
    this.GetAssociateExitList();
  }

  applyFilter(event: Event) {
    if (event) {
      const filterValue = (event.target as HTMLInputElement).value;
      this.dataSourceData.filter = filterValue.trim().toLowerCase();
    } else {
      this.dataSourceData = new MatTableDataSource(this.associateList);
      this.dataSourceData.paginator = this.Paginator;
      this.dataSourceData.sort = this.sort;
    }
  }

  GetAssociateExitList(): void {
    this.abscondService.GetAssociatesAbscondDashboard(this.roleName, this.EmpId, this.DeptId).subscribe((res: any[]) => {
      this.associateList = res['Items']
      this.dataSourceData = new MatTableDataSource(this.associateList);
      this.dataSourceData.sort = this.sort;
      this.dataSourceData.paginator = this.Paginator;
    });

  }
  abscond(element){
    this.route.navigate(['/associateexit/abscond-details/'+ element.AssociateId]);
  }

  getExitChecklist(selectedData: any) {
    this.route.navigate(['/associateexit/abscond/' + selectedData.AssociateId]);
  }

  onList(selectedData: any) {
    this.route.navigate(['/associateexit/abscond-deptchecklist/' + selectedData.AssociateId]);

  }

  Associateexitform(selectedData: any) {
    // this.route.navigate(["/associateexit/view/" + selectedData.EmployeeId]);

    this.route.navigate(['/associateexit/exitfeedback/' + selectedData.EmployeeId]);
  }

  ViewDetailedStatus(EmployeeId: number) {
    const dialogRef = this.dialog.open(ViewAbscondDetailedStatusComponent, {
      width: '500px',
      // disableClose: true,
      data: { value: EmployeeId}
    });
    dialogRef.afterClosed().subscribe(result => {
      this.dialogResponse = result;
    })
  }

}

