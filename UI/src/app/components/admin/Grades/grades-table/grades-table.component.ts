import { Component, OnInit } from '@angular/core';
import { GradesService } from '../../services/grades.service';
import { Grade } from '../../models/grade.model';
import * as servicePath from '../../../../service-paths';
@Component({
  selector: 'app-grades-table',
  templateUrl: './grades-table.component.html',
  styleUrls: ['./grades-table.component.css']
})
export class GradesTableComponent implements OnInit {
  resources = servicePath.API.PagingConfigValue;
  PageSize: number;
  PageDropDown: number[] = [];
  gradesData : Grade[];
  selectedGrade : Grade;

  constructor(private serviceObj : GradesService) { 
    this.PageSize = this.resources.PageSize;
    this.PageDropDown = this.resources.PageDropDown;
  }
  
  ngOnInit() {
    this.serviceObj.GradesData.subscribe((data)=>{
      this.gradesData = data;
    });
    this.GetGradesDetails();
  }
  cols = [
    { field : "GradeCode", header : "Grade Code"},
    { field : "GradeName", header : "Grade Name"}
  ]
  GetGradesDetails(): void{
    this.serviceObj.getGradesDetails();
  }
 
SetEditObj(editObj : Grade): void{
   this.serviceObj.editMode = true;
   this.serviceObj.editObj.next(editObj);
  }
  

  ngOnDestroy() {
    // this.serviceObj.GradesData.unsubscribe();
  }
}
