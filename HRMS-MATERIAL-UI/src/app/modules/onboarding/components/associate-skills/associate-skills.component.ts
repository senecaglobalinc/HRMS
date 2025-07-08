import { Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormControl, FormGroupDirective } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { CommonService } from '../../../../core/services/common.service';
import { GenericType } from '../../../master-layout/models/dropdowntype.model';
import { SkillsService } from '../../services/skills.service';
import { AddSkills, SkillHistory } from '../../../../modules/master-layout/models/skills.model';
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
import _ from "lodash";
import { CompetencyAreaData } from '../../../admin/models/competency-area-data.model';
import { SkillGroupData } from '../../../admin/models/skill-group-data.model';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { SkillsData } from 'src/app/modules/master-layout/models/project-role.model';
import { MatDialog } from '@angular/material/dialog';
import {AssociateSkillDialogComponent} from '../associate-skill-dialog/associate-skill-dialog.component'
interface SelectItem {
  value: number;
  label: string;
}
@Component({
  selector: 'app-associate-skills',
  templateUrl: './associate-skills.component.html',
  styleUrls: ['./associate-skills.component.scss'],
  providers: [BooleanToStringPipe],
  animations: [
    trigger('detailExpand', [
      state('collapsed', style({height: '0px', minHeight: '0'})),
      state('expanded', style({height: '*'})),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
    ]),
  ],
})

export class AssociateSkillsComponent implements OnInit {

  skillGroupList: SelectItem[];
  competencyAreaList: SelectItem[];
  editObj : any[];
  UpdatededitObj : any;
  filteredCompetencyAreaList : Observable<any>;
  filteredSkillGroupList : Observable<any>;
  filteredSkillsList : Observable<any>;
  selectedSkillId:number;
  historyData:SkillHistory[];

  isEdit:boolean = false;

  tempskill: any;
  btnLabel = '';
  errorMsg = false;
  IsChecked = false;
  showSubmitButton: boolean = false;
  submitData: EmployeeSkillDetails;
  checked: boolean;
  id;
  totalProficiencyLevelsData;
  indexOfBasic = 0;
  indexOfBeginner = 0;
  skillsData: any[];
  addSkill: FormGroup;
  skillsList: any[];
  proficiencyLevels: SelectItem[] = [];
  formSubmitted = false;
  PageSize: number;
  PageDropDown: number[] = [];
  private resources = servicePath.API.PagingConfigValue;
  SkillsGridDataForPrimarySkills: any[] = [];
  SkillsGridDataForSecondarySkills: any[] = [];
  count:number=0;
  filteredData:any;
  themeConfigInput = themeconfig.formfieldappearances;
  @ViewChild(FormGroupDirective) formGroupDirective: FormGroupDirective;
  displayedColumns: string[] = [
    'SkillCode',
    'Version',
    'Proficiency',
    'Experience',
    'LastUsed',
    'Status',
    'Action',
  ];
  _historyDisplayedColumns: string[] = [
    'SubmittedRating',
    'RMRating',
    'Experience',
    'LastUsed',
    'Status',
    'Remarks',
    'RequestedDate',
    'ApprovedDate',
  ]
  expandedElement: AddSkills | null;
  dataSourcePrimary: MatTableDataSource<AddSkills>;
  dataSourceSecondary: MatTableDataSource<AddSkills>;
  _skillHistoryDS: MatTableDataSource<SkillHistory>;
  showVersionTextBox :boolean = false;
  
  @ViewChild('primaryPaginator', { static: true }) primaryPaginator: MatPaginator;
  @ViewChild('secondaryPaginator', { static: true }) secondaryPaginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  

  constructor(
    private actRoute: ActivatedRoute,
    private route: Router,
    private yesNoPipe: BooleanToStringPipe,
    private commonService: CommonService,
    private masterService: MasterDataService,
    private skillService: SkillsService,
    private _snackBar: MatSnackBar,
    public dialog: MatDialog
  ) {
    this.PageSize = this.resources.PageSize;
    this.PageDropDown = this.resources.PageDropDown;
    this.id = JSON.parse(sessionStorage["AssociatePortal_UserInformation"]).employeeId
    this.CreateForm();
    this.GetProficiencyList();
    this.GetEmployeeSkills();
  }
  ngOnInit() {
    this.CreateForm();   
    this.GetProficiencyList();
    this.GetEmployeeSkills();   
  }

  clearInput(event : any, fieldName: string){   
      this.cancel();  
    if (fieldName == 'SkillId'){
      this.showVersionTextBox = false;
      this.addSkill.get('SkillId').reset();
      this.addSkill.get('Version').reset();
      this.addSkill.get('ProficiencyLevelId').reset();
      this.addSkill.get('Experience').reset();
      this.addSkill.get('LastUsed').reset();
      this.addSkill.get('CompetencyAreaCode').reset();
      this.addSkill.get('SkillGroup').reset();
      this.skillsList=[];
      event.stopPropagation();
    }
  }

  CreateForm() {
    this.errorMsg = false;
    this.formSubmitted = false;
    this.btnLabel = 'Save';
    this.addSkill = new FormGroup({
      CompetencyAreaCode: new FormControl(null),
      CompetencyAreaId: new FormControl(null),
      SkillGroupId: new FormControl(null),
      SkillGroup:new FormControl(null),
      SkillId: new FormControl(null, [Validators.required]),
      ProficiencyLevelId: new FormControl(null, Validators.required),
      Experience: new FormControl(null, [Validators.required, 
        Validators.pattern(/^(?:\d{1,2}(?:\.\d{1,2})?|99(?:\.99)?|0\d(?:\.\d{1,2})?)$/),
        this.handleExperience,this.zeroHandler]),
      LastUsed: new FormControl(null, [Validators.required, Validators.pattern('(19|20)[0-9]{2}$'),Validators.max(new Date().getFullYear()), Validators.maxLength(4), Validators.minLength(4)]),
      IsPrimary: new FormControl(false),
      // RoleId: new FormControl(null),
      EmployeeId: new FormControl(null),
      Id: new FormControl(0),
      Version : new FormControl(null)
    });
  }

  private _filteredSkillList(value) {
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
          filterValue = value.SkillId.label.toLowerCase();
        }
      }
      return this.skillsList.filter((option) =>
        option.label.toLowerCase().includes(filterValue)
      );
    } else {
      return this.skillsList;
    }
  }

  onlyForNumbers(event: any) {
    this.commonService.onlyNumbers(event);
  }

  GetEmployeeSkills() {
    this.skillService.GetAssociateSkillsById(this.id,JSON.parse(sessionStorage['AssociatePortal_UserInformation']).roleName).subscribe((res: any) => {
      var primarySkills =[];
      var secondarySkills =[];
      for(let i=0;i<res.length;i++)
       {
        if(res[i].IsPrimary==true)
         {
          primarySkills.push(res[i]);
         }
         else{
          secondarySkills.push(res[i]);
         }         
       }
       this.filteredData=res.filter(x=>x.StatusName=='Created');
     this.count=this.filteredData.length;
      this.SkillsGridDataForPrimarySkills = primarySkills;
      this.dataSourcePrimary = new MatTableDataSource(this.SkillsGridDataForPrimarySkills);
      this.dataSourcePrimary.paginator = this.primaryPaginator;
      this.dataSourcePrimary.sort = this.sort;

      this.SkillsGridDataForSecondarySkills = secondarySkills;
      this.dataSourceSecondary = new MatTableDataSource(this.SkillsGridDataForSecondarySkills);
      this.dataSourceSecondary.paginator = this.secondaryPaginator;
      this.dataSourcePrimary.sort = this.sort;
    });
  }
  async getSkillHistoryByEmpId(element){
    let response: any= await this.skillService.GetEmployeeSkillHistory(this.id, element.Id).toPromise()
    return response
  }
  async onRowClick(element:AddSkills | null){
    this.historyData = [];
    if(this.expandedElement==null || this.expandedElement!=element){
      this.historyData = await this.getSkillHistoryByEmpId(element);
      
      this.expandedElement = this.historyData.length == 0 ? null: element;

    }
    else{
      this.historyData = [];
      this.expandedElement = null;
    }
    this._skillHistoryDS =  new MatTableDataSource(this.historyData);
  }
  checkHistory(){
    if (this.historyData != []){
      return true;
    }
    else{
      return false;
    }
  }
  GetSkillsBySearchString(searchString) {
    
    if (searchString != null && searchString.length>=3) {
      this.skillService.GetSkillsBySearchString(searchString).subscribe((skills: any[]) => {
                this.skillsData = skills;
        this.skillsList = [];
        skills.forEach(element => {
          if(element.CompetencyAreaName.toLowerCase() != 'certification'){
            this.skillsList.push({ label: element.SkillName, value: element.SkillId });
          }
          
        });
        this.filteredSkillsList = this.addSkill.valueChanges.pipe(
          startWith(''),
          map((value) => this._filteredSkillList(value))
        );
      });
       this.addSkill.get('SkillId').enable();
    }
    else{
      this.skillsList = [];     
    }
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
    this.editObj = skillObj;
    this.btnLabel = 'Update';
    this.isEdit = true;
    let PautoCompletedObj = { label: skillObj.SkillName, value: skillObj.SkillId };
    this.addSkill.get('SkillGroup').disable();
    this.addSkill.get('CompetencyAreaCode').disable();
    this.addSkill.get('SkillId').disable();
    this.addSkill.patchValue({
      Id: skillObj.Id,
      SkillId: PautoCompletedObj,
      ProficiencyLevelId: skillObj.ProficiencyLevelId,
      Experience: skillObj.Experience,
      LastUsed: skillObj.LastUsed,
      IsPrimary: skillObj.IsPrimary,
      CompetencyAreaCode: skillObj.CompetencyAreaCode,
      SkillGroup: skillObj.SkillGroupName,
      EmployeeId: this.id,
      Version : skillObj.Version,
      CompetencyAreaId : skillObj.CompetencyAreaId,
      SkillGroupId:skillObj.SkillGroupId
    });
    this.UpdatededitObj = {
      ProficiencyLevelId: skillObj.ProficiencyLevelId,
      Experience: skillObj.Experience,
      LastUsed: skillObj.LastUsed,
      IsPrimary: skillObj.IsPrimary,
      EmployeeId: this.id,
      SkillId: skillObj.SkillId,
      Id: skillObj.Id,
      Version : skillObj.Version,
      CompetencyAreaId : skillObj.CompetencyAreaId,
      SkillGroupId:skillObj.SkillGroupId
    };
  }

  displayFn(user: any) {
    return user && user.label ? user.label : '';
  }

  selectedChangeIds(frmCntrl, item) {
    if (frmCntrl== 'SkillId') {
      this.selectedSkillId = item.value;
      var filterData= this.skillsData.filter(x=>x.SkillId==this.selectedSkillId);
      this.addSkill.get('CompetencyAreaCode').setValue(filterData[0].CompetencyAreaName);
      this.addSkill.get('SkillGroup').setValue(filterData[0].SkillGroupName);
      this.addSkill.get('CompetencyAreaId').setValue(filterData[0].CompetencyAreaId);
      this.addSkill.get('SkillGroupId').setValue(filterData[0].SkillGroupId);

    }
  }
  onSave() {
    this.formSubmitted = true;
    let skillObj = new GenericType();
    let current_year = new Date().getFullYear();
    if(this.addSkill.value.LastUsed > current_year){
      this._snackBar.open('Year must be less than or equal to current year', 'x', {
        panelClass: ['error-alert'],
        duration: 3000,
        horizontalPosition: 'right',
        verticalPosition: 'top',
      });
      return false;
    }

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
      
      if (this.addSkill.value.IsPrimary == null)
        this.addSkill.value.IsPrimary = false;
      this.addSkill.value.EmployeeId = this.id;
      // if (JSON.parse(sessionStorage['AssociatePortal_UserInformation']).roleName == "Associate")
      //   this.addSkill.value.RoleId = 2;
      // else
      //   this.addSkill.value.RoleId = 0;
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
        this.tempskill['SkillId'] = this.selectedSkillId;
        this.tempskill['Id']=0;
        if(this.addSkill.value.SkillId==null || this.addSkill.value.SkillId== ""){
          this._snackBar.open('Select skills from the given list', 'x', {
            duration: 3000,
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
          return false;
        }
        this.skillService.SaveAssociateSkills(this.tempskill).subscribe(res => {
          this._snackBar.open('Skill added Successfully', 'x', {
            duration: 3000,
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });

          this.cancel();
          this.GetEmployeeSkills();
        },
          (error) => {
            this.addSkill.value.SkillId = skillObj;
            this._snackBar.open(error.error, 'x', {
              duration: 3000,
              panelClass: ['error-alert'],
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
          });
      }
      else {
        this.tempskill = this.addSkill.value;
        this.tempskill['SkillId'] = this.editObj['SkillId'];
             if(_.isEqual(this.tempskill, this.UpdatededitObj) ){
          this._snackBar.open("No changes to Update", 'x', {
            duration: 3000,
            panelClass: ['error-alert'],
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
        }
        else{
          this.skillService.UpdateAssociateSkills(this.tempskill).subscribe(res => {
            this._snackBar.open('Skill updated Successfully', 'x', {
              duration: 3000,
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
            this.GetEmployeeSkills();
            this.cancel();
            },
            (error) => {
              this.addSkill.value.SkillId = skillObj;
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
    this.isEdit = false;
  }
  cancel() {
    this.isEdit = false;  
    this.addSkill.get('SkillId').enable();
    this.addSkill.get('SkillId').reset();
    this.skillsList=[];
    this.IsChecked = false;
    this.addSkill.patchValue({
      ProficiencyLevelId: this.totalProficiencyLevelsData[this.indexOfBeginner].ProficiencyLevelId
    })
    this.errorMsg = false;
    this.formSubmitted = false;
    this.btnLabel = 'Save';
    this.showVersionTextBox = false;
    this.addSkill.reset();
    setTimeout(() => this.formGroupDirective.resetForm(), 0);
  }
  CheckedIsPrimary(event) {
    this.IsChecked = event.checked;
    if (this.IsChecked === true) {
      if (this.btnLabel === 'Save') {
        this.addSkill.patchValue({
          ProficiencyLevelId: this.totalProficiencyLevelsData[this.indexOfBasic].ProficiencyLevelId
        });
      }
    }
    else {
      if (this.btnLabel === 'Save') {
        this.addSkill.get('ProficiencyLevelId').reset();
      }
    }
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSourcePrimary.filter = filterValue.trim().toLowerCase();
    this.dataSourceSecondary.filter = filterValue.trim().toLowerCase();
  }

  submitForApproval() {
  
     let dialogRef= this.dialog.open(AssociateSkillDialogComponent, {
        width:'500px',
        // disableClose: true ,
        data: { data:this.filteredData}
      });
      dialogRef.afterClosed().subscribe(result => {
        if(result.event=='Ok')
        {
        this.skillService.submitForApproval(this.id).subscribe((res: any) => {
          this._snackBar.open('Submitted For Approval', 'x', {
            duration: 3000,
            horizontalPosition: 'right',
            verticalPosition: 'top'
          });
          this.GetEmployeeSkills();
          }, (error) => {
            this._snackBar.open(error.error, 'x', {
              duration: 3000,
              panelClass: ['error-alert'],
              horizontalPosition: 'right',
              verticalPosition: 'top'
          });
        });    
      }         
      });
  }

  checkStatus(SkillObj){
    if(SkillObj['StatusId'] == 2){
      return true;
    }
    else{
      return false;
    }

  }

  deleteSkill(element){
    this.skillService.deleteSkill(element.Id).subscribe((res)=>{
      this.cancel();
      if(res){
        this.GetEmployeeSkills();
        this._snackBar.open('Skill Deleted Successfully', 'x', {
          duration: 3000,
          panelClass:['success-alert'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
      }
      else{
        this._snackBar.open('Failed Deleting the skill', 'x', {
          duration: 3000,
          horizontalPosition: 'right',
          verticalPosition: 'top',
        }); 
      }
    },(error)=>{
      this.cancel();
      this._snackBar.open('Error Occurred While Deleting the Skill', 'x', {
        duration: 3000,
        horizontalPosition: 'right',
        verticalPosition: 'top',
      }); 
    })

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




