import { Component, OnInit, EventEmitter, Output, Input } from "@angular/core";
import { Router } from "@angular/router";
import { FormGroup, FormControl, Validators, FormBuilder } from "@angular/forms";
import { SelectItem, MessageService } from "primeng/api";
import { MasterDataService } from "../../../services/masterdata.service";
import { ClientBillingRoleService } from "../services/client-billing-roles.service";
import { ProjectCreationService } from "../services/project-creation.service";
import * as moment from "moment";
import { CommonService } from "../../../services/common.service";
import { ConfirmationService } from "primeng/api";
import { ClientBillingRoleDetails } from "../models/client-billing-role.model";
import { PercentageDropDown } from "../../talentmanagement/models/associateallocation.model";
import { ProjectsData } from "../../../models/projects.model";
import { Subscription } from "rxjs";
import { AllocationCount, AllocationDetails, ResourceAllocationDetails } from "../../reports/models/resourcereportbyproject.model";
import * as servicePath from "../../../service-paths";
import { IfStmt } from "../../../../../node_modules/@angular/compiler";
import { ResourceReportService } from "../../reports/services/resource-report.service";
import { BooleanToStringPipe } from "src/app/Pipes/BooleanToStringPipe";


@Component({
  selector: "app-client-billing-roles",
  templateUrl: "./client-billing-roles.component.html",
  styleUrls: ["./client-billing-roles.component.scss"],
  providers: [
    ClientBillingRoleService,
    MasterDataService,
    MessageService,
    CommonService,
    ConfirmationService,
    BooleanToStringPipe
  ]
})
export class ClientBillingRolesComponent implements OnInit {
  projectsList: SelectItem[];
  submitBtnLabel = "Review and Submit";
  currentRole = "";
  clientBillingRolesForm: FormGroup;
  clientBillingRoleData: ClientBillingRoleDetails[];
  selectedBillingRole: ClientBillingRoleDetails;
  formSubmitted: boolean = false;
  projectId: number;
  saveRoleData: boolean = true;
  disableProject: boolean = false;
  positions: number;
  dashboard: string;
  clientBillingRoleOptions: SelectItem[] = [];
  isDrafted: boolean = false;
  projectStateSubscription: Subscription;
  editModeSubscription: Subscription;
  projectIdSubscription: Subscription;
  cancelBtnLabel = "Clear";
  editMode: boolean;
  projectState: string;
  allocationCount: number;
  PageSize: number = 0;
  PageDropDown: number[] = [];
  maxDate = new Date();
  minDate = new Date();
  projectStartDate: string;
  resources = servicePath.API.PagingConfigValue;
  displayDialog: boolean = false;
  // regularExpression: RegExp = /^[a-zA-Z]+(\s[a-zA-Z]+)*$/;
  clientBillingRole: ClientBillingRoleDetails;
  displayCloseCBRDialog: boolean = false;
  closableRole = new ClientBillingRoleDetails();
  closeRoleData: FormGroup;
  submitted = false;
  displayCloseRoleErrorDialog = false;
  billableResourceAllocationList: ResourceAllocationDetails[] = [];
  showAllocatedPositionsList: boolean = false;
  disablestyles = { 'opacity': '0.6', 'pointer-events': 'none'}
  // disablestyles = { 'opacity': '0.6', 'cursor': 'not-allowed' }
  cols = [
    { field: "ClientBillingRoleName", header: "Role Name" },
    { field: "NoOfPositions", header: "Total Position(s)" },
    { field: "AllocationCount", header: "Allocated Position(s)" },
    { field: "AvailablePositions", header: "Open Position(s)" },
    { field: "StartDate", header: "Billing Start Date" },
    { field: "EndDate", header: "Billing End Date" },
    { field: "ClientBillingPercentage", header: "Billing %" }
  ];

  cols1 = [
    { field: "AssociateCode", header: "Associate Code" },
    { field: "AssociateName", header: "Associate Name" },
    { field: "ClientBillingRoleName", header: "Role Name" },
    { field: "AllocationPercentage", header: "Allocation %" },
    { field: "IsPrimaryProject", header: "Primary", type: this.yesNoPipe },
    { field: "IsCriticalResource", header: "Critical", type: this.yesNoPipe }
  ];

  constructor(
    private  fb: FormBuilder,
    private clientBillingRoleService: ClientBillingRoleService,
    private commonService: CommonService,
    private masterDataService: MasterDataService,
    private projectCreationService: ProjectCreationService,
    private messageService: MessageService,
    private confirmationService: ConfirmationService,
    private router: Router,
    private _resourceReportsService: ResourceReportService,
    private yesNoPipe: BooleanToStringPipe,
  ) {
    this.PageSize = this.resources.PageSize;
    this.PageDropDown = this.resources.PageDropDown;
  }

  canCloseRole() {
    if (this.projectState == 'Drafted' || this.projectState == 'SubmittedForApproval')
      return false;
    return true;
  }
  ngOnInit() {
    this.createCloseRoleform();
    this.editModeSubscription = this.projectCreationService.GetEditMode().subscribe(data => {
      this.editMode = data;
    });
    this.projectStateSubscription = this.projectCreationService.GetProjectState().subscribe(data => {
      this.projectState = data;
    });
    this.currentRole = JSON.parse(sessionStorage["AssociatePortal_UserInformation"])
      .roleName;

    this.initiateForm();
    this.projectIdSubscription = this.projectCreationService.GetProjectId().subscribe((data: any) => {
      this.projectId = data;
      if (this.projectId > 0) {
        this.getClientBillingRolesByProjectId(this.projectId);
        this.getProjectsList();
      }
    });
    if (this.currentRole !== 'Department Head')
      this.submitBtnLabel = "Review and Submit";
    else {
      if (this.projectState == "SubmittedForApproval")
        this.submitBtnLabel = "Review and Approve";
      else
        this.submitBtnLabel = "Review and Save";

    }
    this.getProjectsList();
    this.getClientBillingRolePercentages();
  }

  CloneData(cBRData: ClientBillingRoleDetails) {
    this.patchDataToForm(cBRData);
  }

  initiateForm() {
    //to initiate the form
    this.clientBillingRolesForm = this.fb.group({
         ProjectId: [null, [Validators.required]],
         ClientBillingRoleName: [null, [this.commonService.unAllowednames_usingCustom, 
           Validators.required,
           Validators.maxLength(60),
           Validators.pattern("^(?!.*--)^(?!-[0-9])[A-Za-z0-9-.()&/, ]+$") ]],
         NoOfPositions: [, [Validators.required]],
         StartDate: [null, [Validators.required]],
         EndDate: [null],
         ClientBillingPercentage: [4, [Validators.required]]                  
       })
  }

  
  CompleteProjectCreation() {
    //to goto submit tab.
    if (
      this.formSubmitted == false &&
      this.clientBillingRolesForm.touched == true
    ) {
      this.confirmationService.confirm({
        message: "You have unsaved changes do you want to submit project?",
        accept: () => {
          this.projectCreationService.SetSeletedTab(3);
        },
        reject: () => { }
      });
    } else this.projectCreationService.SetSeletedTab(3); // project is successfully created and all observables need to be cleaned.
  }

  getProjectsList(): void {
    //to get list of all projects
    let userRole = JSON.parse(sessionStorage["AssociatePortal_UserInformation"])
      .roleName;
    let employeeId = JSON.parse(
      sessionStorage["AssociatePortal_UserInformation"]
    ).employeeId;
    this.projectsList = [];
    //this.projectsList.push({ label: "Select Project ", value: null });
    if (userRole != "" && employeeId > 0) {
      this.projectCreationService
        .GetProjectsList(userRole, employeeId, this.dashboard)
        .subscribe((res: ProjectsData[]) => {
          if (res.length > 0) {
            res.forEach(e => {
              this.projectsList.push({
                label: e.ProjectName,
                value: e.ProjectId
              });
              if (this.projectId > 0 && e.ProjectId == this.projectId)
                this.projectStartDate = e.ActualStartDate != null ? moment(e.ActualStartDate).format("YYYY-MM-DD") : null;
              if (
                e.ProjectId == this.projectId &&
                e.ProjectState == "Drafted"
              ) {
                this.isDrafted = true;
              }
            });
            if (this.projectId > 0) {
              this.clientBillingRolesForm.patchValue({
                ProjectId: this.projectId
              });
              this.disableProject = true;
            }
          }
        }),
        error => { };
    }
  }


  getClientBillingRolesByProjectId(projectId: number) {
    //To get client billing roles based on project
    if (projectId > 0) {
      this.clientBillingRoleData = new Array<ClientBillingRoleDetails>();
      this.clientBillingRoleService
        .GetClientBillingRolesByProjectId(projectId)
        .subscribe((res: ClientBillingRoleDetails[]) => {
          if (res.length > 0) {
            this.clientBillingRoleData = res;
            this.clientBillingRoleData.forEach(
              (d: ClientBillingRoleDetails) => {
                if (d.IsActive == true)
                  d.AvailablePositions = d.NoOfPositions - d.AllocationCount;
                else
                  d.AvailablePositions = 0;
                d.StartDate =
                  d.StartDate != null
                    ? moment(d.StartDate).format("YYYY-MM-DD")
                    : null;
                d.EndDate =
                  d.EndDate != null
                    ? moment(d.EndDate).format("YYYY-MM-DD")
                    : null;
              }
            );
          }
        }),
        error => { };
    }
  }

  private getClientBillingRolePercentages(): void {
    //to populate client billing role percentage drop-down
    this.masterDataService.GetAllocationPercentages().subscribe(
      (res: PercentageDropDown[]) => {      
        this.clientBillingRoleOptions = [];      
        this.clientBillingRoleOptions.push({
          label: "",
          value: null
        });
        res.forEach((element) => {
          this.clientBillingRoleOptions.push({
            label: JSON.stringify(element.Percentage),
            value: element.AllocationPercentageId
          });
        });       
      },
      error => {
        this.messageService.add({
          severity: "error",
          summary: "Error Message",
          detail: "Failed to Get Client Billing Role Percentages!"
        });
      }
    );
  }

  getResourceReportByProjectId(projectId: number, roleId: number): void {
    this.billableResourceAllocationList = [];
    this.showAllocatedPositionsList = false;
    this._resourceReportsService.GetResourceReportByProjectId(projectId).subscribe((resourceReportResponse: AllocationDetails) => {
        this.billableResourceAllocationList = resourceReportResponse.lstBillableResources;
        if (resourceReportResponse) {
          this.showAllocatedPositionsList = true;
          if (resourceReportResponse.lstBillableResources && resourceReportResponse.lstBillableResources.length > 0) {
            this.billableResourceAllocationList = resourceReportResponse.lstBillableResources.filter(p => p.ClientBillingRoleId === roleId)
          }
        }
        else {
          this.messageService.add({severity: 'info', summary: 'Info', detail: 'No records found'  });
        }
      }),
        (error: any) => {
          this.messageService.add({severity: 'error', summary: 'Error Message', detail:'Failed to get Client Details'  });
        };
  }

  addClientBillingRole() {
    //to add client billing roles
    this.formSubmitted = true;
    if (this.clientBillingRolesForm.valid) {
      this.clientBillingRolesForm.value.StartDate =
        this.clientBillingRolesForm.value.StartDate != null
          ? moment(this.clientBillingRolesForm.value.StartDate).format(
            "YYYY-MM-DD"
          )
          : null;
      this.clientBillingRolesForm.value.EndDate =
        this.clientBillingRolesForm.value.EndDate != null
          ? moment(this.clientBillingRolesForm.value.EndDate).format(
            "YYYY-MM-DD"
          )
          : null;
      if (
        !this.commonService.IsValidDate(
          this.projectStartDate,
          this.clientBillingRolesForm.value.StartDate
        )
      ) {
        this.messageService.add({
          severity: "warn",
          summary: "Warning message",
          detail: "Client billing Start date should be greater than or equal to Project Start date"
        });
        return;
      }
      if (
        this.clientBillingRolesForm.value.StartDate != null &&
        this.clientBillingRolesForm.value.EndDate != null
      ) {
        if (
          !this.commonService.IsValidDate(
            this.clientBillingRolesForm.value.StartDate,
            this.clientBillingRolesForm.value.EndDate
          )
        ) {
          this.messageService.add({
            severity: "error",
            summary: "Error message",
            detail: "Start date should be less than or equal to end date"
          });
          return;
        }
      }
      this.clientBillingRoleService
        .SaveClientBillingRole(this.clientBillingRolesForm.value)
        .subscribe(res => {
          if (res == 1) {
            this.getClientBillingRolesByProjectId(
              this.clientBillingRolesForm.value.ProjectId
            );
            this.reset();
            this.messageService.add({
              severity: "success",
              summary: "Success Message",
              detail: "Client billing role is added successfully"
            });
            this.projectCreationService.SetSeletedTab(2);
          }
          else if (res == -1) {
            this.messageService.add({
              severity: "warn",
              summary: "Warning Message",
              detail: "Client billing role already exist"
            });
          }

        }),
        error => {
          this.messageService.add({
            severity: "error",
            summary: "Error message",
            detail: "Unable to add client billing role"
          });
        };
    } else return;
  }

  isSowPresent() {
    //when sow details are present 
    this.updateClientBillingDetails(this.clientBillingRole);
    this.reset();
    this.displayDialog = false;
    return true;
  }

  editBillingRole(billingRoleData: ClientBillingRoleDetails) {
    //invokes when we click on edit button
    this.saveRoleData = false;
    this.disableProject = true;
    this.cancelBtnLabel = "Cancel";
    this.patchDataToForm(billingRoleData);

  }

  patchDataToForm(billingRoleData: ClientBillingRoleDetails) {
    this.clientBillingRolesForm.patchValue({
      ClientBillingRoleName: billingRoleData.ClientBillingRoleName,
      NoOfPositions: billingRoleData.NoOfPositions,

      ClientBillingPercentage: (billingRoleData.ClientBillingPercentage) / 25,
      StartDate:
        billingRoleData.StartDate != null
          ? moment(billingRoleData.StartDate).format("MM/DD/YYYY")
          : null,
      EndDate:
        billingRoleData.EndDate != null
          ? moment(billingRoleData.EndDate).format("MM/DD/YYYY")
          : null
    });
    if (billingRoleData.ClientBillingPercentage == 0) {
      this.clientBillingRolesForm.patchValue({
        ClientBillingPercentage: 5
      })
    }
    this.selectedBillingRole = billingRoleData;
    this.positions = this.clientBillingRolesForm.value.NoOfPositions;
  }

  updateClientBillingRole() {
    //to update client billing roles
    this.formSubmitted = true;
    if (this.clientBillingRolesForm.valid) {
      this.clientBillingRole = new ClientBillingRoleDetails();
      this.clientBillingRole.ClientBillingRoleId = this.selectedBillingRole.ClientBillingRoleId;
      this.clientBillingRole.ProjectId = this.selectedBillingRole.ProjectId;
      this.clientBillingRole.ClientBillingRoleName = this.clientBillingRolesForm.value.ClientBillingRoleName;
      this.clientBillingRole.NoOfPositions = this.clientBillingRolesForm.value.NoOfPositions;
      this.clientBillingRole.ClientBillingPercentage = this.clientBillingRolesForm.value.ClientBillingPercentage;
      this.clientBillingRole.StartDate =
        this.clientBillingRolesForm.value.StartDate != null
          ? moment(this.clientBillingRolesForm.value.StartDate).format(
            "YYYY-MM-DD"
          )
          : null;
      this.clientBillingRole.EndDate =
        this.clientBillingRolesForm.value.EndDate != null
          ? moment(this.clientBillingRolesForm.value.EndDate).format(
            "YYYY-MM-DD"
          )
          : null;
      if (
        !this.commonService.IsValidDate(
          this.projectStartDate,
          this.clientBillingRole.StartDate
        )
      ) {
        this.messageService.add({
          severity: "warn",
          summary: "Warning message",
          detail: "Client billing Start date should be greater than or equal to Project Start date"
        });
        return;
      }
      if (
        this.clientBillingRole.StartDate != null &&
        this.clientBillingRole.EndDate != null
      ) {
        if (
          !this.commonService.IsValidDate(
            this.clientBillingRole.StartDate,
            this.clientBillingRole.EndDate
          )
        ) {
          this.messageService.add({
            severity: "error",
            summary: "Error message",
            detail: "Start date should be less than or equal to end date"
          });
          return;
        }
      }

      if (this.projectState == "Drafted") {
        this.updateClientBillingDetails(this.clientBillingRole);
      }
      else {
        if (this.clientBillingRolesForm.dirty == true) {
          this.displayDialog = true;
        }
        else {
          this.messageService.add({
            severity: "warn",
            summary: "warn message",
            detail: "Nothing to save"
          });
        }
      };
    }
  }

  updateClientBillingDetails(clientBillingRole) {
    //service call to update client billing role
    this.clientBillingRoleService
      .UpdateClientBillingRole(clientBillingRole)
      .subscribe(res => {
        if (res >= 1) {
          this.messageService.add({
            severity: "success",
            summary: "Success Message",
            detail: "Client billing role is updated successfully"
          });
          this.reset();
          this.getClientBillingRolesByProjectId(
            this.clientBillingRolesForm.value.ProjectId
          );


        } else if (res == 0) {
          this.messageService.add({
            severity: "warn",
            summary: "Warn message",
            detail: "Number of positions cannot be decreased."
          });
        }
        else if (res == -1) {
          this.messageService.add({
            severity: "warn",
            summary: "Warning Message",
            detail: "Client billing role already exist"
          });
        }
        else if (res == -2) {
          this.messageService.add({
            severity: "warn",
            summary: "Warning Message",
            detail: "Cannot decrease Positions"
          });
        }
      }),
      error => {
        this.messageService.add({
          severity: "error",
          summary: "Error message",
          detail: "Unable to Update "
        });
      };
  }

  deleteDialog(rowData) {
    // method to open delete dialog
    this.confirmationService.confirm({
      message: "Do you want to delete Client Billing Role?",
      accept: () => {
        this.deleteBillingRole(rowData);
      },
      reject: () => { }
    });
  }

  deleteBillingRole(rowData) {
    //to delete a client billing role.
    this.clientBillingRoleService
      .DeleteClientBillingRole(rowData.ClientBillingRoleId)
      .subscribe((res: boolean) => {
        if (res == true) {
          this.getClientBillingRolesByProjectId(
            this.clientBillingRolesForm.value.ProjectId
          );
          this.reset();
          this.messageService.add({
            severity: "success",
            summary: "Success message",
            detail: "Deleted Client billing role"
          });
        } else
          this.messageService.add({
            severity: "error",
            summary: "Error message",
            detail: "Deleting failed"
          });
      });
  }

  reset() {
    //reset all the values
    this.formSubmitted = false;
    this.saveRoleData = true;
    this.cancelBtnLabel = "Clear";
    this.clientBillingRolesForm.controls["ClientBillingRoleName"].reset();
    this.clientBillingRolesForm.controls["NoOfPositions"].reset();
    this.clientBillingRolesForm.controls["StartDate"].reset();
    this.clientBillingRolesForm.controls["EndDate"].reset();
    this.clientBillingRolesForm.controls["ClientBillingPercentage"].setValue(4);
  }

  OpenConfirmationDialog() {
    // method to open dialog for cancel
    this.confirmationService.confirm({
      message: 'Do you want to ' + this.cancelBtnLabel + ' ? ',
      accept: () => {
        this.reset()
      },
      reject: () => {

      }
    });
  }
  RollbackDialog() {
    //Invokes when user clicks on cancel button which opens dialog
    this.confirmationService.confirm({
      message: "Do you want to Rollback project details?",
      header: "Confirmation",
      icon: "pi pi-exclamation-triangle",
      accept: () => {
        this.Rollback();
      },
      reject: () => {
        // this.router.navigate(["project/dashboard"]);
      }
    });
  }

  onlyNumbers(event: any) {
    //allows only numbers
    this.commonService.onlyNumbers(event)
  };

  Rollback() {
    //Rollback project details
    this.projectCreationService
      .deleteProjectDetails(this.projectId)
      .subscribe((res: boolean) => {
        if (res == true) {
          this.messageService.add({
            severity: "success",
            summary: "Success message",
            detail: "Rolled back successfully"
          });
          setTimeout(() => {
            this.router.navigate(["project/dashboard"]);
          }, 1000);
        } else
          this.messageService.add({
            severity: "error",
            summary: "Error message",
            detail: "Rollback failed"
          });
      });
  }

  canShowActions(cBRData: ClientBillingRoleDetails) {
    if ((cBRData.IsActive != undefined || cBRData.IsActive != null) && cBRData.IsActive == false) {
      return false;
    }
    return true;
  }

  canEditOrDelete() {
    if (this.projectState == 'Drafted' || this.projectState == 'Created')
      return true;
    if (this.projectState == 'SubmittedForApproval' && this.currentRole == 'Department Head')
      return true;

    return false;
  }

  createCloseRoleform() {
    var date = new Date(), y = date.getFullYear(), m = date.getMonth();
    this.minDate = new Date(y, m - 1, 1); //new Date(this.closableRole.StartDate);
    this.closeRoleData = new FormGroup({
      BillingEndDate: new FormControl(null, [Validators.required]),
    });
  }
  closeCBRData(cBRData: ClientBillingRoleDetails) {
    if (cBRData.AllocationCount > 0) {
      this.displayCloseRoleErrorDialog = true;
      return;
    }
    this.closableRole = cBRData;

    if (this.closableRole.EndDate != null) {
      let date = new Date(moment(this.closableRole.EndDate).format("YYYY-MM-DD"));
      this.closeRoleData.patchValue({
        BillingEndDate: date
      })
    }
    this.displayCloseCBRDialog = true;
  }
  closeDialog() {
    this.displayCloseRoleErrorDialog = false;
  }
  closeClientBillingRecord() {
    this.submitted = true;
    if (this.closeRoleData.value.BillingEndDate != null) {
      this.closeRoleData.value.BillingEndDate = moment(this.closeRoleData.value.BillingEndDate).format("YYYY-MM-DD");
      this.clientBillingRoleService.closeClientBillingRecord(this.closableRole.ClientBillingRoleId, this.closeRoleData.value.BillingEndDate).subscribe(res => {
        if (res > 0) {
          this.clearData();
          this.getClientBillingRolesByProjectId(this.projectId);
          this.messageService.add({
            severity: "success",
            summary: "Success message",
            detail: "Successfully closed Client billing role"
          });

        }
        if (res == -1) {
          this.clearData();
          this.messageService.add({
            severity: "warn",
            summary: "Warn message",
            detail: "There are active allocations you can close it"
          });
        }
      },
        (error) => {
          this.clearData();
          this.messageService.add({
            severity: "error",
            summary: "Closing Failed",
            detail: error.error
          });
        });

    }
  }

  ngOnDestroy() {
    this.projectIdSubscription.unsubscribe();
    this.editModeSubscription.unsubscribe();
    this.projectStateSubscription.unsubscribe();
  }

  clearData() {
    this.closeRoleData.reset();
    this.submitted = false;
    this.displayCloseCBRDialog = false;
  }


  canShowChangeAllocations() {
    if (this.projectState == 'Drafted' || this.projectState == 'SubmittedForApproval')
      return false;
    return true;
  }

  disableEditAndDelete(cBRData: ClientBillingRoleDetails) {
    if ((cBRData.IsActive != undefined || cBRData.IsActive != null) && cBRData.IsActive == false)
      return this.disablestyles;
    return { 'cursor': 'pointer' };

  }

}
