import { Component, OnInit } from '@angular/core';
import { ResourceRelease } from '../models/resoucerelease.model';
import * as moment from 'moment';
import { Message } from 'primeng/components/common/message';
import { SelectItem } from 'primeng/components/common/selectitem';
import { ResourceReleaseService } from '../services/resourcerelease.service';
import { TemporaryAllocationReleaseService } from '../services/temporary-allocation-release.service';
import { GenericType } from '../../../models/dropdowntype.model';
import { MasterDataService } from '../../../services/masterdata.service';
import { ActivatedRoute } from '@angular/router';
import { MessageService } from 'primeng/components/common/messageservice';
import { ProjectCreationService } from '../../projectLifeCycle/services/project-creation.service';
import { ResouceReleaseService } from '../../projectLifeCycle/services/resouce-release.service';
import { ConfirmationService } from 'primeng/api';
import { AssociateAllocationService } from '../services/associate-allocation.service';

@Component({
    selector: 'app-associate-release',
    templateUrl: './associate-release.component.html',
    styleUrls: ['./associate-release.component.scss'],
    providers: [MessageService, ConfirmationService]
})
export class AssociateReleaseComponent implements OnInit {
    releaseResource: ResourceRelease;
    releaseFormSubmitted: boolean = false;
    firstDate: Date;
    lastDate: Date;
    IsValidDate: boolean;
    invalid: boolean;
    projectsList: SelectItem[] = [];
    talentpoolsList: SelectItem[] = [];
    associatesList: SelectItem[] = [];
    associatesProjectsList: any[] = [];
    projectDetails: ResourceRelease[] = [];
    componentName: string;
    hideTrainingProject: boolean = true;
    employeeId: any;
    roleName: any;
    isEligiblePrimary: boolean = false;
    disableddlProject: boolean = false;
    remainingProjects: any = [];
    isPrimaryProject: boolean = false;
    primaryProjectId: any;
    ddlPrimaryProjectId: any;

    constructor(private _service: ResourceReleaseService, private masterDataService: MasterDataService,
        private messageService: MessageService,
        private _tempAllocationReleaseService: TemporaryAllocationReleaseService,
        private actRoute: ActivatedRoute,
        private confirmationService: ConfirmationService,
        private _allocateService: AssociateAllocationService) {
        this.componentName = this.actRoute.routeConfig.component.name;
    }

    ngOnInit() {
        this.releaseResource = new ResourceRelease();
        this.employeeId = JSON.parse(sessionStorage['AssociatePortal_UserInformation']).employeeId;
        this.roleName = JSON.parse(sessionStorage['AssociatePortal_UserInformation']).roleName;
        this.getDates();
        this.getEmployees();
        this.getProjectList();
    }

    private getEmployees(): void {
        this._tempAllocationReleaseService.GetAssociatesToRelease(this.employeeId, this.roleName).subscribe((res: any) => {
            this.associatesList = []
            this.associatesList.push({ label: '', value: null });
            this.projectsList = [];
            this.projectsList.push({ label: '', value: null });
            if (res.length > 0) {
                res.forEach((element: any) => {
                    if (this.associatesList.findIndex(x => x.label == element.EmpName) === -1)
                        this.associatesList.push({ label: element.EmpName, value: element.EmployeeId });
                });
            }
            if (res.length > 0) {
                this.associatesProjectsList = [];
                res.forEach((element: any) => {
                    this.associatesProjectsList.push(element);
                });
            }

        },
            (error: any) => {
                this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Failed to Get Employees!' });
            }
        );
    }

    private getProjectList(): void {
        this.masterDataService.GetProjectsList().subscribe((projectResponse: any[]) => {
            this.projectDetails = [];
            this.projectDetails = projectResponse;
            this.talentpoolsList = [];
            this.talentpoolsList.push({ label: '', value: null });
            this.projectDetails.forEach((element: any) => {
                if (element.ProjectName.indexOf("Talent Pool") != -1 && this.talentpoolsList.findIndex(x => x.label == element.ProjectName) == -1)
                    this.talentpoolsList.push({ label: element.ProjectName, value: element.ProjectId });
            });
        }),
            (error: any) => {
                // if (error._body != undefined && error._body != "")
                //     this._commonService.LogError(this.componentName, error._body).then((data: any) => {
                //     });
                this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Failed to Get Employees!' });

            };
    }

    getProjectsByEmployeeId(employeeId: number): void {
        this.primaryProjectId = this.getPrimaryProjectId(employeeId);
        if (employeeId != null && employeeId != undefined) {
            this.releaseResource.releaseDate = null;
            this.releaseResource.ProjectId = null;
            this.projectsList = [];
            this.projectsList.push({ label: '', value: null });
            let projectIds: any[] = [];
            if (this.associatesProjectsList && this.associatesProjectsList.length > 0) {
                projectIds = this.associatesProjectsList.filter((associate: any) => {
                    return associate.EmployeeId == employeeId;
                });
            }
            let list: ResourceRelease[] = [];
            if (projectIds && projectIds.length > 0) {
                projectIds.forEach((element: any) => {
                    this.projectDetails.forEach((project: any) => {
                        if (project.ProjectId == element.ProjectId)
                            list.push(project);
                    });
                });
            }
            if (list && list.length > 0) {
                list.forEach((element: ResourceRelease) => {
                    if (this.projectsList.findIndex(x => x.label == element.ProjectName) == -1) {
                        this.projectsList.push({ label: element.ProjectName, value: element.ProjectId })
                    };
                    if (element.ProjectName.indexOf("Training") != -1) {
                        this.hideTrainingProject = false;
                        this.releaseResource.ProjectId = element.ProjectId;
                        this.releaseResource.ProjectName = element.ProjectName;
                        this.releaseResource.releaseProjectId = null;
                    }
                    else this.hideTrainingProject = true;
                });
                // if (list.length == 1 && this.projectsList[1].label.indexOf("Training") == -1) {
                if (list.length == 1 && this.hideTrainingProject) {
                    this.releaseResource.ProjectId = this.projectsList[1].value;
                    this.disableddlProject = true;
                    this.getAndfillProjectData(employeeId);
                }
                else this.disableddlProject = false;
            }
        }
        if (this.hideTrainingProject) {
            this.getAssociatepoolProject(employeeId);
        }
    }


    private getAndfillProjectData(employeeId: number): void {
        if (employeeId != null && employeeId != undefined) {
            let list = this.associatesProjectsList.filter((project: any) => {
                return (project.EmployeeId == employeeId && project.ProjectId == this.releaseResource.ProjectId);
            });
            if (list && list.length > 0) {
                //this.releaseResource.projectTypeCode = list[0].projectTypeCode;
                //this.releaseResource.ProjectId = list[0].ProjectId;
                this.releaseResource.EffectiveDate = list[0].EffectiveDate;
                //this.releaseResource.AllocationPercentage = list[0].AllocationPercentage;
            }
            if (this.hideTrainingProject == false) {
                let projectManager = this.projectDetails.filter((project: ResourceRelease) => {
                    return project.ProjectId == this.releaseResource.releaseProjectId;
                });
                if (projectManager && projectManager.length > 0)
                    this.releaseResource.reportingManagerId = projectManager[0].reportingManagerId;
            }
        }
        this.loadRemainingProjects(this.releaseResource.ProjectId);
        this.isPrimary();
    }

    submit(): void {
        if (this.isPrimaryProject && this.remainingProjects.length > 2) {
            this.isEligiblePrimary = true;
        }
        else {
            this.releaseAssociate(this.releaseResource);
        }
    }

    private release(releaseResource: ResourceRelease): void {
        if (releaseResource != null && releaseResource != undefined) {
            this.releaseResource.TalentRequisitionId = 1;
            this._tempAllocationReleaseService.TemporaryReleaseAssociate(this.releaseResource).subscribe((data: any) => {
                if (data.IsSuccessful) {
                    this.messageService.add({ severity: 'success', summary: 'Success Message', detail: 'Associate released from the project successfully!' });
                    this.releaseFormSubmitted = false;
                    this.clearAll();
                    this.getEmployees();
                }
                else {
                    this.messageService.add({ severity: 'error', summary: 'Error Message', detail: data.Message });
                    return;
                }
            }, (error) => {
                // if (error._body != undefined && error._body != "")
                //     this._commonService.LogError(this.componentName, error._body).then((data: any) => {
                //     });
                this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Failed to release associate from project.' });
            });
        }
    }

    private getAssociatepoolProject(EmployeeId: number) {
        if (EmployeeId != null && EmployeeId != undefined) {
            this._service.GetAssociateTalentPool(EmployeeId).subscribe((res: any) => {
                if (res != null) {
                    this.releaseResource.releaseProjectId = res.ProjectId;
                    this.releaseResource.reportingManagerId = res.ReportingManagerId;
                    this.releaseResource.talentpool = res.ProjectName;
                }
            });
        }
        else {
            this.releaseResource.talentpool = "";
            this.releaseResource.releaseProjectId = null;
            this.releaseResource.reportingManagerId = null;
        }
    }

    private getDates(): void {
        var date = new Date(), y = date.getFullYear(), m = date.getMonth();
        this.firstDate = new Date(y, m, 1);
        this.lastDate = new Date();
        //new Date(y, m + 1, 0);
    }

    private CheckDates = function (fromDate: any, toDate: any): boolean {
        if ((fromDate != null || fromDate != undefined) && (toDate != null && toDate != undefined)) {
            if (Date.parse(fromDate) <= Date.parse(toDate)) {
                this.invalid = false;
                return true
            }
            else {
                this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: 'Invalid Release date, please check associate`s allocations.' });
                this.invalid = true;
                return false
            }

        }
    }
    clearAll = function (): void {
        this.hideTrainingProject = true;
        this.releaseResource.ProjectId = null;
        this.releaseResource.ProjectName = null;
        this.releaseResource.employeeId = null;
        this.releaseResource.NotifyAll = false;
        this.resetValues();
        this.projectsList = [];
        this.projectsList.push({ label: 'Select Project', value: 0 });
        this.disableddlProject = false;
    }
    resetValues = function (): void {
        this.releaseFormSubmitted = false;
        this.releaseResource.releaseDate = "";
        this.releaseResource.talentpool = "";
    }

    Clear(): void {
        this.clearAll();
    }

    OpenConfirmationDialog() {   // method to open dialog
        this.confirmationService.confirm({
            message: 'Do you want to clear ?',
            accept: () => {
                this.Clear()
            },
            reject: () => {

            }
        });
    }
    releaseAssociate(releaseResource: ResourceRelease) {
        // this.rel
        if (releaseResource.MakePrimaryProjectId > 0)
            releaseResource.IsPrimary = true;
        this.releaseFormSubmitted = true;
        if (releaseResource.releaseDate == '' || releaseResource.releaseDate == undefined) return;
        if ((releaseResource.releaseProjectId == null || releaseResource.releaseProjectId == undefined) && this.hideTrainingProject == false) return;
        releaseResource.EffectiveDate = moment(releaseResource.EffectiveDate).format('YYYY-MM-DD');
        releaseResource.releaseDate = moment(releaseResource.releaseDate).format('YYYY-MM-DD');
        this.IsValidDate = this.CheckDates(releaseResource.EffectiveDate, releaseResource.releaseDate);
        releaseResource.PrimaryProjectId = this.isEligiblePrimary ? this.ddlPrimaryProjectId : this.setPrimaryKey(this.remainingProjects.length);
        if (this.IsValidDate) {
            this.release(releaseResource);
        }
        this.isEligiblePrimary = false;
        this.isPrimaryProject = false;
    }
    cancel() {
        this.isEligiblePrimary = false;
        this.isPrimaryProject = false;
    }
    loadRemainingProjects(projectId) {
        this.remainingProjects = this.projectsList.filter(item => item.value != projectId);
    }
    setPrimaryKey(remainingProjectslen) {
        if (this.primaryProjectId != this.releaseResource.ProjectId) {
            return this.primaryProjectId;
        }
        else {
            switch (remainingProjectslen) {
                case 1:
                    return null;
                case 2:
                    return this.remainingProjects[1].value;

            }
        }
    }
    isPrimary() {
        if (this.primaryProjectId == this.releaseResource.ProjectId) {
            this.isPrimaryProject = true;
        }
    }
    getPrimaryProjectId(employeeId) {
        this._allocateService.GetEmployeePrimaryAllocationProject(employeeId).subscribe((res: any) => {
            // console.log(res);
            this.primaryProjectId = res[0].ProjectId;
            console.log("Primary project Id for the associate is:" + this.primaryProjectId);
        });
        return this.primaryProjectId;
    }

}



