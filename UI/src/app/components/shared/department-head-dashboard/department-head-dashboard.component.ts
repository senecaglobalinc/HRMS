import { Component, OnInit } from "@angular/core";
import { DashboardService } from "../services/dashboard.service";
import { MessageService } from "primeng/components/common/messageservice";
import { DeliveryHeadService } from "../services/delivery-head.service";
import { ProjectsData } from "../../../models/projects.model";
import * as moment from "moment";
import * as servicePath from '../../../service-paths';
import { ProjectCreationService } from "../../projectLifeCycle/services/project-creation.service";
import { Router } from "@angular/router";
import { Subscription } from "rxjs";

@Component({
  selector: "app-department-head-dashboard",
  templateUrl: "./department-head-dashboard.component.html",
  styleUrls: ["./department-head-dashboard.component.scss"],
  providers: [MessageService]
})
export class DepartmentHeadDashboardComponent implements OnInit {
  EmpId: number;
  roleName: string;
  dashboard: string = "DHDashboard";
  ProjectsList: ProjectsData[];
  PageSize: number;
  editMode : boolean;
  PageDropDown: number[] = [];
  resources = servicePath.API.PagingConfigValue;
  editModeSubscription : Subscription;
  constructor(
    private _deliveryHeadService: DeliveryHeadService,
    private messageService: MessageService,
    private route: Router,
    private ProjectCreationServiceObj: ProjectCreationService
  ) {
    this.PageSize = this.resources.PageSize;
    this.PageDropDown = this.resources.PageDropDown;
  }

  ngOnInit() {
    this.GetProjectsList();
    this.editModeSubscription = this.ProjectCreationServiceObj.GetEditMode().subscribe(data =>
      {
        this.editMode = data;
      });
  }

  cols = [
    { field: "ProjectName", header: "Project Name" },
    { field: "ManagerName", header: "Program Manager" },
    { field: "PracticeAreaCode", header: "Technology" },
    { field: "ClientName", header: "Client Name" },
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

EditProject(ProjectObject: ProjectsData): void {
  this.ProjectCreationServiceObj.SetSeletedTab(0);
  this.ProjectCreationServiceObj.SetEditMode(true);
  this.ProjectCreationServiceObj.SetProjectId(ProjectObject.ProjectId);
  this.route.navigate(['/project/addproject/' + this.dashboard]);
}

  ViewProject(ProjectObject: ProjectsData) {
    this.ProjectCreationServiceObj.SetProjectId(ProjectObject.ProjectId);
    this.route.navigate(["/project/view/" + this.dashboard]);
  }

  ngOnDestroy() {
    this.editModeSubscription.unsubscribe();
  }
}
