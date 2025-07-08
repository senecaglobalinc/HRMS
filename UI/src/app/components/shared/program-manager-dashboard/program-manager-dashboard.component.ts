import { Component, OnInit } from "@angular/core";
import { ProjectsData } from "../../../models/projects.model";
import * as moment from "moment";
import { DeliveryHeadService } from "../services/delivery-head.service";
import * as servicePath from "../../../service-paths";
import { ProjectCreationService } from "../../projectLifeCycle/services/project-creation.service";
import { Router } from "@angular/router";
@Component({
  selector: "app-program-manager-dashboard",
  templateUrl: "./program-manager-dashboard.component.html",
  styleUrls: ["./program-manager-dashboard.component.scss"]
})
export class ProgramManagerDashboardComponent implements OnInit {
  EmpId: number;
  roleName: string;
  dashboard: string = "PMDashboard";
  ProjectsList: ProjectsData[];
  PageSize: number;
  PageDropDown: number[] = [];
  resources = servicePath.API.PagingConfigValue;
  constructor(
    private _deliveryHeadService: DeliveryHeadService,
    private route: Router,
    private ProjectCreationServiceObj: ProjectCreationService
  ) {
    this.PageSize = this.resources.PageSize;
    this.PageDropDown = this.resources.PageDropDown;
  }

  ngOnInit() {
    this.GetProjectsList();
  }

  cols = [
    { field: "ProjectName", header: "Project" },
    { field: "ManagerName", header: "Program Manager" },
    { field: "PracticeAreaCode", header: "Technology" },
    { field: "ClientName", header: "Client" },
    { field: "ActualStartDate", header: "Start Date" },
    { field: "ProjectState", header: "Status" }
  ];

  GetProjectsList(): void {
    this.roleName = JSON.parse(
      sessionStorage["AssociatePortal_UserInformation"]
    ).roleName;
    this.EmpId = JSON.parse(
      sessionStorage["AssociatePortal_UserInformation"]
    ).employeeId;
    // this.EmpId = JSON.parse(sessionStorage['AssociatePortal_UserInformation']).employeeId;
    this._deliveryHeadService
      .GetProjectsList(this.roleName, this.EmpId, this.dashboard)
      .subscribe((res: ProjectsData[]) => {
        this.ModifyDateFormat(res);
      });
  }

  ModifyDateFormat(data: ProjectsData[]): void {
    data.forEach(e => {
      if (e.ActualStartDate != null) {
        e.ActualStartDate = moment(e.ActualStartDate).format("MM/DD/YYYY");
      }
      if (e.ActualEndDate != null) {
        e.ActualEndDate = moment(e.ActualEndDate).format("MM/DD/YYYY");
      }
      if (e.PlannedStartDate != null) {
        e.PlannedStartDate = moment(e.PlannedStartDate).format("MM/DD/YYYY");
      }
      if (e.PlannedEndDate != null) {
        e.PlannedEndDate = moment(e.PlannedEndDate).format("MM/DD/YYYY");
      }
    });
    this.ProjectsList = data;
  }

  ViewProject(ProjectObject: ProjectsData) {
    this.ProjectCreationServiceObj.SetProjectId(ProjectObject.ProjectId);
    this.route.navigate(["/project/view/" + this.dashboard]);
  }
}
