import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { Http, ResponseOptions } from '@angular/http';
import { MasterDataService } from '../../../services/masterdata.service';
import { GenericType } from '../../../models/dropdowntype.model';
import { Employee } from '../models/assignmanagertoproject.model';
import { AssignManagerToProjectService } from '../services/assign-manager-to-project.service';
import { SelectItem, MessageService, Message } from 'primeng/api';
import { EmployeeStatusService } from '../../admin/services/employeestatus.service';

@Component({
  selector: 'app-assignmanagertoproject',
  templateUrl: './assignmanagertoproject.component.html',
  styleUrls: ['./assignmanagertoproject.component.scss'],
  providers: [MessageService, EmployeeStatusService]
})
export class AssignmanagertoprojectComponent implements OnInit {
  formSubmitted: boolean = false;
  reportingManagerList: any[];
  programManagerList: any[];
  isDisable: Boolean;
  projectList: any[];
  projectsData: any[];
  selectDisabled: Boolean;
  employee: Employee;
  myForm: FormGroup;
  filteredManagersIds: GenericType[] = [];
  componentName: string;
  errorMessage: Message[] = [];
  associateList: any[] = [];

  constructor(private fb: FormBuilder, private messageService: MessageService,
    private masterDataService: MasterDataService,
    private service: AssignManagerToProjectService,
    private _empService: EmployeeStatusService) { }

  ngOnInit() {
    let userRole = JSON.parse(sessionStorage["AssociatePortal_UserInformation"]).roleName;
    let employeeId = JSON.parse(sessionStorage["AssociatePortal_UserInformation"]).employeeId;
    this.CreateForm();
    this.getEmployees();
    this.getProjects();
  }
  getEmployees(): void {
    this._empService.GetAssociateNames().subscribe((data: any[]) => {
      this.associateList = data;
    });
  }
  filteredManagers(event: any): void {
    let suggestionString = event.query.toLowerCase();
    //this.masterDataService.GetEmployeesAndManagers(suggestionString).subscribe((managersResponse: GenericType[]) => {
    if (event.query.length > 2) {
      this.filteredManagersIds = [];
      // this.tempAssociate = [];
      // this.tempAssociate = this.associateList;
      this.associateList.forEach((v) => {
        if (this.filteredManagersIds.findIndex(x => x.Name == v.EmpName) === -1 && v.EmpName.toLowerCase().indexOf(suggestionString) > -1) {
          this.filteredManagersIds.push({ Id: v.EmpId, Name: v.EmpName });
        }
      });
    }
    // },
    //     (error: any) => {
    //         // if (error._body != undefined && error._body != "")
    //         //     this._commonService.LogError(this.componentName, error._body).then((data: any) => {
    //         //     });
    //         // this.errorMessage = [];
    //         // this.errorMessage.push({ severity: 'error', summary: 'Failed to get Associates List!' });
    //     });
  }

  CreateForm() {
    this.formSubmitted = false;
    this.myForm = this.fb.group({
      ddlProgramManager: [GenericType, [Validators.required]],
      ddlProject: ['', [Validators.required]],
      ddlReportingManager: [GenericType, [Validators.required]],
      ddlLead: [GenericType, [Validators.required]]
    });
    this.employee = new Employee();
  }

  getManagersByProject(event) {
    if (event.value != 0) {
      this.employee.ProjectId = event.value;
      this.service.GetManagersByProjectId(event.value).subscribe((response: any) => {
        this.myForm.patchValue(response);
        this.employee.ProgramManagerName = "";
        this.employee.ReportingManagerName = "";
        this.employee.LeadName = "";
        if (response && response.ProgramManagerName) {
          this.employee.ManagerId = response.ProgramManagerId;
          this.employee.ProgramManagerName = response.ProgramManagerName;
        }
        if (response && response.ReportingManagerName) {
          this.employee.ReportingManagerId = response.ReportingManagerId;
          this.employee.ReportingManagerName = response.ReportingManagerName;
        }
        if (response && response.LeadName) {
          this.employee.LeadId = response.LeadId;
          this.employee.LeadName = response.LeadName;
        }
      });
    }
    else {
      this.formSubmitted = false;
      this.employee.ProgramManagerName = "";
      this.employee.ReportingManagerName = "";
      this.employee.LeadName = "";
    }
  }

  clearValues() {
    this.CreateForm();
  }

  assignManagers(): void {
    this.formSubmitted = true;
    if (this.myForm.value.ddlProgramManager.Id != null) this.employee.ManagerId = this.myForm.value.ddlProgramManager.Id;
    else { this.employee.ManagerId; }
    if (this.myForm.value.ddlReportingManager.Id != null) this.employee.ReportingManagerId = this.myForm.value.ddlReportingManager.Id;
    else { this.employee.ReportingManagerId; }
    if (this.myForm.value.ddlLead.Id) this.employee.LeadId = this.myForm.value.ddlLead.Id;
    else { this.employee.LeadId; }

    if (this.employee.ProjectId && (this.employee.ManagerId != undefined || this.employee.ReportingManagerId != undefined || this.employee.LeadId != undefined)) {
      this.service.AssignReportingManagerToProject(this.employee).subscribe((response: any) => {
        let message = response == true ? 'Managers Assigned successfully' : response;
        this.messageService.add({
          severity: 'success',
          summary: 'Success Message',
          detail: message
        });
        this.clearValues();
      }, (error) => {
        if (error._body != undefined && error._body != "")
          this.messageService.add({
            severity: 'error',
            summary: 'Error message',
            detail: ''
          });
        else
          this.messageService.add({
            severity: 'error',
            summary: 'Error message',
            detail: 'Failed to assign manager'
          });
      });
    }
  }

  getProjects() {
    this.masterDataService.GetProjectsList().subscribe((projectlist: any) => {
      this.projectList = [];
      let resultData: any[] = projectlist
      this.projectList.push({ label: '', value: null });
      resultData.forEach(element => {
        if (this.projectList.findIndex(x => x.label == element.ProjectName) == -1)
          this.projectList.push({ label: element.ProjectName, value: element.ProjectId });
      });
    });
  }
}
