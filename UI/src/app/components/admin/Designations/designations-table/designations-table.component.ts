import { Component, OnInit } from '@angular/core';
import { Designation } from '../../models/designation.model';
import { DesignationsService } from '../../services/designations.service';
import * as servicePath from '../../../../service-paths';
@Component({
  selector: 'app-designations-table',
  templateUrl: './designations-table.component.html',
  styleUrls: ['./designations-table.component.css']
})
export class DesignationsTableComponent implements OnInit {

  designationList : Designation[];
  list : any;
  selectedDesignation : Designation;
  resources = servicePath.API.PagingConfigValue;
  PageSize: number;
  PageDropDown: number[] = [];
  cols = [
    { field: 'GradeCode', header: 'Grade Code' },
    {field : 'DesignationCode', header : 'Designation Code'},
    {field : 'DesignationName', header : 'Designation Name'}

];
  constructor(private _designationsService : DesignationsService) { 
    this.PageSize = this.resources.PageSize;
    this.PageDropDown = this.resources.PageDropDown;
  }

  ngOnInit() {
    this._designationsService.designationData.subscribe((data)=>{
       data.forEach(function (value) {
                if (value.Grade != null)
                  value.GradeCode = value.Grade.GradeCode;
                else
                  value.GradeCode= '';
      }); 
      this.designationList = data;
    });
    this._designationsService.getDesignation();
  }

  setEditObj(editObj) {
    this._designationsService.editMode = true;
    this._designationsService.editObj.next(editObj);
   }
  ngOnDestroy() {
    // this._designationsService.designationData.unsubscribe();
  }
}
