import { Component, OnInit, ViewChild } from '@angular/core';
import { ProficiencyLevelService } from '../../services/proficiency-level.service';
import { FormGroup, FormControl, Validators, FormBuilder, FormGroupDirective } from '@angular/forms';
import { ProficiencyLevel } from '../../models/proficiency.model';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { themeconfig } from 'src/themeconfig';
import { MatSnackBar } from '@angular/material/snack-bar';
import { NavService } from 'src/app/modules/master-layout/services/nav.service';
import * as servicePath from '../../../../core/service-paths';


@Component({
  selector: 'app-proficiency-level',
  templateUrl: './proficiency-level.component.html',
  styleUrls: ['./proficiency-level.component.scss']
})

export class ProficiencyLevelComponent implements OnInit {
  themeConfigInput = themeconfig.formfieldappearances;
  btnLabel: string;
  valid = true;
  addProficiencyLevel: FormGroup;
  formSubmitted = false;
  proficiencyLevelData: ProficiencyLevel[];
  PageSize: number;
  PageDropDown: number[] = [];
  resources = servicePath.API.PagingConfigValue;

  dataSource: MatTableDataSource<ProficiencyLevel>;
  @ViewChild(FormGroupDirective) formGroupDirective: FormGroupDirective;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  displayedColumns: string[] = ['ProficiencyLevelCode', 'ProficiencyLevelDescription', 'Edit'];


  constructor(private serviceObj: ProficiencyLevelService,
    private fb: FormBuilder, private _snackBar: MatSnackBar,
    public navService: NavService,) {
    this.PageSize = this.resources.PageSize;
    this.PageDropDown = this.resources.PageDropDown;


    this.serviceObj.proficiencyLevelData.subscribe((data) => {
      this.proficiencyLevelData = data;
      this.dataSource = new MatTableDataSource(this.proficiencyLevelData);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
    });

    this.navService.currentSearchBoxData.subscribe(responseData => {
      this.applyFilter(responseData);
    })
  }

  ngOnInit() {

    this.addProficiencyLevel = this.fb.group({

      ProficiencyLevelCode: ['', [Validators.required]],
      ProficiencyLevelDescription: ['', [Validators.required]],

    });
    this.btnLabel = "Save";
    this.serviceObj.editObj.subscribe((data) => {
      if (this.serviceObj.editMode == true) {
        this.addProficiencyLevel.patchValue(data);
        this.btnLabel = "Update";
      }
    });
    this.GetProficiencyLevelData();
    this.Reset();
  }

  applyFilter(event: Event) {
    if (event) {
      const filterValue = (event.target as HTMLInputElement).value;
      this.dataSource.filter = filterValue.trim().toLowerCase();
    } else {
      this.dataSource = new MatTableDataSource(this.proficiencyLevelData);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
    }
  }

  CreateProficiencyLevelData(): void {
    this.formSubmitted = true;
    var creatingObj = new ProficiencyLevel();

    creatingObj.ProficiencyLevelCode = this.addProficiencyLevel.value.ProficiencyLevelCode;
    creatingObj.ProficiencyLevelDescription = this.addProficiencyLevel.value.ProficiencyLevelDescription;

    if (this.addProficiencyLevel.valid == true) {
      creatingObj.ProficiencyLevelCode = this.addProficiencyLevel.value.ProficiencyLevelCode;
      creatingObj.ProficiencyLevelDescription = this.addProficiencyLevel.value.ProficiencyLevelDescription;
      if (this.serviceObj.editMode == true) {
        creatingObj.IsActive = this.serviceObj.editObj.value.IsActive;
        creatingObj.ProficiencyLevelId = this.serviceObj.editObj.value.ProficiencyLevelId;
      }
      this.serviceObj.createProficiencyLevelData(creatingObj)
        .subscribe(res => {
          if (res) {
            this.serviceObj.getProficiencyLevelData();
            if (this.serviceObj.editMode == false) {
              this._snackBar.open('Proficiency level record added  successfully.', 'x', {
                duration: 3000,
                horizontalPosition: 'right',
                verticalPosition: 'top',
              });
              setTimeout(() =>
                this.formGroupDirective.resetForm(), 0)

            }
            else {
              this._snackBar.open('Proficiency level record updated  successfully.', 'x', {
                duration: 3000,
                horizontalPosition: 'right',
                verticalPosition: 'top',
              });
            }
            setTimeout(() =>
              this.formGroupDirective.resetForm(), 0)
            this.Reset();

          }
          else {
            this._snackBar.open('Unable to add proficiency level.', 'x', {
              duration: 3000,
              panelClass: ['error-alert'],
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
            setTimeout(() =>
              this.formGroupDirective.resetForm(), 0)
            this.Reset();
          }
        },
          error => {
            this._snackBar.open(error.error, 'x', {
              duration: 3000,
              panelClass: ['error-alert'],
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
            setTimeout(() =>
              this.formGroupDirective.resetForm(), 0)
            this.Reset();

          });
    }

  }
  GetProficiencyLevelData(): void {
    this.serviceObj.getProficiencyLevelData();
  }
  edditProficiencyLevel(ProficiencyLevel): void {
    this.serviceObj.editMode = true;
    this.serviceObj.editObj.next(ProficiencyLevel);
  }

  Validate(value): void {
    let textRe = /^[a-zA-Z ]*$/;
    this.valid = textRe.test(value);
  }
  Reset(): void {
    this.formSubmitted = false;
    this.addProficiencyLevel.reset();
    this.btnLabel = "Save";
    this.serviceObj.editMode = false;
  }

}