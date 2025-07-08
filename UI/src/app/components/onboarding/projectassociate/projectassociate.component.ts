import { Component, OnInit, Inject, ViewChild, Injector } from "@angular/core";

import { Projects } from "../models/assosiateproject.model";
import { Associate } from "../models/associate.model";
import { Http } from "@angular/http";
import { Router, ActivatedRoute } from "@angular/router";
import { ProjectassosiateService } from "../services/projectassosiate.service";
// import { DataStore } from "../../../shared/datastore";
import * as moment from "moment";
import { CommonService } from "../../../services/common.service";
import { MasterDataService } from "../../../services/masterdata.service";
import { SelectItem } from "primeng/components/common/api";
import { MessageService } from 'primeng/api';
import { ConfirmationService } from 'primeng/api';


@Component({
  selector: 'app-projectassociate',
  templateUrl: './projectassociate.component.html',
  styleUrls: ['./projectassociate.component.scss'],
  providers: [ProjectassosiateService, CommonService, MasterDataService, MessageService, ConfirmationService]
})
export class ProjectassociateComponent implements OnInit {
  id: number;
  projects: Array<Projects>;
  _Associate = new Associate();
  currentempID: number;
  @ViewChild("messageToaster") messageToaster: any;
  _resources: any;
  _dataService: Array<Projects>;
  private _serverURL: string;
  roles: any[] = [];
  submitted = false;
  type: string = "new";
  index: number;
  buttonType: string;
  domainList: SelectItem[] = [];
  filterDuplicates: boolean = false;

  @ViewChild("projectsForm") projectsForm: any;
  @ViewChild("projectDialog") projectDialog: any;

  constructor(
    @Inject(Http) private _http: Http,
    // private _injector: Injector = AppInjector(),
    @Inject(ProjectassosiateService) private _service: ProjectassosiateService,
    private _commonService: CommonService,
    private messageService: MessageService,
    private masterDataService: MasterDataService,
    @Inject(Router) private _router: Router,
    private actRoute: ActivatedRoute, private _dialogservice: ConfirmationService,

  ) {
    this.projects = new Array<Projects>();
    this.projects.push({
      Id: 0,
      Duration: null,
      RoleMasterId: "",
      OrganizationName: "",
      ProjectName: "",
      RoleName: "",
      KeyAchievements: "",
      DomainId: null
    });

  }

  ngOnInit() {
    this.actRoute.params.subscribe(params => {
      this.id = params["id"];
    });
    this.getDomainList();
    this.currentempID = this.id;
  }

  getDomainList() {
    this.masterDataService.GetDomains().subscribe((res: any[]) => {
      res.forEach((element: any) => {
        this.domainList.push({
          label: element.DomainName,
          value: element.DomainID
        });
      });
      this._service.GetAssociateProjects(this.id).subscribe((res: any) => {
        this.projects = res;
        if (this.projects.length != 0) {
          this.type = "edit";
        }
        if (this.projects.length == 0)
          this.projects.push({
            Id: 0,
            Duration: null,
            RoleMasterId: "",
            OrganizationName: "",
            ProjectName: "",
            RoleName: "",
            KeyAchievements: "",
            DomainId: null
          });
      });
    });
  }

  filterProjectDuplicates(project: Projects): boolean {
    if (
      this.projects.length > 0 &&
      project &&
      project.DomainId != null &&
      project.OrganizationName != "" &&
      project.ProjectName != ""
    ) {
      project.OrganizationName = project.OrganizationName.trim();
      project.ProjectName = project.ProjectName.trim();
      let projectList: Projects[] = this.projects.filter((projectOfList: Projects) => {
        return (
          projectOfList.DomainId == project.DomainId &&
          projectOfList.OrganizationName.toLowerCase() === project.OrganizationName.toLowerCase() &&
          projectOfList.ProjectName.toLowerCase() === project.ProjectName.toLowerCase()
        );
      })
      if (projectList.length > 1)
        return true;
      else
        return false;
    }
  }

  OnSubmit(project: Projects[]) {
    if (project.length > 0)
      this.filterDuplicates = this.filterProjectDuplicates(
        project[project.length - 1]
      );

    if (this.filterDuplicates == true) {
      this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: 'Please check Duplicates projects details' });
      // swal("Please check Duplicates", "", "error");
      return false;
    }

    if (this.buttonType == "Save" || this.buttonType == "Update") {
      this.onSaveorUpdate(this.projects);
      return true;
    }
    else if (this.buttonType == "AddNewProject")
      return this.onNewProject();
  }

  OnUpdate() {
    this.buttonType = "Update";
  }

  OnSave() {
    this.buttonType = "Save";
  }

  setNewProjectButtonType() {
    this.buttonType = "AddNewProject";
  }

  onNewProject() {
    this.projects.push({
      Id: 0,
      Duration: null,
      RoleMasterId: "",
      OrganizationName: "",
      ProjectName: "",
      RoleName: "",
      KeyAchievements: "",
      DomainId: null
    });
  }

  onSaveorUpdate(qual: Array<Projects>) {
    for (var i = 0; i < qual.length; i++) {
      if (
        !qual[i].ProjectName ||
        !qual[i].OrganizationName ||
        !qual[i].Duration ||
        !qual[i].KeyAchievements || qual[i].KeyAchievements.trim().length == 0 ||
        !qual[i].DomainId
      ) {
        this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: 'Please provide complete details' });
        // swal("Please provide complete details", "", "error");
        return false;
      }
    }
    if (this.filterDuplicates == true) {
      this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: 'Please check Duplicates projects details' });
      // swal("Please check Duplicates", "", "error");
      return false;
    }
    this._Associate.EmpId = this.currentempID;
    this._Associate.Projects = [];
    for (var i = 0; i < qual.length; i++) {
      this._Associate.Projects.push(qual[i]);
    }
    this._service.SaveProjectDetails(this._Associate).subscribe(
      data => {
        this.messageService.add({ severity: 'success', summary: 'success Message', detail: 'Project details saved successfully' });
        // swal("", "Project details saved successfully", "success");
        this._service
          .GetAssociateProjects(this._Associate.EmpId)
          .subscribe((res: any) => {
            this.projects = res;
            if (this.projects.length != 0) this.type = "edit";
          });
      },
      error => {
        this.messageService.add({ severity: 'error', summary: 'error Message', detail: 'Failed to save projects details' });
        // swal("", "Failed to save projects details", "error");
      }
    );
  }

  onDelete(index: number) {
    this.index = index;
    this.OpenConfirmationDialog();

  }
  OpenConfirmationDialog() {   // method to open dialog
    this._dialogservice.confirm({
      message: 'Are you sure, you want to delete this?',
      accept: () => {
        this.Delete();
      },
      reject: () => {
        this.onCancel();
      }
    });
  }
  Delete() {
    this.projects.splice(this.index, 1);
    // this.projectDialog.nativeElement.close();
  }

  onCancel() {
    // this.projectDialog.nativeElement.close();
  }

  onlyNumbers(event: any) {
    this._commonService.onlyNumbers(event);
  }

  onRoleChage(event: any, selectedProject: any) {
    let roleId = event.target.value;
    let count = this.validate(selectedProject.projectName, roleId);
    if (count == 1) {
      event.target.value = "";
      this.messageService.add({ severity: 'warn', summary: 'warning Message', detail: 'Duplicate project are not allowed' });
      // swal("", "Duplicate project are not allowed", "warning");
    }
  }
  onProjectChange(event: any, selectedProject: any) {
    let count = this.validate(
      selectedProject.projectName,
      selectedProject.roleID
    );
    if (count == 2) {
      selectedProject.projectName = "";
      this.messageService.add({ severity: 'warn', summary: 'warning Message', detail: 'Duplicate project are not allowed' });
      // swal("", "Duplicate project are not allowed", "warning");
    }
  }
  validate(projectName: any, roleId: any): number {
    let count = 0;

    this.projects.forEach((p: any) => {
      if (
        p.projectName.toUpperCase() == projectName.toUpperCase() &&
        p.roleID == roleId
      )
        count++;
    });
    return count;
  }

  omit_special_char(event: any) {
    let k: any;
    k = event.charCode; //         k = event.keyCode;  (Both can be used)
    return (
      (k > 64 && k < 91) ||
      (k > 96 && k < 123) ||
      k == 8 ||
      k == 32 ||
      k == 46 ||
      k == 44 ||
      (k >= 48 && k <= 57)
    );
  }

  toast(msg: string) {
    this.messageToaster.nativeElement.text = msg;
    this.messageToaster.nativeElement.open();
  }


}

