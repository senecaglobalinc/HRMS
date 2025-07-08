import { AfterViewInit, Component, ViewChild, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Department } from '../../models/department.model';
import { GenericType } from '../../../master-layout/models/dropdowntype.model';
import {
  MatSnackBar,
  MatSnackBarHorizontalPosition,
  MatSnackBarVerticalPosition,
} from '@angular/material/snack-bar';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { DepartmentService } from '../../services/department.service';
import { MasterDataService } from '../../../../core/services/masterdata.service';
import { FormGroup, FormControl, Validators, ValidationErrors, FormGroupDirective } from '@angular/forms';
import * as servicePath from '../../../../core/service-paths';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';
import { MatSort } from '@angular/material/sort';
import { themeconfig } from '../../../../../themeconfig';
import { EmployeeData } from '../../models/employee.model';
import { EmployeeStatusService } from '../../services/employeestatus.service';
import { NavService } from 'src/app/modules/master-layout/services/nav.service';
import { CommonService } from 'src/app/core/services/common.service';

@Component({
  selector: 'app-departments',
  templateUrl: './departments.component.html',
  styleUrls: ['./departments.component.scss'],
  providers: [EmployeeStatusService],
})
export class DepartmentsComponent implements OnInit {
  empData: EmployeeData;
  themeappeareance = themeconfig.formfieldappearances;
  filteredOptionsName: Observable<any>;
  addDepartment: FormGroup;
  filteredAssociateIds: GenericType[] = [];
  formSubmitted: boolean = false;
  btnLabel = '';
  isEdit: boolean = false;
  delivery: boolean = true;
  editObj;
  Department: Department[];
  selectedDepartment: Department;
  PageSize: number;
  PageDropDown: number[] = [];
  resources = servicePath.API.PagingConfigValue;
  dataSource = new MatTableDataSource<Department>();
  usersList = [];
  pattern:"^(?:[A-Za-z0-9]+)(?:[A-Za-z0-9 ]*)$";

  @ViewChild(FormGroupDirective) formGroupDirective: FormGroupDirective;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  cols = [
    { field: 'DepartmentCode', header: 'Department Code' },
    { field: 'Description', header: 'Department Name' },
    { field: 'DepartmentTypeDescription', header: 'Department Type' },
    { field: 'DepartmentHeadName', header: 'Department Head' },
  ];
  displayedColumns: string[] = [
    'DepartmentCode',
    'Description',
    'DepartmentTypeDescription',
    'DepartmentHeadName',
    'Edit',
  ];
  columnsToDisplay: string[] = this.displayedColumns.slice();

  drop(event: CdkDragDrop<string[]>) {
    moveItemInArray(
      this.displayedColumns,
      event.previousIndex,
      event.currentIndex
    );
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

  constructor(
    private _router: Router,
    private _departmentService: DepartmentService,
    private _masterDataService: MasterDataService,
    private service: EmployeeStatusService,
    private _snackBar: MatSnackBar,
    public navService: NavService,
    public commonService:CommonService
  ) {
    this.getUsersList();
    this._departmentService.Department.subscribe((data) => {
      if (data != null) {
        this.dataSource.data = data;
        this.dataSource.data.forEach((department) => {
          department.DepartmentTypeDescription =
            department['DepartmentType']['DepartmentTypeDescription'];
        });
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
      }
    });
    this.navService.currentSearchBoxData.subscribe((responseData) => {
      this.applyFilter(responseData);
    });
  }

  ngOnInit() {
    this.getUsersList();
    this.addDepartment = new FormGroup({
      DepartmentCode: new FormControl(null, [Validators.required]),
      Description: new FormControl(null, [Validators.required]),
      DepartmentHead: new FormControl(null),
      IsDelivery: new FormControl(true),
    });

    if (this.isEdit == false) {
      this.getDepartmentDetails();
    }
    this._departmentService.departmentsEdit.subscribe((data) => {
      if (this._departmentService.editMode == true) {
        this.btnLabel = 'Update';
        this.isEdit = this._departmentService.editMode;

        if (this.isEdit == true) {
          if (data.DepartmentTypeId == 1) {
            this.delivery = true;
          } else {
            this.delivery = false;
          }
          this.addDepartment.patchValue({
            DepartmentCode: data.DepartmentCode,
            Description: data.Description,
            IsDelivery: this.delivery,
            DepartmentHead: data.DepartmentHeadName,
          });
        }
      }
    });
    this.btnLabel = 'Save';

    this.filteredOptionsName = this.addDepartment.valueChanges.pipe(
      startWith(''),
      map((value) => this._filterName(value))
    );

  }

  ngAfterViewInit() {
  }

  CreateDepartment() {
    this.formSubmitted = true;
    this.addDepartment.markAllAsTouched();
    var department = new Department();
    if(this.addDepartment.valid == true){
      department.DepartmentCode = this.addDepartment.value.DepartmentCode;
      department.Description = this.addDepartment.value.Description;
      department.DepartmentTypeId = this.addDepartment.value.IsDelivery;
      if (this.addDepartment.value.DepartmentHead != null) {
        department.DepartmentHeadId = this.addDepartment.value.DepartmentHead[
          'value'
        ];
        department.DepartmentHeadName = this.addDepartment.value.DepartmentHead[
          'label'
        ];
      } else {
        department.DepartmentHeadId = null;
        department.DepartmentHeadName = null;
      }

      if (this.addDepartment.value.IsDelivery == true) {
        department.DepartmentTypeId = 1;
      } else {
        department.DepartmentTypeId = 2;
      }
      if (this._departmentService.editMode == true) {
        department.DepartmentId = this._departmentService.departmentsEdit.value.DepartmentId;
      }
      if (this.addDepartment.valid == true) {
        this._departmentService.CreateDepartment(department).subscribe(
          (res: number) => {
            if (res) {
              this._departmentService.getDepartmentDetails();
              if (this._departmentService.editMode == false) {
                this._snackBar.open(
                  'Department record added successfully.',
                  'x',
                  {
                    duration: 3000,
                    horizontalPosition: 'right',
                    verticalPosition: 'top',
                  }
                );
                this.cancel();
              }
              else if (this._departmentService.editMode == true)
                this._snackBar.open(
                  'Department record Updated successfully.',
                  'x',
                  {
                    duration: 3000,
                    horizontalPosition: 'right',
                    verticalPosition: 'top',
                  }
                );
              this.cancel();
            } else {
              this._snackBar.open('Department cannot be added.', 'x', {
                duration: 3000,
                panelClass: ['error-alert'],
                horizontalPosition: 'right',
                verticalPosition: 'top',
              });
              this.cancel();
            }
          },
          (objError) => {
            var sMessage ="Server error.";

            var vDeptNameExists =  "Department Name already exists";
            var vDeptCodeExists  ="Department code already exists";

            if(objError.error== vDeptNameExists || objError.error== vDeptCodeExists )
            {
              sMessage = objError.error;
            }
            this._snackBar.open(sMessage, 'x', {
              duration: 3000,
              panelClass: ['error-alert'],
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
          }
        );
      }
    }
  }

  clearField() {
    this.addDepartment.controls.DepartmentHead.setValue('');
  }

  onUserChange(event: any, empID: any) {
    const selectedUserId = this.addDepartment.get('DepartmentHead').value.value;
    this.empData.EmpId = selectedUserId;
    if (empID !== undefined && selectedUserId !== undefined) {
      let empName = '';
      this.usersList.forEach((e) => {
        if (e.value === selectedUserId) {
          empName = e.label;
        }
      });
    }
  }
  displayFn(user: any) {
    return user && user.label ? user.label : '';
  }

  private _filterName(value): any {
    let filterValue;
    if (value && value.DepartmentHead) {
      filterValue = value.DepartmentHead;
      return this.usersList.filter((option) =>
        option.label.toLowerCase().includes(filterValue)
      );
    } else {
      return this.usersList;
    }
  }

  omit_special_char(event: any) {
    let k: number;
    k = event.charCode;
    return (
      (k > 64 && k < 91) ||
      (k > 96 && k < 123) ||
      k == 8 ||
      k == 32 ||
      k == 38 ||
      k == 44 ||
      k == 45
    );
  }

  cancel() {
    this.addDepartment.reset();
    this.formGroupDirective.resetForm();
    this._departmentService.editMode = false;
    this.btnLabel = 'Save';
    this.isEdit = false;
    this.formSubmitted = false;
  }

  getUsersList() {
    this.service.GetAssociateNames().subscribe((res: any[]) => {
      let dataList: any[] = res;
      this.usersList = [];
      // this.usersList.push({ label: 'Select Department Head', value: null });
      dataList.forEach((e) => {
        this.usersList.push({ label: e.EmpName, value: e.EmpId });
      });
      this.filteredOptionsName = this.addDepartment.valueChanges.pipe(
        startWith(''),
        map((value) => this._filterName(value))
      );
    });
  }

  getDepartmentDetails() {
    this._departmentService.getDepartmentDetails();
  }

  editDepartments(departmentData) {
    this._departmentService.editMode = true;
    this._departmentService.departmentsEdit.next(departmentData);
    this.addDepartment.controls['DepartmentHead'].setValue({
      label: departmentData.DepartmentHeadName,
      value: departmentData.DepartmentHeadId,
    });
  }

  ngOnDestroy() {
  }
}
