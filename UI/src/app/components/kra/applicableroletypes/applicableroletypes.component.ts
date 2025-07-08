import { Component, OnInit } from '@angular/core';
import {FormControl} from '@angular/forms';
import { FormBuilder,FormGroup, Validators } from '@angular/forms';
import {themeconfig} from 'themeconfig';
import { SelectItem} from "primeng/components/common/api";
import {MessageService} from 'primeng/api';
import {SelectionModel} from '@angular/cdk/collections';
import { GenericType } from "../../../models/dropdowntype.model";
import { MasterDataService } from '../../../services/masterdata.service';
import { DepartmentDetails } from 'src/app/models/role.model';
import { Grade } from '../../admin/models/grade.model';
import { Roletype } from "../../admin/models/roletype.model";
import { element } from 'protractor';
import { FinancialYear, SelectedKRAParameters } from 'src/app/models/kra.model';
import { RoleTypeService } from '../../admin/services/role-type.service';
import { ApplicableRoleTypeService } from "src/app/components/kra/Services/applicableroletype.service";
import { ApplicableRoleType , ApplicableRoleTypeData} from '../../../models/kraApplicableRoleType.model';
import * as servicePath from '../../../service-paths';

@Component({
  selector: 'app-applicableroletypes',
  templateUrl: './applicableroletypes.component.html',
  styleUrls: ['./applicableroletypes.component.scss'],
  
})
export class ApplicableroletypesComponent implements OnInit {
  selectedParameters : SelectedKRAParameters;
  formSubmitted = false;
  roletypedata: any;
  selection = new SelectionModel<any>(true, []);
  // gridMessage: string = "No records found";
  public departmentList: SelectItem[] = [];
  public gradeList: SelectItem[] =[];
  public roleTypeList: SelectItem[] =[];
  public financialYearList: SelectItem[] = [];
  approleTypeForm: FormGroup;
  btnLabel = "Save";
  applicableRoleTypeList : ApplicableRoleTypeData[];
  list : any;
  selectedApplicableRoleType : ApplicableRoleTypeData;
  resources = servicePath.API.PagingConfigValue;
  PageSize: number;
  PageDropDown: number[] = [];
  
  cols = [
    { field: 'FinancialYearName', header: 'Finacial Year' },
    {field : 'DepartmentDescription', header : 'Department'},
    {field : 'GradeName', header : 'Grade'},
    {field : 'RoleTypeName', header : 'Role Type'}
    ];

  constructor( 
    private _masterDataService: MasterDataService,    
    private _roleTypeService: RoleTypeService, 
    private _applicableRoleTypeService: ApplicableRoleTypeService,
    private messageService: MessageService,
    private fb: FormBuilder
    ) {      
    this.selectedParameters = new SelectedKRAParameters();
    this.PageSize = this.resources.PageSize;
    this.PageDropDown = this.resources.PageDropDown;    
     }

  ngOnInit() {
     this.approleTypeForm = this.fb.group({    
       FinancialYearId: [null, [Validators.required]],
           DepartmentId: [null, [Validators.required]],
           GradeRoleTypeId: [null, [Validators.required]]    
    });     

    this.getFinancialYears();
    this.getDepartments();  
    this.GetRoleTypes();     
    this.Reset();
   // this.showApplicableRoleTypes();
    // this._applicableRoleTypeService.editObj.subscribe((data)=>{    
    //   if(this._applicableRoleTypeService.editMode == true){
    //     this.btnLabel = "Update";       
    //     this.approleTypeForm.patchValue(data);
    //   }
    // });     
  }
 
  private getFinancialYears(): void {   
     this._masterDataService.GetFinancialYears().subscribe((yearsdata: any[]) => {
      this.financialYearList = [];
      this.financialYearList.push({ label: 'Select Financial Year', value: null });
      yearsdata.forEach((e: any) => {
        this.financialYearList.push({ label: e.FinancialYearName, value: e.Id });
      });
    }, error => {
      this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Failed to get Financial Year List' });
    }
    );
  }

  private getDepartments() : void {
    this._masterDataService.GetDepartmentsMasterData().subscribe(
    (res: DepartmentDetails[]) => {
      this.departmentList =[];
      this.departmentList.push({label : "Select Department", value :null})
      res.forEach((element :DepartmentDetails) => {
        this.departmentList.push({
          label : element.Description,
          value : element.DepartmentId
        });
      });
    },
    (error: any) => {
      this.messageService.add({
        severity: 'error',
        summary: 'Error Message',
        detail: 'Failed to get Departments details.'
      });
    }
  );
}

  GetRoleTypes() {
    this._roleTypeService.GetActiveRoleTypes().subscribe((data) => {     
      this.roletypedata = data;
    });
  }   
 getApplicableRoleTypeList(event): void { 
    this.showApplicableRoleTypes(event.value);
   }

  private saveApplicableRoleType(): void {
    //  this.formSubmitted = true;     
     if(this.approleTypeForm.value.FinancialYearId==null ||
     this.approleTypeForm.value.DepartmentId==null||
     this.approleTypeForm.value.GradeRoleTypeId==null) return;
    if (this.approleTypeForm.valid) {     
      if (this._applicableRoleTypeService.editMode == false)
      {
        this._applicableRoleTypeService.addKRAApplicableRoleType(
        this.approleTypeForm.value).subscribe(res => {
        this.formSubmitted = false;
        this.showApplicableRoleTypes(this.approleTypeForm.value.FinancialYearId); 
        this.Clear();          
        if (res == 1) 
        this.messageService.add({ severity: 'success', summary: 'Success Message', detail: 'ApplicableRole Type added' });      
        else if (res == -1)
          this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: 'ApplicableRole Type already exist' });
          else if (res == 0)
          this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: 'Draft status not found' });
        else 
        this.messageService.add({ severity: 'error', summary: 'Error message', detail: 'Unable to add ApplicableRole Type' });      
        },
      error => {
        this.messageService.add({ severity: 'error', summary: 'Error message', detail: error.error });
      });
      }
      else
      {
        var creatingObj = new ApplicableRoleType();
        creatingObj.applicableRoleTypeId=this._applicableRoleTypeService.editObj.value.ApplicableRoleTypeId;
        creatingObj.financialYearId=this.approleTypeForm.value.FinancialYearId;
        creatingObj.departmentId=this.approleTypeForm.value.DepartmentId;
        creatingObj.gradeRoleTypeId =this.approleTypeForm.value.GradeRoleTypeId;
        this._applicableRoleTypeService.editApplicableRoleType(creatingObj).subscribe(res => { 
        if (res == 1) 
        this.messageService.add({ severity: 'success', summary: 'Success Message', detail: 'ApplicableRole Type updated' });         
      else if (res == -1)
        this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: 'ApplicableRole Type already exist' });
        else if (res == 0)
        this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: 'Draft status not found' });
      else 
       this.messageService.add({ severity: 'error', summary: 'Error message', detail: 'Unable to update ApplicableRole Type' });
       this.showApplicableRoleTypes(this.approleTypeForm.value.FinancialYearId);
        this.Clear(); 
    },
      error => {
        this.messageService.add({ severity: 'error', summary: 'Error message', detail: error.error });
      });
      }
  }
  }
    private showApplicableRoleTypes(financialYearId: number) 
    { 
    this._applicableRoleTypeService.getApplicableRoleTypes(financialYearId).subscribe(
      (data: ApplicableRoleTypeData[]) => {
        this.applicableRoleTypeList = data;       
      },
        (error)=>{    
      this.messageService.add({
        severity: 'error',
        summary: 'Failed to get ApplicableRole Types',
        detail: error.error
      });
    });       
    }

  Reset() {       
    //this.approleTypeForm.reset(); 
     this.approleTypeForm.patchValue({FinancialYearId:null, DepartmentId: null, GradeRoleTypeId:null});
    this.approleTypeForm.controls['FinancialYearId'].setErrors(null);
    this.approleTypeForm.controls['DepartmentId'].setErrors(null);
    this.approleTypeForm.controls['GradeRoleTypeId'].setErrors(null);
    this.applicableRoleTypeList =null;
  }

  //  setEditObj(editObj: ApplicableRoleTypeData): void {
  //    this._applicableRoleTypeService.editMode = true;
  //     this._applicableRoleTypeService.editObj.next(editObj);
  // }
  public deleteApplicableRoleType(rowData): void {
    this._applicableRoleTypeService.DeleteApplicableRoleType
      (rowData.ApplicableRoleTypeId).subscribe(res => {
        //this.Reset();
        this.showApplicableRoleTypes(this.approleTypeForm.value.FinancialYearId);
        this.messageService.add({
          severity: 'success',
          summary: 'Success Message',
          detail: 'Deleted Successfully'
        });
      },
      (error) => {
        this.messageService.add({
          severity: 'error',
          summary: 'Error Message',
          detail: error.error
        });
      });
  }

  Clear() {
    this.approleTypeForm.patchValue({ DepartmentId: null, GradeRoleTypeId: null });
    this.approleTypeForm.controls['DepartmentId'].setErrors(null);
    this.approleTypeForm.controls['GradeRoleTypeId'].setErrors(null);
  }

}


