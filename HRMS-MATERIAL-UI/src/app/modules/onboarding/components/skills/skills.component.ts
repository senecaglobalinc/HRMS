import { Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormControl, FormGroupDirective } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { CommonService } from '../../../../core/services/common.service';
import { GenericType } from '../../../master-layout/models/dropdowntype.model';
import { SkillsService } from '../../services/skills.service';
import { SkillData } from '../../../../modules/master-layout/models/skills.model';
import { MasterDataService } from '../../../../core/services/masterdata.service';
import * as servicePath from '../../../../core/service-paths';
import { Subscription, Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { EmployeeSkillDetails, Skill } from '../../../../modules/master-layout/models/associate-skills.model';
import { BooleanToStringPipe } from '../../pipes/BooleanToStringPipe';
import { themeconfig } from '../../../../../themeconfig';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatSnackBar } from '@angular/material/snack-bar';
import { NgxSpinnerService } from 'ngx-spinner';

interface SelectItem {
  value: number;
  label: string;
}

@Component({
  selector: 'app-skills',
  templateUrl: './skills.component.html',
  styleUrls: ['./skills.component.scss'],
  providers: [BooleanToStringPipe]
})
export class SkillsComponent implements OnInit {
  showDropdown = true
  tempskill: any;
  editedskillobj: any;
  btnLabel = '';
  errorMsg = false;
  IsChecked = false;
  showSubmitButton: boolean = false;
  submitData: EmployeeSkillDetails;
  showRequired: boolean;
  checked: boolean;
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
  themeConfigInput = themeconfig.formfieldappearances;
  filteredMultipleSkill: Observable<GenericType[]>;
  showVersionTextBox : boolean = false
  @ViewChild(FormGroupDirective) formGroupDirective: FormGroupDirective;
  displayedColumns: string[] = [
    'SkillName',
    'Experience',
    'IsPrimary',
    'LastUsed',
    // 'Edit',
  ];
  dataSource: MatTableDataSource<any>;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  constructor(
    private actRoute: ActivatedRoute,
    private yesNoPipe: BooleanToStringPipe,
    private commonService: CommonService,
    private masterService: MasterDataService,
    private skillService: SkillsService,
    private _snackBar: MatSnackBar,
    private spinner: NgxSpinnerService
  ) {
    this.PageSize = this.resources.PageSize;
    this.PageDropDown = this.resources.PageDropDown;
    this.actRoute.params.subscribe(params => { this.id = params.id; });
    this.CreateForm();
    this.GetSkills();
    this.GetProficiencyList();
    this.GetEmployeeSkills();
  }
  roleName=JSON.parse(sessionStorage['AssociatePortal_UserInformation']).roleName;
  ngOnInit() {
    // this.actRoute.params.subscribe(params => { this.id = params['id']; });
    // this.id = "218";
    this.spinner.show()
    this.CreateForm();
    this.GetSkills();
    this.GetProficiencyList();
    this.GetEmployeeSkills();
    this.getFilteredskills()

  }

  private _filterskill(value) {

    let filterValue;
    if (typeof value.SkillId === 'number') {
      return this.skillsList;
    }
    if (value && value.SkillId) {
      if (typeof value.SkillId === 'string') {
        filterValue = value.SkillId.toLowerCase();
      }
      else {
        if (value.SkillId !== null) {
          filterValue = value.SkillId.Name.toLowerCase();
        }
      }
      return this.skillsList.filter((option) =>
        option.Name.toLowerCase().includes(filterValue)
      );
    } else {
      return this.skillsList;
    }
  }
  cols = [
    { field: 'SkillName', header: 'Skill' },
    { field: 'Experience', header: 'Experience (in months)' },
    { field: 'IsPrimary', header: 'Is Primary', type: this.yesNoPipe },
    { field: 'LastUsed', header: 'Last Used' }

  ]
  displayFn(user: any) {
    return user && user ? user.Name : '';
  }
  isEdit() {
    if (this.btnLabel == 'Save')
      return false;
    return true;
  }
  CreateForm() {
    this.errorMsg = false;
    this.showRequired = false;
    this.formSubmitted = false;
    // this.requiredmsg=false;
    this.btnLabel = 'Save';
    this.addSkill = new FormGroup({
      SkillId: new FormControl(null, [Validators.required]),
      ProficiencyLevelId: new FormControl(null, Validators.required),
      // Experience: new FormControl(null, [Validators.required, Validators.pattern('[1-9]|[1-9][0-9]|[0-9][1-9]|[1-9][0-9][0-9]|[0-9][0-9][1-9]|[1-9][0-9][0-9]')]),
      Experience: new FormControl(null, [Validators.required, 
        Validators.pattern(/^(?:\d{1,2}(?:\.\d{1,2})?|99(?:\.99)?|0\d(?:\.\d{1,2})?)$/),
        this.handleExperience,this.zeroHandler]),
      LastUsed: new FormControl(null, [Validators.required, Validators.pattern('(19|20)[0-9]{2}$'), Validators.maxLength(4), Validators.minLength(4)]),
      IsPrimary: new FormControl(false),
      CompetencyAreaId: new FormControl(null),
      SkillGroupId: new FormControl(null),
      RoleId: new FormControl(null),
      EmployeeId: new FormControl(null),
      Id: new FormControl(0),
      RoleName:new FormControl(null),
      Version  : new FormControl(null)
    });
  }
  onlyForNumbers(event: any) {
    this.commonService.onlyNumbers(event);
  }

  GetEmployeeSkills() {
    this.skillService.GetAssociateSkillsById(this.id,this.roleName).subscribe((res: any) => {
      this.spinner.hide()
      this.SkillsGridData = res;
      this.dataSource = new MatTableDataSource(this.SkillsGridData);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
    },(error)=>{
      this.spinner.hide();
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
    this.filteredMultipleSkill = this.addSkill.valueChanges.pipe(
      startWith(''),
      map((value) => this._filterskill(value))
    );

  }
  getFilteredskills(){
    this.filteredMultipleSkill = this.addSkill.valueChanges.pipe(
      startWith(''),
      map((value) => this._filterskill(value))
    );
  }
  GetProficiencyList() {

    this.masterService.GetProficiencyLevels().subscribe(res => {
      this.proficiencyLevels = [];
      this.totalProficiencyLevelsData = [];
      this.totalProficiencyLevelsData = res;
      this.totalProficiencyLevelsData = res;
      this.indexOfBasic = this.totalProficiencyLevelsData.findIndex((x: any) => x.ProficiencyLevelCode === 'Basic');
      for (let i = 0; i < res.length; i++) {
        this.proficiencyLevels.push({
          label: res[i]["ProficiencyLevelCode"],
          value: res[i]["ProficiencyLevelId"]
        });
      }
    });
  }
  editSkill(skillObj) {
    this.btnLabel = 'Update';
    let PautoCompletedObj = { Name: skillObj.SkillName, Id: skillObj.SkillId }
    this.addSkill.patchValue({
      Id: skillObj.Id,
      SkillId: PautoCompletedObj,
      ProficiencyLevelId: skillObj.ProficiencyLevelId,
      Experience: String(skillObj.Experience),
      LastUsed: skillObj.LastUsed,
      IsPrimary: skillObj.IsPrimary,
      CompetencyAreaId: skillObj.CompetencyAreaId,
      SkillGroupId: skillObj.SkillGroupId,
      RoleName:this.roleName,
      Version : skillObj.Version
    });
    this.editedskillobj = this.addSkill.value;
    this.CheckedIsPrimary(false);
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

    if(!this.isValidVersion()){
      this._snackBar.open("Version Can't be Zero", 'x', {
        duration: 3000,
        horizontalPosition: 'right',
        verticalPosition: 'top',
      }); 
      return;
    }

    if (this.addSkill.valid == true) {
      this.errorMsg = false;
      skillObj = this.addSkill.value.SkillId;
      this.addSkill.value.SkillId = this.addSkill.value.SkillId.Id ? this.addSkill.value.SkillId.Id : this.addSkill.value.SkillId;
      if (this.addSkill.value.IsPrimary == null)
        this.addSkill.value.IsPrimary = false;
      this.addSkill.value.EmployeeId = this.id;
      if (this.roleName == "Associate")
        this.addSkill.value.RoleId = 2;
      else
        this.addSkill.value.RoleId = 0;
      if (this.btnLabel == "Save")
        this.GetCompetencyAreaCodeAndSkillGroupId();
      let maxYear = (new Date()).getFullYear()
      if(this.addSkill.value.LastUsed > maxYear){
        this._snackBar.open('Last Used Year should not be greater than current year', 'x', {
          duration: 3000,
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
        return false;
      }

      if(this.addSkill.value.Version != null){
        if(this.addSkill.value.Version[0] == ',' || this.addSkill.value.Version[this.addSkill.value.Version.length -1] == ','){
          this._snackBar.open('Version should not end or begin with commas','x', {
            duration: 3000,
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
          return;
        }

        if(this.addSkill.value.Version[0] == '.' || this.addSkill.value.Version[this.addSkill.value.Version.length -1] == '.'){
          this._snackBar.open('Version should not end or begin with dot','x', {
            duration: 3000,
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
          return; 
        }
       
      }
      
      if (this.btnLabel == 'Save') {
        this.tempskill = this.addSkill.value;
        this.addSkill.patchValue({
          SkillId: this.tempskill.SkillId,
          ProficiencyLevelId: this.tempskill.ProficiencyLevelId,
          Experience: this.tempskill.Experience,
          LastUsed: this.tempskill.LastUsed,
          IsPrimary: this.tempskill.IsPrimary,
          CompetencyAreaId: this.tempskill.CompetencyAreaId,
          SkillGroupId: this.tempskill.SkillGroupId,
          RoleId: this.tempskill.RoleId,
          RoleName:this.roleName,
          EmployeeId: this.tempskill.EmployeeId,
          Id: 0,
          Version : this.tempskill.Version
        });
        if(this.addSkill.value.SkillId==null || this.addSkill.value.SkillId== ""){
          this._snackBar.open('Select skills from the given list', 'x', {
            duration: 1000,
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
          return false;
        }
        this.skillService.SaveAssociateSkills(this.addSkill.value).subscribe(res => {
          this._snackBar.open('Skill added successfully.', 'x', {
            duration: 1000,
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });

          this.cancel();
          this.GetEmployeeSkills();
        },
          (error) => {
            this.addSkill.value.SkillId = skillObj;
            this._snackBar.open(error.error, 'x', {
              duration: 1000,
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
          });
        this.showDropdown = true
      } else {
        if (this.editedskillobj.SkillId == this.addSkill.value.SkillId && this.editedskillobj.ProficiencyLevelId == this.addSkill.value.ProficiencyLevelId && 
          this.editedskillobj.LastUsed == this.addSkill.value.LastUsed && this.editedskillobj.Experience == this.addSkill.value.Experience && this.editedskillobj.Version == this.addSkill.value.Version){
          this._snackBar.open('No changes to update', 'x', {
            duration: 1000,
            panelClass: ['error-alert'],
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
        }
        else{
          this.skillService.UpdateAssociateSkills(this.addSkill.value).subscribe(res => {
            this._snackBar.open('Skill updated successfully.', 'x', {
              duration: 1000,
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
            this.GetEmployeeSkills();
            this.cancel();
          },
            (error) => {
              this.addSkill.value.SkillId = skillObj;
              this._snackBar.open(error.error, 'x', {
                duration: 1000,
                horizontalPosition: 'right',
                verticalPosition: 'top',
              });
            });
        }
        this.showDropdown = true
      }

    }

  }
  cancel() {
    this.showVersionTextBox = false
    this.IsChecked = false;
    this.addSkill.patchValue({
      ProficiencyLevelId: this.totalProficiencyLevelsData[this.indexOfBeginner].ProficiencyLevelId
    })
    this.showDropdown = true
    this.errorMsg = false;
    this.showRequired = false;
    this.formSubmitted = false;
    this.btnLabel = 'Save';
    this.addSkill.reset();
    setTimeout(() => this.formGroupDirective.resetForm(), 0);
  }
  CheckedIsPrimary(bool: boolean) {
    if (this.addSkill.controls.IsPrimary.value !== null) {
      if (bool) {
        this.IsChecked = !this.addSkill.controls.IsPrimary.value;
      }
      else {
        this.IsChecked = this.addSkill.controls.IsPrimary.value;
      }
    }
    else {
      this.IsChecked = true;
    }
    if (this.IsChecked === true) {
      this.showRequired = true;
      if (this.btnLabel === 'Save') {
        this.addSkill.patchValue({
          ProficiencyLevelId: this.totalProficiencyLevelsData[this.indexOfBasic].ProficiencyLevelId
        });
        this.showDropdown = false;
      }
    }
    else {
      this.showRequired = false;
      this.showDropdown = true;
      if (this.btnLabel === 'Save') {
        this.addSkill.patchValue({
          proficiencyLevelId: null
        });
      }
    }
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  clearInput(event : any, fieldName: string){   
  if (fieldName == 'SkillId'){
    this.showVersionTextBox = false;
    this.addSkill.get('SkillId').reset();
    this.addSkill.get('Version').reset();
    event.stopPropagation();
  }
}

enableVersions(event){
  if(event){
   this.showVersionTextBox = true;
  }
 }

isValidVersion () : boolean {
  let version = this.addSkill.get('Version').value;
  let values = version?.split(',');
  if(values){
    for(let i=0;i < values.length;i++){
      if(values[i]=='0' || 
      values[i]=='0.0' || values[i]== '0.00' || values[i]=='00.0' ||
      values[i]=='00.00'){
        return false;
      }
    }
  }
  return true;
}

// omit_special_char(event:any){
//   if((event.charCode >= 32 && event.charCode <= 43)
//    || (event.charCode == 45) || (event.charCode == 47)
//    || (event.charCode >= 58 && event.charCode <= 126) ){
//     event.preventDefault();
//     this._snackBar.open('Only Numbers, decimals, commas are Allowed', 'x', {
//       duration: 3000,
//       horizontalPosition: 'right',
//       verticalPosition: 'top',
//     }); 
//     return
//    }
// }

handleExperience(control) {
  // Check the value of the input field
  const inputValue = new String(control.value);
  let values = inputValue?.split('.');
  
  if(values  && Number(values[1])>11){
    
    return { experienceError: true };
    
  }

  return null; // No error
}

zeroHandler(control){
  const inputValue = new String(control.value);
  
  if(inputValue == '0' || inputValue == '0.00'){
    
    return { zeroErrorHandler: true };
    
  }

  return null; // No error
}

}



