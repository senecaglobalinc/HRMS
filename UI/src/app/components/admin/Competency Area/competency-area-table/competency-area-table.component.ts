import {
  Component,
  OnInit,
} from "@angular/core";
import { CompetencyAreaService } from "../../services/competency-area.service";
import { CompetencyArea } from "../../models/competencyarea.model";
import * as servicePath from '../../../../service-paths';
@Component({
  selector: "app-competency-area-table",
  templateUrl: "./competency-area-table.component.html",
  styleUrls: ["./competency-area-table.component.css"]
})
export class CompetencyAreaTableComponent implements OnInit {
  competencyAreaData: CompetencyArea[];
  resources = servicePath.API.PagingConfigValue;
  PageSize: number;
  PageDropDown: number[] = [];
  cols = [
    { field: "CompetencyAreaCode", header: "Competency Area Code" },
    { field: "CompetencyAreaDescription", header: "Competency Area Description" }
  ];
  selectedCompetencyArea: CompetencyArea;
  
  constructor(private serviceObj: CompetencyAreaService) {
    this.PageSize = this.resources.PageSize;
    this.PageDropDown = this.resources.PageDropDown;
  }

  ngOnInit() {
    this.serviceObj.competencyAreaData.subscribe((data) => {
      this.competencyAreaData = data;
    });
    this.serviceObj.GetCompetencyAreaData();
  }

  setEditObj(editObj) : void {
   this.serviceObj.editMode = true;
   this.serviceObj.editObj.next(editObj);
  }
 
  ngOnDestroy() {
  }

}
