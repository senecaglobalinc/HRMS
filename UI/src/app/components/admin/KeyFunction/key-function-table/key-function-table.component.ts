import { Component, OnInit } from '@angular/core';
import { KeyFunctionService } from '../../services/key-function.service';
import { KeyFunction } from '../../models/key-function.model';
import * as servicePath from '../../../../service-paths';

@Component({
  selector: 'app-key-function-table',
  templateUrl: './key-function-table.component.html',
  styleUrls: ['./key-function-table.component.scss']
})
export class KeyFunctionTableComponent implements OnInit {

  KeyFunctionList : KeyFunction[];
  selectedRow : KeyFunction;
  PageSize: number;
  PageDropDown: number[] = [];
  resources = servicePath.API.PagingConfigValue;

  cols = [
    {field: 'DepartmentCode', header: 'Department Code' },
    {field : 'SGRoleName', header : 'KeyFunction Name'},
];

  constructor(private _keyFunctionService : KeyFunctionService) { 
    this.PageSize = this.resources.PageSize;
    this.PageDropDown = this.resources.PageDropDown;
  }

  ngOnInit() {
    this._keyFunctionService.KeyFunctionList.subscribe((data) => {
       data.forEach(function (value) {
                if (value.Department != null)
                  value.DepartmentCode = value.Department.DepartmentCode;
                else
                  value.DepartmentCode= '';
      });
      this.KeyFunctionList = data;

    });
    this.getKeyFunctions();
  }

  getKeyFunctions() : void{
    this._keyFunctionService.getKeyFunctions();

  }

  editKeyFunction(keyFunctionEdit) : void{
    this._keyFunctionService.editMode = true;
    this._keyFunctionService.KeyFunctionEdit.next(keyFunctionEdit);
   }
}






