import { Component, OnInit } from "@angular/core";
import { FormGroup, FormControl, Validators } from "@angular/forms";
import { SelectItem, MessageService } from "primeng/api";
import { ProjectCreationService } from "../services/project-creation.service";
import { Client } from "../../admin/models/client.model";
import { MasterDataService } from "../../../services/masterdata.service";
import { ProjectsData } from "../../../models/projects.model";
import { ConfirmationService } from "primeng/api";
import * as moment from "moment";
import { Subscription } from "rxjs";
import { CommonService } from "../../../services/common.service";
import { DomainMasterData } from "../../admin/models/domainmasterdata.model";
import { Department } from "../../admin/models/department.model";
import { ProjectTypeData, } from "../../admin/Models/projecttype.model";
import { PracticeArea } from "src/app/models/associate-skills.model";
import { ActivatedRoute } from "../../../../../node_modules/@angular/router";

@Component({
  selector: "app-project-creation",
  templateUrl: "./project-creation.component.html",
  styleUrls: ["./project-creation.component.scss"],
  providers: [MessageService, ConfirmationService]
})
export class ProjectCreationComponent implements OnInit {
  dialogMsg = "";
  isEndDateRequired = false;
  stateDisable = true;
  displayCBR = false;
  btnLabel = "";
  formSubmitted = false;
  showDropdown = false;
  addProject: FormGroup;
  projectTypes: SelectItem[] = [];
  statusList: SelectItem[] = [];
  clientsList: SelectItem[] = [];
  managersList: SelectItem[] = [];
  practiceAreaList: SelectItem[] = [];
  domainList: SelectItem[] = [];
  projectId: number = 0;
  updatedProjectData = new ProjectsData();
  UserRole: string;
  EmpId: number;
  editMode: boolean;
  departmentsList: SelectItem[] = [];
  projectIdSubscription: Subscription;
  hideProjectState: boolean = false;
  
  constructor(
    private actRoute: ActivatedRoute,
    private projectCreationService: ProjectCreationService,
    private commonService: CommonService,
    private masterService: MasterDataService,
    private messageService: MessageService,
    private confirmationService: ConfirmationService
  ) {
    
  }

  ngOnInit() {
    this.projectIdSubscription = this.projectCreationService
      .GetProjectId()
      .subscribe(data => {
        // subscribing to ProjectId observable so that we can get the changed data every time
        this.projectId = data;
        if (this.projectId > 0) {
          this.btnLabel = "Update";
        }
          
      });
  
    this.CreateForm();
    this.GetProjectStates();
    this.GetProjectTypes();
    this.GetClients();
    this.GetPracticeAreas();
    this.SetUserRoleName();
    this.GetProgramManagers();
    this.GetDepartments();
    this.GetDomains();
  }

  GetProjectStates(): void {
    this.projectCreationService.GetProjectStates().subscribe((res: any) => {
      res.forEach(element => {
        if (
          element.StatusCode == "Halted" ||
          element.StatusCode == "Execution" ||
          element.StatusCode == "Closed"
        )
          this.statusList.push({
            label: element.StatusCode,
            value: element.StatusId
          });
      });
    });
  }

  CreateForm(): void {
    // instalize the form
    if (this.projectId > 0)
      this.btnLabel = "Update";
    else
      this.btnLabel = "Save";
    this.addProject = new FormGroup({
      ProjectCode: new FormControl(null, [
        this.commonService.unAllowednames_usingCustom,
        Validators.required,
        Validators.maxLength(30),
        // Validators.pattern("^(?!-)(?!.*--)[A-Za-z0-9- ]+$")
        Validators.pattern("^(?!.*--)^(?!-[0-9])[A-Za-z0-9-.()&/, ]+$")
      ]),
      ProjectTypeId: new FormControl(null, [Validators.required]),
      ProjectName: new FormControl(null, [
        this.commonService.unAllowednames_usingCustom,
        Validators.required,
        Validators.maxLength(50),
        // Validators.pattern("^(?!-)(?!.*--)^(?![0-9])[A-Za-z0-9- ]+$")
        Validators.pattern("^(?!.*--)^(?!-[0-9])[A-Za-z0-9-.()&/, ]+$")
      ]),
      DomainId: new FormControl(null),
      ProjectStateId: new FormControl(null),
      ClientId: new FormControl(null, [Validators.required]),
      ManagerId: new FormControl(null), // required  disabled: this.showDropdown
      PracticeAreaId: new FormControl(null, [Validators.required]),
      ActualStartDate: new FormControl(null, [Validators.required]),
      ActualEndDate: new FormControl(null),
      DepartmentId: new FormControl(1) //required
    });
    if (this.UserRole === "Department Head")
      // add validation to Program manager dropdown if login user is department head
      this.addProject.controls["ManagerId"].setValidators([
        Validators.required
      ]);
  }

  GetDomains() {
    this.domainList.push({ label: "", value: null });
    this.projectCreationService
      .GetDomains()
      .subscribe((res: DomainMasterData[]) => {
        res.forEach((element: DomainMasterData) => {
          this.domainList.push({
            label: element.DomainName,
            value: element.DomainID
          });
        });
      });
  }


  OpenConfirmationDialog(editMode: number, msg: string) {
    // method to open dialog

    this.confirmationService.confirm({
      message: msg,
      accept: () => {
        this.projectCreationService.SetSeletedTab(1); // to navigate to SOW page
      },
      reject: () => {
        // action to be performed when there is no SOW
        if (editMode === 0) {
          // user wants to create project with no sow and CBR
          this.dialogMsg = "Do you want to create client billing roles?";
          this.displayCBR = true;
        }
      }
    });
  }
  NavigateCBR(isAccepted: number) {
    if (isAccepted == 1) {
      this.displayCBR = false;
      this.projectCreationService.SetSeletedTab(2);
    }
  }
  CancelCBR() {
    this.displayCBR = false;
    this.projectCreationService.SetSeletedTab(3);
  }

  GetDepartments(): void {
    this.masterService.GetDepartments().subscribe((res: Department[]) => {
      res.forEach((element: Department) => {
        this.departmentsList.push({
          label: element.DepartmentCode,
          value: element.DepartmentId
        });
      });
      let index = this.statusList.findIndex((x: any) => x.label === "Delivery");
      this.addProject.patchValue({
        DepartmentId: index
      });
      if (this.projectId > 0) {
        //  for update functionality
        this.GetProjectByID(this.projectId); // we call this method once we got all dropdowns data
      }
    });
  }

  canEnableProjectState() {
    let exlabel = this.statusList.find(
      (x: any) => x.value == this.updatedProjectData.ProjectStateId
    );
    if (exlabel == undefined) this.stateDisable = true;
    else if (
      exlabel.label == "Execution" ||
      exlabel.label == "Halted" ||
      exlabel.label == "Closed"
    )
      this.stateDisable = false;
  }
  // makeEndDateMandatory(): void {
  //   this.isEndDateRequired = true;
  //   this.addProject.controls["ActualEndDate"].setValidators([Validators.required]);
  //   this.addProject.controls["ActualEndDate"].updateValueAndValidity();
  // }
  GetProjectTypes(): void {
    // to get all the project types from DB
    this.projectTypes.push({ label: "", value: null });
    this.masterService.GetProjectTypes().subscribe((res: ProjectTypeData[]) => {
      res.forEach(e => {
        this.projectTypes.push({
          label: e.ProjectTypeCode,
          value: e.ProjectTypeId
        });
      });
    });
  }

  GetClients(): void {
    // to get all available clients from DB
    this.clientsList.push({ label: "", value: null });
    this.masterService.GetClientList().subscribe((res: Client[]) => {
      res.forEach(e => {
        this.clientsList.push({ label: e.ClientName, value: e.ClientId });
      });
    });
  }

  GetProgramManagers(): void {
    // to get all available Program managers from DB
    if (this.UserRole === "Department Head") {
     // this.managersList.push({ label: "Select Program Manager", value: null });
    }
    this.projectCreationService
      .GetProgramManagers(this.UserRole, this.EmpId)
      .subscribe((res: any) => {
        if (this.UserRole === "Department Head") {
          res.forEach(e => {
            this.managersList.push({
              label: e.ManagerName,
              value: e.EmployeeId
            });
          });
        } else {
          this.managersList = [
            { label: res[0].ManagerName, value: res[0].EmployeeId }
          ];
        }
      });
  }

  GetPracticeAreas(): void {
    // to get all available practice areas from DB
    this.practiceAreaList.push({ label: "", value: null });
    this.masterService.GetPractiseAreas().subscribe((res: PracticeArea[]) => {
      res.forEach(e => {
        this.practiceAreaList.push({
          label: e.PracticeAreaCode,
          value: e.PracticeAreaId
        });
      });
    });
  }

  SaveProjectData() {
    this.projectCreationService.SaveProject(this.addProject.value).subscribe(
      (res: number) => {
        this.formSubmitted = false;
        if (res > 0) {
          this.messageService.add({
            severity: "success",
            summary: "Success Message",
            detail: "Successfully created"
          });
          this.GetProjectByID(res);
          this.projectId = res;
          this.projectCreationService.SetProjectId(res);
          // update project id
          this.OpenConfirmationDialog(0, "Do you want to add SOW?");
          this.btnLabel = "Update";
        }
        else {
          this.messageService.add({
            severity: "warn",
            summary: "Warn Message",
            detail: "Unable to save project"
          });
        }
      },
      error => {
        this.messageService.add({
          severity: "error",
          summary: "Error message",
          detail: error.error
        });
      }
    );
  }

  SaveProject(): void {
    // method to save/ update a project
    this.addProject.value.ActualStartDate =
      this.addProject.value.ActualStartDate != null
        ? moment(this.addProject.value.ActualStartDate).format("YYYY-MM-DD")
        : null;
    this.addProject.value.ActualEndDate =
      this.addProject.value.ActualEndDate != null
        ? moment(this.addProject.value.ActualEndDate).format("YYYY-MM-DD")
        : null;

    this.addProject.value.DepartmentId = 1; // set department to delivery (disabled field not setting its value so set it manually)
    if (this.UserRole != "Department Head")
      this.addProject.value.ManagerId = this.EmpId; // set mangerId if he is not department head
    this.formSubmitted = true; // variable for validation purpose
    if (this.addProject.valid === true) {
      // Check for all fields are validity

      if (
        this.addProject.value.ActualStartDate &&
        this.addProject.value.ActualEndDate
      ) {
        if (!this.IsValidDate(
          this.addProject.value.ActualStartDate,
          this.addProject.value.ActualEndDate
        )
        ) {
          this.messageService.add({
            severity: "warn",
            summary: "Error in dates",
            detail: "End date can not be less than Start date"
          });
          return;
        }
      }
      if (this.projectId <= 0) {
        //  projectId ==0 if it is new project so call save method
        this.SaveProjectData();
      } else {
        // projectId > 0 so we are updating existing project call update mehtod     
          this.AssignUpdatedFormData();
          this.UpdateProject();
      
      }
    }
  }
 
  UpdateProject() {
    this.projectCreationService
      .updateProjectDetails(this.updatedProjectData)
      .subscribe(
        res => {
          this.formSubmitted = false;
          if (res == 1) {
            this.projectCreationService.SetProjectId(
              this.updatedProjectData.ProjectId
            ); // update project id
            this.messageService.add({
              severity: "success",
              summary: "Success Message",
              detail: "Successfully Updated"
            });
          } else if (res == 2627) {
            this.messageService.add({
              severity: "warn",
              summary: "Warn Message",
              detail: "Entered Duplicate Data"
            });
          } else if (res == -2) {
            this.messageService.add({
              severity: "warn",
              summary: "Warn Message",
              detail: "Either associates are allocated to this project or Requisition is raised against this project."
            });
          }
          else {
            this.messageService.add({
              severity: "error",
              summary: "Error Message",
              detail: "Unable to Update Project"
            });
          }
        },
        error => {
          this.messageService.add({
            severity: "error",
            summary: "Error message",
            detail:
              error.error
          });
        }
      );
  }

  AssignUpdatedFormData(): void {
    // here we are assigning user updated details to an object and will send that object to update method
    this.updatedProjectData.ProjectId = this.projectId;
    this.updatedProjectData.ProjectCode = this.addProject.value.ProjectCode;
    this.updatedProjectData.ProjectName = this.addProject.value.ProjectName;
    this.updatedProjectData.ProjectTypeId = this.addProject.value.ProjectTypeId;
    this.updatedProjectData.ProjectStateId = this.addProject.value.ProjectStateId;
    this.updatedProjectData.ClientId = this.addProject.value.ClientId;
    this.updatedProjectData.ManagerId = this.addProject.value.ManagerId;
    this.updatedProjectData.PracticeAreaId = this.addProject.value.PracticeAreaId;
    this.updatedProjectData.ActualStartDate = this.addProject.value.ActualStartDate;
    this.updatedProjectData.ActualEndDate = this.addProject.value.ActualEndDate;
    this.updatedProjectData.DepartmentId = this.addProject.value.DepartmentId;
    this.updatedProjectData.UserRole = this.UserRole;
    this.updatedProjectData.DomainId = this.addProject.value.DomainId;
    this.updatedProjectData.DomainName = this.addProject.value.DomainName;
    this.updatedProjectData.ClientName = this.addProject.value.ClientName;
    this.updatedProjectData.PracticeAreaCode = this.addProject.value.PracticeAreaCode;
    this.updatedProjectData.ProjectTypeDescription = this.addProject.value.ProjectTypeDescription;
    
  }

  //method to check date validation
  IsValidDate(fromDate: any, toDate: any): boolean {
    if (Date.parse(fromDate) <= Date.parse(toDate)) {
      return true;
    }
    return false;
  }
  // method to reset form, labels
  Reset() {
    this.btnLabel = "Save";
    this.CreateForm();
    this.isEndDateRequired = false;
    this.formSubmitted = false;
  }

  ClearForm() {
    this.projectCreationService.SetProjectId(0);
    this.projectCreationService.SetSeletedTab(0); // to clear
    this.Reset();
  }

  OpenConfirmation() {
    // method to open dialog
    this.confirmationService.confirm({
      message: "Do you want to clear ?",
      accept: () => {
        this.ClearForm();
      },
      reject: () => { }
    });
  }

  // method to get empId and user role from session
  SetUserRoleName(): void {
    if (sessionStorage.getItem("AssociatePortal_UserInformation") != null) {
      const currentRole = JSON.parse(
        sessionStorage.getItem("AssociatePortal_UserInformation")
      ).roleName;
      this.UserRole = currentRole;
      this.EmpId = JSON.parse(
        sessionStorage.getItem("AssociatePortal_UserInformation")
      ).employeeId;
      if (currentRole === "Department Head") {
        this.showDropdown = true;
        this.addProject.controls["ManagerId"].setValidators([
          Validators.required
        ]);
      } else {
        this.showDropdown = false;
      }
    }
  }

  // modify date format to display in p-calender control...(from back end we are gettng date as string so convert it to date)
  ModifyDateFormat(date: string): Date {
    if (date != null) {
      return new Date(moment(date).format("MM/DD/YYYY"));
    }
    return null;
  }
  // method to populate form with existing project details
  PopulateForm(): void {
    if (this.updatedProjectData != null) {
      this.addProject.patchValue({
        ProjectCode: this.updatedProjectData.ProjectCode,
        ProjectName: this.updatedProjectData.ProjectName,
        ClientId: this.updatedProjectData.ClientId,
        ManagerId: this.updatedProjectData.ManagerId,
        ProjectTypeId: this.updatedProjectData.ProjectTypeId,
        ProjectStateId: this.updatedProjectData.ProjectStateId,
        PracticeAreaId: this.updatedProjectData.PracticeAreaId,
        ActualStartDate: this.ModifyDateFormat(
          this.updatedProjectData.ActualStartDate
        ),
        ActualEndDate: this.ModifyDateFormat(
          this.updatedProjectData.ActualEndDate
        ),
        DomainId: this.updatedProjectData.DomainId,
        ProgramManagerName: this.updatedProjectData.ManagerName
      });
    }
  }

  // CanDisableDomain(): Boolean {
  //   if (this.updatedProjectData != null && this.updatedProjectData.ProjectState == 'Drafted') {
  //     return false;
  //   }
  //   return true;
  // }
  //when user clicks on edit we get that projectId so here we are getting project details by thta id..
  GetProjectByID(currentProjectID: number): void {
    this.projectCreationService
      .GetProjectDetailsbyID(currentProjectID)
      .subscribe(
        (projectdata: ProjectsData) => {
          this.updatedProjectData = projectdata;
          this.projectCreationService.SetProjectState(this.updatedProjectData.ProjectState);
          if (this.updatedProjectData.ProjectState == "SubmittedForApproval") {
            this.hideProjectState = true;
          }
          this.PopulateForm();
        },
        error => {
          this.messageService.add({
            severity: "error",
            summary: "Error message",
            detail: "Error while getting the project"
          });
        }
      );
  }
  ngOnDestroy() {
    this.projectIdSubscription.unsubscribe();
  }
}
