import { Component, OnInit } from '@angular/core';
import { SkillGroupData } from '../../../../models/skillgroup.model';
import { SkillGroupService } from '../../services/skill-group.service';
import * as servicePath from '../../../../service-paths';

@Component({
  selector: 'app-skill-group-table',
  templateUrl: './skill-group-table.component.html',
  styleUrls: ['./skill-group-table.component.css']
})
export class SkillGroupTableComponent implements OnInit {
  skillGroupList : SkillGroupData[];
  selectedRow : SkillGroupData;
  PageSize: number;
  PageDropDown: number[] = [];
  resources = servicePath.API.PagingConfigValue;

  cols = [
    { field: 'CompetencyAreaCode', header: 'Competency Area Code' },
    {field : 'SkillGroupName', header : 'Skill Group Name'},
    {field : 'Description', header : 'Skill Group Description'},
];

  constructor(private _skillGroupService : SkillGroupService) { 
    this.PageSize = this.resources.PageSize;
    this.PageDropDown = this.resources.PageDropDown;
  }

  ngOnInit() {
    this._skillGroupService.skillGroupList.subscribe((data) => {
       data.forEach(function (value) {
                if (value.CompetencyArea != null)
                  value.CompetencyAreaCode = value.CompetencyArea.CompetencyAreaCode;
                else
                  value.CompetencyAreaCode= '';
      });
      this.skillGroupList = data;

    });
    this.getSkillGroup();
  }

  getSkillGroup() : void{
    this._skillGroupService.getSkillGroup();

  }

  editSkillGroup(skillgroupEdit) : void{
    this._skillGroupService.editMode = true;
    this._skillGroupService.skillgroupEdit.next(skillgroupEdit);
   }

  ngOnDestroy() {
    // this._skillGroupService.skillGroupList.complete();
  }
}






