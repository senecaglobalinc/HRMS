import { SelectionModel } from '@angular/cdk/collections';
import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { themeconfig } from "../../../../../themeconfig";
import { AssignRmToNonDeliveryComponent } from '../assign-rm-to-non-delivery/assign-rm-to-non-delivery.component';
import { AssignRmForNonDeliveryService } from '../../services/assign-rm-for-non-delivery.service';
import { EmployeeDetails } from '../../models/changeRM.model';
import { DepartmentDetails } from 'src/app/modules/master-layout/models/role.model';
import { FormControl, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { NgxSpinnerService } from 'ngx-spinner';
import { NavService } from 'src/app/modules/master-layout/services/nav.service';
import { MatSort } from '@angular/material/sort';



@Component({
  selector: 'app-change-rm-for-non-delivery',
  templateUrl: './change-rm-for-non-delivery.component.html',
  styleUrls: ['./change-rm-for-non-delivery.component.scss']
})
export class ChangeRmForNonDeliveryComponent implements OnInit {
  themeConfigInput = themeconfig.formfieldappearances;
  displayedColumns: string[] = ['select', 'EmpName', 'ReportingManager'];
  dataSource = new MatTableDataSource<EmployeeDetails>();
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator,{ static: true }) paginator: MatPaginator;
  selection = new SelectionModel<EmployeeDetails>(true, []);
  nonDeliveryAssociatesByDeptId : EmployeeDetails[] = [];
  nonDeliveryDeparmentsList : DepartmentDetails[] =[];
  selectedDepartment:number;
  departmentId = new FormControl('',[Validators.required]);

  constructor(public dialog:MatDialog,public _service:AssignRmForNonDeliveryService,private _snackBar : MatSnackBar,private spinner: NgxSpinnerService,private navService : NavService) { }

  ngOnInit(): void {
    this.getNonDeliveryDepartments();
    this.navService.currentSearchBoxData.subscribe(responseData => {
      this.applyFilter(responseData);
    })
  }
  ngAfterViewInit() {
    this.dataSource.sort = this.sort;
  }
  applyFilter(event: Event) {
    if (event) {
      const filterValue = (event.target as HTMLInputElement).value;
      if (filterValue) {
        this.dataSource.filter = filterValue.trim().toLowerCase();
      }
      else {
        this.dataSource = new MatTableDataSource(this.nonDeliveryAssociatesByDeptId);
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
      }
    } else {
      this.dataSource = new MatTableDataSource(this.nonDeliveryAssociatesByDeptId);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
    }
  }


  getNonDeliveryDepartments(){
    this._service.getDepartments().subscribe((res : DepartmentDetails[])=>{
      this.nonDeliveryDeparmentsList = res;    
      this.nonDeliveryDeparmentsList = this.nonDeliveryDeparmentsList.filter((department)=>
        department.Description.toLowerCase()!=("Technology & Delivery").toLowerCase() && department.Description.toLowerCase()!=("Corporate").toLowerCase())
    },(error)=>{
      this._snackBar.open(
        'Unable to Fetch Departments',
        'x',
        {
          duration: 3000,
          panelClass: ['error-alert'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
        }
      );
    })
  }

  onDepartmentChange(event){
    this.selection = new SelectionModel<EmployeeDetails>(true, []);
    this.spinner.show();
    this.selectedDepartment = event.value;
    this.getServiceDepartmentAssociatesByDeptId();
  }

  getServiceDepartmentAssociatesByDeptId(){
    this._service.getEmpListByDeptId(this.selectedDepartment).subscribe((res:EmployeeDetails[])=>{
      this.spinner.hide();
      this.nonDeliveryAssociatesByDeptId = res;
      this.dataSource = new MatTableDataSource(this.nonDeliveryAssociatesByDeptId);
      this.dataSource.sort = this.sort;
      this.dataSource.paginator = this.paginator;
    },(error)=>{
      this.spinner.hide();
      this._snackBar.open(
        'Unable to Fetch Associates',
        'x',
        {
          duration: 3000,
          panelClass: ['error-alert'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
        }
      );
      this.dataSource = new MatTableDataSource();
      this.dataSource.sort = this.sort;
      this.dataSource.paginator = this.paginator;
    })
  }

  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.dataSource.data.length;
    return numSelected === numRows;
  }


  masterToggle() {
    this.isAllSelected() ?
        this.selection.clear() :
        this.dataSource.data.forEach(row => this.selection.select(row));
  }

  assignRM(){
    let dialogRef = this.dialog.open(AssignRmToNonDeliveryComponent, {
      disableClose: true,
      height:'250px',
      width:'400px',
      data:this.selection.selected
    });
    dialogRef.afterClosed().subscribe(result => {
      this.getServiceDepartmentAssociatesByDeptId();
     this.selection = new SelectionModel<EmployeeDetails>(true, []);
    });
  }

  }
