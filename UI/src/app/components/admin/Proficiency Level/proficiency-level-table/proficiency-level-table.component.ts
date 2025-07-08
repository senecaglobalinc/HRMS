import { Component, OnInit } from '@angular/core';
import { ProficiencyLevel } from '../../models/proficiencyLevel.model';
import { ProficiencyLevelService } from '../../services/proficiency-level.service';
import * as servicePath from '../../../../service-paths';

@Component({
  selector: 'app-proficiency-level-table',
  templateUrl: './proficiency-level-table.component.html',
  styleUrls: ['./proficiency-level-table.component.css']
})
export class ProficiencyLevelTableComponent implements OnInit {
  proficiencyLevelData : ProficiencyLevel[]; 
  selectedProficiencyLevel : ProficiencyLevel; 
  resources = servicePath.API.PagingConfigValue;
  PageSize: number;
  PageDropDown: number[] = [];
  constructor( private serviceObj : ProficiencyLevelService) { 
    this.PageSize = this.resources.PageSize;
    this.PageDropDown = this.resources.PageDropDown;
  }

  ngOnInit() {
   this.serviceObj.proficiencyLevelData.subscribe((data)=>{
     this.proficiencyLevelData = data;
   })
   this.GetProficiencyLevelData();
  }

  cols = [
    { field : "ProficiencyLevelCode", header : "Proficiency Level Code"},
    { field : "ProficiencyLevelDescription", header : "Proficiency Level Description"},
  ]

 
  GetProficiencyLevelData() : void{
    this.serviceObj.getProficiencyLevelData();
  }

  SetEditObj(editObj : ProficiencyLevel) : void{ 
   this.serviceObj.editMode = true;
   this.serviceObj.editObj.next(editObj);
  }

  ngOnDestroy() {
    // this.serviceObj.proficiencyLevelData.unsubscribe();
  }

}
