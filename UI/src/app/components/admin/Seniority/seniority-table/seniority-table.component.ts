import { Component, OnInit} from '@angular/core';
import { Seniority } from '../../models/seniority.model';
import { SeniorityService } from '../../services/seniority.service';
import * as servicePath from '../../../../service-paths';

@Component({
  selector: 'app-seniority-table',
  templateUrl: './seniority-table.component.html',
  styleUrls: ['./seniority-table.component.scss']
})
export class SeniorityTableComponent implements OnInit {
  PageSize: number;
  PageDropDown: number[] = [];
  resources = servicePath.API.PagingConfigValue;
  seniorityList : Seniority[];
  selectedRow : Seniority;

  cols = [
    {field: 'PrefixName', header: 'Seniority Name' }
];

  constructor(private _seniorityService: SeniorityService) {
    this.PageSize = this.resources.PageSize;
      this.PageDropDown = this.resources.PageDropDown;
   }

  ngOnInit() {
    this._seniorityService.seniorityList.subscribe((data) => {
      this.seniorityList = data;
    });
    this.getSeniorities();
  }

  getSeniorities() : void {
    this._seniorityService.getSeniorities();
  }

  editSeniorities(seniorityEdit) : void{
    this._seniorityService.editMode = true;
    this._seniorityService.seniorityEdit.next(seniorityEdit);
  }
}
