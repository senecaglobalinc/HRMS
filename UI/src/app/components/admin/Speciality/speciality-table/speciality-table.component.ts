import { Component, OnInit } from '@angular/core';
import { SpecialityService } from '../../services/speciality.service';
import { Speciality } from '../../models/speciality.model';
import * as servicePath from '../../../../service-paths';

@Component({
  selector: 'app-speciality-table',
  templateUrl: './speciality-table.component.html',
  styleUrls: ['./speciality-table.component.scss']
})
export class SpecialityTableComponent implements OnInit {

  PageSize: number;
  PageDropDown: number[] = [];
  resources = servicePath.API.PagingConfigValue;
  specialityList : Speciality[];
  selectedRow : Speciality;

  cols = [
    {field: 'SuffixName', header: 'Speciality Name' }
];

  constructor(private _specialityService: SpecialityService) {
    this.PageSize = this.resources.PageSize;
      this.PageDropDown = this.resources.PageDropDown;
   }

  ngOnInit() {
    this._specialityService.specialityList.subscribe((data) => {
      this.specialityList = data;
    });
    this.getSpecialities();
  }

  getSpecialities() : void {
    this._specialityService.getSpecialities();
  }

  editSpecialities(specialityEdit) : void{
    this._specialityService.editMode = true;
    this._specialityService.specialityEdit.next(specialityEdit);
  }
}
