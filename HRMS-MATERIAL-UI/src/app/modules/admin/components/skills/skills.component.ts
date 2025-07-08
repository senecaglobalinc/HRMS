import { Component, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup, FormGroupDirective, Validators } from '@angular/forms';
import { themeconfig } from 'src/themeconfig';
import { SkillsService } from '../../services/skills.service';
import { CompetencyAreaData } from '../../models/competency-area-data.model';
import { SkillGroupData } from '../../models/skill-group-data.model';
import { SkillData } from '../../models/skill-data.model';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatSnackBar } from '@angular/material/snack-bar';
import { NavService } from 'src/app/modules/master-layout/services/nav.service';

interface SelectItem {
  value: number;
  label: string;
}

@Component({
  selector: 'app-skills',
  templateUrl: './skills.component.html',
  styleUrls: ['./skills.component.scss']
})
export class SkillsComponent implements OnInit {

  themeConfigInput = themeconfig.formfieldappearances;

  displayedColumns: string[] = ['CompetencyAreaCode', 'SkillGroupName', 'SkillCode', 'SkillDescription', 'Edit'];
  formSubmitted = false;
  requiredValue = true;
  isEdit: boolean;

  competencyAreaList: SelectItem[];
  skillGroupList: SelectItem[];

  skillsList: SkillData[];

  addSkills: FormGroup;
  btnLabel: string = "";

  dataSource: MatTableDataSource<SkillData>;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  @ViewChild(FormGroupDirective) formGroupDirective: FormGroupDirective;

  constructor(private _skillsService: SkillsService, private _snackBar: MatSnackBar, public navService: NavService) {
    this.navService.currentSearchBoxData.subscribe(responseData => {
      this.applyFilter(responseData);
    })

  }

  ngOnInit(): void {

    this.getCompetencyArea();
    this.btnLabel = "SAVE";

    this.addSkills = new FormGroup({
      CompetencyAreaId: new FormControl(null, [
        Validators.required,
      ]),
      SkillGroupId: new FormControl(null, [
        Validators.required
      ]),
      SkillCode: new FormControl(null, [
        Validators.required,
      ]),
      SkillDescription: new FormControl(null),
    });
    this.cancel();

    this._skillsService.skillsEdit.subscribe(data => {
      if (this._skillsService.editMode == true && data != null) {
        if (data.CompetencyAreaId > 0) {
          this.skillGroupList = [];
          this._skillsService.getSkillGroupByCompetencyArea(data.CompetencyAreaId).subscribe((res: SkillData[]) => {
            if (res.length != null) {
              res.forEach(e => {
                this.skillGroupList.push({ label: e.SkillGroupName, value: e.SkillGroupId });
              });
            }
            this.addSkills.patchValue(data);
            this.isEdit = this._skillsService.editMode;
            this.btnLabel = "Update";
          },
            (error) => {
              this._snackBar.open(error.error, 'x', {
                duration: 3000,
                panelClass: ['error-alert'], 
                horizontalPosition: 'right',
                verticalPosition: 'top'
              });

            });
        }

      }
    });

    this._skillsService.skillsList.subscribe((data) => {
      data.forEach(function (value) {
        if (value.CompetencyArea != null)
          value.CompetencyAreaCode = value.CompetencyArea.CompetencyAreaCode;
        else value.CompetencyAreaCode = '';
      });
      data.forEach(function (value) {
        if (value.SkillGroup != null)
          value.SkillGroupName = value.SkillGroup.SkillGroupName;
        else value.SkillGroupName = '';
      });
      this.skillsList = data;
      this.dataSource = new MatTableDataSource(this.skillsList);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
    });
    this.getSkills();

  }

  getCompetencyArea(): void {
    this._skillsService.getCompetencyArea().subscribe((res: CompetencyAreaData[]) => {
      this.competencyAreaList = [];
      res.forEach(element => {
        this.competencyAreaList.push({ label: element.CompetencyAreaCode, value: element.CompetencyAreaId });
      });
    }, (error) => {
      this._snackBar.open(error.error, 'x', {
        duration: 3000,
        panelClass: ['error-alert'],
        horizontalPosition: 'right',
        verticalPosition: 'top'
      });

    });
  }

  getSkillGroupByCompetencyArea(competencyAreaId: number): void {
    if (competencyAreaId != 0 && competencyAreaId != null) {
      this._skillsService.getSkillGroupByCompetencyArea(competencyAreaId).subscribe((res: SkillGroupData[]) => {
        this.skillGroupList = [];
        res.forEach(element => {
          this.skillGroupList.push({ label: element.SkillGroupName, value: element.SkillGroupId });
        });
      }, (error) => {
        this._snackBar.open(error.error, 'x', {
          duration: 3000,
          panelClass: ['error-alert'],
          horizontalPosition: 'right',
          verticalPosition: 'top'
        });

      });
    }
    else {
      this.skillGroupList = [];
    }
  }

  getSkills(): void {
    this._skillsService.getSkills()
  }

  validateSkillCode(event: any) {
    // if (this.addSkills.get('SkillCode').value !== null && event.target.value.length > 0){
    
    // }
    var valueEvent = event.target.value.toString();
    if (valueEvent.charCodeAt(0) == 32) {
        event.preventDefault();
        this.addSkills.get('SkillCode').patchValue(null);
        event.target.value = null;
        this._snackBar.open(
          'No Empty spaces allowed',
          'x',
          {
            duration: 3000,
            horizontalPosition: 'right',
            verticalPosition: 'top',
          }
        );
      }

  }

  addSkill(): void {
    this.formSubmitted = true;
    var skills = new SkillData();
    skills.CompetencyAreaId = this.addSkills.value.CompetencyAreaId;
    skills.SkillGroupId = this.addSkills.value.SkillGroupId;
    skills.SkillCode = this.addSkills.value.SkillCode;
    skills.SkillName=this.addSkills.value.SkillCode;
    skills.SkillDescription = this.addSkills.value.SkillDescription;
    if (this._skillsService.editMode == true) {
      skills.SkillId = this._skillsService.skillsEdit.value.SkillId;
      this.getSkillGroupByCompetencyArea(skills.CompetencyAreaId);
      skills.SkillGroupId = this._skillsService.skillsEdit.value.SkillGroupId;
    }
    if (this.addSkills.valid == true) {
      this._skillsService.createSkills(skills).subscribe(res => {
        if (res) {
          this._skillsService.getSkills();
          if (this._skillsService.editMode == false) {
            this._snackBar.open('Skills record added successfully.', 'x', {
              duration: 3000,
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
            this.cancel();
          }

          else {
            this._snackBar.open('Skills record updated successfully.', 'x', {
              duration: 3000,
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
            this.cancel();
            setTimeout(() =>
              this.formGroupDirective.resetForm(), 0)
          }

        }
        else {
          this._snackBar.open('Unable to add skills.', 'x', {
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

  cancel(): void {
    this.formSubmitted = false;
    this.addSkills.reset();
    this.skillGroupList = [];
    setTimeout(() => this.formGroupDirective.resetForm(), 0)
    this._skillsService.editMode = false;
    this.btnLabel = "SAVE";
    this.isEdit = false;
  }

  editSkill(skillsEdit): void {
    this._skillsService.editMode = true;
    this._skillsService.skillsEdit.next(skillsEdit);
  }

  applyFilter(event: Event) {
    if (event) {
      const filterValue = (event.target as HTMLInputElement).value;
      this.dataSource.filter = filterValue.trim().toLowerCase();
    } else {
      this.dataSource = new MatTableDataSource(this.skillsList);
    }
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

}
