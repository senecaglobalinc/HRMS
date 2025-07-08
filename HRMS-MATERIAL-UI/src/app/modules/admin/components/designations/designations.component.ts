import { Component, OnInit, ViewChild } from '@angular/core';
import { Designation } from '../../models/designation.model';
import { DesignationsService } from '../../services/designations.service';
import * as servicePath from '../../../../core/service-paths';
import { FormGroup, ValidationErrors } from '@angular/forms';
import { FormControl } from '@angular/forms';
import { Validators } from '@angular/forms';
import { DesignationData } from '../../models/designation.model';
import { Grade } from '../../models/grade.model';
import { Observable } from 'rxjs/Observable';
import { map, startWith } from 'rxjs/operators';
import { NavService } from 'src/app/modules/master-layout/services/nav.service';

export interface GradeCode {
  label: string;
  value: number;
}

import {
  MatSnackBar,
  MatSnackBarHorizontalPosition,
  MatSnackBarVerticalPosition,
} from '@angular/material/snack-bar';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';
import { MatSort } from '@angular/material/sort';
import { themeconfig } from '../../../../../themeconfig';

@Component({
  selector: 'app-designations',
  templateUrl: './designations.component.html',
  styleUrls: ['./designations.component.scss'],
})
export class DesignationsComponent implements OnInit {
  themeappeareance = themeconfig.formfieldappearances;

  addDesignation: FormGroup;
  btnLabel = '';
  formSubmitted = false;
  GradeCodeMasterData = [];
  options: GradeCode[] = [];
  gradeData: DesignationData;

  designationList: Designation[];
  list: any;
  selectedDesignation: Designation;
  resources = servicePath.API.PagingConfigValue;
  displayedColumns: string[] = [
    'GradeCode',
    'DesignationCode',
    'DesignationName',
    'Edit',
  ];
  filteredOptionsGrade: Observable<any>;

  columnsToDisplay: string[] = this.displayedColumns.slice();
  dataSource = new MatTableDataSource<Designation>();

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  constructor(
    private _designationService: DesignationsService,
    private _snackBar: MatSnackBar,
    public navService: NavService
  ) {
    this.GetGradesData();
    this._designationService.designationData.subscribe((data) => {
      data.forEach(function (value) {
        if (value.Grade != null) value.GradeCode = value.Grade.GradeCode;
        else value.GradeCode = '';
      });
      this.dataSource.data = data;
      this.designationList = data;
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
    });
    this.navService.currentSearchBoxData.subscribe((responseData) => {
      this.applyFilter(responseData);
    });
  }

  ngOnInit() {
    this.options = this.GradeCodeMasterData;
    this.btnLabel = 'Save';

    this.addDesignation = new FormGroup({
      GradeId: new FormControl('', [Validators.required]),
      DesignationCode: new FormControl('', [Validators.required]),
      DesignationName: new FormControl('', [Validators.required])
    });
    this._designationService.editObj.subscribe((data) => {
      if (this._designationService.editMode == true) {
        this.btnLabel = 'Update';
        this.addDesignation.patchValue(data);
      }
    });
    this.Reset();

    this._designationService.getDesignation();
  }

  onGradeChange(event: any, gradeID: any) {
    const selectedUserId = this.addDesignation.get('GradeId').value.value;
    this.gradeData.GradeId = selectedUserId;
    if (gradeID !== undefined && selectedUserId !== undefined) {
      let gradeName = '';
      this.GradeCodeMasterData.forEach((e) => {
        if (e.value === selectedUserId) {
          gradeName = e.label;
        }
      });
      this.options = this.GradeCodeMasterData;
      event.option.value = '';
    }
  }
  setEditObj(editObj) {
    this._designationService.editMode = true;
    this._designationService.editObj.next(editObj);
    this.addDesignation.controls['GradeId'].setValue({
      label: editObj.GradeCode,
      value: editObj.GradeId,
    });
  }
  GetGradesData() {
    this._designationService.getGradesData().subscribe((res: Grade[]) => {
      res.forEach((element) => {
        this.GradeCodeMasterData.push({
          label: element.GradeCode,
          value: element.GradeId,
        });
        this.filteredOptionsGrade = this.addDesignation
          .get('GradeId')
          .valueChanges.pipe(
            startWith(''),
            map((value) => this._filterGrade(value))
          );
      });
      this.options = this.GradeCodeMasterData;
    });
  }

  applyFilter(event: Event) {
    if (event) {
      const filterValue = (event.target as HTMLInputElement).value;
      this.dataSource.filter = filterValue.trim().toLowerCase();
    } else {
      this.dataSource = new MatTableDataSource(this.designationList);
    }
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  CreateDesignation() {
    this.formSubmitted = true;
    this.addDesignation.markAllAsTouched();
    if (this.addDesignation.valid == true) {
      var creatingObj = new DesignationData();
      creatingObj.DesignationCode = this.addDesignation.value.DesignationCode;
      creatingObj.DesignationName = this.addDesignation.value.DesignationName;
      creatingObj.GradeId = this.addDesignation.value.GradeId['value'];
    }
    if (this._designationService.editMode == true) {
      creatingObj.DesignationId = this._designationService.editObj.value.DesignationId;
      creatingObj.IsActive = this._designationService.editObj.value.IsActive;
    }

    if (this.addDesignation.valid == true) {
      if (this._designationService.editMode == false) {
        this._designationService.createDesignation(creatingObj).subscribe(
          (response) => {
            if (response != null) {
              this._designationService.getDesignation();
              if (this._designationService.editMode == false)
                this._snackBar.open(
                  'Designation record added successfully.',
                  'x',
                  {
                    duration: 3000,
                    horizontalPosition: 'right',
                    verticalPosition: 'top',
                  }
                );
              this.Reset();
            }
          },
          (error) => {
            this._snackBar.open(error.error, 'x', {
              duration: 3000,
              panelClass: ['error-alert'],
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
          }
        );
      } else {
        this._designationService.editDesignation(creatingObj).subscribe(
          (response) => {
            if (response != null) {
              this._designationService.getDesignation();
              this._snackBar.open(
                'Designation record updated successfully.',
                'x',
                {
                  duration: 3000,
                  horizontalPosition: 'right',
                  verticalPosition: 'top',
                }
              );
              this.Reset();
            }
          },
          (error) => {
            this._snackBar.open(error.error, 'x', {
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

  Reset() {
    this.formSubmitted = false;
    this.addDesignation.reset();
    this.btnLabel = 'Save';
    this._designationService.editMode = false;
  }
  private _filterGrade(value): any {
    let filterValue;
    if (typeof value === 'string') {
      filterValue = value.toLowerCase();
    }
    else {
      return this.GradeCodeMasterData;
    }

    return this.GradeCodeMasterData.filter((option) =>
      option.label.toLowerCase().includes(filterValue)
    );
  }
  displayFn(grade: GradeCode): string {
    return grade && grade.label ? grade.label : '';
  }
  getlabel(labelId) {
    return this.GradeCodeMasterData.find((grade) => grade.value === labelId)
      .title;
  }
  clearField() {
    this.addDesignation.controls.GradeId.setValue('');
  }


}
