import { Component, OnInit, ViewChild } from '@angular/core';
import { KeyFunctionService } from '../../services/key-function.service';
import { KeyFunction } from '../../models/key-function.model';
import { Department } from '../../models/department.model';
// import { DepartmentSelect } from '../../models/department-select';

import { Seniority } from '../../models/seniority.model';
import { SeniorityService } from '../../services/seniority.service';
import * as servicePath from '../../../../core/service-paths';
import {
  FormGroup,
  FormControl,
  Validators,
  FormGroupDirective,
  ValidationErrors,
} from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { NavService } from 'src/app/modules/master-layout/services/nav.service';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { themeconfig } from 'src/themeconfig';

import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';

@Component({
  selector: 'app-key-function',
  templateUrl: './key-function.component.html',
  styleUrls: ['./key-function.component.scss'],
})
export class KeyFunctionComponent implements OnInit {
  themeConfigInput = themeconfig.formfieldappearances;
  filteredOptions: Observable<any>;

  isEdit: boolean;
  btnLabel: string = '';
  formSubmitted = false;
  addkeyFunction: FormGroup;
  keyFunction: KeyFunction;
  departmentList = [];

  // KeyFunctionList : KeyFunction[];

  KeyFunctionList: any[] = [];

  selectedRow: KeyFunction;
  PageSize: number;
  PageDropDown: number[] = [];
  resources = servicePath.API.PagingConfigValue;

  dataSource: MatTableDataSource<KeyFunction>;
  @ViewChild(FormGroupDirective) formGroupDirective: FormGroupDirective;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  displayedColumns: string[] = ['Department', 'SGRoleName', 'Edit'];

  constructor(
    private _keyFunctionService: KeyFunctionService,
    private _snackBar: MatSnackBar,
    public navService: NavService
  ) {
    this.getDepartmentList();

    this._keyFunctionService.KeyFunctionList.subscribe((data) => {
      data.forEach(function (value) {
        if (value.Department != null)
          value.Description = value.Department.Description;
        else value.Description = '';
      });
      this.KeyFunctionList = data;
      this.dataSource = new MatTableDataSource(this.KeyFunctionList);

      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
    });

    this.navService.currentSearchBoxData.subscribe((responseData) => {
      this.applyFilter(responseData);
    });
  }

  ngOnInit() {
    // this.getDepartmentList();
    this.btnLabel = 'SAVE';
    this.keyFunction = new KeyFunction();
    this.addkeyFunction = new FormGroup({
      DepartmentId: new FormControl('', [Validators.required]),
      SGRoleName: new FormControl(null, [
        Validators.required,
        Validators.maxLength(100),
        Validators.minLength(2)
      ]),
    });

    this.departmentList;

    this._keyFunctionService.KeyFunctionEdit.subscribe((data) => {
      if (this._keyFunctionService.editMode == true) {
        this.isEdit = this._keyFunctionService.editMode;
        this.addkeyFunction.patchValue(data);
        this.addkeyFunction.controls.DepartmentId.patchValue(
          data.Description
        );
        this.btnLabel = 'UPDATE';
      }
    });
    // this.cancel();

    this._keyFunctionService.KeyFunctionList.subscribe((data) => {
      data.forEach(function (value) {
        if (value.Department != null)
          value.Description = value.Department.Description;
        else value.Description = '';
      });
      this.KeyFunctionList = data;
      this.dataSource = new MatTableDataSource(this.KeyFunctionList);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
    });
    this.getKeyFunctions();
  }

  getDepartmentList(): void {
    this._keyFunctionService.getDepartmentList().subscribe(
      (res: any[]) => {
        this.departmentList = [];
        res.forEach((e) => {
          this.departmentList.push({
            label: e.Description,
            value: e.DepartmentId,
          });
        });
        this.filteredOptions = this.addkeyFunction.valueChanges.pipe(
          startWith(''),
          map((value) => this._filter(value))
        );
      },
      (error) => {
        this._snackBar.open(error.error, '', {
          duration: 3000,
          panelClass: ['error-alert'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
      }
    );
  }

  getKeyFunctions(): void {
    this._keyFunctionService.getKeyFunctions();
  }

  editKeyFunction(keyFunctionEdit): void {
    this.addkeyFunction.controls['DepartmentId'].disable();
    this._keyFunctionService.editMode = true;
    this._keyFunctionService.KeyFunctionEdit.next(keyFunctionEdit);
  }

  addkeyFunctions(): void {
    if (this.addkeyFunction.valid) {
      this.formSubmitted = true;
      var keyFunction = new KeyFunction();

      let tempDepartMentID = this.getDepartmentIDbyName(
        this.addkeyFunction.value.DepartmentId
      );

      // keyFunction.DepartmentId = this.addkeyFunction.value.DepartmentId;
      keyFunction.SGRoleName = this.addkeyFunction.value.SGRoleName;
      if (this._keyFunctionService.editMode == true) {
        keyFunction.SGRoleID = this._keyFunctionService.KeyFunctionEdit.value.SGRoleID;
        keyFunction.DepartmentId = this._keyFunctionService.KeyFunctionEdit.value.DepartmentId;
      }
      else{
        keyFunction.DepartmentId = tempDepartMentID[0].value;
      }
      if (this.addkeyFunction.valid == true) {
        this._keyFunctionService.createKeyFunction(keyFunction).subscribe(
          (res) => {
            if (res != null) {
              this._keyFunctionService.getKeyFunctions();
              if (this._keyFunctionService.editMode == false)
                this._snackBar.open(
                  'Key function record added successfully.',
                  '',
                  {
                    duration: 3000,
                    horizontalPosition: 'right',
                    verticalPosition: 'top',
                  }
                );
              else
                this._snackBar.open(
                  'Key function record updated successfully.',
                  '',
                  {
                    duration: 3000,
                    horizontalPosition: 'right',
                    verticalPosition: 'top',
                  }
                );
              this.cancel();
            }
          },
          (error) => {
            this._snackBar.open(error.error, '', {
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

  cancel(): void {
    this.formSubmitted = false;
    this.addkeyFunction.reset();
    setTimeout(() => this.formGroupDirective.resetForm(), 0);
    this._keyFunctionService.editMode = false;
    this.btnLabel = 'SAVE';
    this.isEdit = false;
    this.addkeyFunction.controls['DepartmentId'].enable();
  }

  applyFilter(event: Event) {
    if (event) {
      const filterValue = (event.target as HTMLInputElement).value;
      this.dataSource.filter = filterValue.trim().toLowerCase();
    } else {
      this.dataSource = new MatTableDataSource(this.KeyFunctionList);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
    }
  }

  private _filter(value) {
    let filterValue;
    if (value && value.DepartmentId) {
      filterValue = value.DepartmentId.toLowerCase();
      return this.departmentList.filter((option) =>
        option.label.toLowerCase().includes(filterValue)
      );
    } else {
      return this.departmentList;
    }
  }

  cliearField() {
    this.addkeyFunction.controls.DepartmentId.setValue('');
  }

  getDepartmentIDbyName(e): any {
    return this.departmentList.filter((x) => x.label === e);
  }
}
