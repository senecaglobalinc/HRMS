import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { FormBuilder, FormControl, Validators, FormGroup, FormGroupDirective } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { AssociateAllocation, PercentageDropDown, RoleDetails } from '../../../master-layout/models/associateallocation.model';
import * as moment from 'moment';
import { AssociateAllocationService } from '../../services/associate-allocation.service';
import { DropDownType, GenericType } from '../../../master-layout/models/dropdowntype.model';
import { ProjectsData } from '../../../master-layout/models/projects.model';
import { ClientBillingRole, InternalBillingRole } from '../../../master-layout/models/associateallocation.model'
import { TemporaryAllocationReleaseService } from '../../services/temporary-allocation-release.service';
import { TagAssociateService } from '../../../project-life-cycle/services/tag-associate.service';
import { themeconfig } from 'src/themeconfig';
import { CommonService } from 'src/app/core/services/common.service';
import { InternalClient } from 'src/app/modules/master-layout/utility/enums';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MasterDataService } from 'src/app/modules/master-layout/services/masterdata.service';
import { AssignManagerToProjectService } from 'src/app/modules/project-life-cycle/services/assign-manager-to-project.service';
import { DialogCBRComponent } from '../dialog-cbr/dialog-cbr.component';
import { Observable } from 'rxjs';
import { NgxSpinnerService } from 'ngx-spinner';
import { catchError, debounceTime, filter, map, startWith, switchMap, tap } from "rxjs/operators";
import { AssignReportingManagerService } from 'src/app/modules/project-life-cycle/services/assign-reporting-manager.service';

@Component({
  selector: 'app-associateallocation',
  templateUrl: './associateallocation.component.html',
  styleUrls: ['./associateallocation.component.scss']
})
export class AssociateallocationComponent implements OnInit {
  filteredRole : Observable<any>;
  filteredEmployee : Observable<any>;
  filteredProject : Observable<any>;
  filteredUsers:Observable<any>;
  isDisabled = true;
    themeConfigInput = themeconfig.formfieldappearances;
  formsubmitted: boolean;
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
  AssociateIsPrimaryAllocation: AssociateAllocation;
  projectsList = [];
  allocationOptions = [];
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
  clientBillingRoleData: MatTableDataSource<ClientBillingRole>;
  cbrRoleData: any
  clientBillingStartDate: string;
  PageSize: number;
  isPrimaryDisabled : boolean = false;
  PageDropDown: number[] = [];
  CBRselectedData: ClientBillingRole = new ClientBillingRole();
  roleName: any;
  isCriticalDisabled:boolean = false
  IsZeroPercentAllocationClicked:boolean=false
  employeesListByGivenName:DropDownType[] = [];
  tempEmployeesList:any=[]
  @ViewChild(FormGroupDirective) formGroupDirective: FormGroupDirective;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  selectedEmpId: number;
  disableFutureMarkingGrid: Boolean = true;
  

  // displayedColumns: string[] = ['ClientBillingRoleName', 'ClientBillingPercentage',
  //   'NoOfPositions', 'StartDate', 'AllocationCount'];
  constructor(private _formBuilder: FormBuilder,
    private masterDataService: MasterDataService,
    private _service: AssociateAllocationService,
    private tagAssociateService: TagAssociateService,
    private _commonService: CommonService,
    private actRoute: ActivatedRoute,
    private snackBar: MatSnackBar,
    private dialog: MatDialog,
    private router: Router,
    private spinner: NgxSpinnerService,
    private _tempAllocationRelease: TemporaryAllocationReleaseService,
    private _assignManagerservice: AssignManagerToProjectService,
    private _assignReportingManagerService: AssignReportingManagerService) {

    this.componentName = this.actRoute.routeConfig.component.name;
    this.requisitionList = new AssociateAllocation();
  }

  ngOnInit() {
    this.spinner.show();
    this.empid = JSON.parse(sessionStorage['AssociatePortal_UserInformation']).employeeId;
    this.roleName = JSON.parse(sessionStorage['AssociatePortal_UserInformation']).roleName;
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
      'isCritical':[true],
      'IsPrimary': '',
      'NotifyAll':[true],
    });

    this.getProjectList();
    this.getEmployeesList();
    this.getAllocationPercentages();
    this.getRoles();
    this.getDates();
   
  }

  private getDates(): void {
    var date = new Date(), y = date.getFullYear(), m = date.getMonth();
    // this.firstDate = new Date(y, m, 1);
    this.firstDate = new Date(y, m, 0);
    if(this.firstDate.getDay()===6){
      this.firstDate.setDate(this.firstDate.getDate() - 1);
    }
    else if(this.firstDate.getDay()===0){
        this.firstDate.setDate(this.firstDate.getDate() - 2);
      }

    this.lastDate = new Date();
  }
  private getProjectList(): void {
    this.masterDataService.GetProjectsList().subscribe((projectResponse: any[]) => {
      this.projectDetails = projectResponse;
      this.projectsList = [];
      // this.projectsList.push({ label: '', value: null });
      this.ManagersList = [];
      this.ManagersList.push({ label: '', value: null });
      this.clientRolesList = [];
      this.clientRolesList.push({ label: '', value: null });
      projectResponse.forEach((pr: ProjectsData) => {
        if (pr.ProjectName.indexOf("Talent Pool") == -1 &&
          this.projectsList.findIndex(x => x.label == pr.ProjectName) == -1  && (pr.ProjectStateId==17 || pr.ProjectStateId==19))
           if(this.roleName=="Training Department Head"){
            if(pr.ProjectName=="Nipuna" || pr.ProjectName=="Training")
            {
              this.projectsList.push({ label: pr.ProjectName, value: pr.ProjectId });
            }
          }
          if(pr.ProjectName.indexOf("Training") == -1)
          {
          if(this.roleName == "Program Manager" && pr.ProgramManagerId==this.empid ) {
            this.projectsList.push({ label: pr.ProjectName, value: pr.ProjectId });
          }
          else {
            if(this.roleName == "HRA" || this.roleName == "HRM") {
            this.projectsList.push({ label: pr.ProjectName, value: pr.ProjectId });
            }
          }
        }
      });
      this.filteredProject = this.allocationForm.valueChanges.pipe(
        startWith(''),
        map((value) => this._filterProject(value))
      );
    }),
      (error: any) => {
        this.snackBar.open('Failed to get Project List', '', {
          duration: 3000,
          panelClass: ['error-alert'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
      };
  }

  private getEmployeesList(): void {
    this._tempAllocationRelease.GetAssociatesForAllocation().subscribe((employees: any[]) => {
      this.employeesList = [];
      // this.employeesList.push({ label: '', value: null });
      employees.forEach((employee: any) => {
        if (this.employeesList.findIndex(x => x.label == employee.EmployeeName) == -1)
          this.employeesList.push({ label: employee.EmployeeName, value: employee.EmployeeId });
      });
      this.tempEmployeesList = this.employeesList
      this.filteredEmployee = this.allocationForm.valueChanges.pipe(
        startWith(''),
        map((value) => this._filterEmployee(value))
      );
      // if (this.empid > 0) {
      //   this.disableAssociateName = true;
      //   this.allocationForm.controls['EmployeeId'].setValue(this.empid);

      //   this.getAllocationHistory(this.empid);
      //   this.validateAllocationPercentage(this.allocationForm.value.AllocationPercentage, this.allocationForm.value.EmployeeId);
      // }
    }),
      (error: any) => {
        this.snackBar.open('Failed to get Employees List', '', {
          duration: 3000,
          panelClass: ['error-alert'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
      };
  }

  private getAllocationPercentages(): void {
    this.masterDataService.GetPercentages().subscribe((res: GenericType[]) => {
    //  this.AllocationOptions = res;
      this.allocationOptions = [];
      this.allocationOptions.push({ label: '', value: null });
      res.forEach((element: GenericType) => {
        this.allocationOptions.push({ label: element.Name, value: element.Id });
        this.AllocationOptions.push({ Percentage: Number(element.Name), AllocationPercentageId: element.Id });
      });
    },
      (error) => {
        this.snackBar.open('Failed to Get Allocation Percentages!', '', {
          duration: 3000,
          panelClass: ['error-alert'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
      }
    );
  }

  validateAllocationPercentage(allocatedpercentageId: number, employeeId: number): void {
    if (allocatedpercentageId == 5){
      this.isPrimaryDisabled = true;
      this.isCriticalDisabled=true 
      this.disableCBR = true;
      this.allocationForm.controls["ClientBillingRoleId"].reset();
      this.allocationForm.controls["ClientBillingRoleName"].reset();
      this.allocationForm.controls["EffectiveDate"].reset();
      this.allocationForm.controls["ClientBillingPercentage"].reset();
      this.allocationForm.controls["BillingStartDate"].reset();
      this.allocationForm.controls["isCritical"].setValue(false);
      this.allocationForm.controls["Billable"].setValue(false);
      this.allocationForm.controls["IsPrimary"].setValue(false);
      this.allocationForm.controls["NotifyAll"].setValue(true);
      this.disableBilling=true;
    }     
    else{
      this.isPrimaryDisabled = false; 
      this.disableBilling=false;
      this.isCriticalDisabled=false; 
    }
    if(employeeId != 0)
      this.getEmployeePrimaryAllocationProject(employeeId);
    let releaseList = this.AllocationOptions.filter((percentage: PercentageDropDown) => {
      return percentage.AllocationPercentageId == allocatedpercentageId;
    });
    if (releaseList && releaseList.length > 0) {
      this.allocatedPercentage = releaseList[0].Percentage;
    }
  }
  private getEmployeePrimaryAllocationProject(employeeId: number): void {
    this._service.GetEmployeePrimaryAllocationProject(employeeId).subscribe((response: AssociateAllocation) => {
      // this.AssociateIsPrimaryAllocation = []
      this.AssociateIsPrimaryAllocation = response;
    },(error)=>{
      this.AssociateIsPrimaryAllocation=undefined
    });
  }

  clearInput(evt: any, fieldName): void {
    if(fieldName=='EffectiveDate'){
      evt.stopPropagation();
      this.allocationForm.get('EffectiveDate').reset();
    }
    if(fieldName=='EmployeeId'){
      evt.stopPropagation();
      this.allocationForm.get('EmployeeId').reset();
      this.employeesListByGivenName=[];
      this.Cancel();
    }
    if(fieldName=='ProjectId'){
      evt.stopPropagation();
      this.allocationForm.get('ProjectId').reset();
    }
    if(fieldName=='RoleMasterId'){
      evt.stopPropagation();
      this.allocationForm.get('RoleMasterId').reset();
    }
  }

  public getRoles() {
    let departmentId: number = 1;
    this._service.GetRolesByDepartmentId(departmentId).subscribe((roledata: RoleDetails[]) => {
      this.spinner.hide();
      this.trRolesList = [];
      // this.trRolesList.push({ label: '', value: null });
      roledata.forEach((e: RoleDetails) => {
        this.trRolesList.push({ label: e.RoleName, value: e.RoleMasterId })
      });
      this.filteredRole = this.allocationForm.valueChanges.pipe(
        startWith(''),
        map((value) => this._filterRole(value))
      );
    }, (error: any) => {
      this.spinner.hide();
      this.snackBar.open('Failed to get roles!', '', {
        duration: 3000,
        panelClass: ['error-alert'],
        horizontalPosition: 'right',
        verticalPosition: 'top',
      });
    }
    );
  }
  displayFn(user: any) {
    return user && user.label ? user.label : '';
  }

  displayEmp(user:any){
    return user && user.label ? user.label : '';
  }

  private _filterRole(value) {
    let filterValue;
    if (typeof value.RoleMasterId === 'number') {
      return this.trRolesList;
    }
    if (value && value.RoleMasterId) {
      if (typeof value.RoleMasterId === 'string') {
        filterValue = value.RoleMasterId.toLowerCase();
      }
      else {
        if (value.RoleMasterId !== null) {
          filterValue = value.RoleMasterId.label.toLowerCase();
        }
      }
      return this.trRolesList.filter((option) =>
        option.label.toLowerCase().includes(filterValue)
      );
    } else {
      return this.trRolesList;
    }
  }
  private _filterProject(value) {
    
    let filterValue;
    if (typeof value.ProjectId === 'number') {
      return this.projectsList;
    }
    if (value && value.ProjectId) {
      if (typeof value.ProjectId === 'string') {
        filterValue = value.ProjectId.toLowerCase();
      }
      else {
        if (value.ProjectId !== null) {
          filterValue = value.ProjectId.label.toLowerCase();
        }
      }
      return this.projectsList.filter((option) =>
        option.label.toLowerCase().includes(filterValue)
      );
    } else {
      return this.projectsList;
    }
  }
  private _filterEmployee(value) {
    
    let filterValue;
    if (typeof value.EmployeeId === 'number') {
      return this.employeesList;
    }
    if (value && value.EmployeeId) {
      if (typeof value.EmployeeId === 'string') {
        filterValue = value.EmployeeId.toLowerCase();
      }
      else {
        if (value.EmployeeId !== null) {
          filterValue = value.EmployeeId.label.toLowerCase();
        }
      }
      return this.employeesList.filter((option) =>
        option.label.toLowerCase().includes(filterValue)
      );
    } else {
      return this.employeesList;
    }
  }

  public getAllocationHistory(employeeId: number) {
    this.selectedEmpId =employeeId;
    this.disableFutureMarkingGrid = true;;
    if (employeeId > 0) {
      this.allocationForm.reset({
        EmployeeId : this.allocationForm.get('EmployeeId').value,
         Billable : this.allocationForm.get('Billable').value,
         isCritical : this.allocationForm.get('isCritical').value,
         AllocationPercentage : this.allocationForm.get('AllocationPercentage').value,
         NotifyAll:  this.allocationForm.get('NotifyAll').value
    });
    // if(this.allocationForm.value.Billable)
    //   this.IsZeroPercentAllocationClicked = false
    this.clientName = "";
    this.formsubmitted = false;
      this._service.GetEmpAllocationHistory(employeeId).subscribe((data: AssociateAllocation[]) => {
        if (data != null && data.length > 0) {
          this.allocationHistory = data;
          this.showAllocationHistoryGrid = true;
          this.requisitionList.AssociateName = data[0].AssociateName;
          this.allocationHistory.forEach(ele => {
           if (ele.ProjectTypeId == 6) {
              this.availableAllocationPercentage = ele.AllocationPercentage;
              this.talentpoolEffectiveDate = moment(ele.EffectiveDate).format('YYYY-MM-DD');
            }
            if(this.IsZeroPercentAllocationClicked){
              this.availableAllocationPercentage = 100;
              this.talentpoolEffectiveDate = null;
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
        this.cbrRoleData = [];
        if (clientRole.length > 0) {
          let cbrdata = [];
          cbrdata = clientRole.filter(x => x.IsActive == true);
          cbrdata.forEach(
            (d: ClientBillingRole) => {
              d.AvailablePositions = d.NoOfPositions - d.AllocationCount;
              if(d.StartDate != null) {
              d.StartDate =
                d.StartDate != null
                  ? moment(d.StartDate).format("YYYY-MM-DD")
                  : null;
              }
            });
          this.cbrRoleData = cbrdata;
        }

      }, (error: any) => {
        this.snackBar.open('Failed to get client billing roles!', '', {
          duration: 3000,
          panelClass: ['error-alert'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
      });
    }
  }



  openDialog(): void {
    this.selectClientBillingRoles();
    let dialogRef;
    if (this.displayAllCBR) {
      dialogRef = this.dialog.open(DialogCBRComponent, {
        disableClose: true,
        hasBackdrop: true,
        width: '800px', height: '700px',
        data: {
          isProjectSelected: this.displayAllCBR,
          isAllSelected: this.displaySelectProject,
          selectedProjectId: this.allocationForm.value.ProjectId,
          cbrList: this.cbrRoleData
        }

      });

    }
    else {
      dialogRef = this.dialog.open(DialogCBRComponent, {
        disableClose: true,
        hasBackdrop: true,
        width: '400px', height: '250px',
        data: {
          isAllSelected: this.displaySelectProject,
        }
      });
    }


    dialogRef.afterClosed().subscribe(result => {
      if(result != null) {
      this.allocationForm.controls['BillingStartDate'].setValue(result.StartDate);
      this.allocationForm.controls['BillingStartDate'].updateValueAndValidity();
      this.allocationForm.controls['ClientBillingPercentage'].setValue(result.ClientBillingPercentage);
      this.allocationForm.controls['ClientBillingPercentage'].updateValueAndValidity();


      this.allocationForm.controls['ClientBillingRoleName'].setValue(result.ClientBillingRoleName);
      this.allocationForm.controls['ClientBillingRoleName'].updateValueAndValidity();

      this.allocationForm.controls['ClientBillingRoleId'].setValue(result.ClientBillingRoleId);
      this.allocationForm.controls['ClientBillingRoleId'].updateValueAndValidity();

      }
    });
  }

  selectClientBillingRoles() {
    if (!this.allocationForm.value.ProjectId){
      this.displaySelectProject = true;
      this.displayAllCBR = false;
    }
    else{
      this.displayAllCBR = true;
      this.displaySelectProject = false;
    }
  }


  selectedCBR() {
    this.allocationForm.controls['ClientBillingRoleId'].setValue(this.CBRselectedData.ClientBillingRoleId);
    this.allocationForm.controls['ClientBillingRoleName'].setValue(this.CBRselectedData.ClientBillingRoleName);
    this.allocationForm.controls['ClientBillingPercentage'].setValue(this.CBRselectedData.ClientBillingPercentage);
    this.clientBillingStartDate = this.CBRselectedData.StartDate;
    this.allocationForm.controls['BillingStartDate'].setValue(this.CBRselectedData.StartDate);
    this.displayAllCBR = false;
  }
  close() {
    this.displaySelectProject = false;
  }

  public getProjectManagerDetails(projectId: number) {
    this.clientName = "";
    this.allocationForm.controls["RoleMasterId"].reset();
    if(this.IsZeroPercentAllocationClicked != true){      
      this.allocationForm.controls["AllocationPercentage"].reset();
    }
    this.allocationForm.controls["ClientBillingRoleId"].reset();
    this.allocationForm.controls["ClientBillingRoleName"].reset();
    this.allocationForm.controls["ClientBillingPercentage"].reset();
    this.allocationForm.controls["BillingStartDate"].reset();
    if (projectId > 0) {
      let projectData: any = this.projectDetails.filter((project: ProjectsData) => { return project.ProjectId == projectId });
      this.clientName = projectData[0].ClientName;
      this.requisitionList.ProjectName = projectData[0].ProjectName;
      if (this.clientName.indexOf(InternalClient[InternalClient.SenecaGlobal]) != -1) {
        // this.disableBilling = true;
        // this.allocationForm.controls["Billable"].setValue(false);
      }
      else {
        // this.allocationForm.controls["Billable"].setValue(true);
        if(this.IsZeroPercentAllocationClicked != true){
        this.disableBilling = false;
        this.isPrimaryDisabled = false; 
        this.isCriticalDisabled = false
        }
      }
      this.allocationForm.controls["ReportingManagerId"].reset();
      this.GetReportingManagers(projectId);
      this.getClientBillingRolesByProject(projectId);
    }
  }

  public getClientBillingRoles() {
    this._service.GetClientBillingRoles().subscribe((clientRole: ClientBillingRole[]) => {
      this.clientRolesList.push({ label: '', value: null });
      clientRole.forEach((e: ClientBillingRole) => {
        this.clientRolesList.push({ label: e.ClientBillingRoleCode, value: e.ClientBillingRoleId })
      });
    }, (error: any) => {
      this.snackBar.open(error.error.Message, '', {
        duration: 3000,
        panelClass: ['error-alert'],
        horizontalPosition: 'right',
        verticalPosition: 'top',
      });
    });
  }




  public getInternalBillingRoles() {
    this._service.GetInternalBillingRoles().subscribe((clinetRole: InternalBillingRole[]) => {
      this.internalRolesList.push({ label: '', value: null });
      clinetRole.forEach((e: InternalBillingRole) => {
        this.internalRolesList.push({ label: e.InternalBillingRoleCode, value: e.InternalBillingRoleId })
      });
    }, (error: any) => {
      this.snackBar.open('Failed to get internal billing roles!', '', {
        duration: 3000,
        panelClass: ['error-alert'],
        horizontalPosition: 'right',
        verticalPosition: 'top',
      });
    });
  }
  public GetReportingManagers(projectId: number) {
    this.ManagersList = [];
    if (projectId > 0) {
      this._assignManagerservice.GetManagersByProjectId(projectId).subscribe((data: any) => {
        if (data.ReportingManagerId != null)
          this.ManagersList.push({ label: data.ReportingManagerName, value: data.ReportingManagerId });

        if (data.LeadId != null && data.ReportingManagerId != data.LeadId)
          this.ManagersList.push({ label: data.LeadName, value: data.LeadId });

        if (data.ProgramManagerId != null && data.ProgramManagerId != data.LeadId && data.ProgramManagerId != data.ReportingManagerId)
          this.ManagersList.push({ label: data.ProgramManagerName, value: data.ProgramManagerId });

      }, (error: any) => {
        this.snackBar.open('Failed to get reporting Managers!', '', {
          duration: 3000,
          panelClass: ['error-alert'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
      });
    }
  }

  public IsBillable(event: any) {
    if (event.checked) {
      this.allocationForm.controls["isCritical"].setValue(true);
      this.disableCBR = false;
    }
    else {
      this.allocationForm.controls["isCritical"].setValue(false);
      this.allocationForm.controls["Billable"].setValue(false);
      this.disableCBR = true;
      this.allocationForm.controls["ClientBillingRoleId"].reset();
      this.allocationForm.controls["ClientBillingRoleName"].reset();
      this.allocationForm.controls["EffectiveDate"].reset();
      this.allocationForm.controls["ClientBillingPercentage"].reset();
      this.allocationForm.controls["BillingStartDate"].reset();
    }
  }
  public onlyNumbers(event: any) {
    this._commonService.onlyNumbers(event)
  };

  public Allocate(requisitionList: any) {
    this.formsubmitted = true;

    this.requisitionList.ClientBillingPercentage = requisitionList.ClientBillingPercentage;
    this.requisitionList.ClientBillingRoleId = requisitionList.ClientBillingRoleId;
    this.requisitionList.EmployeeId = requisitionList.EmployeeId;
    this.requisitionList.ReportingManagerId = requisitionList.ReportingManagerId;
    this.requisitionList.Billable = requisitionList.Billable;

    if (this.requisitionList.Billable == true) {
      if (!this.requisitionList.ClientBillingPercentage || !this.requisitionList.ClientBillingRoleId){
        this.allocationForm.get('ClientBillingRoleName').setValidators(Validators.required);
        this.allocationForm.get('ClientBillingPercentage').setValidators(Validators.required);
      } 
    }
    else {

      this.allocationForm.get('ClientBillingRoleName').clearValidators();
      this.allocationForm.get('ClientBillingPercentage').clearValidators();
    }
    this.allocationForm.controls['ClientBillingRoleName'].updateValueAndValidity();
    this.allocationForm.controls['ClientBillingPercentage'].updateValueAndValidity();
    if (this.allocationForm.valid) {

      requisitionList.EmployeeId = requisitionList.EmployeeId.value;
      requisitionList.RoleMasterId = requisitionList.RoleMasterId.value;
      requisitionList.ProjectId = requisitionList.ProjectId.value;
      if (this.allocatedPercentage <= this.availableAllocationPercentage) {
       
        if (requisitionList.isCritical != true) requisitionList.isCritical = false;
        if (requisitionList.NotifyAll != true) requisitionList.NotifyAll = false;
        this.requisitionList.TalentRequisitionId = requisitionList.TalentRequisitionId = 1; //Written as it is temporary screen and Talent Management module yet to be implemented.
        let validDates: boolean = false;
        requisitionList.EffectiveDate = moment(requisitionList.EffectiveDate).format('YYYY-MM-DD')
        if (this.talentpoolEffectiveDate != null && this.talentpoolEffectiveDate != undefined) {
          validDates = this.CheckDates(this.talentpoolEffectiveDate, requisitionList.EffectiveDate);
        }
        else {
          if(this.IsZeroPercentAllocationClicked){
            this.invalidDates = false;
            validDates = true;
          }
          else{
            this.snackBar.open('Band width is not available!', '', {
              duration: 3000,
              panelClass: ['error-alert'],
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
            return;
          }
        }
        if ((requisitionList.Billable != true) && (this.clientBillingStartDate != undefined) &&
          (!this._commonService.IsValidDate(
            this.clientBillingStartDate,
            requisitionList.EffectiveDate)
          )
        ) {

          this.snackBar.open('Effective Date should be greater than or equal to Client Billing Start date', '', {
            duration: 3000,
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
          return;
        }
        if (this.requisitionList.EmployeeId != this.requisitionList.ReportingManagerId) {
          let primaryValidation = this.ValidateIsPrimary(requisitionList);
          let primaryAllocationProject = this.AssociateIsPrimaryAllocation && this.AssociateIsPrimaryAllocation.ProjectId
          let primaryProjectAllocationPercentage = this.AssociateIsPrimaryAllocation && this.AssociateIsPrimaryAllocation.AllocationPercentage
          if(primaryValidation && requisitionList.IsPrimary == true && primaryAllocationProject != 6 && primaryAllocationProject != undefined && primaryProjectAllocationPercentage == 50 && requisitionList.AllocationPercentage == 2){
            this.snackBar.open("The Associate has already allocated to a primary project.", '', {
              duration: 3000,
              panelClass: ['error-alert'],
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
            return;
          }
          if (primaryValidation && validDates != false) {
            this.spinner.show();
            this._service.ResourceAllocate(requisitionList).subscribe((data: any) => {
              if (data.IsSuccessful == true) {
                this.spinner.hide();
                this.ManagersList = [];
                this.ManagersList.push({ label: '', value: null });
                this.snackBar.open('Associate allocated successfully! ', '', {
                  duration: 3000,
                  horizontalPosition: 'right',
                  verticalPosition: 'top',
                });
                this.deleteFromTagAssociateList(this.tagAssociateId);
                this.getEmployeesList();
                this.Cancel();
              }
              else if (data.IsSuccessful == false) {
                this.spinner.hide();
                if(data.Message == "The Associate has already allocated a primary project."){
                  this.snackBar.open("The Associate has already allocated to a primary project.", '', {
                    duration: 3000,
                    panelClass: ['error-alert'],
                    horizontalPosition: 'right',
                    verticalPosition: 'top',
                  });
                }
                else{
                  this.snackBar.open('The Associate has already allocated to the selected project', '', {
                    duration: 3000,
                    panelClass: ['error-alert'],
                    horizontalPosition: 'right',
                    verticalPosition: 'top',
                  });
                }
              }
            }
            );
          }
        }
        else {
          this.snackBar.open('Associate and Reporting Manager cannot be the same!', '', {
            duration: 3000,
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
        }
      }
      else {
        this.snackBar.open('Band width is not available!', '', {
          duration: 3000,
          panelClass: ['error-alert'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
      }
    }

  }


  deleteFromTagAssociateList(tagAssociateId: number): void {
    if (tagAssociateId > 0) {
      this.tagAssociateService.DeleteTagList(tagAssociateId).subscribe((res: number) => {
        if (res == 1) {
          setTimeout(() => {
            this.router.navigate(['/project/tagAssociate/']);
          }, 3000);
        }
        else {
          this.snackBar.open('Unable to delete record from Tag list.', '', {
            duration: 3000,
            panelClass: ['error-alert'],
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
        }
      },
        (error: any) =>
          this.snackBar.open('Unable to delete record from Tag list.', '', {
            duration: 3000,
            panelClass: ['error-alert'],
            horizontalPosition: 'right',
            verticalPosition: 'top',
          }));
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
    if (this.AssociateIsPrimaryAllocation && Object.keys(this.AssociateIsPrimaryAllocation).length > 0 && requisitionListAllocationPercentage) {
      if (this.AssociateIsPrimaryAllocation.AllocationPercentage < requisitionListAllocationPercentage && (requisitionList.IsPrimary == false || requisitionList.IsPrimary == null)) {
        this.snackBar.open('Please check Primary.', '', {
          duration: 3000,
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
        return false;
      }
      else if (this.AssociateIsPrimaryAllocation.AllocationPercentage == requisitionListAllocationPercentage &&
        (requisitionList.IsPrimary == false || requisitionList.IsPrimary == null)) {
        requisitionList.IsPrimary = false;
        if (this.allocationForm.valid && !this.invalidDates) {
          this._service.ResourceAllocate(requisitionList).subscribe((data : any) => {
            if (data == true) {
              this.ManagersList = [];
              this.ManagersList.push({ label: '', value: null });
              this.snackBar.open('Associate allocated successfully! ', '', {
                duration: 3000,
                horizontalPosition: 'right',
                verticalPosition: 'top',
              });
              this.deleteFromTagAssociateList(this.tagAssociateId);
              this.getEmployeesList();
              this.Cancel();
            }
            else {
              if(data.IsSuccessful == true) {
                this.ManagersList = [];
                this.ManagersList.push({ label: '', value: null });
                this.snackBar.open('Associate allocated successfully! ', '', {
                  duration: 3000,
                  horizontalPosition: 'right',
                  verticalPosition: 'top',
                });
                this.deleteFromTagAssociateList(this.tagAssociateId);
                this.getEmployeesList();
              }
              if (data.IsSuccessful == false) {
                this.snackBar.open('The Associate has already allocated to the selected project', '', {
                  duration: 3000,
                  panelClass: ['error-alert'],
                  horizontalPosition: 'right',
                  verticalPosition: 'top',
                });
              }
              this.Cancel();
            }
          }, (error) => {
            this.snackBar.open('Fail to allocate  same project', '', {
              duration: 3000,
              panelClass: ['error-alert'],
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
            return false;
          });
        }
        else {
          // this.snackBar.open('Please check Primary', '', {
          //   duration: 3000,
          //   panelClass: ['error-alert'],
          //   horizontalPosition: 'right',
          //   verticalPosition: 'top',
          // });
          return false;
        }
      }
      else if (this.AssociateIsPrimaryAllocation.AllocationPercentage > requisitionListAllocationPercentage && requisitionList.IsPrimary == true) {
        this.snackBar.open('Please uncheck Primary.', '', {
          duration: 3000,
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
        return false;
      }
      else {
        return true;
      }
    }
    else {
      if ((requisitionList.IsPrimary == false || requisitionList.IsPrimary == null)  && requisitionListAllocationPercentage > 0) {
        this.snackBar.open('Please check Primary.', '', {
          duration: 3000,
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
        return false;
      }
      else {
        return true;
      }
    }
  }

  public Cancel() {
    this.employeesListByGivenName=[];
    this.allocationForm.reset({
      NotifyAll : true
    });
    this.formsubmitted = false;
    this.disableAssociateName = false;
    this.showAllocationHistoryGrid = false;
    this.disableCBR = true;
    this.allocationHistory = [];
    setTimeout(() => this.formGroupDirective.resetForm({
      NotifyAll : true
    }), 0);
    this.availableAllocationPercentage = 0;
    this.talentpoolEffectiveDate = null;
    this.clientName = "";
    this.isPrimaryDisabled = false;
    this.disableBilling = false;
    this.isCriticalDisabled=false;
    this.requisitionList = new AssociateAllocation();
    this.ManagersList = [];
    this.IsZeroPercentAllocationClicked = false;
    this.employeesList = this.tempEmployeesList;
    this.disableFutureMarkingGrid = false;
  }

  private CheckDates = function (fromDate: any, toDate: any): boolean {
    if ((fromDate != null || fromDate != undefined) && (toDate != null && toDate != undefined)) {
      if (Date.parse(fromDate) <= Date.parse(toDate)) {
        this.invalidDates = false;
        return true
      }
      else {
        this.snackBar.open('Invalid Effective date, please check associate`s allocations.', '', {
          duration: 3000,
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
        this.invalidDates = true;
        return false
      }

    }
  }

  IsZeroPercentAllocation(event: any){
    if (event.checked) {
      this.allocationForm.reset();
      this.IsZeroPercentAllocationClicked = true
      this.employeesList=[]
      let percentageId:number
      this.allocationOptions.forEach((element) => {
        if (element.label == '0') {
          percentageId = element.value
          this.allocationForm.controls["AllocationPercentage"].setValue(percentageId);
        }  
      });
      if(this.allocationForm.value.EmployeeId == null)
        this.validateAllocationPercentage(percentageId,0)
      this.GetAssociatesForDropdown();
    }
    else{
      // this.employeesListByGivenName=[]
      this.employeesList = this.tempEmployeesList
      this.Cancel()

      // this.allocationForm.controls["AllocationPercentage"].setValue(null);
      // this.isPrimaryDisabled = false; 
      // this.disableBilling=false;
      // this.isCriticalDisabled=false
      // this.IsZeroPercentAllocationClicked = false
    }

  }

  GetAssociatesForDropdown() {
    this.employeesListByGivenName=[]
    this.employeesList = [];
    this._assignReportingManagerService.GetAllAssociatesList().subscribe((res: any) => {
      res.forEach((employee: any) => {
        if (this.employeesListByGivenName.findIndex(x => x.label == employee.EmployeeName) == -1)
          this.employeesListByGivenName.push({ label: employee.EmpName, value: employee.EmpCode });
        this.employeesList = this.employeesListByGivenName;
    });
     this.filteredEmployee = this.allocationForm.valueChanges.pipe(
       startWith(''),
       map((value) => this._filterEmployee(value))
     );
   })



  }
  _filterEmployees(value){
    let filterValue;
    if (typeof value.EmpCode === 'number') {
      return this.employeesList;
    }
  }
}
