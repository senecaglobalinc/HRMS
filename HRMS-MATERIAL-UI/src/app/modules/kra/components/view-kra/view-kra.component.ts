import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MasterDataService } from 'src/app/core/services/masterdata.service';
import { DepartmentDetails } from "src/app/modules/master-layout/models/role.model";
import { ApplicableRoleTypeService } from '../../services/applicableroletype.service';
import { themeconfig } from "../../../../../themeconfig";
import {KraDefinitionService} from "src/app/modules/kra/services/kradefinition.service"

interface SelectItem {
  value: number;
  label: string;
}

class GeneratePdf{
  employeeCode : string;
  financialYearId : number;
  departmentId : number;
  roleId : number;
}

@Component({
  selector: 'app-view-kra',
  templateUrl: './view-kra.component.html',
  styleUrls: ['./view-kra.component.scss']
})
export class ViewKraComponent implements OnInit {
  themeappeareance = themeconfig.formfieldappearances;
  viewKRAForm: FormGroup;
  public financialYearList: SelectItem[] = [];
  public departmentList: SelectItem[] = [];
  public gradeList: SelectItem[] = [];
  public roleTypeList: SelectItem[] = [];
  public financialYearId: number = 0;
  public gradeId: number = 0;
  public roleTypeId: number = 0;
  public departmentId: number = 0;
  selectedfromyear: number;
  selectedtoyear: number;
  GeneratePDFRequestObj = new GeneratePdf();

  constructor( private _masterDataService: MasterDataService,private _snackBar: MatSnackBar,  private _appRoleTypeService: ApplicableRoleTypeService,private fb: FormBuilder,  private _kraDefinitionServie: KraDefinitionService) { }

  ngOnInit(): void {
    this.viewKRAForm = this.fb.group({
      financialYearId: ['',[Validators.required]],
      departmentId: [''],
      gradeId: [''],
      roleTypeId: [''],
      EmployeeCode: ['',[Validators.pattern('[aAnNcC][0-9]*'), Validators.minLength(5), Validators.maxLength(5)]]
    });
    this.financialYearList = [];
    this.financialYearList.push({
      label: "Select Financial Year",
      value: null
    });
    this.getFinancialYears();
    this.getDepartments();
  }

  getFinancialYears(): void {
    this._masterDataService.GetFinancialYears().subscribe(
      (yearsdata: any[]) => {
        yearsdata.forEach((e: any) => {
          this.financialYearList.push({
            label: e.FinancialYearName,
            value: e.Id
          });
        });
      },
      error => {
        this._snackBar.open("Failed to get Financial Year List", "", {
          duration: 1000,
          horizontalPosition: "right",
          verticalPosition: "top"
        });
      }
    );
  }

  getDepartments(): void {
    this._masterDataService.GetDepartmentsMasterData().subscribe(
      (res: DepartmentDetails[]) => {
        this.departmentList = [];
        this.departmentList.push({ label: "Select Department", value: null });
        res.forEach((e: any) => {
          this.departmentList.push({
            label: e.Description,
            value: e.DepartmentId
          });
        });
      },
      error => {
        this._snackBar.open("Failed to get Department Details", "", {
          duration: 1000,
          horizontalPosition: "right",
          verticalPosition: "top"
        });
      }
    );
  }

  onchange(event) {
    this.financialYearId = event.value;
    this.viewKRAForm.get("departmentId").setValue(null);
    this.viewKRAForm.get("gradeId").setValue(null);
    this.viewKRAForm.get("roleTypeId").setValue(null);
  }

  ondepartmentchange(event) {
    this.departmentId = event.value;
    this.viewKRAForm.get("gradeId").setValue(null);
    this.viewKRAForm.get("roleTypeId").setValue(null);
    this.getApplicableGradeRoleTypes();
  }

  getApplicableGradeRoleTypes() {
    this.resetgraderoletypes();
    this.gradeList = [];
    this._appRoleTypeService
      .getGradesByDepartment(        
        this.departmentId,        
      )
      .subscribe(
        (res: any[]) => {
          if (res) {
            this.gradeList.push({ label: "Select Grade", value: null });
            res.forEach((element: any) => {
              this.gradeList.push({
                label: element.Name,
                value: element.Id
              });
              if (res.length === 1){
                this.gradeId = element.Id;
                this.viewKRAForm.get("gradeId").setValue(this.gradeId);
                }
            });
              if(res.length>0){
                this.getApplicableGradeRoleTypesByGrade();
              }
          } else {
            this.resetgraderoletypes();
          }
        },
        (error: any) => {
          this._snackBar.open("Failed to get Role Type details.", "", {
            duration: 1000,
            horizontalPosition: "right",
            verticalPosition: "top"
          });
        }
      );
  }

  getApplicableGradeRoleTypesByGrade() {
    this.roleTypeList = [];
    this.roleTypeList.push({ label: "Select Role Type", value: null });
    this._appRoleTypeService
      .getRoleTypesByGrade(        
        this.departmentId,        
        this.viewKRAForm.value.gradeId
      )
      .subscribe(
        (res: any[]) => {
          if (res) {
            res.forEach((element: any) => {
              this.roleTypeList.push({
                label: element.Name,
                value: element.Id
              });
                if (res.length === 1){
                  this.roleTypeId = element.Id;
                  this.viewKRAForm.get("roleTypeId").setValue(this.roleTypeId);
                  }
            });
          } else {
            this.resetgraderoletypes(); 
          }
        },
        (error: any) => {
          this._snackBar.open("Failed to get Role Type details.", "", {
            duration: 1000,
            horizontalPosition: "right",
            verticalPosition: "top"
          });
        }
      );
  }

  resetgraderoletypes() {
    this.roleTypeList = [];
    this.gradeList = [];
    this.roleTypeList.push({ label: "Select Role Type", value: null });
    this.gradeList.push({ label: "Select Grade", value: null });
  }

  GeneratePDF(){
    this.GeneratePDFRequestObj.employeeCode = this.viewKRAForm.value.EmployeeCode
    if(this.GeneratePDFRequestObj.employeeCode == null){
      this.GeneratePDFRequestObj.employeeCode=""
    }
    this.GeneratePDFRequestObj.financialYearId = this.viewKRAForm.value.financialYearId
    this.GeneratePDFRequestObj.departmentId = this.viewKRAForm.value.departmentId
    this.GeneratePDFRequestObj.roleId = this.viewKRAForm.value.roleTypeId
    if (this.viewKRAForm.valid) {
      this._kraDefinitionServie.GeneratePDF(this.GeneratePDFRequestObj.employeeCode, this.GeneratePDFRequestObj.financialYearId, this.GeneratePDFRequestObj.departmentId, this.GeneratePDFRequestObj.roleId).subscribe((res) => {
        this._snackBar.open("PDF Generated Successfully", "", {
          duration: 1000,
          horizontalPosition: "right",
          verticalPosition: "top"
        });
        this.clear()
      }, (error) => {
        this._snackBar.open("Failed Generating PDF", "", {
          duration: 1000,
          horizontalPosition: "right",
          verticalPosition: "top"
        })
        this.clear()
      })
    }
  }
  clear(){
    this.viewKRAForm.reset()
    this.GeneratePDFRequestObj.employeeCode=""
    this.GeneratePDFRequestObj.departmentId=0
    this.GeneratePDFRequestObj.financialYearId=0
    this.GeneratePDFRequestObj.roleId=0
  }
}
