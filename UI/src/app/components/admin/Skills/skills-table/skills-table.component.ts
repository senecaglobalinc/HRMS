import { Component, OnInit } from '@angular/core';
import { SkillData } from '../../../../models/skills.model';
import { SkillsService } from '../../services/skills.service';
import * as servicePath from '../../../../service-paths';

@Component({
  selector: 'app-skills-table',
  templateUrl: './skills-table.component.html',
  styleUrls: ['./skills-table.component.css']
})
export class SkillsTableComponent implements OnInit {
  skillsList : SkillData[];
  selectedRow : SkillData;
  PageSize: number;
  PageDropDown: number[] = [];
  resources = servicePath.API.PagingConfigValue;

  cols = [
    {field : 'CompetencyAreaCode', header: 'Competency Area Code' },
    {field : 'SkillGroupName', header : 'Skill Group'},
    {field : 'SkillCode', header : 'Skill'},
    {field : 'SkillDescription', header : 'Skill Description'},
];

  constructor(private _skillsService : SkillsService) {
    this.PageSize = this.resources.PageSize;
      this.PageDropDown = this.resources.PageDropDown;
   }

  ngOnInit() {
    this._skillsService.skillsList.subscribe((data) => {
       data.forEach(function (value) {
                if (value.CompetencyArea != null)
                  value.CompetencyAreaCode = value.CompetencyArea.CompetencyAreaCode;
                else
                  value.CompetencyAreaCode= '';

                if(value.SkillGroup != null)
                  value.SkillGroupName = value.SkillGroup.SkillGroupName;
                else 
                  value.SkillGroupName = '';              
      });
      this.skillsList = data;
    });
    this.getSkills();
  }

  getSkills(){
    this._skillsService.getSkills();
  }

  editSkills(skillsEdit){
    this._skillsService.editMode = true;
    this._skillsService.skillsEdit.next(skillsEdit);
   }

  ngOnDestroy() {
    // this._skillsService.skillsList.complete();
  }
}



