import { Component, ComponentFactoryResolver, OnInit,ViewChild, Inject } from "@angular/core";
import { FormGroup, FormControl, Validators,FormGroupDirective, ValidationErrors, AsyncValidatorFn } from "@angular/forms";
// import { SelectItem, MessageService } from "primeng/api";
import { ProjectCreationService } from "../../services/project-creation.service";
import { Client } from "../../../admin/models/client.model";
import { MasterDataService } from "../../../../core/services/masterdata.service";
import { ProjectsData } from "../../../master-layout/models/projects.model";
// import { ConfirmationService } from "primeng/api";
import * as moment from "moment";
import { Observable, Subscription } from "rxjs";
import { CommonService } from "../../../../core/services/common.service";
import { DomainMasterData } from "../../../admin/models/domainmasterdata.model";
import { Department } from "../../../admin/models/department.model";
import { ProjectTypeData, } from "../../../admin/models/projecttype.model";
import { PracticeArea } from "../../../master-layout/models/associate-skills.model";
import { ActivatedRoute } from "../../../../../../node_modules/@angular/router";
import { themeconfig } from '../../../../../themeconfig';
import {MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog';
import {
  MatSnackBar,
  MatSnackBarHorizontalPosition,
  MatSnackBarVerticalPosition,
} from '@angular/material/snack-bar';
import { ConfirmDialogComponent } from '../confirm-dialog/confirm-dialog.component';
import { GenericType } from "../../../master-layout/models/dropdowntype.model";
import { map, startWith } from "rxjs/operators";
import { MatAutocompleteTrigger } from "@angular/material/autocomplete";
import { element } from "protractor";
import { SelectorListContext } from "@angular/compiler";


export interface DialogData {
  masg : string;
  status: string; 
}

interface SelectItem {
  value : number;
  label : string;
}

@Component({
  selector: "app-project-creation",
  templateUrl: "./project-creation.component.html",
  styleUrls: ["./project-creation.component.scss"],
  providers: []
})



export class ProjectCreationComponent implements OnInit {
  @ViewChild('inputAutoComplete') inputAutoComplete: any;
  themeConfigInput = themeconfig.formfieldappearances;
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
  addProjectData = new ProjectsData();
  UserRole: string;
  EmpId: number;
  editMode: boolean;
  departmentsList: SelectItem[] = [];
  matchList:SelectItem[];
  projectIdSubscription: Subscription;
  hideProjectState: boolean = false;
  msg : string;
  status : string;
  controlName: string;
  @ViewChild(FormGroupDirective) formGroupDirective: FormGroupDirective;

  filteredPracticeArea : Observable<any>;
  filteredProjectType : Observable<any>;
  filteredClient : Observable<any>;
  filteredDomain : Observable<any>;

  selectedProjectTypeId:any;
  selectedPracticeAreaId:any;
  selectedClientId:any;
  selectedDomainId:any;
  domainListLabels: string[];
  isSave: boolean = false;

  minStartDate = new Date();
  maxStartDate = new Date();
  

  
  constructor(
    private actRoute: ActivatedRoute,
    private projectCreationService: ProjectCreationService,
    private commonService: CommonService,
    private masterService: MasterDataService,
    public dialog: MatDialog,
    private _snackBar: MatSnackBar,
    // private messageService: MessageService,
    // private confirmationService: ConfirmationService
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
    this.GetProgramManagers();
    this.getDateRange();
  }

  getDateRange() : void{
    var date = new Date(), year = date.getFullYear(), month = date.getMonth();
    this.minStartDate = new Date(year, month - 1, 1);
    this.maxStartDate = new Date();
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
    this.GetProjectStates();
    this.GetProjectTypes();
    this.GetClients();
    this.GetPracticeAreas();
    this.SetUserRoleName();
    this.GetProgramManagers();
    this.GetDepartments();
    this.GetDomains();
    if (this.projectId > 0)
      this.btnLabel = "Update";
    else
      this.btnLabel = "Save";
    this.addProject = new FormGroup({
      ProjectCode: new FormControl(null, [
        Validators.required,
        Validators.pattern("^(?!.*--)^(?!-[0-9])[A-Za-z0-9-.()&/, ]+$")
      ]),
      ProjectTypeId: new FormControl(null, [Validators.required, this.requireMatch.bind(this)]),
      ProjectName: new FormControl(null, [
        Validators.required,
        // Validators.pattern("^(?!-)(?!.*--)^(?![0-9])[A-Za-z0-9- ]+$")
        Validators.pattern("^(?!.*--)^(?!-[0-9])[A-Za-z0-9-.()&/, ]+$")
      ]),
      DomainId: new FormControl(null, [this.requireMatch.bind(this)]),
      ProjectStateId: new FormControl(null),
      ClientId: new FormControl(null, [Validators.required, this.requireMatch.bind(this)]),
      ManagerId: new FormControl(null), // required  disabled: this.showDropdown
      PracticeAreaId: new FormControl(null, [Validators.required, this.requireMatch.bind(this)]),
      ActualStartDate: new FormControl(null, [Validators.required]),
      ActualEndDate: new FormControl(null),
      DepartmentId: new FormControl(1) //required
    });
    if (this.UserRole == "Department Head")
      // add validation to Program manager dropdown if login user is department head
     { this.addProject.controls['ManagerId'].setValidators(
        Validators.required
      );
    }
      else{
        this.addProject.patchValue({
          DepartmentId: 1
        });

        this.EmpId = Number(this.EmpId);
        this.addProject.patchValue({
          ManagerId : this.EmpId
        });
      } 
  }

  GetDomains() {
    this.projectCreationService
      .GetDomains()
      .subscribe((res: DomainMasterData[]) => {
        res.forEach((element: DomainMasterData) => {
          this.domainList.push({
            label: element.DomainName,
            value: element.DomainID
          });
          
        });
        // this.getDomainsLabels();
        this.filteredDomain = this.addProject.valueChanges.pipe(
          startWith(''),
          map((value) => this._filterDomain(value))
          );
      });
  }
  // getDomainsLabels(){
  //       this.domainList.forEach((element: domainList) => {
  //         this.domainListLabels.push(element.DomainName);
  //       });
  //   }             

  private _filterDomain(value) {
  
    let filterValue;
    if (value && value.DomainId) {
      filterValue = value.DomainId.toLowerCase();
      return this.domainList.filter((option) =>
        option.label.toLowerCase().includes(filterValue)
      );
    } else {
      return this.domainList;
    }
  }
  getFormControl(value:any){
    this.controlName = value;
  }
  
  private requireMatch(control: FormControl): ValidationErrors | null {
    const selection: any = control.value;
    switch (this.controlName){
      case 'DomainId': this.matchList = this.domainList;
      break;

      case 'ClientId': this.matchList = this.clientsList;
      break;

      case 'PracticeAreaId': this.matchList = this.practiceAreaList;
      break;

      case 'ProjectTypeId': this.matchList = this.projectTypes;
      break;

    }
    if (this.matchList && this.matchList.findIndex(x => x.label === selection) < 0 && !(selection === '' || selection === null) && !this.isSave) {
      return { requireMatch: true };
    }
    return null;
  }

  GetDepartments(): void {
    this.masterService.GetDepartments().subscribe((res: Department[]) => {
      res.forEach((element: Department) => {
        this.departmentsList.push({
          label: element.DepartmentCode,
          value: element.DepartmentId
        });
      });
      let index = this.departmentsList.findIndex(x => x.label === "Delivery");
      this.addProject.patchValue({
        DepartmentId: this.departmentsList[index].value
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
    //this.projectTypes.push({ label: "Select Project Type", value: null });
    this.masterService.GetProjectTypes().subscribe((res: ProjectTypeData[]) => {
      res.forEach(e => {
        this.projectTypes.push({
          label: e.ProjectTypeCode,
          value: e.ProjectTypeId
        });
      });
      this.filteredProjectType = this.addProject.valueChanges.pipe(
        startWith(''),
        map((value) => this._filterProjectType(value))
      ); 
    });
  }

  private _filterProjectType(value) {
  
    let filterValue;
    if (value && value.ProjectTypeId) {
      filterValue = value.ProjectTypeId.toLowerCase();
      return this.projectTypes.filter((option) =>
        option.label.toLowerCase().includes(filterValue)
      );
    } else {
      return this.projectTypes;
    }
  }

  GetClients(): void {
    // to get all available clients from DB
    //this.clientsList.push({ label: "Select Client", value: null });
    this.masterService.GetClientList().subscribe((res: GenericType[]) => {
      res.forEach(e => {
        this.clientsList.push({ label: e.Name, value: e.Id });
      });
      this.filteredClient = this.addProject.valueChanges.pipe(
        startWith(''),
        map((value) => this._filterClient(value))
        );
    });
  }

  private _filterClient(value) {
  
    let filterValue;
    if (value && value.ClientId) {
      filterValue = value.ClientId.toLowerCase();
      return this.clientsList.filter((option) =>
        option.label.toLowerCase().includes(filterValue)
      );
    } else {
      return this.clientsList;
    }
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
          this.addProject.patchValue({
            ManagerId: res[0].EmployeeId
          });
        }
      });
  }

  GetPracticeAreas(): void {
    // to get all available practice areas from DB
    //this.practiceAreaList.push({ label: "Select Technology", value: null });
    this.masterService.GetPractiseAreas().subscribe((res: GenericType[]) => {
      res.forEach(e => {
        this.practiceAreaList.push({
          label: e.Name,
          value: e.Id
        });
      });
      this.filteredPracticeArea = this.addProject.valueChanges.pipe(
        startWith(''),
        map((value) => this._filterPracticeArea(value))
      ); 
    });
  }

  private _filterPracticeArea(value) {
  
    let filterValue;
    if (value && value.PracticeAreaId) {
      filterValue = value.PracticeAreaId.toLowerCase();
      return this.practiceAreaList.filter((option) =>
        option.label.toLowerCase().includes(filterValue)
      );
    } else {
      return this.practiceAreaList;
    }
  }

  selectedChangeIds(frmCntrl, item) {
    if (frmCntrl== 'ProjectTypeId') {
      this.selectedProjectTypeId = item.value;
    }
    else if (frmCntrl == 'PracticeAreaId') {
       this.selectedPracticeAreaId = item.value;
     }
   else if (frmCntrl == 'ClientId') {
     this.selectedClientId = item.value;
   }
   else if (frmCntrl == 'DomainId') {
    this.selectedDomainId = item.value;
  }
  }

  openOrClosePanel(evt: any, trigger: MatAutocompleteTrigger): void {
    evt.stopPropagation();
    if(trigger.panelOpen)
      trigger.closePanel();
    else
      trigger.openPanel();
  }

  clearInput(evt: any, fieldName): void {
    if(fieldName=='ProjectTypeId'){
      evt.stopPropagation();
      this.addProject.get('ProjectTypeId').reset();
    }
    if(fieldName=='PracticeAreaId'){
      evt.stopPropagation();
      this.addProject.get('PracticeAreaId').reset();
    }  
    if(fieldName=='ClientId'){
      evt.stopPropagation();
      this.addProject.get('ClientId').reset();
    } 
    if(fieldName=='DomainId'){
      evt.stopPropagation();
      this.addProject.get('DomainId').reset();
      this.selectedDomainId = null;
    } 

    if(fieldName=='ActualEndDate'){
      evt.stopPropagation();
      this.addProject.get('ActualEndDate').reset();
    }

    if(fieldName=='ActualStartDate'){
      evt.stopPropagation();
      this.addProject.get('ActualStartDate').reset();
    }
  }

  SaveProjectData() {
    this.addProjectData.ProjectCode = this.addProject.value.ProjectCode;
    this.addProjectData.ProjectName = this.addProject.value.ProjectName;
    this.addProjectData.ProjectTypeId = this.selectedProjectTypeId;
    this.addProjectData.ProjectStateId = this.addProject.value.ProjectStateId;
    this.addProjectData.ClientId = this.selectedClientId;
    this.addProjectData.ManagerId = this.addProject.value.ManagerId;
    this.addProjectData.PracticeAreaId = this.selectedPracticeAreaId;
    this.addProjectData.ActualStartDate = this.addProject.value.ActualStartDate;
    this.addProjectData.ActualEndDate = this.addProject.value.ActualEndDate;
    this.addProjectData.DepartmentId = this.addProject.value.DepartmentId;
    this.addProjectData.DomainId = this.selectedDomainId;
    this.isSave = true;

    this.projectCreationService.SaveProject(this.addProjectData).subscribe(
      (res: number) => {
        this.formSubmitted = false;
        if (res > 0) {
          this._snackBar.open('Successfully created', 'x', {
          duration: 3000,
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
          this.GetProjectByID(res);
          this.projectId = res;
          this.projectCreationService.SetProjectId(res);
          // update project id
          const dialogRef = this.dialog.open(ConfirmDialogComponent,{
            disableClose: true,
            hasBackdrop: true,
            data:{
              message: 'Do you want to add SOW?',
            }
          });
          dialogRef.afterClosed().subscribe(result => {
            if(result){
              this.projectCreationService.SetSeletedTab(1);
            }else{
              const dialogRef = this.dialog.open(ConfirmDialogComponent,{
                disableClose: true,
                hasBackdrop: true,
                data:{
                  message: 'Do you want to create client billing roles?',
                }
              });
              dialogRef.afterClosed().subscribe(result => {
                
                if(result){
                  this.projectCreationService.SetSeletedTab(2);
                }else
                this.projectCreationService.SetSeletedTab(3);
              });

            }
          });
          this.btnLabel = "Update";
        }
        else {
          this._snackBar.open('Unable to save project', 'x', {
            duration: 3000,
            panelClass: ['error-alert'],
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
          
        }
      },
      error => {
        this._snackBar.open("Project already exists", 'x', {
          duration: 3000,
          panelClass: ['error-alert'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
        }
    );
  }

  


  SaveProject(): void {
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
          this._snackBar.open('End date cannot be less than Start date', 'x', {
            duration: 3000,
            horizontalPosition: 'right',
            verticalPosition: 'top',
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
            this._snackBar.open('Successfully Updated', 'x', {
              duration: 3000,
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
          } else if (res == 2627) {
            this._snackBar.open('Entered Duplicate Data', 'x', {
              duration: 3000,
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
          } else if (res == -2) {
            this._snackBar.open('Either associates are allocated to this project or Requisition is raised against this project.', 'x', {
              duration: 3000,
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
          }
          else {
            this._snackBar.open('Unable to update project', 'x', {
              duration: 3000,
              panelClass: ['error-alert'],
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
          }
        },
        error => {
          this._snackBar.open(error.error, 'x', {
            duration: 3000,
            panelClass: ['error-alert'],
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
        }
      );
  }
  

  AssignUpdatedFormData(): void {
    // here we are assigning user updated details to an object and will send that object to update method
    this.updatedProjectData.ProjectId = this.projectId;
    this.updatedProjectData.ProjectCode = this.addProject.value.ProjectCode;
    this.updatedProjectData.ProjectName = this.addProject.value.ProjectName;
    if(this.selectedProjectTypeId)
      this.updatedProjectData.ProjectTypeId = this.selectedProjectTypeId;
    this.updatedProjectData.ProjectStateId = this.addProject.value.ProjectStateId;
    if(this.selectedClientId)
      this.updatedProjectData.ClientId = this.selectedClientId;
    this.updatedProjectData.ManagerId = this.addProject.value.ManagerId;
    if(this.selectedPracticeAreaId)
      this.updatedProjectData.PracticeAreaId = this.selectedPracticeAreaId;
    this.updatedProjectData.ActualStartDate = this.addProject.value.ActualStartDate;
    this.updatedProjectData.ActualEndDate = this.addProject.value.ActualEndDate;
    this.updatedProjectData.DepartmentId = this.addProject.value.DepartmentId;
    this.updatedProjectData.UserRole = this.UserRole;
    if(this.selectedDomainId || this.selectedDomainId == null)
      this.updatedProjectData.DomainId = this.selectedDomainId;

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
    this.addProject.reset({
      ManagerId: this.addProject.get('ManagerId').value, 
      DepartmentId:this.addProject.get('DepartmentId').value
    });
    this.formGroupDirective.resetForm();
    this.isEndDateRequired = false;
  }

  onEmpty(event : any, controlName : string){
    if (event.target.value.length === 0 && controlName === 'ProjectCode')
      this.addProject.controls.ProjectCode.patchValue(null);
    
    else if (event.target.value.length === 0 && controlName === 'ProjectName')
      this.addProject.controls.ProjectName.patchValue(null);  
       
  }


  ClearForm() {
    this.projectCreationService.SetProjectId(0);
    this.projectCreationService.SetSeletedTab(0); // to clear
    this.Reset();
  }

  OpenConfirmation() {
    const dialogRef = this.dialog.open(ConfirmDialogComponent,{
      disableClose: true,
      hasBackdrop: true,
      data:{
        message: 'Do you want to clear?',
      }
    });
    dialogRef.afterClosed().subscribe(result => {
      if(result){
        this.formSubmitted = false;
        this.ClearForm();
      }
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
      if (currentRole == "Department Head") {
        this.showDropdown = true;
        // this.addProject.controls['ManagerId'].setValidators(
        //   Validators.required
        // );
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
        ClientId: this.updatedProjectData.ClientName,
        ManagerId: this.updatedProjectData.ManagerId,
        ProjectTypeId: this.updatedProjectData.ProjectTypeCode,
        ProjectStateId: this.updatedProjectData.ProjectStateId,
        PracticeAreaId: this.updatedProjectData.PracticeAreaCode,
        ActualStartDate: this.ModifyDateFormat(
          this.updatedProjectData.ActualStartDate
        ),
        ActualEndDate: this.ModifyDateFormat(
          this.updatedProjectData.ActualEndDate
        ),
        DomainId: this.updatedProjectData.DomainName,
        ProgramManagerName: this.updatedProjectData.ManagerName
      });
    }
  }

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
          this._snackBar.open('Error while fetching the project', 'x', {
            duration: 3000,
            panelClass: ['error-alert'],
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
        }
      );
  }
  ngOnDestroy() {
    this.projectIdSubscription.unsubscribe();
  }
}
