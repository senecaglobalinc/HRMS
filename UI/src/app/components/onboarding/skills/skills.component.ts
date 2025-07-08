import { Component, OnInit } from '@angular/core';
import { SelectItem, TreeNode, MenuItem } from 'primeng/api';
import { FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { CommonService } from '../../../services/common.service';
import { GenericType } from '../../../models/dropdowntype.model';
import { SkillsService } from '../services/skills.service';
import { SkillData } from '../../../models/skills.model';
import { MasterDataService } from '../../../services/masterdata.service';
import * as servicePath from '../../../service-paths';
import { MessageService } from 'primeng/api';
import { EmployeeSkillDetails, Skill } from '../../../models/associate-skills.model';
import { BooleanToStringPipe } from '../../../Pipes/BooleanToStringPipe';
import { ConfirmationService } from 'primeng/api';

@Component({
  selector: 'app-skills',
  templateUrl: './skills.component.html',
  styleUrls: ['./skills.component.scss'],
  providers: [MessageService, BooleanToStringPipe, ConfirmationService]
})
export class SkillsComponent implements OnInit {
  btnLabel = '';
  errorMsg = false;
  showSubmitButton: boolean = false;
  submitData: EmployeeSkillDetails;
  showRequired: boolean;
  checked: boolean
  id;
  totalProficiencyLevelsData;
  indexOfBasic = 0;
  indexOfBeginner = 0;
  skillsData: any[];
  addSkill: FormGroup;
  skillsList: GenericType[];
  proficiencyLevels: SelectItem[] = [];
  formSubmitted = false;
  filteredSkillsList;
  PageSize: number;
  PageDropDown: number[] = [];
  private resources = servicePath.API.PagingConfigValue;
  SkillsGridData: any[] = [];
  constructor(
    private actRoute: ActivatedRoute,
    private yesNoPipe: BooleanToStringPipe,
    private commonService: CommonService,
    private masterService: MasterDataService,
    private skillService: SkillsService,
    private messageService: MessageService,
    private confirmationService: ConfirmationService
  ) {
    this.PageSize = this.resources.PageSize;
    this.PageDropDown = this.resources.PageDropDown;
  }
  ngOnInit() {
    this.actRoute.params.subscribe(params => { this.id = params['id']; });
    this.CreateForm();
    this.GetSkills();
    this.GetProficiencyList();
    this.GetEmployeeSkills();
  }
  cols = [
    { field: 'SkillName', header: 'Skill' },
    { field: 'Experience', header: 'Experience (in months)' },
    { field: 'IsPrimary', header: 'Is Primary', type: this.yesNoPipe },
    { field: 'LastUsed', header: 'Last Used' },
    // { field: 'StatusCode', header: 'Status' },

  ]
  isEdit() {
    if (this.btnLabel == 'Save')
      return false;
    return true;
  }
  CreateForm() {
    this.errorMsg = false;
    this.showRequired = false;
    this.formSubmitted = false;
    this.btnLabel = 'Save';
    this.addSkill = new FormGroup({
      SkillId: new FormControl(null, [Validators.required]),
      ProficiencyLevelId: new FormControl(null),
      Experience: new FormControl(null),
      LastUsed: new FormControl(null, [Validators.pattern('(19|20)[0-9]{2}$')]),
      IsPrimary: new FormControl(null),
      CompetencyAreaId: new FormControl(null),
      SkillGroupId: new FormControl(null),
      RoleId: new FormControl(null),
      EmployeeId: new FormControl(null),
      Id: new FormControl(0),
    });
  }
  onlyForNumbers(event: any) {
    this.commonService.onlyNumbers(event);
  }
  // CheckValue(event){
  //   if(event.value > 500)
  //     this.errorMsg = true;
  //   else
  //     this.errorMsg = false;
  // }
  GetEmployeeSkills() {
    this.skillService.GetAssociateSkillsById(this.id).subscribe((res: any) => {
      this.SkillsGridData = res;
    });
  }
  GetSkills() {
    this.skillService.getSkills().subscribe((skills: any[]) => {
      this.skillsData = skills;
      this.skillsList = [];
      for (var i = 0; i < skills.length; i++) {
        this.skillsList[i] = new GenericType();
        this.skillsList[i].Name = skills[i].SkillCode;
        this.skillsList[i].Id = skills[i].SkillId;
      }
    });
  }

  GetProficiencyList() {

    this.masterService.GetProficiencyLevels().subscribe(res => {
      this.totalProficiencyLevelsData = res;
      this.indexOfBasic = this.totalProficiencyLevelsData.findIndex((x: any) => x.ProficiencyLevelCode === "Basic");
      this.indexOfBeginner = this.totalProficiencyLevelsData.findIndex((x: any) => x.ProficiencyLevelCode === "Beginner");
      //  this.addSkill.value.proficiencyLevelId =res[index].ProficiencyLevelId;

      this.proficiencyLevels.push({ label: this.totalProficiencyLevelsData[this.indexOfBeginner].ProficiencyLevelCode, value: this.totalProficiencyLevelsData[this.indexOfBeginner].ProficiencyLevelId });
      this.totalProficiencyLevelsData.forEach((e: any) => {
        if (e.ProficiencyLevelCode != "Beginner")
          this.proficiencyLevels.push({ label: e.ProficiencyLevelCode, value: e.ProficiencyLevelId })
      });
      this.addSkill.patchValue({
        ProficiencyLevelId: this.totalProficiencyLevelsData[this.indexOfBeginner].ProficiencyLevelId
      });
    });
  }
  editSkill(skillObj) {
    this.btnLabel = 'Update';
    let PautoCompletedObj = { Name: skillObj.SkillName, Id: skillObj.SkillId }
    this.addSkill.patchValue({
      Id: skillObj.Id,
      SkillId: PautoCompletedObj,
      ProficiencyLevelId: skillObj.ProficiencyLevelId,
      Experience: skillObj.Experience,
      LastUsed: skillObj.LastUsed,
      IsPrimary: skillObj.IsPrimary,
      CompetencyAreaId: skillObj.CompetencyAreaId,
      SkillGroupId: skillObj.SkillGroupId,
    });
    this.CheckedIsPrimary();
  }
  filteredMultipleSkills(event: any): void {
    let suggestionString = event.query;
    this.filteredSkillsList = this.filteredSkills(
      suggestionString,
      this.skillsList);

  }
  filteredSkills(suggestionString: string, skillsList: GenericType[]): GenericType[] {
    let filtered: GenericType[] = [];
    for (let i = 0; i < skillsList.length; i++) {
      let skillList = skillsList[i];
      if (skillList.Name.toLowerCase().includes(suggestionString.toLowerCase()) == true) {
        filtered.push(skillList);
      }
    }
    return filtered;
  }
  GetCompetencyAreaCodeAndSkillGroupId() {
    let length = this.skillsData.length;
    let selectedSkillId = this.addSkill.value.SkillId;
    for (var i = 0; i < length; i++) {
      if (this.skillsData[i].SkillId == selectedSkillId) {
        this.addSkill.value.CompetencyAreaId = this.skillsData[i].CompetencyArea.CompetencyAreaId;
        this.addSkill.value.SkillGroupId = this.skillsData[i].SkillGroup.SkillGroupId;
        break;
      }
    }
  }
  onSave() {
    this.formSubmitted = true;
    let skillObj = new GenericType();
    if ((this.addSkill.value.Experience != null && this.addSkill.value.Experience != "") && (this.addSkill.value.Experience > 500 || this.addSkill.value.Experience <= 0)) {
      this.errorMsg = true;
      return;
    }
    else {
      this.errorMsg = false;
    }

    if (this.addSkill.valid == true) {
      this.errorMsg = false;
      skillObj = this.addSkill.value.SkillId;
      this.addSkill.value.SkillId = this.addSkill.value.SkillId.Id;
      if (this.addSkill.value.IsPrimary == null)
        this.addSkill.value.IsPrimary = false;
      this.addSkill.value.EmployeeId = this.id;
      if (JSON.parse(sessionStorage['AssociatePortal_UserInformation']).roleName == "Associate")
        this.addSkill.value.RoleId = 2;
      else
        this.addSkill.value.RoleId = 0;
      if (this.btnLabel == "Save")
        this.GetCompetencyAreaCodeAndSkillGroupId();
      // this.skillService.SaveAssociateSkills(this.addSkill.value).subscribe(res => {
      //     if (this.btnLabel == 'Save') {
      //       if (res > 0 || res == -4) {
      //         this.messageService.add({ severity: 'success', summary: 'Success Message', detail: 'Skill added successfully' });
      //         this.GetEmployeeSkills();
      //         this.cancel();
      //       }
      //       else {
      //         this.addSkill.value.skillID = skillObj;
      //         if (res == -2)
      //           this.messageService.add({ severity: 'warn', summary: 'Warn Message', detail: 'Failed! only 50 skills are allowed in technology and tools competencyarea as primary skills' });
      //         else if(res == -1)
      //           this.messageService.add({ severity: 'warn', summary: 'Warn Message', detail: 'Skill already exits' });
      //         else if (res == -3)
      //           this.messageService.add({ severity: 'warn', summary: 'Warn Message', detail: 'Failed! only 10 skills are allowed as primary skills' });
      //         else
      //           this.messageService.add({ severity: 'warn', summary: 'Warn Message', detail: 'Failed to add skill' });
      //       }
      //     }
      //     else {
      //       if (res == 1) {
      //         this.messageService.add({ severity: 'success', summary: 'Success Message', detail: 'Skill has been updated' });
      //         this.GetEmployeeSkills();
      //         this.cancel();
      //       }
      //       else if(res == -2){
      //         this.addSkill.value.skillID = skillObj;
      //         this.messageService.add({ severity: 'warn', summary: 'Warn Message', detail: 'Skill already exits' });
      //       }
      //       else {
      //         this.addSkill.value.skillID = skillObj;
      //         this.messageService.add({ severity: 'warn', summary: 'Warn Message', detail: 'Failed to update existing skill' });
      //       }
      //     }

      //   },
      //     (error) => {
      //       this.addSkill.value.skillID = skillObj;
      //       if (this.btnLabel == 'Save')
      //         this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Error while saving skill' });
      //       else
      //         this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Error while updating skill' });

      //     });
      if (this.btnLabel == 'Save') {
        this.skillService.SaveAssociateSkills(this.addSkill.value).subscribe(res => {
          this.messageService.add({ severity: 'success', summary: 'Success Message', detail: 'Skill added successfully' });
          this.GetEmployeeSkills();
          this.cancel();
        },
          (error) => {
            this.addSkill.value.SkillId = skillObj;
            this.messageService.add({ severity: 'error', summary: 'Error Message', detail: error.error });
          });
      } else {
        this.skillService.UpdateAssociateSkills(this.addSkill.value).subscribe(res => {
          this.messageService.add({ severity: 'success', summary: 'Success Message', detail: 'Skill has been updated' });
          this.GetEmployeeSkills();
          this.cancel();
        },
          (error) => {
            this.addSkill.value.SkillId = skillObj;
            this.messageService.add({ severity: 'error', summary: 'Error Message', detail: error.error });
          });
      }

    }
    // else
    //this.messageService.add({ severity: 'warn', summary: 'Warn Message', detail: 'Invalid data' });

  }
  cancel() {
    this.CreateForm();
    this.addSkill.patchValue({
      ProficiencyLevelId: this.totalProficiencyLevelsData[this.indexOfBeginner].ProficiencyLevelId
    })
  }
  OpenConfirmationDialog() {   // method to open dialog
    this.confirmationService.confirm({
      message: 'Do you want to clear ?',
      accept: () => {
        this.cancel()
      },
      reject: () => {

      }
    });
  }
  CheckedIsPrimary() {
    if (this.addSkill.value.IsPrimary == true) {
      this.showRequired = true;
      this.addSkill.controls["LastUsed"].setValidators([Validators.required, Validators.pattern('(19|20)[0-9]{2}$')]);
      this.addSkill.controls["Experience"].setValidators([Validators.required]);
      this.addSkill.controls["LastUsed"].updateValueAndValidity();
      this.addSkill.controls["Experience"].updateValueAndValidity();
      if (this.btnLabel == "Save")
        this.addSkill.patchValue({
          ProficiencyLevelId: this.totalProficiencyLevelsData[this.indexOfBasic].ProficiencyLevelId
        });
    }
    else { // user selected primary skill
      this.showRequired = false;
      if (this.btnLabel == "Save")
        this.addSkill.patchValue({
          proficiencyLevelId: this.totalProficiencyLevelsData[this.indexOfBeginner].ProficiencyLevelId
        });
      // todo remove validations;
      this.addSkill.controls["LastUsed"].clearValidators();  //todo
      this.addSkill.controls["Experience"].clearValidators();
      this.addSkill.controls["LastUsed"].setValidators([Validators.pattern('(19|20)[0-9]{2}$')]);
      this.addSkill.controls["LastUsed"].updateValueAndValidity();
      this.addSkill.controls["Experience"].updateValueAndValidity();
    }
  }


  // ToShowEdit(rowData) {
  //   // if(rowData.StatusCode == 'Draft'){
  //   //   this.ShowsubmitButton();
  //   return true;
  // }

  // // return false;
  // // }
  // ShowsubmitButton() {
  //   if (JSON.parse(sessionStorage['AssociatePortal_UserInformation']).roleName == "Associate")
  //     this.showSubmitButton = true;
  // }


  // GetSubmitDataFromGrid() {
  //   let dataArray = [];
  //   this.submitData = new EmployeeSkillDetails();
  //   for (let i = 0; i < this.SkillsGridData.length; i++) {
  //     let data = new Skill();
  //     if (this.SkillsGridData[i].StatusCode == 'Draft') {
  //       data.skillID = this.SkillsGridData[i].skillID;
  //       data.experience = this.SkillsGridData[i].experience;
  //       data.isPrimary = this.SkillsGridData[i].isPrimary;
  //       data.LastUsed = this.SkillsGridData[i].LastUsed;
  //       data.StatusCode = this.SkillsGridData[i].StatusCode;
  //       data.proficiencyLevelId = this.SkillsGridData[i].proficiencyLevelId;
  //       data.ID = this.SkillsGridData[i].ID;
  //       data.CompetencyAreaID = this.SkillsGridData[i].CompetencyAreaID;
  //       data.SkillGroupID = this.SkillsGridData[i].SkillGroupID;
  //       data.empID = this.id;
  //       if (JSON.parse(sessionStorage['AssociatePortal_UserInformation']).roleName == "Associate")
  //         data.RoleId = 2;
  //       else
  //         data.RoleId = 0;
  //       dataArray.push(data);
  //     }
  //   }
  //   // this.submitData.empID = JSON.parse(sessionStorage['AssociatePortal_UserInformation']).employeeId;
  //   this.submitData.empID = this.id;
  //   if (JSON.parse(sessionStorage['AssociatePortal_UserInformation']).roleName == "Associate")
  //     this.submitData.RoleId = 2;
  //   else
  //     this.submitData.RoleId = 0;

  //   this.submitData.skillDetails = dataArray;
  // }
  // onSubmit() {
  //   this.GetSubmitDataFromGrid();
  //   this.skillService.SubmitAssociateSkills(this.submitData).subscribe(res => {
  //     if (res == 0) {
  //       this.messageService.add({ severity: 'warn', summary: 'Warn Message', detail: 'Failed to submit skills' });
  //     }
  //     else {
  //       this.GetEmployeeSkills();
  //       this.showSubmitButton = false;
  //       this.messageService.add({ severity: 'success', summary: 'Success Message', detail: 'Skill submitted' });
  //     }
  //   });
  // }
}



