import { Component, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { FormControl } from '@angular/forms';
import { Validators } from '@angular/forms';
import { SkillGroupService } from '../../services/skill-group.service';
import { SkillGroupData } from '../../../../models/skillgroup.model';
import { CompetencyAreaService } from '../../services/competency-area.service';
import { CompetencyArea } from '../../models/competencyarea.model';
import { SelectItem } from 'primeng/components/common/selectitem';
import {MessageService} from 'primeng/api';

@Component({
  selector: 'app-skill-group-form',
  templateUrl: './skill-group-form.component.html',
  styleUrls: ['./skill-group-form.component.css'],
  providers: [MessageService]
})
export class SkillGroupFormComponent implements OnInit {
  isEdit : boolean;
  btnLabel : string = "";
  formSubmitted = false;
  addSkillGroup : FormGroup;
  skillGroupData : SkillGroupData;
  competencyAreaList: SelectItem[];
  
  constructor(private _skillGroupService : SkillGroupService,private  messageService: MessageService) { }

  ngOnInit() {
    this.getCompetencyAreaList();
    this.btnLabel = "SAVE";
    this.skillGroupData = new SkillGroupData();
    this.addSkillGroup = new FormGroup({
      CompetencyAreaId : new FormControl(null,[
        Validators.required
      ]),
      SkillGroupName : new FormControl(null,[
        Validators.required,
        Validators.pattern("^[a-zA-Z ]*$"),
        Validators.maxLength(100)
      ]),
      Description : new FormControl(null,[Validators.maxLength(4000)]),
    });
    
    this._skillGroupService.skillgroupEdit.subscribe(data => {
      if (this._skillGroupService.editMode == true) {
        this.isEdit = this._skillGroupService.editMode;
        this.addSkillGroup.patchValue(data);
        this.btnLabel = "UPDATE";
      }
    });
    this.cancel();
  }

  getCompetencyAreaList() : void{
    this._skillGroupService.getCompetencyAreaList().subscribe((res : CompetencyArea[]) => {
      this.competencyAreaList = [];
      this.competencyAreaList.push({ label: "Select Competency Area", value: null });
      res.forEach(e => {
        this.competencyAreaList.push({ label: e.CompetencyAreaCode, value: e.CompetencyAreaId });
      });
    },
    (error)=>{
      this.messageService.add({severity:'error', summary: 'Error message', detail:error.error});   
      
    });
  }

  addSkillGroups() : void {  
    this.formSubmitted = true;
    var skillgroup = new SkillGroupData();
    skillgroup.CompetencyAreaId = this.addSkillGroup.value.CompetencyAreaId;
    skillgroup.SkillGroupName = this.addSkillGroup.value.SkillGroupName;
    skillgroup.Description = this.addSkillGroup.value.Description;
    if(this._skillGroupService.editMode == true){
      skillgroup.SkillGroupId = this._skillGroupService.skillgroupEdit.value.SkillGroupId;
      skillgroup.CompetencyAreaId = this._skillGroupService.skillgroupEdit.value.CompetencyAreaId;
    }
    if(this.addSkillGroup.valid == true){
      this._skillGroupService.createSkillGroup(skillgroup).subscribe(res => {
        if(res != null){
          this._skillGroupService.getSkillGroup();
          if(this._skillGroupService.editMode == false)
            this.messageService.add({severity:'success', summary: 'Success Message', detail:'Skill group record added successfully.'});
          
          else
            this.messageService.add({severity:'success', summary: 'Success Message', detail:'Skill group record updated successfully.'});
            this.cancel();
        }
      },
      (error)=>{
        this.messageService.add({severity:'error', summary: 'Error message', detail:error.error});   
        
      }
    );
      
    }
    else{
     // this.messageService.add({severity:'warn', summary: 'Warning Message', detail:'Invalid data'});
     // this.cancel();
    }
  }

  cancel() : void{
    this.formSubmitted = false;
    this.addSkillGroup.reset();
    this._skillGroupService.editMode = false;
    this.btnLabel = "SAVE";
    this.isEdit = false;
  }

  ngOnDestroy() {
    // this._skillGroupService.skillgroupEdit.complete();
  }

}


