import { Component, OnInit, ViewChild } from '@angular/core';
import {
  FormGroup,
  FormControl,
  Validators,
  FormGroupDirective,
} from '@angular/forms';
import { CompetencyArea } from '../../models/competencyarea.model';
import { CompetencyAreaService } from '../../services/competency-area.service';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { themeconfig } from '../../../../../themeconfig';
import { MatPaginator } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { NavService } from 'src/app/modules/master-layout/services/nav.service';

@Component({
  selector: 'app-competency-area',
  templateUrl: './competency-area.component.html',
  styleUrls: ['./competency-area.component.scss'],
})
export class CompetencyAreaComponent implements OnInit {
  displayedColumns: string[] = [
    'CompetencyAreaCode',
    'CompetencyAreaDescription',
    'Edit',
  ];
  dataSource: MatTableDataSource<CompetencyArea>;
  @ViewChild(FormGroupDirective) formGroupDirective: FormGroupDirective;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  constructor(
    private serviceObj: CompetencyAreaService,
    private _snackBar: MatSnackBar,
    public navService: NavService
  ) {
    this.serviceObj.competencyAreaData.subscribe((data) => {
      this.competencyAreaData = data;
      this.dataSource = new MatTableDataSource(this.competencyAreaData);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
    });
    this.navService.currentSearchBoxData.subscribe((responseData) => {
      this.applyFilter(responseData);
    });
  }
  res: any;
  formSubmitted = false;
  btnLabel = '';
  addCompetencyArea: FormGroup;
  themeConfigInput = themeconfig.formfieldappearances;
  competencyAreaData: CompetencyArea[];

  ngOnInit(): void {
    this.addCompetencyArea = new FormGroup({
      CompetencyAreaCode: new FormControl(null, [Validators.required]),
      CompetencyAreaDescription: new FormControl(null, [Validators.required]),
    });

    this.serviceObj.editObj.subscribe((data) => {
      if (this.serviceObj.editMode == true) {
        this.addCompetencyArea.patchValue(data);
        this.addCompetencyArea.get('CompetencyAreaCode').disable();
        this.btnLabel = 'Update';
      }
    });
    this.btnLabel = 'Save';
    this.addCompetencyArea.get('CompetencyAreaCode').enable();
    this.Reset();
    this.res = this.serviceObj.GetCompetencyAreaData();
  }

  Reset() {
    this.formSubmitted = false;
    this.addCompetencyArea.reset();
    setTimeout(() => this.formGroupDirective.resetForm(), 0);
    this.serviceObj.editMode = false;
    this.addCompetencyArea.get('CompetencyAreaCode').enable();
    this.btnLabel = 'Save';
    this.res = this.serviceObj.GetCompetencyAreaData();
  }
  CreateCompetencyArea() {
    this.formSubmitted = true;
    var createObj = new CompetencyArea();
    createObj.CompetencyAreaCode = this.addCompetencyArea.value.CompetencyAreaCode;
    createObj.CompetencyAreaDescription = this.addCompetencyArea.value.CompetencyAreaDescription;
    if (this.serviceObj.editMode == true) {
      createObj.CompetencyAreaId = this.serviceObj.editObj.value.CompetencyAreaId;
      createObj.CompetencyAreaCode = this.addCompetencyArea.getRawValue().CompetencyAreaCode;
      createObj.CompetencyAreaDescription = this.addCompetencyArea.value.CompetencyAreaDescription;
      createObj.IsActive = this.serviceObj.editObj.value.IsActive;
      this.serviceObj.GetCompetencyAreaData();
    }
    if (this.addCompetencyArea.valid == true) {
      this.serviceObj.CreateCompetencyArea(createObj).subscribe(
        (res) => {
          if (res) {
            if (this.serviceObj.editMode == false) {
              this._snackBar.open(
                'Competency area record added successfully.',
                'x',
                {
                  duration: 3000,
                  horizontalPosition: 'right',
                  verticalPosition: 'top',
                }
              );
              this.Reset();
            } else if (this.serviceObj.editMode == true) {
              this.serviceObj.GetCompetencyAreaData();
              this._snackBar.open(
                'Competency area record updated successfully.',
                'x',
                {
                  duration: 3000,
                  horizontalPosition: 'right',
                  verticalPosition: 'top',
                }
              );
              this.Reset();
            }
          } else {
            this.Reset();
            this._snackBar.open('Unable to add competency', 'x', {
              duration: 3000,
              panelClass: ['error-alert'],
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });

            
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

  setEditObj(editObj) {
    this.serviceObj.editMode = true;
    // this.onDisable();
    this.serviceObj.editObj.next(editObj);
  }
  applyFilter(event: Event) {
    if (event) {
      const filterValue = (event.target as HTMLInputElement).value;
      this.dataSource.filter = filterValue.trim().toLowerCase();
    } else {
      this.dataSource = new MatTableDataSource(this.competencyAreaData);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
    }
  }
  // onDisable()
  // { if(this.serviceObj.editMode==true)
  //      return true;

  // }
}
