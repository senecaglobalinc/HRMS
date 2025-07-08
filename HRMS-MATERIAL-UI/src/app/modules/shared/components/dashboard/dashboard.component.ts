import { Component, OnInit, ViewChild,ElementRef } from '@angular/core';
// import { Message, SelectItem, MessageService } from 'primeng/api';
import { TalentRequisitionHistoryData } from '../../../master-layout/models/talentrequisitionhistory.model';
import { SkillsData, ProjectRoleParam, ProjectRole } from '../../../master-layout/models/project-role.model';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { KraSetData } from '../../../master-layout/models/kra.model';
import { Router, ActivatedRoute } from '@angular/router';
import { MasterDataService } from '../../../master-layout/services/masterdata.service';
import { DashboardService } from '../../services/dashboard.service';
import { FinanceHeadService } from '../../services/finance-head.service';
import * as moment from 'moment';
import { CommonService } from '../../../../core/services/common.service';
import { DeliveryHeadService } from '../../services/delivery-head.service';
import { ProjectRoleService } from '../../../master-layout/services/project-role.service';
declare var jQuery: any;
import * as servicePath from '../../../../core/service-paths';
import { Observable, of } from 'rxjs';

import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
 import { MatSort } from '@angular/material/sort';

import { NavService } from '../../../master-layout/services/nav.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatDialog } from '@angular/material/dialog';
import { RejectDialogComponent } from '../reject-dialog/reject-dialog.component';
import { NgxSpinnerService } from 'ngx-spinner';


interface SelectItem {
    value : number;
    label : string;
  }

@Component({
    selector: 'app-dashboard',
    templateUrl: './dashboard.component.html',
    styleUrls: ['./dashboard.component.css'],
    providers: [ MasterDataService, DashboardService, FinanceHeadService, CommonService, DeliveryHeadService, ProjectRoleService]
})
export class DashboardComponent implements OnInit {

    resources = servicePath.API.PagingConfigValue;
    // errorMessage: Message[] = [];
    public roleName: string;
    public employeeId: number;
    public userName: string;
    isRejectDialogDisplay: boolean = false;
    isRejectReasonValid: boolean = false;
    rejectReason: string = "";
    public dataList: any[] = [];
    public detailsList: any[] = [];
    public SkillDetails: SkillsData[] = [];
    proficiencyLevels: SelectItem[];
    public trList: TalentRequisitionHistoryData[] = [];
    public trfinaceList: TalentRequisitionHistoryData[] = [];
    selectedTR: TalentRequisitionHistoryData;
    public itNotificationsList: any[] = [];
    public financeNotificationsList: any[] = [];
    public adminNotificationsList: any[] = [];
    public AssociateKRAs: any[] = [];
    public hraList: any[] = [];
    public _projectRoleData: ProjectRole;
    formSubmitted: boolean = false;
    valid: boolean = false;

    AddSkillTitle = "Edit Skill";
    addSkill: SkillsData = new SkillsData();

    skillDisplay: boolean = false;
    saveButton: string = "Save";
    myForm: FormGroup;
    UserRole = '';

    showNotifications: boolean = false;
    public HRcnt: number;
    isVisible: boolean;
    isDisplay: boolean;
    isFinanceDisplay: boolean;
    isAdminDisplay: boolean;
    isHRADisplay: boolean;
    isEmployeeSkill: boolean;
    pageSize: number;
    _visibleRowCount = 5;
    public roleList: string[] = [];
    public employeeCode: any;
    componentName: string;
    name: string;
    isAuthorised: boolean = true;
    authorizedFinanceHead: boolean = false;
    departmentHeadList: KraSetData[] = [];
    PageSize: number;
    PageDropDown: number[] = [];

    displayedColumns: string[] = ['NotificationType', 'EmpCode', 'EmpName', 'HRAdvisor', 'Download', 'Approve', 'Reject', 'Edit'];
    displayedColumnshra: string[] = ['NotificationType', 'EmpCode', 'EmpName', 'Remarks'];

    dataSource = new MatTableDataSource();
 

    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatSort) sort: MatSort;


    // sort;
    // @ViewChild(MatSort, {static: false}) set content(content: ElementRef) {
    //     this.sort = content;
    //     if (this.sort){
    //        this.dataSource.sort = this.sort;
    //     }
    // }
  
    // applyFilterhra(event: Event) {
    //     if (event) {
    //       const filterValue = (event.target as HTMLInputElement).value;
    //       this.dataSourcehra.filter = filterValue.trim().toLowerCase();
    //     } else {
    //       this.dataSourcehra = new MatTableDataSource(this.dataSourcehra.data);
    //     }
    //     if (this.dataSourcehra.paginator) {
    //       this.dataSourcehra.paginator.firstPage();
    //     }
    //     this.dataSourcehra.paginator = this.paginator;
    //     this.dataSourcehra.sort = this.sort;
    //   }

    constructor(private _router: Router, public fb: FormBuilder, private actRoute: ActivatedRoute
        ,private  masterDataService: MasterDataService,private  _common: CommonService
        , private _projectService: ProjectRoleService
        , private _deliveryHeadService: DeliveryHeadService
        , private _financeHeadService: FinanceHeadService, private _dashboardService: DashboardService,
        private _snackBar: MatSnackBar,
        public navService: NavService,
        private spinner: NgxSpinnerService,
        private dialog?: MatDialog,
      ) {
        this.navService.currentSearchBoxData.subscribe((responseData) => {
          this.applyFilter(responseData);
        //   this.applyFilterhra(responseData);
        });

        this.componentName = this.actRoute.routeConfig.component.name;
        this.PageSize = this.resources.PageSize;
        this.PageDropDown = this.resources.PageDropDown;

        this.myForm = this.fb.group({
            'experienceInMonths': [null],
            'LastUsed': [null],
            'isPrimary': [null],
            'empName': [null],
            'skillName': [null],
            'ProficiencyLevel': [null, [Validators.required]],
            'statusCode': [null],
        });
    }

    ngOnInit() {
        this.UserRole = JSON.parse(sessionStorage['AssociatePortal_UserInformation']).roleName;
        this.userName = JSON.parse(sessionStorage["AssociatePortal_UserInformation"]).email;
        if (this.validateUser()) {
            this.roleName = JSON.parse(sessionStorage["AssociatePortal_UserInformation"]).roleName;
            this.employeeId = JSON.parse(sessionStorage["AssociatePortal_UserInformation"]).employeeId;

            this.isDisplay = false;
            this.isFinanceDisplay = false;
            this.isAdminDisplay = false;
            this.isHRADisplay = false;
            this.isEmployeeSkill = false;
            this.isVisible = false;
            this.pageSize = this._visibleRowCount;
            this.getCount();
            this.ShowHide();
            this.getProficiencyLevels();

        }
        else {
            this._router.navigate(["/login"]);
        }


        setTimeout(() => {
            this.dataSource.paginator = this.paginator;
            this.dataSource.sort = this.sort;
        } , 500);
    }
     

    // cols = [
    //     { field: 'NotificationType', header: 'Notification Type' },
    //     { field: 'EmpCode', header: 'Employee Code' },
    //     { field: 'EmpName', header: 'Name' },
    //     { field: 'HRAdvisor', header: 'HR Advisor' },
    // ];

    validateUser(): boolean {
        if (this.userName == undefined || (this.userName == "")) {
            return false;
        }
        else
            return true;
    }

    getCount() {
        // this._dashboardService.getDashboardNotificationCount().subscribe((res: number) => {
        //     this.HRcnt = res;
        // }, (error) => {
        //     swal("Failed to get the notifications count", error._body, "error");
        // });
    }

    getProficiencyLevels(): void {

        this.masterDataService.GetProficiencyLevels().subscribe(
            (res: SkillsData[]) => {
                this.proficiencyLevels = [];
                let resultData: SkillsData[] = res;
                this.proficiencyLevels.push({
                    label: "Select Proficiency Level",
                    value: null
                });
                resultData.forEach(element => {
                    this.proficiencyLevels.push({
                        label: element.Name,
                        value: element.ID
                    });
                });
            },
            (error: any) => {
                if (error._body != undefined && error._body != "")
                    // this._common
                    //     .LogError(this.componentName, error._body)
                    //     .then((data: any) => { });
                    var comment;
                    // this.errorMessage = [];
                // this.errorMessage.push({
                //     severity: "error",
                //     summary: "Failed to Get Proficiency Level List!"
                // });
            }
        );
    }

    ShowHide(): void {
        this.roleList = this.roleName.split(',');
        this.roleList.forEach(roleName => {
            this.name = roleName.toUpperCase();
            if (this.name == "HRM") {
                this.isDisplay = false;
                this.isFinanceDisplay = false;
                this.isAdminDisplay = false;
                this.isEmployeeSkill = false;
                this.isVisible = true;
                this.getHRHeadDetails();
            }
            if (this.name == "RM") {
                this.isDisplay = false;
                this.isFinanceDisplay = false;
                this.isAdminDisplay = false;
                this.isVisible = true;
                this.isEmployeeSkill = true;
                this.getSkillDetails();
            }
            if (this.name == "ITHEAD") {
                this.isVisible = false;
                this.isFinanceDisplay = false;
                this.isAdminDisplay = false;
                this.isHRADisplay = false;
                this.isEmployeeSkill = false;
                this.isDisplay = this.isDisplay ? false : true;
                this.getItNotifications();
            }
            if (this.name == "FINANCEHEAD") {
                this.isVisible = false;
                this.isDisplay = false;
                this.isAdminDisplay = false;
                this.isHRADisplay = false;
                this.isEmployeeSkill = false;
                this.isFinanceDisplay = true;
                this.getFinanceHeadDetails()
            }
            if (this.name == "ADMINHEAD") {
                this.isVisible = false;
                this.isDisplay = false;
                this.isFinanceDisplay = false;
                this.isHRADisplay = false;
                this.isEmployeeSkill = false;
                this.isAdminDisplay = true;
                this.getAdminNotifications();
            }
            if (this.name == "HRA") {
                this.isDisplay = false;
                this.isFinanceDisplay = false;
                this.isAdminDisplay = false;
                this.isEmployeeSkill = false;
                this.isHRADisplay = true;
                this.getHRANotifications();
            }
            if (this.name == "SYSTEMADMIN") {
            }
            if (this.name == "PROGRAM MANAGER") {

            }
            // commented in angular 2
            // if (this.name == "DEPARTMENT HEAD") {
            //     this.isDepartmentHead = true;
            //     this.getDepartmentHeadDetails()
            // }
            // if (this.name == "KRACONFIGURATOR") {
            //     this.isKRAConfigurator = true;
            //     this.getKRAConfiguratordetails()
            // }
            // if (this.name == "MD") {
            //     this.isMDDisplay = true;
            //     this.getPendingKrasForMD();
            // }
        });
        // this.getTalentRequisitionDeatils();
    }

    getHRHeadDetails() {
        this._dashboardService.getHRHeadDetails().subscribe((res: any[]) => {
            this.detailsList = res;
            this.dataSource.data = res;
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
        });
    }

    getSkillDetails() {
        this._dashboardService.getSkillDetails().subscribe((res: SkillsData[]) => {
            this.SkillDetails = res;
        });
    }

    getItNotifications() {
        this._dashboardService.getItNotifications().subscribe((res: any[]) => {
            this.itNotificationsList = res
        }, (error) => {
            this._snackBar.open("Failed to get the IT notifications", 'x', {
                duration: 1000,
                horizontalPosition: 'right',
                verticalPosition: 'top',
              });
            // swal("Failed to get the IT notifications", error._body, "error");
            // this.messageService.add({ severity: "error", summary: "error Message", detail: "Failed to get the IT notifications" + error._body + "error" });
        });
    }
    getFinanceNotifications() {
        this._dashboardService.getFinanceNotifications().subscribe((res: any[]) => {
            this.financeNotificationsList = res
        }, (error) => {
            this._snackBar.open("Failed to get the Finance notifications", 'x', {
                duration: 1000,
                horizontalPosition: 'right',
                verticalPosition: 'top',
              });
            // swal("Failed to get the Finance notifications", error._body, "error");
            // this.messageService.add({ severity: "error", summary: "error Message", detail: "Failed to get the Finance notifications " + error._body + "error" });

        });
    }
    getAdminNotifications() {
        this._dashboardService.getAdminNotifications().subscribe((res: any[]) => {
            this.adminNotificationsList = res
        }, (error) => {
            this._snackBar.open("Failed to get the Admin notifications", 'x', {
                duration: 1000,
                horizontalPosition: 'right',
                verticalPosition: 'top',
              });
            // this.messageService.add({ severity: "error", summary: "error Message", detail: "Failed to get the Admin notifications" + error._body + "error" });

            //swal("Failed to get the Admin notifications", error._body, "error");
        });
    }

    ngAfterViewInit() {
        this.dataSource.sort = this.sort;
      }

    getHRANotifications() {
        this._dashboardService.getHRANotifications().subscribe((res: any[]) => {
            this.hraList = res;
            this.dataSource.data = res;
            this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
        }, (error) => {
            this._snackBar.open("Failed to get the HRA notifications", 'x', {
                duration: 1000,
                horizontalPosition: 'right',
                verticalPosition: 'top',
              });
            // this.messageService.add({ severity: "error", summary: "error Message", detail: "Failed to get the HRA notifications" + error._body + "error" });
            //swal("Failed to get the HRA notifications", error._body, "error");
        });
    }

    getFinanceHeadDetails(): void {
        this._financeHeadService.GetPendingRequisitionsForApproval().subscribe((res: TalentRequisitionHistoryData[]) => {
            if (res.length == 0)
                this.authorizedFinanceHead = false;
            else {
                this.authorizedFinanceHead = true;
                this.trfinaceList = res;
                this.trfinaceList.forEach((tr: TalentRequisitionHistoryData) => {
                    tr.RequestedDate = moment(tr.RequestedDate).format('YYYY-MM-DD');
                });
            }
        },
            (error) => {
                if (error._body != undefined && error._body != "")
                    this._common.LogError(this.componentName, error._body).subscribe((data: any) => {
                    });
                    this._snackBar.open("Failed to get Pending Requisitions for Approval List !", 'x', {
                duration: 1000,
                horizontalPosition: 'right',
                verticalPosition: 'top',
              });
                // this.messageService.add({ severity: "error", summary: "error Message", detail: "Failed to get Pending Requisitions for Approval List ! error" });

                //swal("", "Failed to get Pending Requisitions for Approval List !", "error");
            });
    }

    getTalentRequisitionDeatils(): void {
        this._deliveryHeadService.GetPendingRequisitionsForApproval().subscribe((res: TalentRequisitionHistoryData[]) => {
            if (res.length != 0) {
                if (res[0].TRCode == null)
                    this.isAuthorised = false;
                else
                    this.trList = res;
                this.trList.forEach((tr: TalentRequisitionHistoryData) => {
                    if (tr.RequisitionType == 25)
                        tr.RequisitionTypeName = "New Request";
                    else if (tr.RequisitionType == 26)
                        tr.RequisitionTypeName = "Replacement";
                    tr.RequestedDate = moment(tr.RequestedDate).format('YYYY-MM-DD');
                });
            }
        },
            (error) => {
                if (error._body != undefined && error._body != "")
                    this._common.LogError(this.componentName, error._body).subscribe((data: any) => {
                    });
                    this._snackBar.open("Failed to get the IT notifications", 'x', {
                        duration: 1000,
                        horizontalPosition: 'right',
                        verticalPosition: 'top',
                      });
                // this.messageService.add({ severity: "error", summary: "error Message", detail: "Failed to get Pending Requisitions for Approval List ! error" });

                //swal("", "Failed to get Pending Requisitions for Approval List !", "error");
            });
    }
    selectedTRCode(selectedData: TalentRequisitionHistoryData) {
        sessionStorage["DeliveryHeadDashboardTalentRequisitionCode"] = selectedData.TRCode;
        sessionStorage["DeliveryHeadDashboardTalentRequisitionType"] = selectedData.RequisitionType;
        this._router.navigate(['ap/dashboard/talentrequisition-details/', selectedData.TalentRequisitionId]);
    }

    onEditSkillDetils(selectedData: SkillsData): void {
        this.AddSkillTitle = "Edit Skill Data"
        this.saveButton = "Update";

        this.skillDisplay = true;
        this.addSkill = new SkillsData();
        if (selectedData.SkillsSubmittedForApprovalId) {
            this.addSkill.SkillsSubmittedForApprovalId = selectedData.SkillsSubmittedForApprovalId;
        }
        if (selectedData.WorkFlowId) {
            this.addSkill.WorkFlowId = selectedData.WorkFlowId;
        }
        if (selectedData.SkillName) {
            this.addSkill.SkillName = selectedData.SkillName;
            this.addSkill.skillID = selectedData.skillID;
        }
        if (selectedData.EmployeeName) {
            this.addSkill.EmployeeName = selectedData.EmployeeName;
            this.addSkill.empID = selectedData.empID;
        }
        if (selectedData.CompetencyAreaID) {
            this.addSkill.CompetencyAreaID = selectedData.CompetencyAreaID;
        }
        if (selectedData.SkillGroupID) {
            this.addSkill.SkillGroupID = selectedData.SkillGroupID;
        }
        if (selectedData.proficiencyLevelId) {
            this.addSkill.ProficiencyLevel = selectedData.ProficiencyLevel;
            this.addSkill.proficiencyLevelId = selectedData.proficiencyLevelId;
        }
        if (selectedData.LastUsed)
            this.addSkill.LastUsed = selectedData.LastUsed;
        if (selectedData.experience)
            this.addSkill.experience = selectedData.experience;
        if (selectedData.StatusCode) {
            this.addSkill.StatusCode = selectedData.StatusCode;
        }
        this.addSkill.IsPrimary = selectedData.IsPrimary;
        //this.addSkill.SubmittedTo = JSON.parse(sessionStorage["AssociatePortal_UserInformation"]).employeeId;
    }

    resetForm(): void {
        this.formSubmitted = false;
        this.valid = false;
        this.myForm.reset();

        // if (this.saveButton == "Save") {        
        //   this.addClientBillingRole = new ClientBillingRole();
        //   this.addClientBillingRole = {
        //     ClientBillingRoleCode: "",
        //     ClientBillingRoleId: null,
        //     ClientBillingRoleName: "",
        //     ClientId: null,
        //     ClientName: ""          
        //   };        

        // } 

    }
    saveEmployeeSkill(skillsData: SkillsData): void {
        this.formSubmitted = true;
        let numberRegex = /^[a-zA-Z][a-zA-Z-&,\s]*$/;
        if (!this.myForm.valid) {
            if (skillsData.SkillName == "") return;
            if (skillsData.skillID == 0) return;
        }

        this._dashboardService.CreateEmployeeSkill(skillsData).subscribe(
            (response: boolean) => {
                if (response == true) {
                    this._snackBar.open("Failed to get the IT notifications", 'x', {
                        duration: 1000,
                        horizontalPosition: 'right',
                        verticalPosition: 'top',
                      });
                    // this.messageService.add({ severity: "success", summary: "Success Message", detail: "Employee skill approved successfully" });

                    // this.growlerrormessage("success", "Employee skill approved successfully", "");
                    this.resetForm();
                    this.skillDisplay = false;
                    this.getSkillDetails();
                } else if (response == false) {
                    this._snackBar.open("Failed to get the IT notifications", 'x', {
                        duration: 1000,
                        horizontalPosition: 'right',
                        verticalPosition: 'top',
                      });
                    // this.messageService.add({ severity: "error", summary: "error Message", detail: "Employee skill is already exists." });
                    // this.growlerrormessage("error", "Employee skill is already exists", "");
                } else {
                    this._snackBar.open("Failed to get the IT notifications", 'x', {
                        duration: 1000,
                        horizontalPosition: 'right',
                        verticalPosition: 'top',
                      });
                    // this.messageService.add({ severity: "error", summary: "error Message", detail: "Failed to Crate Employee skill." });
                    // this.growlerrormessage("error", "Failed to Crate Employee skill.", "");
                }
            },
            error => {
                if (error._body != undefined && error._body != "")
                    this._common
                        .LogError(this.componentName, error._body)
                        .subscribe((data: any) => { });
                        this._snackBar.open("Failed to approve skill", 'x', {
                            duration: 1000,
                            horizontalPosition: 'right',
                            verticalPosition: 'top',
                          });
                // this.messageService.add({ severity: "error", summary: "error Message", detail: "Failed to approve skill." });

                // this.growlerrormessage("error", "Failed to approve skill.", "");
            }
        );
    }

    getRolesNotifications() {
        this._projectService.GetRoleNotifications(this.employeeId).subscribe((res: any[]) => {
            this.dataList = res
            jQuery('#notificationCount').html(res.length);
            jQuery('#notificationCount1').html('You have' + res.length + 'notifications');
        });
    }
    approveProjectRole(selectedData: any) {
        let _projectRoleParam = new ProjectRoleParam();
        _projectRoleParam.ProjectRoleData = selectedData;
        _projectRoleParam.Status = "RoleApproved";
        this._projectService.RoleApproval(_projectRoleParam).subscribe((data) => {
            this._snackBar.open("Role approved successfully", 'x', {
                duration: 1000,
                horizontalPosition: 'right',
                verticalPosition: 'top',
              });
            
            // swal("", 'Role approved successfully', "success");
            // this.messageService.add({ severity: "success", summary: "Success Message", detail: "Role approved successfully" });

            this.getRolesNotifications();
        }, (error) => {
            this._snackBar.open("Failed to approve the data" , 'x', {
                duration: 1000,
                horizontalPosition: 'right',
                verticalPosition: 'top',
              });
            // swal("Failed to approve the data", error._body, "error");
            // this.messageService.add({ severity: "error", summary: "error Message", detail: "Failed to approve the data" + error._body + "error" });

        });
    }

    rejectProjectRole(selectedRowData: any) {
        this.isRejectDialogDisplay = true;
        this._projectRoleData = new ProjectRole();
        this._projectRoleData = selectedRowData;
    }
    RejectReasonSubmit() {
        if (this.rejectReason == "")
            this.isRejectReasonValid = true;
        else {
            let _projectRoleParam = new ProjectRoleParam();
            _projectRoleParam.ProjectRoleData = this._projectRoleData;
            _projectRoleParam.ProjectRoleData.RejectReason = this.rejectReason;
            _projectRoleParam.Status = "RoleChangeRejected";
            this._projectService.RoleRejection(_projectRoleParam).subscribe((data) => {
                this._snackBar.open("Role change notification rejected successfully", 'x', {
                    duration: 1000,
                    horizontalPosition: 'right',
                    verticalPosition: 'top',
                  });
                // swal("", 'Role change notification rejected successfully', "success");
                // this.messageService.add({ severity: "success", summary: "Success Message", detail: "Role change notification rejected successfully" });

                this.getRolesNotifications();
                this.rejectReason = "";
            }, (error) => {
                this._snackBar.open("Failed to reject the notification", 'x', {
                    duration: 1000,
                    horizontalPosition: 'right',
                    verticalPosition: 'top',
                  });
                // this.messageService.add({ severity: "error", summary: "error Message", detail: "Failed to reject the notification" + error._body + "error" });

                //swal("Failed to reject the notification", error._body, "error");
            });
            this.isRejectDialogDisplay = false;
        }
    }

    updateItStatus(empCode: string, taskId: string) {
        this._dashboardService.updateItStatus(empCode, taskId).subscribe((data) => {
            this.getCount();
            this.getItNotifications();
        }, (error) => {
            this._snackBar.open("Failed to update the IT status", 'x', {
                duration: 1000,
                horizontalPosition: 'right',
                verticalPosition: 'top',
              });
            // this.messageService.add({ severity: "error", summary: "error Message", detail: "Failed to update the IT status" + error._body + "error" });

            //swal("Failed to update the IT status", error._body, "error");
        });
    }
    updateFinanceStatus(empCode: string, taskId: string) {
        this._dashboardService.updateFinanceStatus(empCode, taskId).subscribe((data) => {
            this.getCount();
        }, (error) => {
            this._snackBar.open("Failed to update the Finance status", 'x', {
                duration: 1000,
                horizontalPosition: 'right',
                verticalPosition: 'top',
              });
            // this.messageService.add({ severity: "error", summary: "error Message", detail: "Failed to update the Finance status" + error._body + "error" });
            // swal("Failed to update the Finance status", error._body, "error");
        });
    }
    updateAdminStatus(empCode: string, taskId: string) {
        this._dashboardService.updateAdminStatus(empCode, taskId).subscribe((data) => {
            this.getCount();
            this.getAdminNotifications();
        }, (error) => {
            this._snackBar.open("Failed to update the Admin status", 'x', {
                duration: 1000,
                horizontalPosition: 'right',
                verticalPosition: 'top',
              });
            // this.messageService.add({ severity: "error", summary: "error Message", detail: "Failed to update the Admin status" + error._body + "error" });
            // swal("Failed to update the Admin status", error._body, "error");
        });
    }
    generatePDF(id: number) {

        this._dashboardService.GetAssociateCodeByEmpID(id).subscribe((res: any) => { this.employeeCode = res.EmployeeCode; });

        this._dashboardService.generatePDF(id).subscribe((data: ArrayBuffer) => {
            if (data.byteLength > 0) {
                var mediaType = 'application/pdf';
                var blob = new Blob([data], { type: mediaType });
                var url = window.URL;
                var fileURL = url.createObjectURL(blob);

                var downloadLink = document.createElement('a');
                downloadLink.href = fileURL;
                downloadLink.target = '_self';

                downloadLink.download = this.employeeCode + '.pdf';
                document.body.appendChild(downloadLink);
                downloadLink.click();
            }
        }, (error) => {
            this._snackBar.open("Failed to download the report", 'x', {
                duration: 1000,
                horizontalPosition: 'right',
                verticalPosition: 'top',
              });
            // this.messageService.add({ severity: "error", summary: "Error Message", detail: "Failed to download the report " + error._body + " error" });
        });
    }

    profileApproval(id: number, action: string, reason: string) {
        //Reason is not mandatory while approving employee profile so commenting the below code
        // if (reason == null) {
        //     this._snackBar.open("Please provode reason", 'x', {
        //         duration: 1000,
        //         horizontalPosition: 'right',
        //         verticalPosition: 'top',
        //       });
        //     this.messageService.add({ severity: "warn", summary: "Warn Message", detail: "Please provide Reason" });
        // }
        this.spinner.show();
        var empID = id;
        let reqType: any;
        if (action == "Approved") {
            reqType = "Approved";
        }
        else {
            reqType = "Rejected";
        }
        this._dashboardService.profileApproval(id, action, reason).subscribe((data) => {
            this.spinner.hide();
            this._snackBar.open('Employee profile ' + reqType + ' successfully', 'x', {
                duration: 1000,
                horizontalPosition: 'right',
                verticalPosition: 'top',
              });
            // this.messageService.add({ severity: "success", summary: "Success Message", detail: "Employee profile " + reqType + " successfully" });
            this.getHRHeadDetails();
        }, (error) => {
            this.spinner.hide();
            this._snackBar.open("Something went wrong.", 'x', {
                duration: 1000,
                horizontalPosition: 'right',
                verticalPosition: 'top',
              });
            // this.messageService.add({ severity: "error", summary: "error Message", detail: "Something went wrong.\nPlease try after some time" + error._body + "error" });

        });
    }
    rejectConfirmation(empID: number, index: number) {
        this.employeeId = empID;
        this.isRejectDialogDisplay = true;
        this.isRejectReasonValid = false;
        const dialogRef = this.dialog.open(RejectDialogComponent, {
            disableClose: true,
            data: {}
          });
          dialogRef.afterClosed().subscribe(result => {
            // this.closeProject();
            if(result){
            this.rejectReason = result.rejectReason;
            }
            else{
                this.rejectReason = '';
                return;
            }
            if(this.isVisible && this.rejectReason!="" && this.rejectReason!=null ){
                
                this.rejectWithReasonSubmit();

            }else if(this.rejectReason!="" && this.rejectReason!=null){
                this.RejectReasonSubmit();

            }
            
          });

    }
    rejectWithReasonSubmit() {
        if (this.rejectReason == "")
            this.isRejectReasonValid = true;
        else {
            this.profileApproval(this.employeeId, "reject", this.rejectReason);
            this.rejectReason = "";
            this.isRejectDialogDisplay = false;
        }
    }

    openProfile(event: any) {
        this._router.navigate(['associates/prospectivetoassociate/edit/' + event.EmpId + '/EPU']);
    }

    viewRequisition(selectedData: TalentRequisitionHistoryData) {
        sessionStorage["DeliveryHeadDashboardTalentRequisitionCode"] = selectedData.TRCode;
        sessionStorage["RequisitionForProject"] = selectedData.ProjectName;
        this._router.navigate(['ap/dashboard/talent-requisition-details/', selectedData.TalentRequisitionId]);
    }
    onProfile(currentAssociate: any) {
        let subType = "EPU";
        let currentID = currentAssociate.EmpId;
        let notificationType = currentAssociate.notificationType;
        this._router.navigate(["associates/prospectivetoassociate/edit/" + currentID + "/" + subType]);
    }

    applyFilter(event: Event) {
        if (event) {
          const filterValue = (event.target as HTMLInputElement).value;
          this.dataSource.filter = filterValue.trim().toLowerCase();
        } else {
          this.dataSource = new MatTableDataSource(this.dataSource.data);
        }
        if (this.dataSource.paginator) {
          this.dataSource.paginator.firstPage();
        }
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
      }
}
