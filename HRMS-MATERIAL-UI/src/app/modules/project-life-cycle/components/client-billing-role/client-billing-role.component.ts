import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, FormGroupDirective, Validators } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import * as moment from 'moment';
import { Subscription } from 'rxjs';
import { CommonService } from 'src/app/core/services/common.service';
import { PercentageDropDown } from 'src/app/modules/master-layout/models/associateallocation.model';
import { ProjectsData } from 'src/app/modules/master-layout/models/projects.model';
import { MasterDataService } from 'src/app/modules/master-layout/services/masterdata.service';
import { themeconfig } from 'src/themeconfig';
import { ClientBillingRoleDetails } from '../../models/client-billing-role.model';
import { ClientBillingRoleService } from '../../services/client-billing-roles.service';
import { ProjectCreationService } from '../../services/project-creation.service';
import { AllocationDetails, ResourceAllocationDetails } from '../../../reports/models/resourcereportbyproject.model';
import { ResourceReportService } from '../../../reports/services/resource-report.service';
import { MatDialog } from '@angular/material/dialog';
import { CloseCbrDialogComponent } from '../close-cbr-dialog/close-cbr-dialog.component';
import { MessageDialogComponent } from '../message-dialog/message-dialog.component';
import { AllocatedResourcesDialogComponent } from '../allocated-resources-dialog/allocated-resources-dialog.component';
import { NavService } from 'src/app/modules/master-layout/services/nav.service';
import { DeleteDialogComponent } from '../delete-dialog/delete-dialog.component';
import { ConfirmDialogComponent } from '../confirm-dialog/confirm-dialog.component';

interface SelectItem {
  value: number;
  label: string;
}

@Component({
  selector: 'app-client-billing-role',
  templateUrl: './client-billing-role.component.html',
  styleUrls: ['./client-billing-role.component.scss']
})
export class ClientBillingRoleComponent implements OnInit {

  themeConfigInput = themeconfig.formfieldappearances;

  projectsList: SelectItem[];
  clientBillingRoleOptions: SelectItem[] = [];

  formSubmitted: boolean = false;
  saveRoleData: boolean = true;
  displayDialog: boolean = false;
  editMode: boolean;
  isDrafted: boolean = false;
  disableProject: boolean = false;
  submitted = false;

  clientBillingRolesForm: FormGroup;
  closeRoleData: FormGroup;

  projectStartDate: string;
  projectState: string;
  currentRole = "";
  projectId: number;
  dashboard: string;
  cancelBtnLabel = "Clear";
  positions: number;
  displayCloseRoleErrorDialog = false;
  displayCloseCBRDialog: boolean = false;
  showAllocatedPositionsList: boolean = false;

  closableRole = new ClientBillingRoleDetails();

  clientBillingRoleData: ClientBillingRoleDetails[];
  billableResourceAllocationList: ResourceAllocationDetails[] = [];

  clientBillingRole: ClientBillingRoleDetails;
  selectedBillingRole: ClientBillingRoleDetails;

  minDate = new Date();
  minClientBillingStartDate = new Date();
  maxClientBillingStartDate = new Date();

  submitBtnLabel = "Review and Submit";

  editModeSubscription: Subscription;
  projectStateSubscription: Subscription;
  projectIdSubscription: Subscription;

  disablestyles = { 'opacity': '0.6', 'pointer-events': 'none' }

  displayedColumns: string[];

  dataSource: MatTableDataSource<ClientBillingRoleDetails>;
  allocatedResourcesDataSource: MatTableDataSource<ResourceAllocationDetails>;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  @ViewChild(FormGroupDirective) formGroupDirective: FormGroupDirective;

  constructor(private fb: FormBuilder,
    private _commonService: CommonService,
    private _snackBar: MatSnackBar,
    private _clientBillingRoleService: ClientBillingRoleService,
    private _projectCreationService: ProjectCreationService,
    private _masterDataService: MasterDataService,
    private _resourceReportsService: ResourceReportService,
    public dialog: MatDialog,
    public navService: NavService) {

    this.navService.changeSearchBoxData('');

    this.navService.currentSearchBoxData.subscribe(responseData => {
      this.applyFilter(responseData);
    });
  }

  ngOnInit(): void {
    this.createCloseRoleform();
    this.editModeSubscription = this._projectCreationService.GetEditMode().subscribe(data => {
      this.editMode = data;
    });
   
    this.currentRole = JSON.parse(sessionStorage["AssociatePortal_UserInformation"])
      .roleName;
      this.projectStateSubscription = this._projectCreationService.GetProjectState().subscribe(data => {
        this.projectState = data;
        if(this.currentRole == 'Department Head'){
          this.dashboard = 'ProjectDashboard';
          if(this.projectState == 'SubmittedForApproval'){
            this.dashboard = 'DHDashboard'
          }
        }
        else{
          this.dashboard = 'ProjectDashboard';
        }
        
      });
    this.initiateForm();
    this.projectIdSubscription = this._projectCreationService.GetProjectId().subscribe((data: any) => {

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

  getdisplayedColumns(): string[] {
    if (this.canEditOrDelete()) {
      this.displayedColumns = ['ClientBillingRoleName', 'NoOfPositions', 'AllocationCount', 'AvailablePositions', 'StartDate', 'EndDate', 'ClientBillingPercentage', 'Edit', 'Delete', 'CloseRole', 'Clone'];
      return this.displayedColumns;
    }
    else {
      this.displayedColumns = ['ClientBillingRoleName', 'NoOfPositions', 'AllocationCount', 'AvailablePositions', 'StartDate', 'EndDate', 'ClientBillingPercentage', 'CloseRole', 'Clone'];
      return this.displayedColumns;
    }
  }

  createCloseRoleform() {
    var date = new Date(), y = date.getFullYear(), m = date.getMonth();
    this.minDate = new Date(y, m - 1, 1); //new Date(this.closableRole.StartDate);
    this.minClientBillingStartDate = new Date(y, m - 1, 1);
    this.maxClientBillingStartDate = new Date();
    this.closeRoleData = new FormGroup({
      BillingEndDate: new FormControl(null, [Validators.required]),
       Reason: new FormControl(null, Validators.required)
    });
  }

  CloneData(cBRData: ClientBillingRoleDetails) {

    this.patchDataToForm(cBRData);
  }

  initiateForm() {
    //to initiate the form
    this.clientBillingRolesForm = this.fb.group({
      ProjectId: [null, [Validators.required]],
      ClientBillingRoleName: [null, [Validators.required,
      Validators.pattern("^(?!.*--)^(?!-[0-9])[A-Za-z0-9-.()&/, ]+$")]],
      NoOfPositions: [, [Validators.required, Validators.pattern("^[1-9][0-9]*")]],
      StartDate: [null, [Validators.required]],
      EndDate: [null],
      ClientBillingPercentage: ['', [Validators.required]]
    })
  }

  getProjectsList(): void {
    //to get list of all projects
    let userRole = JSON.parse(sessionStorage["AssociatePortal_UserInformation"])
      .roleName;
    let employeeId = JSON.parse(
      sessionStorage["AssociatePortal_UserInformation"]
    ).employeeId;
    this.projectsList = [];
    
    // this.dashboard = 'ProjectDashboard'; 
    //this.projectsList.push({ label: "Select Project ", value: null });
    if (userRole != "" && employeeId > 0) {
      this._projectCreationService
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
      this._clientBillingRoleService
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
                    ? moment(d.StartDate).format("MM/DD/YYYY")
                    : null;
                d.EndDate =
                  d.EndDate != null
                    ? moment(d.EndDate).format("MM/DD/YYYY")
                    : null;
              }
            );
          }
          this.dataSource = new MatTableDataSource(this.clientBillingRoleData);
          this.dataSource.paginator = this.paginator;
          this.dataSource.sort = this.sort;
          this.dataSource.sortingDataAccessor = (data: any, sortHeaderId: string): string => {
            if (typeof data[sortHeaderId] === 'string') {
              return data[sortHeaderId].toLocaleLowerCase();
            }
          
            return data[sortHeaderId];
          };
        }),
        error => { };
    }
  }

  private getClientBillingRolePercentages(): void {
    //to populate client billing role percentage drop-down
    this._masterDataService.GetAllocationPercentages().subscribe(
      (res: PercentageDropDown[]) => {
        this.clientBillingRoleOptions = [];
        this.clientBillingRoleOptions.push({
          label: "Select Client Billing %",
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
        this._snackBar.open('Failed to get Client Billing Role Percentages', 'x', {
          duration: 3000,
          panelClass:['error-alert'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
      }
    );
  }


  clearInput(evt: any, fieldName): void {
    if(fieldName=='StartDate'){
      evt.stopPropagation();
      this.clientBillingRolesForm.get('StartDate').reset();
    }
    if(fieldName=='EndDate'){
      evt.stopPropagation();
      this.clientBillingRolesForm.get('EndDate').reset();
    }
  }
  
  onEmpty(event : any){
    if (event.target.value.length === 0)
      this.clientBillingRolesForm.controls.ClientBillingRoleName.patchValue(null);
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
        !this._commonService.IsValidDate(
          this.projectStartDate,
          this.clientBillingRolesForm.value.StartDate
        )
      ) {

        this._snackBar.open('Client billing Start date should be greater than or equal to Project Start date', 'x', {
          duration: 3000,
          // panelClass:['error-alert'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
        return;
      }
      if (
        this.clientBillingRolesForm.value.StartDate != null &&
        this.clientBillingRolesForm.value.EndDate != null
      ) {

        if (
          !this._commonService.IsValidDate(
            this.clientBillingRolesForm.value.StartDate,
            this.clientBillingRolesForm.value.EndDate
          )
        ) {

          this._snackBar.open('Client Billing Start Date should be less than or equal to Client Billing End Date', 'x', {
            duration: 3000,
            //  panelClass:['error-alert'],
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
          return;
        }
      }
      this._clientBillingRoleService
        .SaveClientBillingRole(this.clientBillingRolesForm.value)
        .subscribe(res => {
          if (res == 1) {
            this.getClientBillingRolesByProjectId(
              this.clientBillingRolesForm.value.ProjectId
            );
            this._snackBar.open('Client Billing Role added successfully.', 'x', {
              duration: 3000,
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
            this._projectCreationService.SetSeletedTab(2);
            this.reset();
          }
          else if (res == -1) {
            this._snackBar.open('Client Billing Role Already Exists', 'x', {
              duration: 3000,
              panelClass:['error-alert'],
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
            return;
          }
          // this.reset();
        }),
        error => {
          this._snackBar.open('Unable to add Client Billing Role', 'x', {
            duration: 3000,
            panelClass: ['error-alert'],
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
        };
      // this.reset();
    } else return;
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
        !this._commonService.IsValidDate(
          this.projectStartDate,
          this.clientBillingRole.StartDate
        )
      ) {
        this._snackBar.open('Client billing Start date should be greater than or equal to Project Start date', 'x', {
          duration: 3000,
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
        return;
      }
      if (
        this.clientBillingRole.StartDate != null &&
        this.clientBillingRole.EndDate != null
      ) {
        if (
          !this._commonService.IsValidDate(
            this.clientBillingRole.StartDate,
            this.clientBillingRole.EndDate
          )
        ) {
          this._snackBar.open('Start Date should be less than or equal to End Date', 'x', {
            duration: 3000,
            horizontalPosition: 'right',
            verticalPosition: 'top',
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

          const dialogRef = this.dialog.open(MessageDialogComponent, {
            disableClose: true,
            hasBackdrop: true,
            data: { heading: "Confirmation", message: "Update SOW details, if available." }
          });

          dialogRef.afterClosed().subscribe(result => {
            this.isSowPresent();
          });

        }
        else {
          this._snackBar.open('Nothing to save', 'x', {
            duration: 3000,
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
        }
      };
      this.reset();
    }
  }

  isSowPresent() {
    //when sow details are present 
    this._projectCreationService.SetSeletedTab(1);
    this.updateClientBillingDetails(this.clientBillingRole);
    this.reset();
    this.displayDialog = false;
    return true;
  }

  updateClientBillingDetails(clientBillingRole) {

    //service call to update client billing role
    this._clientBillingRoleService
      .UpdateClientBillingRole(clientBillingRole)
      .subscribe(res => {
        if (res >= 1) {
          this._snackBar.open('Client Billing Role updated successfully.', 'x', {
            duration: 3000,
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
          this.getClientBillingRolesByProjectId(
            this.clientBillingRolesForm.value.ProjectId
          );

          this.reset();
        } else if (res == 0) {
          this._snackBar.open('No. of positions cannot be decreased', 'x', {
            duration: 3000,
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
        }
        else if (res == -1) {
          this._snackBar.open('Client Billing Role already exists', 'x', {
            duration: 3000,
            panelClass:['error-alert'],
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
        }
        else if (res == -2) {
          this._snackBar.open('Cannot decrease positions', 'x', {
            duration: 3000,
            // panelClass:['error-alert'],
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
        }
      }),
      error => {
        this._snackBar.open('Unable to update Client Billing Role', 'x', {
          duration: 3000,
          panelClass:['error-alert'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
      };
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
        this._snackBar.open('No records found.', 'x', {
          duration: 3000,
          panelClass:['error-alert'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
      }
      this.allocatedResourcesDataSource = new MatTableDataSource(this.billableResourceAllocationList);
      this.allocatedResourcesDataSource.paginator = this.paginator;
      this.allocatedResourcesDataSource.sort = this.sort;
      this.allocatedResourcesDataSource.sortingDataAccessor = (data: any, sortHeaderId: string): string => {
        if (typeof data[sortHeaderId] === 'string') {
          return data[sortHeaderId].toLocaleLowerCase();
        }
      
        return data[sortHeaderId];
      };
      const dialogRef = this.dialog.open(AllocatedResourcesDialogComponent, {
        disableClose: true,
        hasBackdrop: true,
        height: '400px',
        width: '800px',
        data: { datasource: this.allocatedResourcesDataSource, paginator: this.allocatedResourcesDataSource.paginator, sort: this.allocatedResourcesDataSource.sort }
      });
      dialogRef.afterClosed().subscribe(result => {
      });

    }),
      (error: any) => {
        this._snackBar.open('Failed to get Client details.', 'x', {
          duration: 3000,
          panelClass:['error-alert'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
      };
  }


  canEditOrDelete() {
    if (this.projectState == 'Drafted' || this.projectState == 'Created')
      return true;
    if (this.projectState == 'SubmittedForApproval' && this.currentRole == 'Department Head')
      return true;

    return false;
  }

  disableEditAndDelete(cBRData: ClientBillingRoleDetails) {
    if ((cBRData.IsActive != undefined || cBRData.IsActive != null) && cBRData.IsActive == false)
      return this.disablestyles;
    return { 'cursor': 'pointer' };

  }

  editBillingRole(billingRoleData: ClientBillingRoleDetails) {

    //invokes when we click on edit button
    this.saveRoleData = false;
    this.disableProject = true;
    this.cancelBtnLabel = "Cancel";
    this.patchDataToForm(billingRoleData);

  }

  patchDataToForm(billingRoleData: ClientBillingRoleDetails) {

    // if(billingRoleData.StartDate != null)
    //     billingRoleData.StartDate = new Date(moment(billingRoleData.StartDate).format("MM/DD/YYY")).toString();
    this.clientBillingRolesForm.patchValue({
      ClientBillingRoleName: billingRoleData.ClientBillingRoleName,
      NoOfPositions: billingRoleData.NoOfPositions,

      ClientBillingPercentage: (billingRoleData.ClientBillingPercentage) / 25,
      StartDate:
        billingRoleData.StartDate != null
          ? new Date(moment(billingRoleData.StartDate).format("MM/DD/YYYY"))
          : null,
      EndDate:
        billingRoleData.EndDate != null
          ? new Date(moment(billingRoleData.EndDate).format("MM/DD/YYYY"))
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

  canCloseRole() {
    if (this.projectState == 'Drafted' || this.projectState == 'SubmittedForApproval')
      return false;
    return true;
  }

  canShowActions(cBRData: ClientBillingRoleDetails) {
    if ((cBRData.IsActive != undefined || cBRData.IsActive != null) && cBRData.IsActive == false) {
      return false;
    }
    return true;
  }

  closeCBRData(cBRData: ClientBillingRoleDetails) {
    if (cBRData.AllocationCount > 0) {
      this.displayCloseRoleErrorDialog = true;
      const dialogRef = this.dialog.open(MessageDialogComponent, {
        disableClose: true,
        hasBackdrop: true,
        data: { heading: "Close Client Billing Record", message: "This Role has active allocation(s). First Release associate(s)" }
      });
      dialogRef.afterClosed().subscribe(result => {
      });
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


    const dialogRef = this.dialog.open(CloseCbrDialogComponent, {
      disableClose: true,
      hasBackdrop: true,
      data: { BillingEndDate: this.closableRole.EndDate, BillingStartDate: this.closableRole.StartDate }
    });

    dialogRef.afterClosed().subscribe(result => {
      this.closeRoleData.value.BillingEndDate = result.BillingEndDate;
      this.closeRoleData.value.Reason=result.Reason;
      this.closeClientBillingRecord();
    });
  }

  closeClientBillingRecord() {

    this.submitted = true;
    if (this.closeRoleData.value.BillingEndDate != null) {

      this.closeRoleData.value.BillingEndDate = moment(this.closeRoleData.value.BillingEndDate).format("YYYY-MM-DD");
      this._clientBillingRoleService.closeClientBillingRecord(this.closableRole.ClientBillingRoleId, this.closeRoleData.value.BillingEndDate, this.closeRoleData.value.Reason).subscribe(res => {
        if (res > 0) {
          this.reset();
          this.getClientBillingRolesByProjectId(this.projectId);
          this._snackBar.open('Closed Client Billing Role Successfully.', 'x', {
            duration: 3000,
            //  panelClass:['success-alert'],
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });

        }
        if (res == -1) {
          this.reset();
          this._snackBar.open('There are active allocations you can close it.', 'x', {
            duration: 3000,
            //  panelClass:['error-alert'],
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
        }
      },
        (error) => {
          this.reset();
          this._snackBar.open('Closing Failed.', 'x', {
            duration: 3000,
             panelClass:['error-alert'],
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
        });

    }
  }

  ngOnDestroy() {
    this.projectIdSubscription.unsubscribe();
    this.editModeSubscription.unsubscribe();
    this.projectStateSubscription.unsubscribe();
  }

  deleteDialog(rowData) {
    // method to open delete dialog
    const dialogRef = this.dialog.open(DeleteDialogComponent, {
      disableClose: true,
      hasBackdrop: true,
      data: {message: "Do you want to delete Client Billing Role?" }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result)
        this.deleteBillingRole(rowData);
    });

  }


  deleteBillingRole(rowData) {
    //to delete a client billing role.
    this._clientBillingRoleService
      .DeleteClientBillingRole(rowData.ClientBillingRoleId)
      .subscribe((res: boolean) => {
        if (res == true) {
          this.getClientBillingRolesByProjectId(
            this.clientBillingRolesForm.value.ProjectId
          );
          this._snackBar.open('Deleted Client Billing Role.', 'x', {
            duration: 3000,
            //  panelClass:['success-alert'],
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
        } else
          this._snackBar.open('Deleting Failed.', 'x', {
            duration: 3000,
            panelClass:['error-alert'],
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
        this.reset();
      });
  }

  clearClientDialog(){
    let clearMessage: string = 'Do you want to clear?';
    if (this.cancelBtnLabel == 'Cancel'){
      clearMessage = 'Do you want to cancel?'
    }
    const dialogRef = this.dialog.open(ConfirmDialogComponent,{
      disableClose: true,
      hasBackdrop: true,
      data:{
        message: clearMessage,        
      }
    });
    dialogRef.afterClosed().subscribe(result => {
      if(result){
        this.reset();
      }
    });
  }


  reset() {
    //reset all the values
    this.formSubmitted = false;
    this.saveRoleData = true;
    this.cancelBtnLabel = "Clear";
    this.clientBillingRolesForm.reset({
      // ClientBillingPercentage: this.clientBillingRolesForm.get('ClientBillingPercentage').value,
      ProjectId: this.clientBillingRolesForm.get('ProjectId').value
    });
    this.formGroupDirective.resetForm({
      // ClientBillingPercentage: this.clientBillingRolesForm.get('ClientBillingPercentage').value,
      ProjectId: this.clientBillingRolesForm.get('ProjectId').value
    });
  }

  applyFilter(event: Event) {
    if (event) {
      const filterValue = (event.target as HTMLInputElement).value;
      this.dataSource.filter = filterValue.trim().toLowerCase();
    } else {
      this.dataSource = new MatTableDataSource(this.clientBillingRoleData);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;

    }
  }

}
