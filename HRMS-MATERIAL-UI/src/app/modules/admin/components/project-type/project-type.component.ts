import { Component, OnInit, ViewChild } from '@angular/core';
import { ProjectTypeService } from '../../services/project-type.service';
import { FormGroup, FormControl, Validators, FormBuilder, FormGroupDirective } from '@angular/forms';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { themeconfig } from 'src/themeconfig';
import { ProjectTypeData } from '../../models/projecttype.model';
import { MatSnackBar } from '@angular/material/snack-bar';
import { NavService } from 'src/app/modules/master-layout/services/nav.service';
import * as servicePath from '../../../../core/service-paths';

@Component({
  selector: 'app-project-type',
  templateUrl: './project-type.component.html',
  styleUrls: ['./project-type.component.scss']
})
export class ProjectTypeComponent implements OnInit {
  themeConfigInput = themeconfig.formfieldappearances;
  valid = false;
  btnLabel = "";
  addProjectType: FormGroup;
  formSubmitted: boolean;
  projectTypeData: ProjectTypeData[];
  PageSize: number;
  PageDropDown: number[] = [];
  resources = servicePath.API.PagingConfigValue;
  displayedColumns: string[] = ['ProjectTypeCode', 'Description', 'Edit'];
  dataSource: MatTableDataSource<ProjectTypeData>;
  @ViewChild(FormGroupDirective) formGroupDirective: FormGroupDirective;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  constructor(private serviceObj: ProjectTypeService, private _snackBar: MatSnackBar,
    private fb: FormBuilder, public navService: NavService) {
    this.PageSize = this.resources.PageSize;
    this.PageDropDown = this.resources.PageDropDown;
    this.serviceObj.projectTypeData.subscribe(data => {
      this.projectTypeData = data;
      this.dataSource = new MatTableDataSource(this.projectTypeData);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
    });
    this.navService.currentSearchBoxData.subscribe((responseData) => {
      this.applyFilter(responseData);
    })
  }


  ngOnInit() {

    this.addProjectType = this.fb.group({

      ProjectTypeCode: ['', [Validators.required]],
      Description: ['', [Validators.required]],

    });
    this.btnLabel = "Save";

    this.serviceObj.editObj.subscribe((data) => {
      if (this.serviceObj.editMode == true) {
        this.addProjectType.patchValue(data);
        this.btnLabel = "Update";
      }
    });

    this.GetProjectTypeData();
    this.Reset();
  }
  applyFilter(event: Event) {
    if (event) {
      const filterValue = (event.target as HTMLInputElement).value;
      this.dataSource.filter = filterValue.trim().toLowerCase();
    } else {
      this.dataSource = new MatTableDataSource(this.projectTypeData);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
    }
  }


  CreateProjectType(): void {
    this.formSubmitted = true;
    var creatingObj = new ProjectTypeData();

    creatingObj.Description = this.addProjectType.value.Description;
    creatingObj.ProjectTypeCode = this.addProjectType.value.ProjectTypeCode;

    if (this.serviceObj.editMode == true) {
      creatingObj.ProjectTypeId = this.serviceObj.editObj.value.ProjectTypeId;
    }
    if (this.addProjectType.valid == true) {

      this.serviceObj.createProjectType(creatingObj).subscribe(res => {
        if (res) {
          this.serviceObj.getProjectTypeData();
          if (this.serviceObj.editMode == false) {
            this._snackBar.open('Project type record added successfully.', 'x', {
              duration: 3000,
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
            setTimeout(() =>
              this.formGroupDirective.resetForm(), 0)
            this.Reset();
          }

          else {
            this._snackBar.open('Project type record updated successfully.', 'x', {
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
          this._snackBar.open('Unable to add project type.', '', {
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
          error;
          this._snackBar.open(error.error, 'x', {
            duration: 3000,
            panelClass: ['error-alert'],
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
          setTimeout(() =>
            this.formGroupDirective.resetForm(), 0)

        });
    }

  }
  Validate(value): void {
    let textRe = /^[a-zA-Z ]*$/;
    this.valid = textRe.test(value);
  }
  GetProjectTypeData(): void {
    this.serviceObj.getProjectTypeData();
  }

  edditProjecttype(ProjectTypeData): void {
    this.serviceObj.editMode = true;
    this.serviceObj.editObj.next(ProjectTypeData);
  }
  Reset(): void {
    this.formSubmitted = false;
    this.addProjectType.reset();
    this.serviceObj.editMode = false;
    this.btnLabel = "Save";
  }

}
