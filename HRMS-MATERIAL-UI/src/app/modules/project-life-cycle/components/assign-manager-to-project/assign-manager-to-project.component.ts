import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, Validators, FormGroup, FormGroupDirective } from '@angular/forms';
import { MasterDataService } from '../../../master-layout/services/masterdata.service';
import { GenericType } from '../../../master-layout/models/dropdowntype.model';
import { Employee } from '../../models/assign-manager-to-project.model';
import { AssignManagerToProjectService } from '../../services/assign-manager-to-project.service';
import { EmployeeStatusService } from '../../../admin/services/employeestatus.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { themeconfig } from 'src/themeconfig';
import { Observable } from 'rxjs';
import { startWith,map } from 'rxjs/operators';
import * as moment from 'moment';


@Component({
  selector: 'app-assign-manager-to-project',
  templateUrl: './assign-manager-to-project.component.html',
  styleUrls: ['./assign-manager-to-project.component.scss'],
  providers: [EmployeeStatusService]
})
export class AssignManagerToProjectComponent implements OnInit {
  themeConfigInput = themeconfig.formfieldappearances;
  filteredOptions: Observable<any>;


  formSubmitted: boolean = false;
  reportingManagerList: any[];
  programManagerList: any[];
  isDisable: Boolean;
  projectList: any[];
  projectsData: any[];
  selectDisabled: Boolean;
  employee: Employee;
  assignManagerToProject: FormGroup;
  filteredManagersIds: GenericType[] = [];
  componentName: string;
  errorMessage: any[] = [];
  associateList: any[] = [];
  employeeList: any[] = [];
  employeeId:number;
  userRole:string;
  today = new Date();
  firstDate: Date;
  isDateError:boolean = false
  dateErrorMessage:string;

  selectedChangeProgramManager:any;
  selectedChangeReportingManager:any;
  selectedChangeLeadManager:any;

  @ViewChild(FormGroupDirective) formGroupDirective: FormGroupDirective;

  constructor(private fb: FormBuilder,
    private masterDataService: MasterDataService,
    private service: AssignManagerToProjectService,
    private _empService: EmployeeStatusService,private _snackBar: MatSnackBar) { }

  ngOnInit() {
    this.userRole = JSON.parse(sessionStorage["AssociatePortal_UserInformation"]).roleName;
    this.today.setDate(this.today.getDate());
    this.employeeId = JSON.parse(sessionStorage["AssociatePortal_UserInformation"]).employeeId;
    this.getDates();
    this.CreateForm();
    this.getEmployees();
    this.getProjects();
  }

  getDates() {
    var date = new Date(), y = date.getFullYear(), m = date.getMonth()
    this.firstDate = new Date(y, m, 0);
    if(this.firstDate.getDay()===6){
      this.firstDate.setDate(this.firstDate.getDate() - 1);
    }
    else if(this.firstDate.getDay()===0){
        this.firstDate.setDate(this.firstDate.getDate() - 2);
      }

  }

  getEmployees(): void {
    this._empService.GetAssociateNames().subscribe((data: any[]) => {
      this.associateList = data;
      this.associateList.forEach(element => {
        if (this.employeeList.findIndex(x => x.label == element.EmpName) == -1)
          this.employeeList.push({ label: element.EmpName, value: element.EmpId });
      });
    });
  }

  filteredManagers(event: any): void {
    if(event){
    let suggestionString = event.target.value.toLowerCase();
    //this.masterDataService.GetEmployeesAndManagers(suggestionString).subscribe((managersResponse: GenericType[]) => {
      this.filteredManagersIds = [];
      this.associateList.forEach((v) => {
        if (this.filteredManagersIds.findIndex(x => x.Name == v.EmpName) === -1 && v.EmpName.toLowerCase().indexOf(suggestionString) > -1) {
          this.filteredManagersIds.push({ Id: v.EmpId, Name: v.EmpName });
        }
      });
  }
  else {
    this.filteredManagersIds=[];
     this.pushFilteredManagersIds();
  }
  }

  CreateForm() {
    this.formSubmitted = false;
    this.assignManagerToProject = this.fb.group({
      ddlProgramManager: [''],
      ddlProject: ['', [Validators.required]],
      ddlReportingManager: [''],
      ddlLead: [''],
      EffectiveDate:['', Validators.required]
    });
    this.employee = new Employee();
  }

  getManagersByProject(event) {

    let tempProjectID=event.value;

    if (tempProjectID) {
      this.employee.ProjectId = tempProjectID;
      this.service.GetManagersByProjectId(tempProjectID).subscribe((response: any) => {
        if(response){
          this.assignManagerToProject.patchValue(response);
          if (response.ProgramManagerName) {
            this.employee.ManagerId = response.ProgramManagerId;
            this.employee.ProgramManagerName = response.ProgramManagerName;
          }
          else{
            this.employee.ProgramManagerName = "";
          }
          if (response.ReportingManagerName) {
            this.employee.ReportingManagerId = response.ReportingManagerId;
            this.employee.ReportingManagerName = response.ReportingManagerName;
          }
          else{
            this.employee.ReportingManagerName = "";
          }
          if (response.LeadName) {
            this.employee.LeadId = response.LeadId;
            this.employee.LeadName = response.LeadName;
          }
          else{
            this.employee.LeadName = "";
          }
        }
      },
      (error)=>{
        this.emptyEmployee();
      });
      this.filteredManagers('');
    }
    else {
      this.formSubmitted = false;
      this.emptyEmployee();
      this.filteredManagers('');
    }
  }

  onChangeEvent(event: any){
    if(event.target.value == ''){
      this.clearValues();
    }
  }

  clearValues() {
    this.assignManagerToProject.reset();
    this.isDateError=false
    setTimeout(() => this.formGroupDirective.resetForm(), 0);
    this.emptyEmployee();
  }

  assignManagers(): void {
    if (this.assignManagerToProject.value.EffectiveDate){
      this.employee.EffectiveDate = moment(this.assignManagerToProject.value.EffectiveDate).format('YYYY-MM-DD')
    }
    if(this.selectedChangeProgramManager!=null)
      this.employee.ManagerId = this.selectedChangeProgramManager.Id;
    else
      this.employee.ManagerId;
    if(this.selectedChangeReportingManager!=null)
        if(this.selectedChangeReportingManager!=null)
    if(this.selectedChangeReportingManager!=null)
      this.employee.ReportingManagerId =  this.selectedChangeReportingManager.Id;
    else
      this.employee.ReportingManagerId;
    if(this.selectedChangeLeadManager)
        if(this.selectedChangeLeadManager)
    if(this.selectedChangeLeadManager)
      this.employee.LeadId =  this.selectedChangeLeadManager.Id;
    else
      this.employee.LeadId;
             this.employee.LeadId;
      this.employee.LeadId;
    if(this.employee.ManagerId == null){
      this.assignManagerToProject.get('ddlProgramManager').setValidators([Validators.required])
      this.assignManagerToProject.get('ddlProgramManager').updateValueAndValidity();
    }
    else{
      this.assignManagerToProject.get('ddlProgramManager').setErrors(null)
      this.assignManagerToProject.get('ddlProgramManager').clearValidators();
      this.assignManagerToProject.get('ddlProgramManager').updateValueAndValidity();
    }
    if (this.assignManagerToProject.valid && this.employee.ManagerId) {
      this.service.AssignReportingManagerToProject(this.employee).subscribe((response: any) => {
        let message = response == true ? 'Managers Assigned successfully' : response;
        this._snackBar.open(message, '', {
          duration: 1000,
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
        this.clearValues();
        this.getProjects();
      }, (error) => {
        let message;
        if (error._body != undefined && error._body != "")
        {
          message = 'Error message'
          this._snackBar.open(message, '', {
            duration: 1000,
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
        }
        else
        {
          if(error.error.text.includes('Invalid date. Effective date should be greater than are eqaul to')){
            this.isDateError = true
            this.dateErrorMessage=error.error.text
          }
          else if (error.error.text == "Record already exists."){
            message = "Record already exists"
            this._snackBar.open(message, '', {
              duration: 1000,
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
          }
          else{
            message = 'Failed to assign manager'
            this._snackBar.open(message, '', {
              duration: 1000,
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
          }
        }
      });
    }

 /* else
  {
    this._snackBar.open('Failed to assign manager', '', {
      duration: 1000,
      horizontalPosition: 'right',
      verticalPosition: 'top',
    });
  }
*/
  }

  getProjects() {
    this.masterDataService.GetProjectsList().subscribe((projectlist: any) => {
      this.projectList = [];
      let resultData: any[] = projectlist
      this.projectList.push({ label: '', value: null });
      resultData.forEach(element => {
        if(element.ProgramManagerId === this.employeeId){
        if (this.projectList.findIndex(x => x.label == element.ProjectName) == -1)
          {this.projectList.push({ label: element.ProjectName, value: element.ProjectId });
        }
      }
      if(this.userRole.toUpperCase() === 'HRA'){
        if (this.projectList.findIndex(x => x.label == element.ProjectName) == -1)
          {this.projectList.push({ label: element.ProjectName, value: element.ProjectId });
        }
      }
      });
      this.filteredOptions = this.assignManagerToProject.valueChanges.pipe(
        startWith(''),
        map((value) => this._filter(value))
      );

    });
  }

  private _filter(value) {
    let filterValue;
    if (value && value.ddlProject) {
      filterValue = value.ddlProject.toLowerCase();
      return this.projectList.filter((option) =>
        option.label.toLowerCase().includes(filterValue)
      );
    } else {
      return this.projectList;
    }
  }
  emptyEmployee(){
    this.employee.ManagerId=null
    this.employee.ReportingManagerId=null
    this.employee.LeadId=null
    this.employee.ReportingManagerName = "";
    this.employee.LeadName = "";
    this.employee.ProgramManagerName = "";
  }
  cliearField(event : any, fieldName) {
    if(fieldName=='ddlProject'){
      this.assignManagerToProject.controls.ddlProject.reset();
      event.stopPropagation();
      this.clearValues();
    }
    else if(fieldName=='ddlProgramManager'){
      this.assignManagerToProject.controls.ddlProgramManager.reset();
      event.stopPropagation();
    }
    else if(fieldName=='ddlReportingManager'){
      this.assignManagerToProject.controls.ddlReportingManager.reset();
      event.stopPropagation();
    }
    else if(fieldName=='ddlLead'){
      this.assignManagerToProject.controls.ddlLead.reset();
      event.stopPropagation();
    }
  }

  getProjectIDbyName(projectName) {
    if (this.projectList) {
      return this.projectList.filter(x => x.label == projectName);
    }
  }
  getEmpIDbyName(empName) {
    if (this.projectList) {
      let x = this.filteredManagersIds.filter(x => x.Name == empName);
      return x[0].Id;
    }
  }
  pushFilteredManagersIds(){
    this.filteredManagersIds=[];
    for (let i = 0; i < this.associateList.length; i++) {
      this.filteredManagersIds.push({ Id: this.associateList[i].EmpId, Name: this.associateList[i].EmpName });
    }
  }

  selectedChangeIds(frmCntrl, item) {
    if (frmCntrl== 'ddlProgramManager') {
      this.selectedChangeProgramManager = item;
    }
    else if (frmCntrl == 'ddlReportingManager') {
      this.selectedChangeReportingManager = item;
    }
    else if (frmCntrl == 'ddlLead') {
      this.selectedChangeLeadManager = item;
    }
  }

  clearInput(evt: any, fieldName): void {
    if (fieldName == 'EffectiveDate') {
      this.isDateError=false
      evt.stopPropagation();
      this.assignManagerToProject.get('EffectiveDate').reset();
    }
  }

  removeDateErrorMessage(){
    this.isDateError=false
  }
}

