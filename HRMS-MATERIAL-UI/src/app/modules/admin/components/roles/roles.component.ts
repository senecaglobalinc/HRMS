import { Component, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup, FormGroupDirective, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { RoleData, RoleSufixPrefix } from 'src/app/modules/master-layout/models/role.model';
import { themeconfig } from 'src/themeconfig';
import { Department } from '../../models/department.model';
import { RoleService } from '../../services/role.service';
import { editorConfig } from '../../../../core/angularEditorConfiguratioan';


interface SelectItem {
  value: number;
  label: string;
}

@Component({
  selector: 'app-roles',
  templateUrl: './roles.component.html',
  styleUrls: ['./roles.component.scss']
})
export class RolesComponent implements OnInit {
  selectedStatus:any
  themeConfigInput = themeconfig.formfieldappearances;
  addRole: FormGroup;
  deptId;
  destroyflag=false;
  editDisable = false;
  btnLabel: string = "";
  editorConfig = editorConfig;
  departmentList: SelectItem[];
  keyFunctionList: SelectItem[] = [];
  seniorityList: SelectItem[] = [];
  specialityList: SelectItem[] = [];

  formSubmitted = false;
  disableDepartment: boolean = false;
  disableSenoritySpeciality: boolean = false;
  showSenoritySpeciality: boolean = false;

  roleMasterdata = new RoleData();

  @ViewChild(FormGroupDirective) formGroupDirective: FormGroupDirective;

  constructor(private _rolesService: RoleService, private _snackBar: MatSnackBar, private route: Router) { }

  ngOnInit(): void {

    this.btnLabel = "SAVE";

    this.addRole = new FormGroup({
      DepartmentId: new FormControl(null, [Validators.required]),
      PrefixID: new FormControl(null),
      SGRoleID: new FormControl(null, [Validators.required]),
      SuffixID: new FormControl(null),
      DepartmentCode: new FormControl(null),
      RoleName: new FormControl(null),
      RoleDescription: new FormControl(null, [Validators.maxLength(4000), Validators.minLength(2)]),
      EducationQualification: new FormControl(null, [Validators.maxLength(500), Validators.minLength(2)]),
      KeyResponsibilities: new FormControl(null, [Validators.maxLength(500), Validators.minLength(2)]),
      RoleMasterID: new FormControl(0)
    });

    this._rolesService.roleData.subscribe(data => {

      if (this._rolesService.editMode == true) {
        this.addRole.patchValue(data);
        this.btnLabel = "UPDATE";
        this.editDisable = true;
      }
    });

    this._rolesService.selectedDepartment.subscribe(data => {
      this.deptId = data;
      this.addRole.value.DepartmentId = this.deptId;
    });

    this.getDepartmentList();
  }


  getDepartmentList(): void {
    this._rolesService.getDepartmentList().subscribe((res: Department[]) => {
      this.departmentList = [];
      res.forEach(element => {
        this.departmentList.push({ label: element.Description, value: element.DepartmentId });
      });

      if (this.deptId > 0) {
        this.addRole.controls['DepartmentId'].setValue(this.deptId);
        this.disableDepartment = true;
        this.getRoleSuffixAndPrefixlist(this.deptId);
      }
    }, (error) => {
      this._snackBar.open(error.error.text, 'x', {
        duration: 3000, 
        horizontalPosition: 'right',
        verticalPosition: 'top'
      });
    });
  }

  getRoleSuffixAndPrefixlist(DepartmentId: number): void {
    let status =this.departmentList.find(s => s.value == DepartmentId);
    if (status)
       this.selectedStatus = status.label;
   this._rolesService.selectedDepartmentname=this.selectedStatus
    if (DepartmentId == 1) {
      this.showSenoritySpeciality = true;
    }
    else
      this.showSenoritySpeciality = false;
    this._rolesService.GetRoleSuffixAndPrefix(DepartmentId).subscribe(
      (roleSufixPrefixResponse: RoleSufixPrefix) => {
        this.roleMasterdata.SGRoleID = null;
        this.roleMasterdata.PrefixID = null;
        this.roleMasterdata.SuffixID = null;
        this.keyFunctionList = [];
        roleSufixPrefixResponse.Roles.forEach((element) => {
          this.keyFunctionList.push({ label: element.SGRoleName, value: element.SGRoleID });
        });
        this.seniorityList = [];
        roleSufixPrefixResponse.Prefix.forEach((element) => {
          this.seniorityList.push({ label: element.PrefixName, value: element.PrefixID });
        });
        this.specialityList = [];
        roleSufixPrefixResponse.Suffix.forEach((element) => {
          this.specialityList.push({ label: element.SuffixName, value: element.SuffixID });
        });
      }
    );
  }

  onAddRole(): void {

    this.formSubmitted = true;
    if (this.addRole.valid == true) {
      if (this.addRole.value.EducationQualification == null)
        this.addRole.value.EducationQualification = "";
      // else
      // this.addRole.value.EducationQualification = this.addRole.value.EducationQualification.content[0].content[0].text;
      if (this.addRole.value.KeyResponsibilities == null)
        this.addRole.value.KeyResponsibilities = "";
      // else
      // this.addRole.value.KeyResponsibilities = this.addRole.value.KeyResponsibilities.content[0].content[0].text;
      if (this._rolesService.editMode == false) {
        this._rolesService.createRole(this.addRole.value).subscribe((res) => {
          if (res) {
            this._rolesService.selectedDepartmentname = true;
            if (this._rolesService.editMode == false) {
              this._snackBar.open('Role record added successfully.', 'x', {
                duration: 3000,
                horizontalPosition: 'right',
                verticalPosition: 'top',
              });
              this.cancel();
            }
            this._rolesService.selectedDepartmentname=true;
            this.destroyflag=true;
            setTimeout(() => {
              this.route.navigate(['/admin/rolelist/']);
            }, 1000);
           
          }
          else if (res == 2627) {
            this._snackBar.open('Role already exist', 'x', {
              duration: 3000,
              panelClass: ['error-alert'],
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
            this.cancel();
          }
          else {
            this._snackBar.open('Unable to add role.', 'x', {
              duration: 3000,
              panelClass: ['error-alert'],
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });

            this.cancel();
          }
        },
          error => {
            this._snackBar.open(error.error, 'x', {
              duration: 3000,
              panelClass: ['error-alert'],
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });

          });
      }
      else {
        this.editDisable = false;

        this._rolesService.editRole(this.addRole.value).subscribe((res) => {
          if (res) {
            this._snackBar.open('Role record updated successfully.', 'x', {
              duration: 3000,
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
            this.cancel();
            this.route.navigate(['/admin/rolelist/']);
          }
          else {
            this._snackBar.open('Role cannot be updated.', 'x', {
              duration: 3000,
              panelClass: ['error-alert'],
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });

            this.cancel();
          }
        },
          error => {
            this._snackBar.open(error.error, 'x', {
              duration: 3000,
              panelClass: ['error-alert'],
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });

          });
        this._rolesService.editMode = false;
      }

    }

  }



  backToRoleList(): void {
    this.editDisable = false;
    this.destroyflag=true;
    this._rolesService.editMode = false;
    this._rolesService.selectedDepartmentname=true;
    this.cancel();
    this.route.navigate(['/admin/rolelist/']);
  }

  cancel(): void {
    if (this.btnLabel == "UPDATE") {
      this._rolesService.selectedDepartmentname = true;
      this.addRole.controls.RoleDescription.reset();
      this.addRole.controls.EducationQualification.reset();
      this.addRole.controls.KeyResponsibilities.reset();
    }
    else {
      this.formSubmitted = false;
      this.disableSenoritySpeciality = false;
      this.showSenoritySpeciality = false;
      this.btnLabel = "SAVE";
      this.editDisable = false;
      this.dynamicClientValidations('PrefixID', this.addRole);
      this.dynamicClientValidations('SGRoleID', this.addRole);
      this.dynamicClientValidations('SuffixID', this.addRole);
      this.dynamicClientValidations('RoleDescription', this.addRole);
      this.dynamicClientValidations('EducationQualification', this.addRole);
      this.dynamicClientValidations('KeyResponsibilities', this.addRole);

     }


  }
  dynamicClientValidations(clientControl: string, newFormClientValidate: FormGroup){
    newFormClientValidate.controls[clientControl].setValue(null);
    newFormClientValidate.controls[clientControl].setErrors(null);
    newFormClientValidate.controls[clientControl].clearValidators();
    newFormClientValidate.controls[clientControl].updateValueAndValidity();    
  }
  ngOnDestroy(){
    if(this.destroyflag)
       return
    else{this._rolesService.selectedDepartmentname=false;
    this.cancel();
    }
  }
}
