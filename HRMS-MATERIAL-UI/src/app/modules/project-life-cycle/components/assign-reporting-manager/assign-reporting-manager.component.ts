import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, Validators, FormGroup, FormGroupDirective } from "@angular/forms";
import { MasterDataService } from "../../../master-layout/services/masterdata.service";
import { CommonService } from "../../../master-layout/services/common.service";
import { Employee, DepartmentAssociates } from '../../models/assign-manager-to-project.model';
import { GenericType } from '../../../master-layout/models/dropdowntype.model';
import { AssignReportingManagerService } from '../../services/assign-reporting-manager.service';
import { ResouceReleaseService } from '../../services/resouce-release.service';
import { HttpClient } from '@angular/common/http';
import { MatSnackBar } from '@angular/material/snack-bar';
import { themeconfig } from '../../../../../themeconfig';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { EmployeeStatusService } from '../../../admin/services/employeestatus.service';
import * as moment from 'moment';
//declare var _: any;

@Component({
  selector: 'app-assign-reporting-manager',
  templateUrl: './assign-reporting-manager.component.html',
  styleUrls: ['./assign-reporting-manager.component.scss'],
  providers: [EmployeeStatusService]
})

export class AssignReportingManagerComponent implements OnInit {
  projId: any;
  form_Submitted: boolean = false;
  themeConfigInput = themeconfig.formfieldappearances;
  associatesList: any[];
  allAssociatesList: any[];
  reportingManagerList: any[] = [];
  isDisable: Boolean;
  projectsList: any[];
  employeeList: any[] = [];
  nonDeliveryEmployeeList: any[] = [];
  projectsData: any[];
  selectDisabled: Boolean;
  employee: Employee;
  assignReportingManagerForm: FormGroup;
  formSubmitted: boolean = false;
  empId: number;
  projectId: number;
  filteredOptions: Observable<any>;
  associateList: any[] = [];
  filteredManagersIds: GenericType[] = [];
  filteredRMManagersIds: GenericType[] = [];
  componentName: string;
  isDelivery: boolean = true;
  isChecked = true;
  associateResultData: DepartmentAssociates[] = [];
  resultData: any[] = [];
  @ViewChild(FormGroupDirective) formGroupDirective: FormGroupDirective;
  selectedcurrentproject: any;
  selectedChangeProgramManager: any;
  selectedChangeReportingManager: any;
  selectedChangeLeadManager: any;
  today = new Date();
  firstDate: Date;
  isDateError:boolean=false
  dateErrorMessage:string
  //filteredmanagers: Observable<any>;

  constructor(private _http: HttpClient, private fb: FormBuilder, private _service: ResouceReleaseService, private service: AssignReportingManagerService, private masterDataService: MasterDataService, private _empService: EmployeeStatusService, private _snackBar: MatSnackBar) { }

  ngOnInit() {
    this.today.setDate(this.today.getDate());
    this.getDates()
    this.createForm();
    this.getEmployees();
    this.getAssociates();
    this.getAllAssociatesList();

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

  createForm() {
    this.assignReportingManagerForm = this.fb.group({
      ddlDelivery: [true],
      ddlEmployee: [null, Validators.required],
      ddlProject: [null, Validators.required],
      ddlReportingManager: [''],
      ddlProgramManager: [''],
      ddlLead: [''],
      EffectiveDate:['',Validators.required]
    });

    this.employee = new Employee();
    this.isDelivery = true;
  }
  getAssociates() {
    this.isDisable = false;
    this.service.GetAllocatedAssociates().subscribe(
      (res:any) => {
        this.associatesList = [];
        this.associateResultData = [];
        if(this.isDelivery==true){
          this.resultData = res.filter(associate => associate.DepartmentId == 1);
          this.associatesList = this.resultData.map(element => {
              return { label: element.EmployeeName, value: element.EmployeeId };
            });
        }
        else{
          this.resultData = this.nonDeliveryEmployeeList.filter(associate => associate.DepartmentId != 1);
          this.associatesList = this.resultData.map(element => {
              return { label: element.EmpName, value: element.EmpId };
            });
        }
        this.associateList = this.associatesList;
        this.projectsList = [];
        this.projectsList.splice(0, 0, {
          label: "",
          value: null
        });
        this.filteredOptions = this.assignReportingManagerForm.valueChanges.pipe(
          startWith(''),
          map((value) => this._filter(value))
        );
      },
      error => {
        this._snackBar.open(error.error, '', {
          duration: 1000,
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
      }
    );
  }
  getEmployees(): void {
    this._empService.GetAssociateNames().subscribe((data: any[]) => {
      this.allAssociatesList = data;
      this.nonDeliveryEmployeeList = data;
      this.allAssociatesList.forEach(element => {
        if (this.employeeList.findIndex(x => x.label == element.EmpName) == -1)
          this.employeeList.push({ label: element.EmpName, value: element.EmpId });
      });

      if (this.allAssociatesList) {
        this.pushFilteredManagersIds();
      }
    });
  }

  filteredManagers(event: any): void {
    if (event) {
      let suggestionString = event.target.value.toLowerCase();
      //this.masterDataService.GetEmployeesAndManagers(suggestionString).subscribe((managersResponse: GenericType[]) => {
      this.filteredManagersIds = [];
      this.allAssociatesList.forEach((v) => {
        if (this.filteredManagersIds.findIndex(x => x.Name == v.EmpName) === -1 && v.EmpName.toLowerCase().indexOf(suggestionString) > -1) {
          this.filteredManagersIds.push({ Id: v.EmpId, Name: v.EmpName });
        }
      });
    }
    else {
      this.filteredManagersIds = [];
      this.pushFilteredManagersIds();
    }
  }

  AllLeadsAndManagers(event: any): void {
    let suggestionString = event.query;
    this.service.GetAllLeadsManagers(suggestionString).subscribe((managersResponse: GenericType[]) => {
      this.filteredRMManagersIds = [];
      this.filteredRMManagersIds = managersResponse;
    },
      (error: any) => {
        // if (error._body != undefined && error._body != "")
        // this._commonService
        //   .LogError(this.componentName, error._body)
        //   .then((data: any) => { });
        //   this.errorMessage = [];
        // this.errorMessage.push({
        //   severity: "error",
        //   summary: "Failed to get All Lead Manager List!"
        // });
      }
    );
  }
  assignReportingManager(): void {
    this.formSubmitted = true;
    if (this.assignReportingManagerForm.value.EffectiveDate){
      this.employee.EffectiveDate = moment(this.assignReportingManagerForm.value.EffectiveDate).format('YYYY-MM-DD')
    }
    if(this.isDelivery == true){
      this.assignReportingManagerForm.get('ddlProgramManager').setValidators([Validators.required])
      this.assignReportingManagerForm.get('ddlProgramManager').updateValueAndValidity();
      if(this.employee.ManagerId || this.assignReportingManagerForm.value.ddlProgramManager){
        this.assignReportingManagerForm.get('ddlProgramManager').setErrors(null)
        this.assignReportingManagerForm.get('ddlProgramManager').clearValidators();
        this.assignReportingManagerForm.get('ddlProgramManager').updateValueAndValidity();
      }
    }
    if((this.assignReportingManagerForm.value.ddlReportingManager || this.employee.ReportingManagerId )&& this.isDelivery == false){
      this.assignReportingManagerForm.get('ddlReportingManager').setErrors(null)
      this.assignReportingManagerForm.get('ddlReportingManager').clearValidators();
      this.assignReportingManagerForm.get('ddlReportingManager').updateValueAndValidity();
    }
    if(this.isDelivery == false){
      this.assignReportingManagerForm.get('ddlProject').setErrors(null)
      this.assignReportingManagerForm.get('ddlProject').clearValidators();
      this.assignReportingManagerForm.get('ddlProject').updateValueAndValidity();
    }
    if (this.assignReportingManagerForm.valid)  {
      if (this.assignReportingManagerForm.value.ddlProgramManager && this.selectedChangeProgramManager) {
        this.employee.ManagerId = this.selectedChangeProgramManager.Id;
        this.employee.ProgramManagerName = this.selectedChangeProgramManager.Name;
      }
      else { this.employee.ManagerId; }
      if (this.assignReportingManagerForm.value.ddlReportingManager && this.selectedChangeReportingManager) {
        this.employee.ReportingManagerId = this.selectedChangeReportingManager.Id;
        this,this.employee.ReportingManagerName = this.selectedChangeReportingManager.Name;
      }
      else { this.employee.ReportingManagerId; }
      if (this.assignReportingManagerForm.value.ddlLead && this.selectedChangeLeadManager) {
        this.employee.LeadId = this.selectedChangeLeadManager.Id;
        this.employee.LeadName = this.selectedChangeLeadManager.Name;
      }
      else { this.employee.LeadId; }
      this.service.AssignReportingManager(this.employee, this.isDelivery).subscribe(
        data => {
          this._snackBar.open('Managers are assigned to associate successfully', 'x', {
            duration: 1000,
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
          this.isDelivery = true;
          this.formSubmitted = false;
          this.clearAllForm();
        },
        error => {
          if (error.error.includes('Invalid date. Effective date should be greater than are eqaul to')) {
            this.isDateError = true
            this.dateErrorMessage = error.error
          }
          else {
            this._snackBar.open('Managers are failed to assign', 'x', {
              duration: 1000,
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
          }
        }
      );
    }
    // else {
    //   this._snackBar.open('Managers are failed to assign', '', {
    //     duration: 1000,
    //     horizontalPosition: 'right',
    //     verticalPosition: 'top',
    //   });
    // }
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
      else{
        this.employee.ProgramManagerName = '';
      }
      if (res[0] && res[0].ReportingManagerName) {
        this.employee.ReportingManagerId = res[0].ReportingManagerId;
        this.employee.ReportingManagerName = res[0].ReportingManagerName;
      }
      else{
        this.employee.ReportingManagerName = '';
      }
      if (res[0] && res[0].LeadName) {
        this.employee.LeadId = res[0].LeadId;
        this.employee.LeadName = res[0].LeadName;
      }
      else{
        this.employee.LeadName = '';
      }
    }, error => {
      this.employee.ProjectId = projectId;
      this.employee.EmployeeId = employeeId;
      this.getEmptyEmployee();
    }
    );
  }

  getEmptyEmployee() {
    this.employee.LeadName = '';
    this.employee.ReportingManagerName = '';
    this.employee.ProgramManagerName = '';
  }

  onChangeEvent(event: any){
    if(event.target.value == ''){
      this.clearAllFormall();
    }
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
          this.assignReportingManagerForm.controls.ddlProject.setValue(this.projectsData[0].ProjectId);
        } else {
          this.employee.ProgramManagerName = "";
          this.employee.ReportingManagerName = "";
          this.employee.LeadName = "";
          this.assignReportingManagerForm.controls.ddlProject.reset()
          this.employee = new Employee();
          this.employee.EmployeeId = this.empId;
          this.selectDisabled = false;
        }
      });
    } else {
      this.employee.ProgramManagerName = "";
      this.employee.ReportingManagerName = "";
      this.employee.LeadName = "";
      this.projectsList = [];
      this.employee.EmployeeId = this.empId;
      this.formSubmitted = false;
    }
  }

  getProjectsprogrammanagers(data: any) {
    this.projId = data.value;
    // this.formSubmitted = false;
    if (data != null) {
      // this.service.GetProjects(data.Id).subscribe((res: any) => {
      //   this.projectsData = [];
      //   this.projectsData = res;
      //   this.projectsList = [];

      //   this.projectsList = this.projectsData.map(element => {
      //     return { label: element.ProjectName, value: element.ProjectId };
      //   });
      //   this.projectsList.splice(0, 0, {
      //     label: "",
      //     value: null
      //   });
      // if (this.projectsData.length == 1) {
      // this.selectDisabled = true;
      // this.employee.ProjectId = this.projectsData[0].ProjectId;
      this.getCurrentManagers(this.projId, this.empId);
      //   this.projectsList = [{ label: this.projectsData[0].ProjectName, value: this.projectsData[0].ProjectId }];
      //   this.assignReportingManagerForm.controls.ddlProject.setValue(this.projectsData[0].ProjectId);
      // } else {
      //     this.employee.ProjectId = null;
      //     this.employee.ProgramManagerName = "";
      //     this.employee.ReportingManagerName = "";
      //     this.employee.LeadName = "";
      //     this.selectDisabled = false;
      //  // }
      // });
      // } else {
      //   this.employee.ProgramManagerName = "";
      //   this.employee.ReportingManagerName = "";
      //   this.employee.LeadName = "";
      //   this.projectsList = [];
      //   this.formSubmitted = false;
    }
  }
  onDeliveryChange(event: any) {
    let x = event.checked;
    this.clearAll(x);
    this.isChecked = event.checked
    if(this.isChecked === true){
      this.assignReportingManagerForm.controls.ddlDelivery.setValue(true);
    }
    else{
      this.assignReportingManagerForm.controls.ddlDelivery.setValue(false);
      this.assignReportingManagerForm.get('ddlReportingManager').setValidators([Validators.required])
      this.assignReportingManagerForm.get('ddlReportingManager').updateValueAndValidity();
    }
    this.getAssociates();
  }
  clearAll(isDelivery: boolean) {
    this.isDelivery = isDelivery;
    this.assignReportingManagerForm.controls["ddlEmployee"].reset();
    this.assignReportingManagerForm.controls["ddlProject"].reset();
    this.assignReportingManagerForm.controls["ddlReportingManager"].reset();
    this.assignReportingManagerForm.controls["ddlProgramManager"].reset();
    this.assignReportingManagerForm.controls["ddlLead"].reset();
    this.projectsList.push({ label: '', value: null });
    this.selectDisabled = false;
    this.employee.ProgramManagerName = "";
    this.employee.ReportingManagerName = "";
    this.employee.LeadName = "";
  }

  clearAllForm() {
    this.employee = new Employee();
    this.assignReportingManagerForm.reset();
    setTimeout(() => {
      this.formGroupDirective.resetForm();
      this.assignReportingManagerForm.controls.ddlDelivery.setValue(true);
      this.assignReportingManagerForm.controls.ddlDelivery.updateValueAndValidity();
      this.isDelivery = true;
    }, 0);
  }
  clearAllFormall(){
    this.isDateError=false
    this.employee = new Employee();
    if(!this.assignReportingManagerForm.value.ddlDelivery){
      this.assignReportingManagerForm.reset();
    setTimeout(() => {
      this.formGroupDirective.resetForm();
    }, 0);
    this.isDelivery = false;
    }
    else
     this.clearAllForm();
  }

  private _filter(value) {
    let filterValue;
    if (value && value.ddlEmployee) {
      filterValue = value.ddlEmployee.toLowerCase();
      return this.associatesList.filter((option) =>
        option.label.toLowerCase().includes(filterValue)
      );
    } else {
      return this.associatesList;
    }
  }

  cliearField(event : any, fieldName) {
    if (fieldName == 'ddlEmployee') {
      this.clearAllFormall();
      event.stopPropagation();
    }
    else if (fieldName == 'ddlProgramManager') {
      this.assignReportingManagerForm.controls.ddlProgramManager.reset();
      event.stopPropagation();
    }
    else if (fieldName == 'ddlReportingManager') {
      this.assignReportingManagerForm.controls.ddlReportingManager.reset();
      event.stopPropagation();
    }
    else if (fieldName == 'ddlLead') {
      this.assignReportingManagerForm.controls.ddlLead.reset();
      event.stopPropagation();
    }
  }

  pushFilteredManagersIds() {
    this.filteredManagersIds = [];
    for (let i = 0; i < this.allAssociatesList.length; i++) {
      this.filteredManagersIds.push({ Id: this.allAssociatesList[i].EmpId, Name: this.allAssociatesList[i].EmpName });
    }
  }

  getAllAssociatesList() {
    this.service.GetAllAssociatesList().subscribe((resp: GenericType[]) => {
      this.allAssociatesList = [];
      this.allAssociatesList = resp;
    });
  }


  selectedChangeIds(frmCntrl, item) {
    if (frmCntrl == 'ddlProgramManager') {
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
      this.assignReportingManagerForm.get('EffectiveDate').reset();
    }
  }
  removeDateErrorMessage(){
    this.isDateError=false
  }
}


