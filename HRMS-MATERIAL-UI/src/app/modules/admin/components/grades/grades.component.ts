import { Component, OnInit, ViewChild } from '@angular/core';
import { GradesService } from '../../services/grades.service';
import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';
import { Grade } from '../../models/grade.model';
import * as servicePath from '../../../../core/service-paths';
import { FormGroup, FormControl, Validators, FormGroupDirective } from '@angular/forms';
import {
  MatSnackBar,
  MatSnackBarHorizontalPosition,
  MatSnackBarVerticalPosition,
} from '@angular/material/snack-bar';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { themeconfig } from '../../../../../themeconfig';
import { NavService } from 'src/app/modules/master-layout/services/nav.service';

@Component({
  selector: 'app-grades',
  templateUrl: './grades.component.html',
  styleUrls: ['./grades.component.scss'],
})
export class GradesComponent implements OnInit {
  @ViewChild(FormGroupDirective) formGroupDirective: FormGroupDirective;
  themeappeareance = themeconfig.formfieldappearances;
  resources = servicePath.API.PagingConfigValue;
  PageSize: number;
  PageDropDown: number[] = [];
  gradesData: Grade[];
  selectedGrade: Grade;
  btnLabel = '';
  valid = true;
  addGrades: FormGroup;
  formSubmitted = false;

  constructor(
    private serviceObj: GradesService,
    private _snackBar: MatSnackBar,
    public navService: NavService
  ) {
    this.serviceObj.GradesData.subscribe((data) => {
      this.gradesData = data;
      this.dataSource.data = data;
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
      this.dataSource.sortingDataAccessor = (data, attribute) => data[attribute];
    });

    this.navService.currentSearchBoxData.subscribe((responseData) => {
      this.applyFilter(responseData);
    });
  }

  ngOnInit() {
    this.GetGradesDetails();
    this.addGrades = new FormGroup({
      GradeCode: new FormControl(null, [Validators.required]),
      GradeName: new FormControl(null, [Validators.required]),
    });
    this.serviceObj.editObj.subscribe((data) => {
      if (this.serviceObj.editMode == true) {
        this.addGrades.patchValue(data);
        this.btnLabel = 'Update';
      }
    });
    this.btnLabel = 'Save';
    this.Reset();
  }

  cols = [
    { field: 'GradeCode', header: 'Grade Code' },
    { field: 'GradeName', header: 'Grade Name' },
  ];
  displayedColumns: string[] = ['GradeCode', 'GradeName', 'Edit'];
  columnsToDisplay: string[] = this.displayedColumns.slice();
  dataSource = new MatTableDataSource<Grade>();
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

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
      this.dataSource = new MatTableDataSource(this.gradesData);
    }
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  GetGradesDetails(): void {
    this.serviceObj.getGradesDetails();
  }

  SetEditObj(editObj: Grade): void {
    this.serviceObj.editMode = true;
    this.serviceObj.editObj.next(editObj);
  }

  Reset() {
    this.formSubmitted = false;
    this.addGrades.reset();
    setTimeout(() => this.formGroupDirective.resetForm(), 0);
    this.serviceObj.editMode = false;
    this.btnLabel = 'Save';
  }

  CreateGrades() {
    this.formSubmitted = true;
    this.addGrades.markAllAsTouched();
    var creatingObj = new Grade();
    creatingObj.GradeCode = this.addGrades.value.GradeCode;
    creatingObj.GradeName = this.addGrades.value.GradeName;

    if (this.serviceObj.editMode == true) {
      creatingObj.IsActive = this.serviceObj.editObj.value.IsActive;
      creatingObj.GradeId = this.serviceObj.editObj.value.GradeId;
      // creatingObj.GradeCode = this.serviceObj.editObj.value.GradeCode;
    }
    if (this.addGrades.valid == true) {
      this.serviceObj.createGrades(creatingObj).subscribe(
        (res) => {
          if (res != null) {
            this.serviceObj.getGradesDetails();
            if (this.serviceObj.editMode == false)
              //  this.messageService.add({severity:'success', summary: 'Success Message', detail:'Grade record added successfully.'});
             { this._snackBar.open('Grade record added successfully.', 'x', {
                duration: 3000,
                horizontalPosition: 'right',
                verticalPosition: 'top',
              });
              this.Reset();
            }
            // this.messageService.add({severity:'success', summary: 'Success Message', detail:'Grade record updated successfully.'});
            else{
              this._snackBar.open('Grade record updated successfully.', 'x', {
                duration: 3000,
                horizontalPosition: 'right',
                verticalPosition: 'top',
              });
            this.Reset();}
          }
        },
        (error) => {
          this._snackBar.open(error.error, 'x', {
            duration: 3000,
            panelClass: ['error-alert'],
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
          // this.messageService.add({severity:'error', summary: 'Error message', detail:error.error});
        }
      );
    } else {
      // this.messageService.add({severity:'warn', summary: 'Warning message', detail:'Invalid data'});
      // this.Reset();
    }
  }

  Validate(value) {
    let textRe = /^[a-zA-Z ]*$/;
    this.valid = textRe.test(value);
  }

  ngOnDestroy() {
    // this.serviceObj.GradesData.unsubscribe();
  }
}
