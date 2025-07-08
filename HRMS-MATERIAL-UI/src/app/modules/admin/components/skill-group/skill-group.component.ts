import { Component, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup, FormGroupDirective, ValidationErrors, Validators } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { themeconfig } from 'src/themeconfig';
import { CompetencyAreaData } from '../../models/competency-area-data.model';
import { SkillGroupData } from '../../models/skill-group-data.model';
import { SkillGroupService } from '../../services/skill-group.service';
import { NavService } from 'src/app/modules/master-layout/services/nav.service';
import { Observable } from 'rxjs';
import { startWith, map } from 'rxjs/operators';

interface SelectItem {
  value: number;
  label: string;
}

@Component({
  selector: 'app-skill-group',
  templateUrl: './skill-group.component.html',
  styleUrls: ['./skill-group.component.scss']
})
export class SkillGroupComponent implements OnInit {

  themeConfigInput = themeconfig.formfieldappearances;
  formSubmitted = false;
  filteredCompArea: Observable<any>;

  displayedColumns: string[] = ['CompetencyAreaCode', 'SkillGroupName', 'Description', 'Edit'];

  competencyAreaList: SelectItem[];
  skillGroupList: SkillGroupData[];

  addSkillGroup: FormGroup;
  btnLabel: string = "";
  isEdit: boolean;

  dataSource: MatTableDataSource<SkillGroupData>;
  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: false }) sort: MatSort;
  @ViewChild(FormGroupDirective) formGroupDirective: FormGroupDirective;

  constructor(private _skillGroupService: SkillGroupService, private _snackBar: MatSnackBar, public navService: NavService) {
    // this.getCompetencyArea();

    this._skillGroupService.skillGroupList.subscribe((data) => {

      data.forEach(function (value) {
        if (value.CompetencyArea != null)
          value.CompetencyAreaCode = value.CompetencyArea.CompetencyAreaCode;
        else value.CompetencyAreaCode = '';
      });
      this.skillGroupList = data;
      this.dataSource = new MatTableDataSource(this.skillGroupList);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
    });



    this.navService.currentSearchBoxData.subscribe(responseData => {
      this.applyFilter(responseData);
    });


  }

  ngOnInit(): void {
    this.getCompetencyArea();
    this.dataSource.sortingDataAccessor = (item, property) => {
      if (property === 'CompetencyAreaCode') {
        return item.CompetencyArea.CompetencyAreaCode;
      } else {
        return item[property];
      }
    };
    this.addSkillGroup = new FormGroup({
      CompetencyAreaId: new FormControl(null, 
      [Validators.required]),
      SkillGroupName: new FormControl(null, [
        Validators.required,
        Validators.maxLength(100),
        Validators.minLength(2)
      ]),
      Description: new FormControl(null, [Validators.maxLength(4000), Validators.minLength(2)]),
    });

    this._skillGroupService.skillGroupList.subscribe((data) => {
      data.forEach(function (value) {
        if (value.CompetencyArea != null)
          value.CompetencyAreaCode = value.CompetencyArea.CompetencyAreaCode;
        else value.CompetencyAreaCode = '';
      });
      this.skillGroupList = data;
      this.dataSource = new MatTableDataSource(this.skillGroupList);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
    });
    this.getCompetencyArea();
    this.getSkillGroup();
    this.btnLabel = "SAVE";

    this._skillGroupService.skillGroupEdit.subscribe(data => {

      if (this._skillGroupService.editMode == true) {
        // this.addSkillGroup.get('CompetencyAreaId').disable();
        this.isEdit = this._skillGroupService.editMode;
        this.addSkillGroup.patchValue(data);
        this.addSkillGroup.controls.CompetencyAreaId.patchValue(
          data.CompetencyAreaCode
        );
        this.btnLabel = "UPDATE";
      }
    });
    this.cancel();
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  getCompetencyArea(): void {

    this._skillGroupService.getCompetencyArea().subscribe((res: CompetencyAreaData[]) => {

      this.competencyAreaList = [];
      res.forEach(element => {
        this.competencyAreaList.push({ label: element.CompetencyAreaCode, value: element.CompetencyAreaId });
      });

      this.filteredCompArea = this.addSkillGroup.get('CompetencyAreaId').valueChanges.pipe(
        startWith(''),
        map((value) => this._filterCompArea(value))
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

  getSkillGroup(): void {
    this._skillGroupService.getSkillGroup();
  }

  addSkillGroups(): void {
    if (this.addSkillGroup.valid == true) {
      this.formSubmitted = true;
      var skillGroup = new SkillGroupData();

      skillGroup.CompetencyAreaId = this.addSkillGroup.value.CompetencyAreaId['value'];
      skillGroup.SkillGroupName = this.addSkillGroup.value.SkillGroupName;
      skillGroup.Description = this.addSkillGroup.value.Description;
      if (this._skillGroupService.editMode == true) {
        skillGroup.SkillGroupId = this._skillGroupService.skillGroupEdit.value.SkillGroupId;
        skillGroup.CompetencyAreaId = this._skillGroupService.skillGroupEdit.value.CompetencyAreaId;
      }
      if (this.addSkillGroup.valid == true) {
        this._skillGroupService.createSkillGroup(skillGroup).subscribe(res => {
          if (res != null) {
            this._skillGroupService.getSkillGroup();
            if (this._skillGroupService.editMode == false) {
              this._snackBar.open('Skills Group record added successfully.', 'x', {
                duration: 3000,
                 horizontalPosition: 'right',
                verticalPosition: 'top'
              });
              this.cancel();

            }
            else {
              this._snackBar.open('Skills Group record updated successfully.', 'x', {
                duration: 3000, 
                 horizontalPosition: 'right',
                verticalPosition: 'top'
              });
              this.cancel();
            }
          }
          else {
            this._snackBar.open('Unable to add Skills Group.', 'x', {
              duration: 3000,
              panelClass: ['error-alert'],
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
            this.cancel();
          }
        },
          error => {
            this._snackBar.open(error.error, 'x', {
              duration: 3000,
              panelClass: ['error-alert'],
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });

          });
      }
    }
  }


  editSkillGroup(skillGroupEdit): void {
    this._skillGroupService.editMode = true;
    this._skillGroupService.skillGroupEdit.next(skillGroupEdit);
    this.addSkillGroup.controls['CompetencyAreaId'].setValue({
      label: skillGroupEdit.CompetencyAreaCode,
      value: skillGroupEdit.CompetencyAreaId,
    });

  }

  cancel(): void {
    this.formSubmitted = false;
    this.addSkillGroup.reset();
    setTimeout(() => this.formGroupDirective.resetForm(), 0)
    this._skillGroupService.editMode = false;
    this.btnLabel = "SAVE";
    this.isEdit = false;
  }

  applyFilter(event: Event) {
    if (event) {
      const filterValue = (event.target as HTMLInputElement).value;
      this.dataSource.filter = filterValue.trim().toLowerCase();
    } else {
      this.dataSource = new MatTableDataSource(this.skillGroupList);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;

    }
  }

  private _filterCompArea(value) {
    let filterValue;
    if (typeof value === 'string') {
      filterValue = value.toLowerCase();
    } else if (value !== null) {
      filterValue = value.label;
    } else {
      return this.competencyAreaList;
    }

    return this.competencyAreaList.filter((option) =>
      option.label.toLowerCase().includes(filterValue)
    );

  }

  clearCompArea(): void {
    this.addSkillGroup.controls.CompetencyAreaId.setValue('');
  }

  displayFn(compArea: SelectItem): string {

    return compArea && compArea.label ? compArea.label : '';
  }

}
