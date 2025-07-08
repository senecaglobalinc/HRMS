import { Component, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup, FormGroupDirective, Validators } from '@angular/forms';
import { themeconfig } from '../../../../../themeconfig';
import { RoleService } from '../../services/role.service';
import { Department } from '../../models/department.model';
import { RoleData } from '../../models/role-data.model'
import { MatSnackBar } from '@angular/material/snack-bar';
import { NavService } from 'src/app/modules/master-layout/services/nav.service';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';

interface SelectItem {
  value: number;
  label: string;
}

@Component({
  selector: 'app-roles-table',
  templateUrl: './roles-table.component.html',
  styleUrls: ['./roles-table.component.scss']
})
export class RolesTableComponent implements OnInit {
  showadd=false;
  themeConfigInput = themeconfig.formfieldappearances;
  searchDepartment: FormGroup;
  showTable: boolean;
  formSubmitted = false;
  roleserviceObj:Subscription

  departmentList: SelectItem[];
  selectedRoleData: any;
  rolesData: RoleData[];
  deptId: number;

  dataSource: MatTableDataSource<RoleData>;
  @ViewChild(FormGroupDirective) formGroupDirective: FormGroupDirective;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  

  displayedColumns: string[] = ['FunctionalRole', 'RoleDescription', 'Edit'];


  constructor(private _rolesService: RoleService, private route: Router, private _snackBar: MatSnackBar, public navService: NavService) {


    this.navService.currentSearchBoxData.subscribe(responseData => {
      this.applyFilter(responseData);
    })
  }
  ngOnInit(): void {

    this.searchDepartment = new FormGroup({
      DepartmentId: new FormControl(null,Validators.required)
    });
    this.roleserviceObj=this._rolesService.selectedDepartment.subscribe(data => {
      this.searchDepartment.value.DepartmentId = data;
      if(this._rolesService.selectedDepartmentname){
        this.searchDepartment.controls['DepartmentId'].setValue(data);
        // this.showadd=true;
        // this.showTable=true;
        this.searchRoleByDepartment(data);
        return;
      }
      this.deptId = this.searchDepartment.value.DepartmentId;
      this.dataSource = new MatTableDataSource(this.searchDepartment.value.DepartmentId);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
    });
   

    this.getDepartmentList();



  }


  getDepartmentList(): void {
    this._rolesService.getDepartmentList().subscribe((res: Department[]) => {
      this.departmentList = [];
      res.forEach(element => {
        this.departmentList.push({ label: element.Description, value: element.DepartmentId });
      });
    }, (error) => {
      this._snackBar.open(error.error, 'x', {
        duration: 3000,
        panelClass: ['error-alert'],
        horizontalPosition: 'right',
        verticalPosition: 'top',
      });
    }
    );
  }




  searchRoleByDepartment(selectedDepartment): void {
    this.formSubmitted = true;
    this.showTable = true;
    this.showadd=true;
    this._rolesService.getRoleDetailsByDepartmentID(selectedDepartment).subscribe((res: RoleData[]) => {
      this.rolesData = res;
      this.dataSource = new MatTableDataSource(this.rolesData);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
    });
  }

  createRole(selectedDepartment): void {
    this._rolesService.selectedDepartment.next(this.searchDepartment.value.DepartmentId);
    this._rolesService.editMode =false;
    this.route.navigate(['/admin/addrole']);
  }

  editRole(roleData) {

    this._rolesService.selectedDepartment.next(this.searchDepartment.value.DepartmentId);
    this._rolesService.editMode = true;
    this._rolesService.roleData.next(roleData);
    this.route.navigate(['/admin/addrole']);
  }

  cancel(): void {
    this.formSubmitted = false;
    this.searchDepartment.reset();
    setTimeout(() => this.formGroupDirective.resetForm(), 0)
    this.showTable = false;
    this.showadd=false;
    // this._skillsService.editMode = false;
  }

  applyFilter(event: Event) {
    if (event) {
      const filterValue = (event.target as HTMLInputElement).value;
      this.dataSource.filter = filterValue.trim().toLowerCase();
    } else {
      this.dataSource = new MatTableDataSource(this.rolesData);

      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;

    }
  }
 
  ngOnDestroy() {
    this._rolesService.selectedDepartmentname=false;
    this.roleserviceObj.unsubscribe();
  }
}
