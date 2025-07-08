import { Component, OnInit, ViewChild } from '@angular/core';
import { ProjectCreationService } from '../../services/project-creation.service';
import { ProjectsData } from '../../../master-layout/models/projects.model';
import { ClientBillingRoleService } from '../../services/client-billing-roles.service';
import * as moment from "moment";
import { SowService } from '../../services/sow.service';
import { SOW } from '../../../admin/models/sow.model';
import { ClientBillingRoleDetails } from '../../models/client-billing-role.model';
import { Subscription } from 'rxjs';
import { Router, ActivatedRoute } from '@angular/router';
import { DeliveryHeadService } from '../../../shared/services/delivery-head.service';
import * as servicePath from "../../../../core/service-paths";
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ConfirmDialogComponent } from '../confirm-dialog/confirm-dialog.component';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';


@Component({
  selector: 'app-view-project',
  templateUrl: './view-project.component.html',
  styleUrls: ['./view-project.component.scss'],
  providers: []
})
export class ViewProjectComponent implements OnInit {
  projectId: number;
  PageSize: number = 0;
  PageDropDown: number[] = [];
  resources = servicePath.API.PagingConfigValue;
  projectData: ProjectsData;
  projectStateSubscription: Subscription;
  projectState: string;
  selectedTabSubscription: Subscription;
  clientBillingRoleData = [];
  SOWData: SOW[] = [];
  dashboard: string;
  isDrafted: boolean = false;
  projectIdSubscription: Subscription;
  canApprove: boolean;
  EmpId: number;
  hideBack: boolean = false;
  showback: boolean = false;
  displayedColumnsSOW: string[] = ['SOWId', 'SOWFileName', 'SOWSignedDate'];
  displayedColumnsClient: string[] = ['ClientBillingRoleName', 'NoOfPositions', 'ClientBillingPercentage', 'StartDate', 'EndDate', "Position"];
  // @ViewChild('sowPaginator') sowPaginator: MatPaginator;
  // @ViewChild('clientPaginator') clientPaginator: MatPaginator;
  @ViewChild('sowPaginator', { static: false }) sowPaginator: MatPaginator;
  @ViewChild('clientPaginator', { static: false }) clientPaginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  dataSourceSOW = new MatTableDataSource();
  dataSourceClient = new MatTableDataSource();

  constructor(private projectCreationService: ProjectCreationService,
    private actRoute: ActivatedRoute, private router: Router,
    // private confirmationService: ConfirmationService,
    private clientBillingRoleService: ClientBillingRoleService,
    private _deliveryHeadService: DeliveryHeadService,
    private _snackBar: MatSnackBar,
    public dialog: MatDialog,
    // private messageService: MessageService,
    private SowService: SowService) {
    this.projectData = new ProjectsData();
    this.PageSize = this.resources.PageSize;
    this.PageDropDown = this.resources.PageDropDown;
  }

  ngAfterViewInit() {
    this.dataSourceSOW.paginator = this.sowPaginator;
    this.dataSourceClient.paginator = this.clientPaginator;
  }

  ngOnInit() {
    this.EmpId = this.EmpId = JSON.parse(sessionStorage["AssociatePortal_UserInformation"]).employeeId;
    this.actRoute.params.subscribe(params => { this.dashboard = params["id"]; });
    let winpath = window.location.pathname.split(";")[0].split("/").reverse()[0];
    if (winpath === 'PMDashboard' || window.location.pathname === '/project/view/DHDashboard' || window.location.pathname === '/project/view/projectDashboard') {
      this.showback = true;
    }


    this.projectStateSubscription = this.projectCreationService.GetProjectState().subscribe(data => {
      this.projectState = data;
    });
    this.projectIdSubscription = this.projectCreationService.GetProjectId().subscribe(data => {
      this.projectId = data;
    });
    this.selectedTabSubscription = this.projectCreationService.GetSelectedTab().subscribe(data => {
      if (data == 3)
        this.hideBack = true;
      this.GetProjectDetails();  // we need to get refreshed data every time 
    });

    if (this.dashboard == "DHDashboard") {
      this.canApprove = true;
    }
    else {
      this.canApprove = false;
    }
    this.dataSourceClient.paginator = this.clientPaginator;
    this.dataSourceSOW.paginator = this.sowPaginator;

  }

  canShowActions(cBRData: ClientBillingRoleDetails) {
    if ((cBRData.IsActive != undefined || cBRData.IsActive != null) && cBRData.IsActive == false) {
      return false;
    }
    if (cBRData.IsActive == true && cBRData.AvailablePositions == 0) {
      return false;
    }
    return true;
  }

  GetProjectDetails() {
    if (this.projectId > 0) {
      this.GetProjectByID(this.projectId);
      this.GetClientBillingRolesByProjectId(this.projectId);
      this.GetSOWDetailsById(this.projectId);
    }
  }
  private GetProjectByID(currentProjectID: number): void {
    this.projectCreationService
      .GetProjectDetailsbyID(currentProjectID)
      .subscribe(
        (projectdata: ProjectsData) => {
          this.projectData = projectdata;
          if (this.projectData.ProjectState == "Drafted")
            this.isDrafted = true;
        },
        error => {
          this._snackBar.open("Error while getting the project details", 'x', {
            duration: 1000,
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
          // this.messageService.add({
          //   severity: "error",
          //   summary: "Error message",
          //   detail: "Error while getting the project details"
          // });
        }
      );
  }

  private GetClientBillingRolesByProjectId(projectId: number) {
    if (projectId > 0) {
      this.clientBillingRoleData = new Array<ClientBillingRoleDetails>();
      this.clientBillingRoleService.GetClientBillingRolesByProjectId(projectId).subscribe((res: ClientBillingRoleDetails[]) => {
        if (res.length > 0) {
          this.clientBillingRoleData = res;
          this.clientBillingRoleData.forEach(
            (d: ClientBillingRoleDetails) => {
              if (d.IsActive == true)
                d.AvailablePositions = d.NoOfPositions - d.AllocationCount;
              else
                d.AvailablePositions = 0;
            }
          );
          this.clientBillingRoleData.forEach((d: ClientBillingRoleDetails) => {
            d.StartDate = d.StartDate != null ? moment(d.StartDate).format('YYYY-MM-DD') : null;
            d.EndDate = d.EndDate != null ? moment(d.EndDate).format('YYYY-MM-DD') : null;
          });
        }
        this.dataSourceClient.data = res;
        this.dataSourceClient.paginator = this.clientPaginator;
        this.dataSourceClient.sort = this.sort;
      }),
        (error) => {

        };
    }
  }

  private GetSOWDetailsById(currentProjectId: number): void {
    if (currentProjectId != null) {
      this.SowService.GetSowByProjectId(currentProjectId).subscribe((response: any[]) => {
        this.SOWData = response;
        this.SOWData.forEach((d: SOW) => {
          d.SOWSignedDate = d.SOWSignedDate != null ? moment(d.SOWSignedDate).format('YYYY-MM-DD') : null;
        });
        this.dataSourceSOW.data = response;
        this.dataSourceSOW.paginator = this.sowPaginator;
        this.dataSourceSOW.sort = this.sort;
      });

    }
  }

  ShowCBR(): boolean {
    return this.clientBillingRoleData.length > 0;

  }

  ShowSOW(): boolean {
    return this.SOWData.length > 0;
  }

  onBack() {
    this.showback = false;
    if (this.dashboard == "DHDashboard" || this.dashboard == "PMDashboard")
      this.router.navigate(['shared/dashboard'])
    else
      this.router.navigate(['project/dashboard'])
  }

  ApproveOrRejectByDH() {
    this._deliveryHeadService.ApproveOrRejectByDH(this.projectData.ProjectId, "Approve", this.EmpId).subscribe(res => {
      if (res > 0) {
        this._snackBar.open("Succesfully approved the project", 'x', {
          duration: 1000,
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });

        // this.messageService.add({
        //   severity: 'success',
        //   summary: 'Success Message',
        //   detail: 'Succesfully approved the project'
        // });
        setTimeout(() => {
          this.router.navigate(['shared/dashboard'])
        }, 1000);


      }
      else {
        this._snackBar.open("Failed to approve the project", 'x', {
          duration: 1000,
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
        // this.messageService.add({
        //   severity: 'error',
        //   summary: 'Failed Message',
        //   detail: 'Failed to approve the project'
        // });
      }
    },
      error => {
        this._snackBar.open("Failed to approve the project", 'x', {
          duration: 1000,
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
      }
    );
  }

  RollbackDialog() {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      disableClose: true,
      hasBackdrop: true,
      data: {
        message: 'Do you want Delete project details?',
      }
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.Rollback();
      }
      // else
      // this.router.navigate(["project/dashboard"]);
    });

  }
  // this.confirmationService.confirm({
  //   message: "Do you want to Rollback project details?",
  //   header: "Confirmation",
  //   icon: "pi pi-exclamation-triangle",
  //   accept: () => {
  //     this.Rollback();
  //   },
  //   reject: () => {
  //     // this.router.navigate(["project/dashboard"]);
  //   }
  // });

  Rollback() {
    this.projectCreationService
      .deleteProjectDetails(this.projectId)
      .subscribe((res: boolean) => {
        if (res == true) {
          this._snackBar.open("Project deleted successfully", 'x', {
            duration: 1000,
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
          setTimeout(() => {
            this.router.navigate(["project/dashboard"]);
          }, 1000);
        } else
          this._snackBar.open("Rollback failed", 'x', {
            duration: 1000,
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
      });
  }

  ngOnDestroy() {
    this.projectIdSubscription.unsubscribe();
    this.selectedTabSubscription.unsubscribe();
  }

}
