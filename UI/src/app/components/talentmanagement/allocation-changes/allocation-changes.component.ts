import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, Validators, FormGroup } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { SelectItem } from 'primeng/components/common/api';
import { Message, ConfirmationService } from 'primeng/components/common/api';
import { PercentageDropDown, AssociateAllocation } from '../models/associateallocation.model';
import * as moment from 'moment';
import { AssociateAllocationService } from '../services/associate-allocation.service';
import { DropDownType, GenericType } from '../../../models/dropdowntype.model';
import { ProjectDetails } from '../models/projects.model';
import { CommonService } from '../../../services/common.service';
import { ClientBillingRole, InternalBillingRole } from '../models/associateallocation.model'
import { InternalClient } from "../../shared/utility/enums";
import { MasterDataService } from '../../../services/masterdata.service';
import { TemporaryAllocationReleaseService } from '../services/temporary-allocation-release.service';
import { MessageService } from 'primeng/components/common/messageservice';
import { ProjectCreationService } from '../../projectLifeCycle/services/project-creation.service';
import { ResourceReleaseService } from '../services/resourcerelease.service';
import { Dropdown } from 'primeng/primeng';
import { AssignManagerToProjectService } from '../../projectLifeCycle/services/assign-manager-to-project.service';
// import { AssociateAllocationChanges } from 'src/app/models/associateallocation.model';

@Component({
  selector: 'app-allocation-changes',
  templateUrl: './allocation-changes.component.html',
  styleUrls: ['./allocation-changes.component.scss'],
  providers: [AssociateAllocationService, CommonService, ConfirmationService
    , MasterDataService, TemporaryAllocationReleaseService, MessageService, ProjectCreationService, ResourceReleaseService]
})
export class AllocationChangesComponent implements OnInit {
  formsubmitted: boolean = false;
  clientRolesList: DropDownType[] = [];
  internalRolesList: DropDownType[] = [];
  ManagersList: DropDownType[] = [];
  employeesList: DropDownType[] = [];
  trRolesList: DropDownType[] = [];
  showCBRdiv: boolean = false;
  requisitionList: AssociateAllocation;
  componentName: string;
  allocationForm: FormGroup;
  allocationDetailList: AssociateAllocation[];
  allocationHistory: AssociateAllocation[] = [];
  AssociateIsPrimaryAllocation: AssociateAllocation[] = [];
  projectsList: SelectItem[] = [];
  allocationOptions: SelectItem[] = [];
  primaryProjectId: any;
  AllocationOptions: PercentageDropDown[] = [];
  allocatedPercentage: number;
  availableAllocationPercentage: number = 0;
  showAllocationHistoryGrid: boolean = false;
  disableBilling: boolean;
  EmployeeName: string;
  trCode: string;
  projectDetails: ProjectDetails[] = [];
  talentpoolEffectiveDate: string;
  firstDate: Date;
  lastDate: Date;
  invalidDates: boolean = false;
  changingAllocationDetails = new AssociateAllocation();
  clientBillingRoleData: ClientBillingRole[] = [];
  displaySelectProject: boolean = false;
  displayAllCBR: boolean = false;
  clientBillingStartDate: string;
  talentpoolAvailablePercentage: number = 0;
  projectAvailablePercentage: number = 0;
  CBRselectedData: ClientBillingRole = new ClientBillingRole();
  changeAllocationDetails: boolean = false;
  isEligiblePrimary: boolean = false;
  isPrimaryProject: boolean = false;
  remainingProjects: AssociateAllocation[] = [];
  remainingProjectsDropdown: any[] = [];
  CBRcols = [
    { field: "ClientBillingRoleName", header: "Client Billing Role Name" },
    { field: "ClientBillingPercentage", header: "Billing %" },
    { field: "NoOfPositions", header: "Total position(s)" },
    { field: "StartDate", header: "Billing Start Date" },
    { field: "AllocationCount", header: "Allocated Position(s)" }
  ]

  constructor(
    private _formBuilder: FormBuilder,
    private masterDataService: MasterDataService,
    private _service: AssociateAllocationService,
    private _commonService: CommonService,
    private actRoute: ActivatedRoute,
    private confirmationService: ConfirmationService,
    private messageService: MessageService,
    private _tempAllocationRelease: TemporaryAllocationReleaseService,
    private projectService: ProjectCreationService,
    private releaseService: ResourceReleaseService,
    private _assignManagerservice: AssignManagerToProjectService

  ) {
    this.requisitionList = new AssociateAllocation();
  }

  ngOnInit() {
    let userRole = JSON.parse(sessionStorage["AssociatePortal_UserInformation"]).roleName;
    let employeeId = JSON.parse(sessionStorage["AssociatePortal_UserInformation"]).employeeId;
    this.allocationForm = this._formBuilder.group({
      'RoleMasterID': [null, [Validators.required]],
      'ProjectId': [null, [Validators.required]],
      'AssociateAllocationId': [null],
      'Client': [null, [Validators.required]],
      'EmployeeId': [null, [Validators.required]],
      'AllocationPercentage': [null, [Validators.required]],
      'internalBillingRole': [null],
      'ClientBillingRoleName': [null],
      'InternalBillingPercentage': [''],
      'ClientBillingRoleId': [null],
      'ClientBillingPercentage': [null],
      'BillingStartDate': [''],
      'ReportingManagerId': [null, [Validators.required]],
      'EffectiveDate': ['', [Validators.required]],
      'NewEffectiveDate': [''],
      'MakePrimaryProjectId': [null],
      'Billable': '',
      'isCritical': '',
      'IsPrimary': '',
      'NotifyAll': ''
    });
    this.getExecutionProjectsList(userRole, employeeId);
    this.getRoles();
    this.getAllocationPercentages();
    this.getDates();
    this.employeesList = [];
    this.employeesList.push({ label: '', value: null });
    this.remainingProjectsDropdown.push({ label: '', value: null });

  }


  private getExecutionProjectsList(userRole: string, employeeId: number): void {
    this._service.GetExecutionProjectsList(userRole, employeeId).subscribe((projectResponse: ProjectDetails[]) => {
      this.projectDetails = projectResponse;
      this.projectsList = [];
      this.projectsList.push({ label: '', value: null });
      this.ManagersList = [];
      this.ManagersList.push({ label: '', value: null });
      this.clientRolesList = [];
      this.clientRolesList.push({ label: '', value: null });
      projectResponse.forEach((projectResponse: ProjectDetails) => {
        if (projectResponse.ProjectName.indexOf("Talent Pool") == -1 && projectResponse.ProjectName.indexOf("Training") == -1)
          this.projectsList.push({ label: projectResponse.ProjectName, value: projectResponse.ProjectId });
      });
    }),
      (error: any) => {
        // if (error._body != undefined && error._body != "")
        //   this._commonService.LogError(this.componentName, error._body).subscribe((data: any) => {
        //   });
        this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Failed to get Project List' });
      };
  }

  getAssociatesByProject(projectId: number) {
    this.allocationForm.controls["AllocationPercentage"].reset();
    this.allocationForm.controls["RoleMasterID"].reset();
    this.allocationForm.controls["EffectiveDate"].reset();
    this.allocationForm.controls["ReportingManagerId"].reset();
    this.allocationForm.controls["IsPrimary"].reset();
    this.allocationForm.controls["Billable"].reset();
    this.allocationForm.controls["isCritical"].reset();
    this.allocationForm.controls["ClientBillingRoleId"].reset();
    this.allocationForm.controls["internalBillingRole"].reset();
    this.allocationForm.controls["InternalBillingPercentage"].reset();
    this.allocationForm.controls["ClientBillingPercentage"].reset();
    this.allocationForm.controls["NotifyAll"].reset();
    this.allocationForm.controls["BillingStartDate"].reset();
    this.showAllocationHistoryGrid = false;
    this.employeesList = [];
    this.releaseService.GetEmployeesByProjectID(projectId).subscribe((employees: any[]) => {
      this.employeesList.push({ label: '', value: null });
      employees.forEach((employee: any) => {
        this.employeesList.push({ label: employee.Name, value: employee.Id });
      });
    }),
      (error: any) => {
        // if (error._body != undefined && error._body != "")
        //   this._commonService.LogError(this.componentName, error._body).subscribe((data: any) => {
        //   });
        this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Failed to get Employees List' });
      };
  }

  private getAllocationPercentages(): void {
    this.masterDataService.GetAllocationPercentages().subscribe((res: PercentageDropDown[]) => {
      this.AllocationOptions = res;
      this.allocationOptions = [];
      this.allocationOptions.push({ label: '', value: null });
      res.forEach((element: PercentageDropDown) => {
        this.allocationOptions.push({ label: JSON.stringify(element.Percentage), value: element.AllocationPercentageId });
      });
    },
      (error) => {
        this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Failed to Get Allocation Percentages!' });
      }
    );
  }

  validateAllocationPercentage(allocatedpercentageId: number, employeeId: number): void {
    this.getEmployeePrimaryAllocationProject(employeeId);

    let releaseList = this.AllocationOptions.filter((percentage: PercentageDropDown) => {
      return percentage.AllocationPercentageId == allocatedpercentageId;
    });
    if (releaseList && releaseList.length > 0) {
      this.allocatedPercentage = releaseList[0].Percentage;
    }
  }

  private getEmployeePrimaryAllocationProject(employeeId: number): void {

    this._service.GetEmployeePrimaryAllocationProject(employeeId).subscribe((response: AssociateAllocation[]) => {
      this.AssociateIsPrimaryAllocation = []
      this.AssociateIsPrimaryAllocation = response;
      if (this.AssociateIsPrimaryAllocation[0].ProjectId == this.allocationForm.value.ProjectId)
        this.isPrimaryProject = true;
    });

  }

  private ValidateIsPrimary(requisitionList: AssociateAllocation): boolean {
    if (requisitionList.IsPrimary == true && (requisitionList.ProjectId != this.AssociateIsPrimaryAllocation[0].ProjectId) && (this.remainingProjects.length >= 1)) {
      this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Please Uncheck Primary' });
      return false;

    }
    if ((requisitionList.IsPrimary == false || requisitionList.IsPrimary == null) && (requisitionList.ProjectId == this.AssociateIsPrimaryAllocation[0].ProjectId) && this.remainingProjects.length == 0) {
      this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Please check Primary' });
      return false;
    }
    else if ((requisitionList.IsPrimary == false || requisitionList.IsPrimary == null) && (requisitionList.ProjectId == this.AssociateIsPrimaryAllocation[0].ProjectId) && this.remainingProjects.length == 1) {
      //If there is one remamining project, then make that project as primary
      requisitionList.IsPrimary = false;
      requisitionList.MakePrimaryProjectId = this.remainingProjects[0].ProjectId;
      return true;
    }
    else {
      return true;
    }
  }

  public getRoles() {
    let departmentId: number = 1;
    this.masterDataService.GetRolesByDepartmentId(departmentId).subscribe((roledata: GenericType[]) => {
      this.trRolesList = [];
      this.trRolesList.push({ label: '', value: null });
      //this.trRolesList.push({ label: "Developer", value: 162 });
      roledata.forEach((e: GenericType) => {
        this.trRolesList.push({ label: e.Name, value: e.Id })
      });
    }, (error: any) => {
      // if (error._body != undefined && error._body != "")
      //   this._commonService.LogError(this.componentName, error._body).subscribe((data: any) => {
      //   });
      this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Failed to get roles!' });
    });

  }

  private CheckDates = function (fromDate: any, toDate: any): boolean {
    if ((fromDate != null || fromDate != undefined) && (toDate != null && toDate != undefined)) {
      if (Date.parse(fromDate) < Date.parse(toDate)) {
        this.invalidDates = false;
        return true
      }
      else {
        this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: 'Invalid Effective date, please check associate`s allocations.' });
        this.invalidDates = true;
        return false
      }

    }
  }


  public getAllocationHistory(employeeId: number) {
    this.allocationForm.controls["AllocationPercentage"].reset();
    this.allocationForm.controls["RoleMasterID"].reset();
    this.allocationForm.controls["EffectiveDate"].reset();
    this.allocationForm.controls["ReportingManagerId"].reset();
    this.allocationForm.controls["IsPrimary"].reset();
    this.allocationForm.controls["Billable"].reset();
    this.allocationForm.controls["isCritical"].reset();
    this.allocationForm.controls["ClientBillingRoleId"].reset();
    this.allocationForm.controls["internalBillingRole"].reset();
    this.allocationForm.controls["InternalBillingPercentage"].reset();
    this.allocationForm.controls["ClientBillingPercentage"].reset();
    this.allocationForm.controls["NotifyAll"].reset();
    this.allocationForm.controls["BillingStartDate"].reset();
    this.remainingProjects = [];


    if (employeeId > 0) {
      this._service.GetEmpAllocationHistory(employeeId).subscribe((data: AssociateAllocation[]) => {

        if (data != null && data.length > 0) {
          this.allocationHistory = data;
          this.showAllocationHistoryGrid = true;
          this.requisitionList.AssociateName = data[0].AssociateName;
          this.allocationHistory.forEach(ele => {
            if (ele.Project.indexOf("Talent Pool") != -1) {
              this.talentpoolAvailablePercentage = ele.AllocationPercentage;
              this.talentpoolEffectiveDate = moment(ele.NewEffectiveDate).format('YYYY-MM-DD');
            }
          }
          )
          this.remainingProjects = [];
          this.loadRemainingProjects(this.allocationForm.value.ProjectId);
        }
        else
          this.showAllocationHistoryGrid = false;
      });
    }
  }

  loadRemainingProjects(projectId) {
    this.remainingProjects = this.allocationHistory.filter(x => (x.ProjectId != projectId) && x.Project.indexOf("Talent Pool") == -1);
    this.remainingProjectsDropdown = [];
    this.remainingProjectsDropdown.push({ label: '', value: null });
    this.remainingProjects.forEach(e => {
      this.remainingProjectsDropdown.push({ label: e.Project, value: e.ProjectId });
    });

    return this.remainingProjects;
  }

  public getProjectManagerDetails(projectId: number) {
    this.allocationForm.value.Client = "";
    this.allocationForm.controls["ClientBillingRoleId"].reset();
    this.allocationForm.controls["ClientBillingRoleName"].reset();
    this.allocationForm.controls["ClientBillingPercentage"].reset();
    this.allocationForm.controls["BillingStartDate"].reset();
    if (projectId > 0) {
      let projectData: any = this.projectDetails.filter((project: ProjectDetails) => { return project.ProjectId == projectId });
      this.allocationForm.value.Client = projectData[0].ClientName;
      this.allocationForm.controls['Client'].setValue(projectData[0].ClientName);
      this.requisitionList.ProjectName = projectData[0].ProjectName;
      if (this.allocationForm.value.Client.indexOf(InternalClient[InternalClient.SenecaGlobal]) != -1)
        this.disableBilling = true;
      else
        this.disableBilling = false;
      this.GetReportingManagers(projectId);
    }
  }

  public GetReportingManagers(projectId: number) {
    this.ManagersList = [];
    this.ManagersList.push({ label: '', value: null });
    if (projectId > 0) {
      this._assignManagerservice.GetManagersByProjectId(projectId).subscribe((data: any) => {

        if (data.ReportingManagerId != null)
          this.ManagersList.push({ label: data.ReportingManagerName, value: data.ReportingManagerId });

        if (data.LeadId != null && data.ReportingManagerId != data.LeadId)
          this.ManagersList.push({ label: data.LeadName, value: data.LeadId });

        if (data.ProgramManagerId != null && data.ProgramManagerId != data.LeadId && data.ProgramManagerId != data.ReportingManagerId)
          this.ManagersList.push({ label: data.ProgramManagerName, value: data.ProgramManagerId });

      }, (error: any) => {
        // if (error._body != undefined && error._body != "")
        //   this._commonService.LogError(this.componentName, error._body).subscribe((data: any) => {
        //   });
        this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Failed to get reporting Managers!' });
      });
    }
  }




  private getDates(): void {
    var date = new Date(), y = date.getFullYear(), m = date.getMonth();
    this.firstDate = new Date(y, m, 1);
    this.lastDate = new Date();
  }

  public IsBillable(billable: any) {
    // let billable = event.target.checked;
    if (billable) {
      // this.requisitionList.isCritical = true;
      // this.allocationForm.patchValue({ isCritical: true });
      this.showCBRdiv = true;
    }
    else {
      // this.requisitionList.isCritical = false;
      // this.allocationForm.patchValue({ isCritical: false });
      this.showCBRdiv = false;
      this.allocationForm.controls["ClientBillingRoleId"].reset();
      this.allocationForm.controls["ClientBillingRoleName"].reset();
      // this.allocationForm.controls["EffectiveDate"].reset();
      this.allocationForm.controls["ClientBillingPercentage"].reset();
      this.allocationForm.controls["BillingStartDate"].reset();
      this.allocationForm.controls["isCritical"].enable();
    }
  }

  public getClientBillingRolesByProject(projectId: number) {

    this.clientRolesList = [];
    this.clientRolesList.push({ label: '', value: null });
    if (projectId > 0) {
      this._service.GetClientBillingRolesByProjectId(projectId).subscribe((clientRole: ClientBillingRole[]) => {
        this.clientBillingRoleData = [];
        if (clientRole.length > 0) {
          // this.clientBillingRoleData = clientRole;
          this.clientBillingRoleData = clientRole.filter(x => x.IsActive == true);
          this.clientBillingRoleData.forEach(
            (d: ClientBillingRole) => {
              d.AvailablePositions = d.NoOfPositions - d.AllocationCount;
              d.StartDate =
                d.StartDate != null
                  ? moment(d.StartDate).format("YYYY-MM-DD")
                  : null;
            }
          );
        }

      }, (error: any) => {
        // if (error._body != undefined && error._body != "")
        //   this._commonService.LogError(this.componentName, error._body).subscribe((data: any) => {
        //   });
        this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Failed to get client billing roles!' });
      });
    }
  }

  selectClientBillingRoles() {
    if (!this.allocationForm.value.ProjectId)
      this.displaySelectProject = true;
    else
      this.displayAllCBR = true;
  }

  selectedCBR() {
    this.allocationForm.controls['ClientBillingRoleId'].setValue(this.CBRselectedData.ClientBillingRoleId);
    this.allocationForm.controls['ClientBillingRoleName'].setValue(this.CBRselectedData.ClientBillingRoleName);
    this.allocationForm.controls['ClientBillingPercentage'].setValue(this.CBRselectedData.ClientBillingPercentage);
    // this.allocationForm.controls['EffectiveDate'].setValue(this.CBRselectedData.StartDate);
    this.clientBillingStartDate = this.CBRselectedData.StartDate;
    this.allocationForm.controls['BillingStartDate'].setValue(this.CBRselectedData.StartDate);
    this.displayAllCBR = false;
  }

  close() {
    this.displaySelectProject = false;
  }

  public Cancel() {
    this.allocationForm.reset();
    // this.allocationForm.controls["isCritical"].enable();
    this.formsubmitted = false;
    this.showAllocationHistoryGrid = false;
    this.allocationHistory = [];
    this.availableAllocationPercentage = 0;
    this.talentpoolEffectiveDate = null;
    this.requisitionList = new AssociateAllocation();
    this.clientBillingRoleData = [];
    this.employeesList = [];
    this.changeAllocationDetails = false;
    this.remainingProjects = [];
    this.isPrimaryProject = false;
    this.isEligiblePrimary = false;
    this.remainingProjectsDropdown = [];
  }

  OpenConfirmationDialog() {   // method to open dialog
    this.confirmationService.confirm({
      message: 'Do you want to clear ?',
      accept: () => {
        this.Cancel()
      },
      reject: () => {

      }
    });
  }


  editAllocation(allocationId) {
    this.changeAllocationDetails = true;
    this.allocationForm.controls['NewEffectiveDate'].setValidators([
      Validators.required
    ]);
    this._service.GetAllocationDetailsByAllocationId(allocationId).subscribe((allocationDetails: AssociateAllocation[]) => {
      this.changingAllocationDetails = allocationDetails[0];


      this.populateAllocationForm();
    }, (error: any) => {

      this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Failed to get Allocation Details!' });
    });

  }

  populateAllocationForm() {
    this.allocationForm.controls['AllocationPercentage'].setValue(this.changingAllocationDetails.AllocationPercentage);
    this.allocationForm.controls['RoleMasterID'].setValue(this.changingAllocationDetails.RoleMasterId);
    this.allocationForm.controls['ClientBillingRoleId'].setValue(this.changingAllocationDetails.ClientBillingRoleId);
    this.allocationForm.controls['ClientBillingRoleName'].setValue(this.changingAllocationDetails.ClientBillingRoleName);
    this.allocationForm.controls['ClientBillingPercentage'].setValue(this.changingAllocationDetails.ClientBillingPercentage);
    this.allocationForm.controls['ReportingManagerId'].setValue(this.changingAllocationDetails.ReportingManagerId);
    this.allocationForm.controls['Billable'].setValue(this.changingAllocationDetails.IsBillable);
    this.allocationForm.controls['isCritical'].setValue(this.changingAllocationDetails.IsCritical);
    this.allocationForm.controls['IsPrimary'].setValue(this.changingAllocationDetails.IsPrimary);
    this.allocationForm.controls['EffectiveDate'].setValue(moment(this.changingAllocationDetails.EffectiveDate).format('YYYY-MM-DD'));
    if (this.changingAllocationDetails.StartDate != null)
      this.allocationForm.controls['BillingStartDate'].setValue(moment(this.changingAllocationDetails.StartDate).format('YYYY-MM-DD'));
    else
      this.allocationForm.controls['BillingStartDate'].setValue("");
    this.IsBillable(this.allocationForm.value.Billable);
    this.calculateAvailableAllocationPercentage();

  }

  calculateAvailableAllocationPercentage() {
    let releaseList = this.AllocationOptions.filter((percentage: PercentageDropDown) => {
      return percentage.AllocationPercentageId == this.changingAllocationDetails.AllocationPercentage;
    });
    if (releaseList && releaseList.length > 0) {
      this.projectAvailablePercentage = releaseList[0].Percentage;
    }
    this.availableAllocationPercentage = this.talentpoolAvailablePercentage + this.projectAvailablePercentage;
    this.validateAllocationPercentage(this.allocationForm.value.AllocationPercentage, this.allocationForm.value.EmployeeId);
  }

  submit(requisitionList): void {
    requisitionList.AssociateAllocationId = this.changingAllocationDetails.AssociateAllocationId;
    requisitionList.Availability = this.talentpoolAvailablePercentage;
    if (requisitionList.MakePrimaryProjectId) {
      this._service.ResourceAllocate(requisitionList).subscribe((data) => {
        if (data == true) {
          this.ManagersList = [];
          this.ManagersList.push({ label: '', value: null });
          this.messageService.add({ severity: 'success', summary: 'Success Message', detail: 'Allocation changes are done successfully!' });
          this.Cancel();
          this.dialogCancel();
        }
        else {
          this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Failed to change Allocation details!' });
        }
      }, (error) => {
        this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Failed to change allocation details!' });
      });

    }
  }

  changeAllocation(requisitionList: AssociateAllocation) {
    this.formsubmitted = true;
    requisitionList.AssociateAllocationId = this.changingAllocationDetails.AssociateAllocationId;
    this.requisitionList.ClientBillingPercentage = requisitionList.ClientBillingPercentage;
    this.requisitionList.ClientBillingRoleId = requisitionList.ClientBillingRoleId;
    this.requisitionList.EmployeeId = requisitionList.EmployeeId;
    this.requisitionList.ReportingManagerId = requisitionList.ReportingManagerId;
    this.requisitionList.Billable = requisitionList.Billable;
    // this.requisitionList.isCritical = requisitionList.isCritical;
    // requisitionList.MakePrimaryProjectId = this.setPrimaryKey(this.remainingProjects.length);
    if (this.requisitionList.Billable == true) {
      if (!this.requisitionList.ClientBillingPercentage || !this.requisitionList.ClientBillingRoleId)
        return;
    }

    if (this.allocationForm.valid) {
      if (this.allocatedPercentage <= this.availableAllocationPercentage) {
        requisitionList.NewEffectiveDate = moment(requisitionList.NewEffectiveDate).format('YYYY-MM-DD')
        if (requisitionList.isCritical != true) requisitionList.isCritical = false;
        this.requisitionList.TalentRequisitionId = requisitionList.TalentRequisitionId = 1; //Written as it is temporary screen and Talent Management module yet to be implemented.
        let validDates: boolean = false;
        if (this.allocationForm.value.EffectiveDate != null && this.allocationForm.value.EffectiveDate != undefined) {
          validDates = this.CheckDates(this.allocationForm.value.EffectiveDate, requisitionList.NewEffectiveDate);
        }
        else {
          this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: 'Band width is not available!' });
          return;
        }
        if ((requisitionList.Billable != true) && (this.clientBillingStartDate != undefined) &&
          (!this._commonService.IsValidDate(
            this.clientBillingStartDate,
            requisitionList.NewEffectiveDate)
          )
        ) {
          this.messageService.add({
            severity: "warn",
            summary: "Warning message",
            detail: "New Effective Date should be greater than or equal to Client Billing Start date"
          });
          return;
        }

        if (this.requisitionList.EmployeeId != this.requisitionList.ReportingManagerId) {
          requisitionList.Availability = this.talentpoolAvailablePercentage;
          if (this.invalidDates == false) {
            if ((requisitionList.IsPrimary == false || requisitionList.IsPrimary == null) && (requisitionList.ProjectId == this.AssociateIsPrimaryAllocation[0].ProjectId) && this.remainingProjects.length > 1) {
              //If there are multiple primary projects, ask user which project he wants to make primary
              this.isEligiblePrimary = true;
              return;
            }
            var primaryValidation = this.ValidateIsPrimary(requisitionList);
          }
          if (primaryValidation != false && validDates != false) {

            this._service.ResourceAllocate(requisitionList).subscribe((data) => {
              if (data == true) {
                this.ManagersList = [];
                this.ManagersList.push({ label: '', value: null });
                this.messageService.add({ severity: 'success', summary: 'Success Message', detail: 'Allocation changes are done successfully!' });
                this.Cancel();
                this.dialogCancel();
              }

              else {
                this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Failed to change Allocation details!' });
              }
            }, (error) => {

              this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Failed to change allocation details!' });
            });
          }
        }
        else {
          this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: 'Associate and Reporting Manager cannot be the same!' });
        }
      }
      else {
        this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: 'Band width is not available!' });
      }
    }
  }

  dialogCancel() {
    this.isPrimaryProject = false;
    this.isEligiblePrimary = false;
  }
}
