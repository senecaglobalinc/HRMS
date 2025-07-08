import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, Validators, FormGroup } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { SelectItem } from 'primeng/components/common/api';
import { Message, ConfirmationService } from 'primeng/components/common/api';
import { AssociateAllocation, PercentageDropDown, RoleDetails } from '../models/associateallocation.model';
import * as moment from 'moment';
import { AssociateAllocationService } from '../services/associate-allocation.service';
import { DropDownType, GenericType } from '../../../models/dropdowntype.model';
//import { ProjectDetails } from '../models/projects.model';
import { ProjectsData } from "../../../models/projects.model";
import { CommonService } from '../../../services/common.service';
import { ClientBillingRole, InternalBillingRole } from '../models/associateallocation.model'
import { InternalClient } from "../../shared/utility/enums";
import { MasterDataService } from '../../../services/masterdata.service';
import { TemporaryAllocationReleaseService } from '../services/temporary-allocation-release.service';
import { MessageService } from 'primeng/components/common/messageservice';
import { TagAssociateList } from '../../projectLifeCycle/models/tag-associate.model';
import { TagAssociateService } from '../../projectLifeCycle/services/tag-associate.service';
import { AssignManagerToProjectService } from '../../projectLifeCycle/services/assign-manager-to-project.service';

@Component({
  selector: 'app-associateallocation',
  templateUrl: './associateallocation.component.html',
  styleUrls: ['./associateallocation.component.scss'],
  providers: [AssociateAllocationService, CommonService, ConfirmationService
    , MasterDataService, TemporaryAllocationReleaseService, MessageService]
})
export class AssociateallocationComponent implements OnInit {

  formsubmitted: boolean = false;
  clientRolesList: DropDownType[] = [];
  internalRolesList: DropDownType[] = [];
  ManagersList: DropDownType[] = [];
  employeesList: DropDownType[] = [];
  trRolesList: DropDownType[] = [];
  disableCBR: boolean = true;
  requisitionList: AssociateAllocation;
  componentName: string;
  allocationForm: FormGroup;
  allocationDetailList: AssociateAllocation[];
  allocationHistory: AssociateAllocation[] = [];
  AssociateIsPrimaryAllocation: AssociateAllocation[] = [];
  projectsList: SelectItem[] = [];
  allocationOptions: SelectItem[] = [];
  // errorMessage: Message[] = [];
  AllocationOptions: PercentageDropDown[] = [];
  allocatedPercentage: number;
  availableAllocationPercentage: number = 0;
  showAllocationHistoryGrid: boolean = false;
  disableBilling: boolean;
  EmployeeName: string;
  trCode: string;
  clientName: string;
  projectDetails: ProjectsData[] = [];
  talentpoolEffectiveDate: string;
  firstDate: Date;
  lastDate: Date;
  invalidDates: boolean = false;
  disableAssociateName: boolean = false;
  empid: number;
  tagAssociateId: number;
  displayAllCBR: boolean = false;
  displaySelectProject: boolean = false;
  clientBillingRoleData: ClientBillingRole[] = [];
  clientBillingStartDate: string;
  CBRselectedData: ClientBillingRole = new ClientBillingRole();
  CBRcols = [
    { field: "ClientBillingRoleName", header: "Client Billing Role Name" },
    { field: "ClientBillingPercentage", header: "Billing %" },
    { field: "NoOfPositions", header: "Total position(s)" },
    { field: "StartDate", header: "Billing Start Date" },
    { field: "AllocationCount", header: "Allocated Position(s)" }
  ]
  constructor(private _formBuilder: FormBuilder,
    private masterDataService: MasterDataService,
    private _service: AssociateAllocationService,
    private tagAssociateService: TagAssociateService,
    private _commonService: CommonService,
    private actRoute: ActivatedRoute,
    private confirmationService: ConfirmationService,
    private messageService: MessageService,
    private router: Router,
    private _tempAllocationRelease: TemporaryAllocationReleaseService,
    private _assignManagerservice: AssignManagerToProjectService) {
    this.componentName = this.actRoute.routeConfig.component.name;
    this.requisitionList = new AssociateAllocation();
  }

  ngOnInit() {
    this.actRoute.params.subscribe(params => { this.empid = params["empid"]; });
    // this.actRoute.params.subscribe(params => { this.availableallocation = params["availableallocation"]; });
    this.actRoute.params.subscribe(params => { this.tagAssociateId = params["tagid"]; });
    this.allocationForm = this._formBuilder.group({
      'RoleMasterId': [null, [Validators.required]],
      'ProjectId': [null, [Validators.required]],
      'EmployeeId': [null, [Validators.required]],
      'AllocationPercentage': [null, [Validators.required]],
      'internalBillingRole': [null],
      'InternalBillingPercentage': [''],
      'ClientBillingRoleId': [null],
      'ClientBillingRoleName': [null],
      'ClientBillingPercentage': [null, Validators.max(100)],
      'ReportingManagerId': [null, [Validators.required]],
      'EffectiveDate': ['', [Validators.required]],
      'BillingStartDate': [''],
      'Billable': [true],
      'isCritical': [true],
      'IsPrimary': [''],
      'NotifyAll': [true]
    });
    this.getProjectList();
    this.getEmployeesList();
    this.getAllocationPercentages();
    this.getRoles();
    //this.getInternalBillingRoles();
    this.getDates();
  }

  private getDates(): void {
    var date = new Date(), y = date.getFullYear(), m = date.getMonth();
    this.firstDate = new Date(y, m, 1);
    this.lastDate = new Date();
  }

  private getProjectList(): void {
    this.masterDataService.GetProjectsList().subscribe((projectResponse: any[]) => {
      this.projectDetails = projectResponse;
      this.projectsList = [];
      this.projectsList.push({ label: '', value: null });
      this.ManagersList = [];
      this.ManagersList.push({ label: '', value: null });
      this.clientRolesList = [];
      this.clientRolesList.push({ label: '', value: null });
      projectResponse.forEach((pr: ProjectsData) => {
        if (pr.ProjectName.indexOf("Talent Pool") == -1 && pr.ProjectName.indexOf("Training") == -1 &&
          this.projectsList.findIndex(x => x.label == pr.ProjectName) == -1)
          this.projectsList.push({ label: pr.ProjectName, value: pr.ProjectId });
      });
    }),
      (error: any) => {
        // if (error._body != undefined && error._body != "")
        //   this._commonService.LogError(this.componentName, error._body).subscribe((data: any) => {
        //   });
        this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Failed to get Project List' });
      };
  }

  private getEmployeesList(): void {
    this._tempAllocationRelease.GetEmployeesAllocations().subscribe((employees: any[]) => {
      this.employeesList = [];
      this.employeesList.push({ label: '', value: null });
      employees.forEach((employee: any) => {
        if (this.employeesList.findIndex(x => x.label == employee.EmployeeName) == -1)
          this.employeesList.push({ label: employee.EmployeeName, value: employee.EmployeeId });
      });
      if (this.empid > 0) {
        this.disableAssociateName = true;
        this.allocationForm.controls['EmployeeId'].setValue(this.empid);
        // if (this.availableallocation == 0)
        //   this.allocationForm.controls['AllocationPercentage'].setValue(5);
        // else
        //   this.allocationForm.controls['AllocationPercentage'].setValue(this.availableallocation / 25);
        this.getAllocationHistory(this.empid);
        this.validateAllocationPercentage(this.allocationForm.value.AllocationPercentage, this.allocationForm.value.EmployeeId);
      }
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
    if(employeeId == null)
      return;
    this._service.GetEmployeePrimaryAllocationProject(employeeId).subscribe((response: AssociateAllocation[]) => {
      this.AssociateIsPrimaryAllocation = []
      this.AssociateIsPrimaryAllocation = response;
    });
  }

  public getRoles() {
    let departmentId: number = 1;
    this._service.GetRolesByDepartmentId(departmentId).subscribe((roledata: RoleDetails[]) => {
      this.trRolesList = [];
      this.trRolesList.push({ label: '', value: null });
      //this.trRolesList.push({ label: "Developer", value: 162 });
      roledata.forEach((e: RoleDetails) => {
        this.trRolesList.push({ label: e.RoleName, value: e.RoleMasterId })
      });
    }, (error: any) => {
      // if (error._body != undefined && error._body != "")
      //   this._commonService.LogError(this.componentName, error._body).subscribe((data: any) => {
      //   });
      this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Failed to get roles!' });
    });

  }

  public getAllocationHistory(employeeId: number) {
    // this.allocationForm.controls["AllocationPercentage"].reset();
    // this.allocationForm.controls["RoleMasterId"].reset();
    // this.allocationForm.controls["EffectiveDate"].reset();
    // this.allocationForm.controls["ReportingManagerId"].reset();
    // this.allocationForm.controls["ProjectId"].reset();
    // this.allocationForm.controls["IsPrimary"].reset();
    // this.allocationForm.controls["isCritical"].reset();
    // this.allocationForm.controls["ClientBillingRoleId"].reset();
    // this.allocationForm.controls["internalBillingRole"].reset();
    // this.allocationForm.controls["InternalBillingPercentage"].reset();
    // this.allocationForm.controls["ClientBillingPercentage"].reset();
    // this.allocationForm.controls["NotifyAll"].reset();
    // this.clientName = '';

    if (employeeId > 0) {
      this._service.GetEmpAllocationHistory(employeeId).subscribe((data: AssociateAllocation[]) => {
        if (data != null && data.length > 0) {
          this.allocationHistory = data;
          this.showAllocationHistoryGrid = true;
          this.requisitionList.AssociateName = data[0].AssociateName;
          this.allocationHistory.forEach(ele => {
            if (ele.Project.indexOf("Talent Pool") != -1) {
              this.availableAllocationPercentage = ele.AllocationPercentage;
              this.talentpoolEffectiveDate = moment(ele.EffectiveDate).format('YYYY-MM-DD');
            }
          }
          )
        }
        else
          this.showAllocationHistoryGrid = false;
      });
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

  public getProjectManagerDetails(projectId: number) {
    //this.getClientBillingRolesByProject(projectId);
    this.clientName = "";
    this.allocationForm.controls["ClientBillingRoleId"].reset();
    this.allocationForm.controls["ClientBillingRoleName"].reset();
    this.allocationForm.controls["ClientBillingPercentage"].reset();
    this.allocationForm.controls["BillingStartDate"].reset();
    if (projectId > 0) {
      let projectData: any = this.projectDetails.filter((project: ProjectsData) => { return project.ProjectId == projectId });
      this.clientName = projectData[0].ClientName;
      this.requisitionList.ProjectName = projectData[0].ProjectName;
      if (this.clientName.indexOf(InternalClient[InternalClient.SenecaGlobal]) != -1) {
        this.disableBilling = true;
        this.allocationForm.controls["Billable"].patchValue(false);
      }
      else {
        this.disableBilling = false;
        this.allocationForm.controls["Billable"].patchValue(true);
      }
      this.allocationForm.controls["ReportingManagerId"].reset();
      this.GetReportingManagers(projectId);
    }
  }

  public getClientBillingRoles() {
    this._service.GetClientBillingRoles().subscribe((clientRole: ClientBillingRole[]) => {
      this.clientRolesList.push({ label: '', value: null });
      clientRole.forEach((e: ClientBillingRole) => {
        this.clientRolesList.push({ label: e.ClientBillingRoleCode, value: e.ClientBillingRoleId })
      });
    }, (error: any) => {
      // if (error._body != undefined && error._body != "")
      //   this._commonService.LogError(this.componentName, error._body).subscribe((data: any) => {
      //   });
      this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Failed to get client billing roles!' });
    });
  }

  public getInternalBillingRoles() {
    this._service.GetInternalBillingRoles().subscribe((clinetRole: InternalBillingRole[]) => {
      this.internalRolesList.push({ label: '', value: null });
      clinetRole.forEach((e: InternalBillingRole) => {
        this.internalRolesList.push({ label: e.InternalBillingRoleCode, value: e.InternalBillingRoleId })
      });
    }, (error: any) => {
      // if (error._body != undefined && error._body != "")
      //   this._commonService.LogError(this.componentName, error._body).subscribe((data: any) => {
      //   });
      this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Failed to get internal billing roles!' });
    });
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

  public IsBillable(event: any) {
    if (event.target.checked) {
      // this.requisitionList.isCritical = true;
      this.allocationForm.patchValue({ isCritical: true });
      this.disableCBR = false;
    }
    else {
      // this.requisitionList.isCritical = false;
      this.allocationForm.patchValue({ isCritical: false });
      this.disableCBR = true;
      this.allocationForm.controls["ClientBillingRoleId"].reset();
      this.allocationForm.controls["ClientBillingRoleName"].reset();
      this.allocationForm.controls["EffectiveDate"].reset();
      this.allocationForm.controls["ClientBillingPercentage"].reset();
      this.allocationForm.controls["BillingStartDate"].reset();
      this.allocationForm.controls["isCritical"].enable();
    }
  }
  public onlyNumbers(event: any) {
    this._commonService.onlyNumbers(event)
  };

  public Allocate(requisitionList: AssociateAllocation) {
    this.formsubmitted = true;
    this.requisitionList.ClientBillingPercentage = requisitionList.ClientBillingPercentage;
    this.requisitionList.ClientBillingRoleId = requisitionList.ClientBillingRoleId;
    this.requisitionList.EmployeeId = requisitionList.EmployeeId;
    this.requisitionList.ReportingManagerId = requisitionList.ReportingManagerId;
    this.requisitionList.Billable = requisitionList.Billable;
    // this.requisitionList.isCritical = requisitionList.isCritical;
    if (this.requisitionList.Billable == true) {
      if (!this.requisitionList.ClientBillingPercentage || !this.requisitionList.ClientBillingRoleId)
        return;
    }

    if (this.allocationForm.valid) {
      if (this.allocatedPercentage <= this.availableAllocationPercentage) {
        requisitionList.EffectiveDate = moment(requisitionList.EffectiveDate).format('YYYY-MM-DD')
        if (requisitionList.isCritical != true) requisitionList.isCritical = false;
        this.requisitionList.TalentRequisitionId = requisitionList.TalentRequisitionId = 1; //Written as it is temporary screen and Talent Management module yet to be implemented.
        let validDates: boolean = false;
        if (this.talentpoolEffectiveDate != null && this.talentpoolEffectiveDate != undefined) {
          validDates = this.CheckDates(this.talentpoolEffectiveDate, requisitionList.EffectiveDate);
        }
        else {
          this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: 'Band width is not available!' });
          return;
        }
        if ((requisitionList.Billable != true) && (this.clientBillingStartDate != undefined) &&
          (!this._commonService.IsValidDate(
            this.clientBillingStartDate,
            requisitionList.EffectiveDate)
          )
        ) {
          this.messageService.add({
            severity: "warn",
            summary: "Warning message",
            detail: "Effective Date should be greater than or equal to Client Billing Start date"
          });
          return;
        }
        if (this.requisitionList.EmployeeId != this.requisitionList.ReportingManagerId) {
          let primaryValidation = this.ValidateIsPrimary(requisitionList);
          if (primaryValidation && validDates != false) {
            this._service.ResourceAllocate(requisitionList).subscribe((data : any) => {
              if (data.IsSuccessful) {
                this.ManagersList = [];
                this.ManagersList.push({ label: '', value: null });
                this.messageService.add({ severity: 'success', summary: 'Success Message', detail: 'Associate allocated successfully!' });
                this.deleteFromTagAssociateList(this.tagAssociateId);
                this.Cancel();
              }
              else if(data.Message == ""){
                this.messageService.add({severity:'error', summary: 'Failure Message', detail:'Failed to allocate associate!'});
              }
              else {
                this.messageService.add({ severity: 'error', summary: 'Error Message', detail: data.Message });
                return;
              }
            }, (error) =>
                this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: 'The Associate has already allocated to the selected project' }));
              // if (error._body != undefined && error._body != "")
              //   this._commonService.LogError(this.componentName, error._body).subscribe((data: any) => {
              //   });
            //   else
            //     this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Failed to allocate associate!' });
            // });
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


  deleteFromTagAssociateList(tagAssociateId: number): void {
    if (tagAssociateId > 0) {
      this.tagAssociateService.DeleteTagList(tagAssociateId).subscribe((res: number) => {
        if (res == 1) {
          setTimeout(() => {
            this.router.navigate(['/project/tagAssociate/']);
          }, 1000);
        }
        else {
          this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Unable to delete record from Tag list.' })
        }
      },
        (error: any) => this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Unable to delete record from Tag list.' })
      );
    }
  }


  private ValidateIsPrimary(requisitionList: AssociateAllocation): boolean {
    let requisitionListAllocationPercentage: number;
    let releaseList = this.AllocationOptions.filter((percentage: PercentageDropDown) => {
      return percentage.AllocationPercentageId == requisitionList.AllocationPercentage;
    });
    if (releaseList && releaseList.length > 0) {
      requisitionListAllocationPercentage = releaseList[0].Percentage;
    }
    if (this.AssociateIsPrimaryAllocation && this.AssociateIsPrimaryAllocation.length > 0 && requisitionListAllocationPercentage) {
      if (this.AssociateIsPrimaryAllocation[0].AllocationPercentage < requisitionListAllocationPercentage && (requisitionList.IsPrimary == false || requisitionList.IsPrimary == null)) {
        this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Please check Primary' });
        return false;
      }
      else if (this.AssociateIsPrimaryAllocation[0].AllocationPercentage == requisitionListAllocationPercentage && (requisitionList.IsPrimary == false || requisitionList.IsPrimary == null)) {
        this.confirmationService.confirm({
          message: 'Are you sure, you want to proceed with out checking primary?',
          header: 'Confirmation',
          icon: 'fa fa-question-circle',
          accept: () => {
            requisitionList.IsPrimary = false;
            if (this.allocationForm.valid && !this.invalidDates) {
              this._service.ResourceAllocate(requisitionList).subscribe((data) => {
                if (data == true) {
                  this.ManagersList = [];
                  this.ManagersList.push({ label: '', value: null });
                  this.messageService.add({ severity: 'success', summary: 'Success Message', detail: 'Associate allocated successfully!' });
                  this.deleteFromTagAssociateList(this.tagAssociateId);
                  this.Cancel();
                }
                else {
                  this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Failed to allocate associate!' });
                }
              }, (error) => {
                // if (error._body != undefined && error._body != "")
                //   this._commonService.LogError(this.componentName, error._body).subscribe((data: any) => {
                //   });
                this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Failed to allocate associate!' });
              });
            }
            return false;
          },
          reject: () => {
            this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Please check Primary' });
            return false;
          }
        });
      }
      else if (this.AssociateIsPrimaryAllocation[0].AllocationPercentage > requisitionListAllocationPercentage && requisitionList.IsPrimary == true) {
        this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Please Uncheck Primary' });
        return false;
      }
      else {
        return true;
      }
    }
    else {
      if ((requisitionList.IsPrimary == false || requisitionList.IsPrimary) == null && requisitionListAllocationPercentage > 0) {
        this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Please check Primary' });
        return false;
      }
      else {
        return true;
      }
    }
  }

  public Cancel() {
    this.allocationForm.reset();
    // this.allocationForm.controls["isCritical"].enable();
    this.formsubmitted = false;
    this.disableAssociateName = false;
    this.showAllocationHistoryGrid = false;
    this.disableCBR = true;
    this.allocationHistory = [];
    this.clientBillingRoleData = [];
    this.availableAllocationPercentage = 0;
    this.talentpoolEffectiveDate = null;
    this.clientName = "";
    this.requisitionList = new AssociateAllocation();
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

  private CheckDates = function (fromDate: any, toDate: any): boolean {
    if ((fromDate != null || fromDate != undefined) && (toDate != null && toDate != undefined)) {
      if (Date.parse(fromDate) <= Date.parse(toDate)) {
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

  backToTagList() {
    this.router.navigate(['/project/tagAssociate/']);
  }

}
