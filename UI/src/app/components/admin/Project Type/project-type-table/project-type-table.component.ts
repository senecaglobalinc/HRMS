import { Component, OnInit } from "@angular/core";
import { ProjectTypeData } from "../../Models/projecttype.model";
import { ProjectTypeService } from "../../services/project-type.service";
import * as servicePath from "../../../../service-paths";

@Component({
  selector: "app-project-type-table",
  templateUrl: "./project-type-table.component.html",
  styleUrls: ["./project-type-table.component.css"]
})
export class ProjectTypeTableComponent implements OnInit {
  projectTypeData: ProjectTypeData[];
  selectedProjectTypeData: ProjectTypeData;
  resources = servicePath.API.PagingConfigValue;
  PageSize: number;
  PageDropDown: number[] = [];
  constructor(private serviceObj: ProjectTypeService) {
    this.PageSize = this.resources.PageSize;
    this.PageDropDown = this.resources.PageDropDown;
  }

  ngOnInit() {
    this.serviceObj.projectTypeData.subscribe(data => {
      this.projectTypeData = data;
    });
    this.GetProjectTypeData();
  }

  cols = [
    { field: "ProjectTypeCode", header: "Project Type Code" },
    { field: "Description", header: "Project Type Description" }
  ];

  GetProjectTypeData(): void {
    this.serviceObj.getProjectTypeData();
  }

  SetEditObj(editObj: ProjectTypeData): void {
    this.serviceObj.editMode = true;
    this.serviceObj.editObj.next(editObj);
  }

  ngOnDestroy() {
    // this.serviceObj.projectTypeData.unsubscribe();
  }
}
