import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { SelectItem } from 'primeng/components/common/api';
import { RoleData, RoleSufixPrefix } from '../../../../models/role.model';
import { RoleService } from '../../services/role.service';
import { GenericType } from '../../../../models/dropdowntype.model';
import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
import { MessageService } from 'primeng/api';
import { KRAService } from 'src/app/components/kra/Services/kra.service';

@Component({
  selector: 'app-roles-form',
  templateUrl: './roles-form.component.html',
  styleUrls: ['./roles-form.component.css'],
  providers: [MessageService]
})
export class RolesFormComponent implements OnInit {

  addRole: FormGroup;
  editDisable = false;
  departmentList: SelectItem[] = [];
  roleList: SelectItem[] = [];
  prefixList: SelectItem[] = [];
  suffixList: SelectItem[] = [];
  // kraList: SelectItem[] = [];
  roleMasterdata = new RoleData();
  financialYearId;
  financialYear;
  deptId;
  formSubmitted = false;
  disableDepartment: boolean = false;

  constructor(private rolesServiceObj: RoleService,
    private kraService: KRAService, private route: Router, private messageService: MessageService) { }

  ngOnInit() {
    this.addRole = new FormGroup({
      DepartmentId: new FormControl(null, [Validators.required]),
      PrefixID: new FormControl(null),
      SGRoleID: new FormControl(null, [Validators.required]),
      SuffixID: new FormControl(null),
      // KRARole: new FormControl(null, ),
      DepartmentCode: new FormControl(null),
      // KRAName : new FormControl(null, ),
      // KRAGroupID : new FormControl(null, ),
      RoleName: new FormControl(null),
      RoleDescription: new FormControl(null, [Validators.maxLength(4000)]),
      EducationQualification: new FormControl(null, [Validators.maxLength(500)]),
      KeyResponsibilities: new FormControl(null, [Validators.maxLength(500)]),
      RoleMasterID: new FormControl(0)
    });

    this.rolesServiceObj.roleData.subscribe(data => {
      if (this.rolesServiceObj.editMode == true) {
        this.addRole.patchValue(data);
        this.editDisable = true;
        // this.editRoles(this.addRole.value.RoleMasterID);
      }
    });

    this.rolesServiceObj.selectedDepartment.subscribe(data => {
      this.deptId = data;
      this.addRole.value.DepartmentId = this.deptId;
    });
    this.getDepartmentList();
    // this.getCurrentFinancialYear();
  }

  getDepartmentList(): void {
    this.rolesServiceObj.getDepartmentList().subscribe((res: any) => {

      res.forEach(element => {
        this.departmentList.push({ label: element.Description, value: element.DepartmentId });
      });
      if (this.deptId > 0) {
        this.addRole.controls['DepartmentId'].setValue(this.deptId);
        this.disableDepartment = true;
        this.getRoleSuffixAndPrefixlist(this.deptId);
      }
    });
  }

  // getKRAList(event) : void{
  //   this.getRoleSuffixAndPrefixlist(event.value);
  //   this.rolesServiceObj.getKRAList(event.value).subscribe((res :  any)=>{
  //     res.forEach(element => {
  //       this.kraList.push({ label: element.Description, value: element.DepartmentId });
  //   });
  //   });

  // }

  getKRAGroupList(event): void {
    this.getRoleSuffixAndPrefixlist(event.value);
    // this.kraService.getKRAGroupList(event.value).subscribe( (res :  any) => {
    //   res.forEach(element => {
    //     this.kraList.push({ label: element.Name, value: element.Id });
    // });
    // });

  }
  
  getRoleSuffixAndPrefixlist(DepartmentId: number): void {
    this.rolesServiceObj.GetRoleSuffixAndPrefix(DepartmentId).subscribe(
      (roleSufixPrefixResponse: RoleSufixPrefix) => {
        this.roleMasterdata.SGRoleID = null;
        this.roleMasterdata.PrefixID = null;
        this.roleMasterdata.SuffixID = null;
        this.roleList = [];
        this.roleList.push({ label: "Select a Role", value: null });
        roleSufixPrefixResponse.Roles.forEach((element) => {
          this.roleList.push({ label: element.SGRoleName, value: element.SGRoleID });
        });
        this.prefixList = [];
        this.prefixList.push({ label: "Select a Prefix", value: null });
        roleSufixPrefixResponse.Prefix.forEach((element) => {
          this.prefixList.push({ label: element.PrefixName, value: element.PrefixID });
        });
        this.suffixList = [];
        this.suffixList.push({ label: "Select a Suffix", value: null });
        roleSufixPrefixResponse.Suffix.forEach((element) => {
          this.suffixList.push({ label: element.SuffixName, value: element.SuffixID });
        });
      },
    );
  }

  //  editRoles(selectedRoleId: number): void {
  //   this.rolesServiceObj
  //    .GetRoleDetailsbyRoleID(selectedRoleId)
  //    .subscribe((selectedRole: RoleData) => {
  //   //   if (this._selectedDepartmentId == 0)
  //   //   this.getKRAList(this._selectedDeptId);
  //   // else this.getKRAList(this._selectedDepartmentId);
  //   // this.roleMasterdata = selectedRole;

  // });
  //}

  public getCurrentFinancialYear(): void {
    this.rolesServiceObj.getCurrentFinancialYear().subscribe(
      (yearsdata: GenericType) => {
        if (yearsdata != null) {
          this.financialYearId = yearsdata.Id;
          this.financialYear = yearsdata.Name;
        }
      })
  }

  onAddRole(): void {
    this.formSubmitted = true;
    if (this.addRole.valid == true) {
      if (this.addRole.value.EducationQualification == null)
        this.addRole.value.EducationQualification = "";
      if (this.addRole.value.KeyResponsibilities == null)
        this.addRole.value.KeyResponsibilities = "";
      if (this.rolesServiceObj.editMode == false) {
        this.rolesServiceObj.createRole(this.addRole.value).subscribe((res) => {
          if (res) {
            this.messageService.add({ severity: 'success', summary: 'Success Message', detail: 'Role record added successfully.' });
            this.resetForm();
            setTimeout(() => {
              this.route.navigate(['/admin/rolelist/']);
            }, 1000);
            // this.route.navigate(['/admin/rolelist/']);
          }
          else if (res == 2627) {
            this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: 'Role already exist.' });
          }
          else {
            this.messageService.add({ severity: 'error', summary: 'Failure Message', detail: 'Role cannot be added.' });
          }
        },
          error => {
            this.messageService.add({ severity: 'error', summary: 'Failure Message', detail: error.error });

          });
      }
      else {
        this.editDisable = false;
        this.rolesServiceObj.editRole(this.addRole.value).subscribe((res) => {
          if (res) {
            this.messageService.add({ severity: 'success', summary: 'Success Message', detail: 'Role record updated successfully.' });
            this.resetForm();
            setTimeout(() => {
              this.route.navigate(['/admin/rolelist/']);
            }, 1000);
          }
          else {
            this.messageService.add({ severity: 'error', summary: 'failed Message', detail: 'Role cannot be updated.' });
          }
        },
          error => {
            this.messageService.add({ severity: 'error', summary: 'Failure Message', detail: error.error });

          }
        );
        this.rolesServiceObj.editMode = false;
      }

    }
    else {
      // this.messageService.add({severity:'warn', summary: 'Warning Message', detail:'Invalid data'});
      // this.resetForm();
      // this.route.navigate(['/admin/rolelist/']);
    }

  }


  resetForm(): void {
    this.addRole.reset();
    this.addRole.value.Role = null;
    this.formSubmitted = false;
  }

  backToRoleList(): void {
    this.editDisable = false;
    this.rolesServiceObj.editMode = false;
    this.route.navigate(['/admin/rolelist/']);
  }
}

