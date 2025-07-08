import { Component, OnInit } from '@angular/core';
import { Http } from "@angular/http";
import { FormBuilder, Validators, FormGroup } from "@angular/forms";
import { MasterDataService } from "../../../services/masterdata.service";
import { CommonService } from "../../../services/common.service";
import { Employee, DepartmentAssociates } from '../models/assignmanagertoproject.model';
import { GenericType } from '../../../models/dropdowntype.model';
import { AssignReportingManagerService } from '../services/assign-reporting-manager.service';
import { ResouceReleaseService } from '../services/resouce-release.service';
import { SelectItem, MessageService, Message } from 'primeng/api';

declare var _: any;

@Component({
  selector: 'app-assignreportingmanager',
  templateUrl: './assignreportingmanager.component.html',
  styleUrls: ['./assignreportingmanager.component.scss'],
  providers: [MessageService]
})
export class AssignreportingmanagerComponent implements OnInit {
  associatesList: any[];
  reportingManagerList: any[] = [];
  isDisable: Boolean;
  projectsList: any[];
  projectsData: any[];
  selectDisabled: Boolean;
  employee: Employee;
  myForm: FormGroup;
  formSubmitted: boolean = false;
  empId: number;
  projectId: number;
  filteredManagersIds: GenericType[] = [];
  filteredRMManagersIds: GenericType[] = [];
  componentName: string;
  errorMessage: Message[] = [];
  isDelivery: boolean = true;
  associateResultData: DepartmentAssociates[] = [];
  resultData: DepartmentAssociates[] = [];

  constructor(private _http: Http, private fb: FormBuilder, private messageService: MessageService, private _service: ResouceReleaseService, private service: AssignReportingManagerService, private masterDataService: MasterDataService) { }

  ngOnInit() {
    this.createForm();
    this.getAssociates();
  }
  createForm() {
    this.myForm = this.fb.group({
      ddlDelivery: [true],
      ddlEmployee: [null, [Validators.required]],
      ddlProject: [null, [Validators.required]],
      ddlReportingManager: [GenericType, [Validators.required]],
      ddlProgramManager: [GenericType, [Validators.required]],
      ddlLead: [GenericType, [Validators.required]]
    });

    this.employee = new Employee();
    this.isDelivery = true;
  }
  getAssociates() {
    this.isDisable = false;
    this.service.GetAllocatedAssociates().subscribe(
      (res: DepartmentAssociates[]) => {
        this.associatesList = [];
        this.associateResultData = [];
        this.associateResultData = res;
        this.resultData = this.getDeliveryDepartmentAssociates(
          this.associateResultData,
          this.isDelivery
        );
        this.associatesList = this.resultData.map(element => {
          return { label: element.EmployeeName, value: element.EmployeeId };
        });
        this.associatesList.splice(0, 0, {
          label: "",
          value: null
        });
        this.projectsList = [];
        this.projectsList.splice(0, 0, {
          label: "",
          value: null
        });
      },
      error => {
        this.messageService.add({
          severity: 'error',
          summary: 'Error message',
          detail: 'Failed to Get Associate details'
        });
      }
    );
  }
  getDeliveryDepartmentAssociates(departmentAssociates: DepartmentAssociates[], IsDelivery: boolean): DepartmentAssociates[] {
    if (!departmentAssociates) return [];

    this.resultData = [];
    if (IsDelivery) {
      this.resultData = departmentAssociates.filter(associate => associate.DepartmentId == 1);
    }
    else {
      this.resultData = departmentAssociates.filter(associate => associate.DepartmentId != 1);
    }
    return this.resultData;
  }
  filteredManagers(event: any): void {
    let suggestionString = event.query;
    this.service.GetEmployeesAndManagers(suggestionString).subscribe((managersResponse: GenericType[]) => {
      this.filteredManagersIds = [];
      this.filteredManagersIds = managersResponse;
    },
      (error: any) => {
        if (error._body != undefined && error._body != "")
          // this._commonService
          //   .LogError(this.componentName, error._body)
          //   .then((data: any) => { });
          this.errorMessage = [];
        this.errorMessage.push({
          severity: "error",
          summary: "Failed to get Associates List!"
        });
      }
    );
  }
  AllLeadsAndManagers(event: any): void {
    let suggestionString = event.query;
    this.service.GetAllLeadsManagers(suggestionString).subscribe((managersResponse: GenericType[]) => {
      this.filteredRMManagersIds = [];
      this.filteredRMManagersIds = managersResponse;
    },
      (error: any) => {
        if (error._body != undefined && error._body != "")
          // this._commonService
          //   .LogError(this.componentName, error._body)
          //   .then((data: any) => { });
          this.errorMessage = [];
        this.errorMessage.push({
          severity: "error",
          summary: "Failed to get All Lead Manager List!"
        });
      }
    );
  }
  assignReportingManager(): void {
    this.formSubmitted = true;
    if (this.myForm.value.ddlProgramManager && this.myForm.value.ddlProgramManager.EmployeeId) {
      this.employee.ManagerId = this.myForm.value.ddlProgramManager.EmployeeId;
    }
    else { this.employee.ManagerId; }
    if (this.myForm.value.ddlReportingManager && this.myForm.value.ddlReportingManager.EmployeeId) {
      this.employee.ReportingManagerId = this.myForm.value.ddlReportingManager.EmployeeId;
    }
    else { this.employee.ReportingManagerId; }
    if (this.myForm.value.ddlLead && this.myForm.value.ddlLead.EmployeeId) {
      this.employee.LeadId = this.myForm.value.ddlLead.EmployeeId;
    }
    else { this.employee.LeadId; }

    if (this.employee.EmployeeId && this.employee.ProjectId && (this.employee.ManagerId != undefined || this.employee.ReportingManagerId != undefined || this.employee.LeadId != undefined)) {
      this.service.AssignReportingManager(this.employee, this.isDelivery).subscribe(
        data => {
          this.messageService.add({
            severity: 'success',
            summary: 'Success Message',
            detail: 'Managers are assigned to associate successfully'
          });
          this.isDelivery = true;
          this.formSubmitted = false;
          this.clearAll(this.isDelivery);
        },
        error => {
          this.messageService.add({
            severity: 'error',
            summary: 'Error message',
            detail: '"Managers are failed to assign'
          });
        }
      );
    }
  }
  getManagerName(event) {
    this.getCurrentManagers(event.value, this.empId);
  }
  getCurrentManagers(projectId: number, employeeId: number) {
    this.service.GetManagerName(projectId, employeeId).subscribe((res: any) => {
      this.employee.ProgramManagerName = "";
      this.employee.ReportingManagerName = "";
      this.employee.LeadName = "";
      this.employee.ProjectId = projectId;
      this.employee.EmployeeId = employeeId;
      if (res[0] && res[0].ProgramManagerName) {
        this.employee.ManagerId = res[0].ProgramManagerId;
        this.employee.ProgramManagerName = res[0].ProgramManagerName;
      }
      if (res[0] && res[0].ReportingManagerName) {
        this.employee.ReportingManagerId = res[0].ReportingManagerId;
        this.employee.ReportingManagerName = res[0].ReportingManagerName;
      }
      if (res[0] && res[0].LeadName) {
        this.employee.LeadId = res[0].LeadId;
        this.employee.LeadName = res[0].LeadName;
      }
    });
  }
  getProjects(data: any) {
    this.empId = data.value;
    this.formSubmitted = false;
    if (data != null) {
      this.service.GetProjects(data.value).subscribe((res: any) => {
        this.projectsData = [];
        this.projectsData = res;
        this.projectsList = [];
        this.projectsList = this.projectsData.map(element => {
          return { label: element.ProjectName, value: element.ProjectId };
        });
        this.projectsList.splice(0, 0, {
          label: "",
          value: null
        });
        if (this.projectsData.length == 1) {
          this.selectDisabled = true;
          // this.employee.ProjectId = this.projectsData[0].ProjectId;
          this.getCurrentManagers(this.projectsData[0].ProjectId, this.empId);
          this.projectsList = [{ label: this.projectsData[0].ProjectName, value: this.projectsData[0].ProjectId }];
        } else {
          this.employee.ProjectId = null;
          this.employee.ProgramManagerName = "";
          this.employee.ReportingManagerName = "";
          this.employee.LeadName = "";
          this.selectDisabled = false;
        }
      });
    } else {
      this.employee.ProgramManagerName = "";
      this.employee.ReportingManagerName = "";
      this.employee.LeadName = "";
      this.projectsList = [];
      this.formSubmitted = false;
    }
  }
  onDeliveryChange(event: any) {
    this.clearAll(event.target.checked);
    this.associatesList = [];
    let filteredData = this.getDeliveryDepartmentAssociates(
      this.associateResultData,
      event.target.checked
    );
    this.associatesList = filteredData.map(element => {
      return { label: element.EmployeeName, value: element.EmployeeId };
    });
    this.associatesList.splice(0, 0, {
      label: "Select Associate",
      value: null
    });
  }
  clearAll(isDelivery: boolean) {
    this.createForm();
    this.isDelivery = isDelivery;
    this.myForm.controls["ddlDelivery"].setValue(isDelivery);
    // this.myForm.reset();
    this.myForm.controls["ddlEmployee"].reset();
    this.myForm.controls["ddlProject"].reset();
    this.myForm.controls["ddlReportingManager"].reset();
    this.myForm.controls["ddlProgramManager"].reset();
    this.myForm.controls["ddlLead"].reset();
    this.projectsList.push({ label: '', value: null });
    this.selectDisabled = false;
  }
}
