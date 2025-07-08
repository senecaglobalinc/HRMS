import { Component, OnInit, Input } from '@angular/core';
import { SelectItem } from 'primeng/components/common/selectitem';
import { RoleService } from '../../services/role.service';
import { Router } from '@angular/router';
import { RoleData, DepartmentDetails } from '../../../../models/role.model';
import { ActivatedRoute } from '@angular/router';
import { FormGroup, FormControl } from '@angular/forms';
import * as servicePath from '../../../../service-paths';

@Component({
  selector: 'app-roles-table',
  templateUrl: './roles-table.component.html',
  styleUrls: ['./roles-table.component.css']
})
export class RolesTableComponent implements OnInit {
  searchDepartment : FormGroup;
  recordsPerPage = 5;
  rolesData : RoleData[];
  departmentList: SelectItem[] = [];
  deptId : number;
  PageSize: number;
  PageDropDown: number[] = [];
  resources = servicePath.API.PagingConfigValue;
  selectedRoleData: any;
  cols = [
    { field: "RoleName", header: "Role Name" },
    { field: "RoleDescription", header: "Role Description" },
  ]

  constructor(private rolesServiceObj: RoleService, private route: Router) {
    this.PageSize = this.resources.PageSize;
    this.PageDropDown = this.resources.PageDropDown;
  }

  ngOnInit() {
    this.searchDepartment = new FormGroup({
      DepartmentId : new FormControl(null, ),
    });
    this.rolesServiceObj.selectedDepartment.subscribe(data => {
      this.searchDepartment.value.DepartmentId = data;
      this.deptId = this.searchDepartment.value.DepartmentId;
  });
    this.getDepartmentList();
     
  }

  getDepartmentList(): void {
    this.rolesServiceObj.getDepartmentList().subscribe((res: DepartmentDetails[]) => {
      this.departmentList.push({ label: 'Select Department', value: -1 });
      res.forEach(element => {
        this.departmentList.push({ label: element.Description, value: element.DepartmentId });
      });
     if (this.deptId > 0) {
      this.searchDepartment.controls['DepartmentId'].setValue(this.deptId);
      //  this.searchDepartment.value.DepartmentId = this.deptId;
      this.searchRoleByDepartment(this.deptId);
      }
    });
  }

  searchRoleByDepartment(selectedDepartment) : void{
    this.rolesServiceObj.getRoleDetailsByDepartmentId(selectedDepartment).subscribe((res: RoleData[]) => {
      // this.searchDepartment.value.DepartmentId = selectedDepartment;
      this.rolesData = res;
      this.rolesData.forEach(role=>{
        role.RoleName = `${role.SGRolePrefix.PrefixName} ${role.SGRoleSuffix.SuffixName} ${role.SGRole.SGRoleName}`;
        role.DepartmentCode = role.Department.DepartmentCode;                                                                                                                                                                                                                                                  
      })
    })
  }

  createRole(selectedDepartment) : void{
    this.rolesServiceObj.selectedDepartment.next(this.searchDepartment.value.DepartmentId);
    this.route.navigate(['/admin/addrole']);
  }

  // getRows(event) {
  //   this.recordsPerPage = event.rows;
  // }

  editRoles(roleData): void {
    this.rolesServiceObj.selectedDepartment.next(this.searchDepartment.value.DepartmentId);
    this.rolesServiceObj.editMode = true;
    this.rolesServiceObj.roleData.next(roleData);
    this.route.navigate(['/admin/addrole']);
  }
}
