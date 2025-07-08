import { Component, OnInit} from '@angular/core';
import { DepartmentTypeService } from '../../services/department-type.service';
import { DepartmentTypeData } from '../../models/department-type.model';
import * as servicePath from '../../../../service-paths';

@Component({
  selector: 'app-department-type-table',
  templateUrl: './department-type-table.component.html',
  styleUrls: ['./department-type-table.component.scss']
})
export class DepartmentTypeTableComponent implements OnInit {
  PageSize: number;
  PageDropDown: number[] = [];
  resources = servicePath.API.PagingConfigValue;
  departmentList : DepartmentTypeData[];
  selectedRow : DepartmentTypeData;

  cols = [
    {field: 'DepartmentTypeDescription', header: 'Department Type Description' }
];

  constructor(private _departmenttypeService: DepartmentTypeService) {
    this.PageSize = this.resources.PageSize;
      this.PageDropDown = this.resources.PageDropDown;
   }

  ngOnInit() {
    this._departmenttypeService.departmentList.subscribe((data) => {
      this.departmentList = data;
    });
    this.getDepartmentType();
  }

  getDepartmentType() : void {
    this._departmenttypeService.getDepartmentType();
  }

  editDepartmentType(departmentEdit) : void{
    this._departmenttypeService.editMode = true;
    this._departmenttypeService.departmentEdit.next(departmentEdit);
  }

  ngOnDestroy() {
    // this._domainService.domainsList.unsubscribe();
  }

}






