import { Component, OnInit } from '@angular/core';
import { Router } from "@angular/router";
import { Department } from "../../models/department.model";
import { SelectItem, Message } from "primeng/components/common/api";
import { DepartmentService } from "../../services/department.service";
import { CommonService } from "../../../../services/common.service";
import { MasterDataService } from "../../../../services/masterdata.service";
import { FormGroup, FormBuilder, FormControl, Validators } from "@angular/forms";
import * as servicePath from '../../../../service-paths';


@Component({
  selector: 'app-departments-table',
  templateUrl: './departments-table.component.html',
  styleUrls: ['./departments-table.component.css']
})
export class DepartmentsTableComponent implements OnInit {
  Department: Department[];
  selectedDepartment : Department;
  PageSize: number;
  PageDropDown: number[] = [];
  resources = servicePath.API.PagingConfigValue;
  cols = [
    { field: "DepartmentCode", header: "Department Code" },
    { field: "Description", header: "Department Name" },
      { field: "DepartmentTypeDescription", header: "Department Type" },
      { field: "DepartmentHeadName", header: "Department Head" }
  ];

  constructor(private _departmentService: DepartmentService) {
    this.PageSize = this.resources.PageSize;
      this.PageDropDown = this.resources.PageDropDown;
  }

  ngOnInit() {
    this._departmentService.Department.subscribe((data) => {
      if(data!= null)  {
        this.Department = data;
        this.Department.forEach(department => {
          department.DepartmentTypeDescription = 
          department['DepartmentType']['DepartmentTypeDescription'];
        });
      }

    });
     this.getDepartmentDetails();
  }
  getDepartmentDetails() {
    this._departmentService.getDepartmentDetails();
  }

  editDepartments(departmentData) {
   this._departmentService.editMode = true;
   this._departmentService.departmentsEdit.next(departmentData);
  }

  
  ngOnDestroy() {
  //  this._departmentService.Department.complete();
  }

}
