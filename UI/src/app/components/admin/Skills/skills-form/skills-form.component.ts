import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { SelectItem } from 'primeng/components/common/selectitem';
import { SkillData } from '../../../../models/skills.model';
import { SkillsService } from '../../services/skills.service';
import { CompetencyArea } from '../../models/competencyarea.model';
import { SkillGroupData } from '../../../../models/skillgroup.model';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-skills-form',
  templateUrl: './skills-form.component.html',
  styleUrls: ['./skills-form.component.css'],
  providers: [MessageService]
})
export class SkillsFormComponent implements OnInit {
  btnLabel = "";
  addSkills: FormGroup;
  formSubmitted = false;
  competencyAreaList: SelectItem[];
  skillGroupList: SelectItem[];
  isEdit: boolean;
  constructor(private _skillsService: SkillsService, private messageService: MessageService) { }

  ngOnInit() {
    this.getCompetencyAreaList();
    this.btnLabel = "Save";
    this.addSkills = new FormGroup({
      CompetencyAreaId: new FormControl(null, [
        Validators.required
      ]),
      SkillGroupId: new FormControl(null, [
        Validators.required
      ]),
      SkillCode: new FormControl(null, [
        Validators.required
      ]),
      SkillDescription: new FormControl(null),
    });
    this.cancel();

    this._skillsService.skillsEdit.subscribe(data => {
      if (this._skillsService.editMode == true && data != null) {
        if (data.CompetencyAreaId > 0) {
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
              this.messageService.add({ severity: 'error', summary: 'Error message', detail: error.error });

            });
        }

      }
    });
    // this.cancel();
  }

  getCompetencyAreaList(): void {
    this._skillsService.getCompetencyAreaList().subscribe((res: CompetencyArea[]) => {
      this.competencyAreaList = [];
      this.competencyAreaList.push({ label: "Select Competency Area", value: null });
      this.skillGroupList = [];
      this.skillGroupList.push({ label: "Select Skill Group", value: null });
      res.forEach(e => {
        this.competencyAreaList.push({ label: e.CompetencyAreaCode, value: e.CompetencyAreaId });
      });
    },
      (error) => {
        this.messageService.add({ severity: 'error', summary: 'Error message', detail: error.error });

      });
  }


  getSkillGroupByCompetencyArea(competencyAreaId: number): void {
    if (competencyAreaId != 0 && competencyAreaId != null) {
      this.skillGroupList = [];
      this.skillGroupList.push({ label: "Select Skill Group", value: null });
      this._skillsService.getSkillGroupByCompetencyArea(competencyAreaId).subscribe((res: SkillData[]) => {
        res.forEach(e => {
          this.skillGroupList.push({ label: e.SkillGroupName, value: e.SkillGroupId });
        });
      },
        (error) => {
          this.messageService.add({ severity: 'error', summary: 'Error message', detail: error.error });

        }
      );
    }
    else {
      this.skillGroupList = [];
      this.skillGroupList.push({ label: "Select Skill Group", value: null });
    }
  }

  addSkill(): void {
    this.formSubmitted = true;
    var skills = new SkillData();
    skills.CompetencyAreaId = this.addSkills.value.CompetencyAreaId;
    skills.SkillGroupId = this.addSkills.value.SkillGroupId;
    skills.SkillCode = this.addSkills.value.SkillCode;
    skills.SkillDescription = this.addSkills.value.SkillDescription;
    if (this._skillsService.editMode == true) {
      skills.SkillId = this._skillsService.skillsEdit.value.SkillId;
      this.getSkillGroupByCompetencyArea(skills.CompetencyAreaId);
      skills.SkillGroupId = this._skillsService.skillsEdit.value.SkillGroupId;
    }
    if (this.addSkills.valid == true) {
      this._skillsService.createSkills(skills).subscribe(res => {
        if (res != null) {
          this._skillsService.getSkills();
          if (this._skillsService.editMode == false)
            this.messageService.add({ severity: 'success', summary: 'Success Message', detail: 'Skill record added successfully.' });
          else
            this.messageService.add({ severity: 'success', summary: 'Success Message', detail: 'Skill record updated successfully.' });
          this.cancel();
        }
      },
        error => {
          this.messageService.add({ severity: 'error', summary: 'Error message', detail: error.error });

        });
    }
    else {
      // this.messageService.add({severity:'warn', summary: 'Warning Message', detail:'Invalid data'});
      // this.cancel();
    }
  }

  cancel(): void {
    this.formSubmitted = false;
    this.addSkills.reset();
    this._skillsService.editMode = false;
    this.btnLabel = "SAVE";
    this.isEdit = false;
  }

  ngOnDestroy() {
    // this._skillsService.skillsEdit.complete();
  }
}




