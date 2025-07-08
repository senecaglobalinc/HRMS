import { Component, OnInit } from '@angular/core';
import { PracticeAreaService } from '../../services/practice-area.service';
import { PracticeArea } from '../../../../models/practicearea.model';
import * as servicePath from '../../../../service-paths';

@Component({
  selector: 'app-practice-area-table',
  templateUrl: './practice-area-table.component.html',
  styleUrls: ['./practice-area-table.component.css']
})
export class PracticeAreaTableComponent implements OnInit {
  practiceAreaList : PracticeArea[];
  selectedRow : PracticeArea;
  PageSize: number;
  PageDropDown: number[] = [];
  resources = servicePath.API.PagingConfigValue;
  cols = [
    { field: 'PracticeAreaCode', header: 'Practice Area Code' },
    {field : 'PracticeAreaDescription', header : 'Practice Area Description'},

];
  constructor(private _practiceAreaService : PracticeAreaService) { 
    this.PageSize = this.resources.PageSize;
    this.PageDropDown = this.resources.PageDropDown;
  }

  ngOnInit() {
    this._practiceAreaService.practiceAreaList.subscribe((data) => {
      this.practiceAreaList = data;
    });
    this.getPracticeAreas();
  }

  getPracticeAreas() : void {
    this._practiceAreaService.getPracticeAreas();
  }

  editPracticeArea(practiceAreaEdit) : void{
  this._practiceAreaService.editMode = true;
  this._practiceAreaService.practiceAreaEdit.next(practiceAreaEdit);
  }

  ngOnDestroy() {
      // this._practiceAreaService.practiceAreaList.complete();
  }
}





