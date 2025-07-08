import { Component, OnInit, Injector, ViewChild, Inject } from '@angular/core';
import { Router, ActivatedRoute, } from '@angular/router';
import * as moment from 'moment';
import { MasterDataService } from '../../../services/masterdata.service';
import { CommonService } from '../../../services/common.service';
import { SelectItem, Message } from 'primeng/components/common/api';
import { Validators, FormControl, FormGroup, FormBuilder } from '@angular/forms';
import { ProjectsService } from '../../onboarding/services/projects.service';
import { ProjectsData } from '../../../models/projects.model';
import { GenericType } from '../../../models/dropdowntype.model';
import { ClientBillingRole } from 'src/app/components/projectLifeCycle/models/client-billing-role.model';
import { ProjectCreationService } from '../../projectLifeCycle/services/project-creation.service';
import { MessageService } from 'primeng/api';

@Component({
    selector: 'AddProject',
    templateUrl: './addproject.component.html',
    styleUrls: ['./projects.component.scss'],
    providers: [MessageService, ProjectsService, MasterDataService, CommonService]
})

export class AddProjectComponent implements OnInit {
    id: number;
    userRole: string;
    projectTypes: SelectItem[] = [];
    practiceAreas: SelectItem[] = [];
    reportingManagers: SelectItem[] = [];
    programManagers: SelectItem[] = [];
    statusDetails: any[] = [];
    leads: SelectItem[] = [];
    customersList: SelectItem[] = [];
    departmentList: any[] = [];
    departments: SelectItem[];
    roleList: SelectItem[];
    statusList: SelectItem[] = [{ label: 'Select Status', value: null }, { label: 'Active', value: true }, { label: 'Inactive', value: false }];
    public project: ProjectsData;
    currentProjectID: number;
    selectedRoles: string[];
    isSelectedRoles: boolean = true;
    userform: FormGroup;
    formSubmitted: boolean;
    existingRoleIds: any[] = [];
    filteredManagersIds: GenericType[];
    componentName: string;
    errorMessage: Message[] = [];
    lead: string = "";
    ReportingManager: string = "";
    ProgramManager: string = "";


    constructor(private messageService: MessageService, private _actRoute: ActivatedRoute, private _service: ProjectsService, private masterDataService: MasterDataService, private _commonService: CommonService,
        private _router: Router,
        private actRoute: ActivatedRoute,
        private fb: FormBuilder) {
        this.project = new ProjectsData();
        this.project.IsActive = true;
        this.componentName = this._actRoute.routeConfig.component.name;
    }
    ngOnInit() {
        this.userRole = JSON.parse(sessionStorage["AssociatePortal_UserInformation"]).roleName;
        let employeeId = JSON.parse(sessionStorage["AssociatePortal_UserInformation"]).employeeId;
        this.project.UserRole = this.userRole;
        this.userform = this.fb.group({
            'projectCode': new FormControl('', [Validators.required, Validators.pattern('^[a-zA-Z0-9]*$')]),
            'projectName': new FormControl('', [Validators.required]),
            'plannedEndDate': new FormControl(''),
            'plannedStartDate': new FormControl(''),
            // 'role': new FormControl('', Validators.required),
            'PracticeAreaId': new FormControl('', Validators.required),
            'ProjectTypeId': new FormControl('', Validators.required),
            'status': new FormControl('', Validators.required),
            'programManager': new FormControl(0),
            'actualStartDate': new FormControl(''),
            'actualEndDate': new FormControl(''),
            'reportingManager': new FormControl(0),
            'lead': new FormControl(0),
            'customer': new FormControl('', Validators.required),
            'department': new FormControl('', Validators.required)
        });
        this.formSubmitted = false;

        //get project types
        this.masterDataService.GetProjectTypes().subscribe((res: any) => {
            this.projectTypes = [];
            let resultData: any[] = res
            this.projectTypes.push({ label: 'Select Project type', value: null });
            resultData.forEach(element => {
                this.projectTypes.push({ label: element.ProjectTypeCode, value: element.ProjectTypeId });
            });
        });

        //get practice areas
        this.masterDataService.GetPractiseAreas().subscribe((res: any) => {
            this.practiceAreas = [];
            let resultData: any[] = res
            this.practiceAreas.push({ label: 'Select Technology', value: null });
            resultData.forEach(element => {
                this.practiceAreas.push({ label: element.PracticeAreaCode, value: element.PracticeAreaId });
            });
        });

        //GetCustomers
        this._service.getCustomers().subscribe((res: any) => {
            this.customersList = []
            let resultData: any[] = res
            this.customersList.push({ label: 'Select Customer', value: null });
            resultData.forEach(element => {
                this.customersList.push({ label: element.ClientName, value: element.ClientId });
            });
        });

        this.masterDataService.GetDepartments().subscribe((res: any) => {
            this.departments = [];
            let resultData: any[] = res
            this.departments.push({ label: 'Select Department', value: null });
            resultData.forEach(element => {
                this.departments.push({ label: element.DepartmentCode, value: element.DepartmentId });
            });
        });

        this.actRoute.params.subscribe(params => { this.id = params['id']; });
        this.currentProjectID = this.id;
        this.userform.get('projectCode').enable();
        if (this.currentProjectID != 0)
            this.GetProjectByID(this.currentProjectID);
        this.project.DepartmentId = 1;
    }


    private filteredManagers(event: any): void {
        let suggestionString = event.query;
        this.masterDataService.GetEmployeesAndManagers(suggestionString).subscribe((managersResponse: GenericType[]) => {
            this.filteredManagersIds = [];
            this.filteredManagersIds = managersResponse;
        },
            (error: any) => {
                if (error._body != undefined && error._body != "")
                    this._commonService.LogError(this.componentName, error._body).subscribe((data: any) => {
                    });
                this.errorMessage = [];
                this.errorMessage.push({ severity: 'error', summary: 'Failed to get Associates List!' });
            });
    }

    GetProjectByID(currentProjectID: number) {
        this._service.GetProjectDetailsbyID(currentProjectID).subscribe((projectdata: ProjectsData) => {
            this.project = new ProjectsData();
            projectdata = projectdata[0];
            this.project = projectdata;
            if (projectdata.ManagerId) {
                this.project.ProgramManager = new GenericType();
                this.project.ProgramManager.Id = projectdata.ManagerId;
                this.project.ProgramManager.Name = this.project.ManagerName;
                this.ProgramManager = this.project.ProgramManager.Name;

            }
            if (projectdata.ReportingManagerId) {
                this.project.ReportingManager = new GenericType();
                this.project.ReportingManager.Id = projectdata.ReportingManagerId;
                this.project.ReportingManager.Name = projectdata.ReportingManagerName;
                this.ReportingManager = this.project.ReportingManager.Name;

            }
            if (projectdata.LeadId) {
                this.project.Lead = new GenericType();
                this.project.Lead.Id = projectdata.LeadId;
                this.project.Lead.Name = projectdata.LeadName;
                this.lead = this.project.Lead.Name;

            }
            if (projectdata.ActualStartDate != null) {
                this.project.ActualStartDate = moment(projectdata.ActualStartDate.toString()).format('YYYY-MM-DD');
            }
            if (projectdata.ActualEndDate != null) {
                this.project.ActualEndDate = moment(projectdata.ActualEndDate.toString()).format('YYYY-MM-DD');
            }
            if (projectdata.PlannedStartDate != null) {
                this.project.PlannedStartDate = moment(projectdata.PlannedStartDate.toString()).format('YYYY-MM-DD');
            }
            if (projectdata.PlannedEndDate != null) {
                this.project.PlannedEndDate = moment(projectdata.PlannedEndDate.toString()).format('YYYY-MM-DD');
            }
            // this.project=projectdata;
            this.project.UserRole = this.userRole;
            this.bindProject(this.project[0]);

        }, (error: any) => {
            this.messageService.add({
                severity: 'error',
                summary: 'Error message',
                detail: 'Failed to get project details'
            });
        });
    }

    bindProject(projectdata: ProjectsData): any {
        this.userform.patchValue({
            projectCode: projectdata.ProjectCode,
            projectName: projectdata.ProjectName,
            customer: projectdata.ClientId,
            lead: projectdata.LeadId,
            ProjectTypeId: projectdata.ProjectTypeId,
            status: projectdata.IsActive,
            programManager: projectdata.ProgramManager,
            reportingManager: projectdata.ReportingManagerId,
            department: projectdata.DepartmentId,
            PracticeAreaId: projectdata.PracticeAreaId,
            actualStartDate: this.ModifyDateFormat(projectdata.ActualStartDate),
            actualEndDate: this.ModifyDateFormat(projectdata.ActualEndDate),
            plannedStartDate: this.ModifyDateFormat(projectdata.PlannedStartDate),
            plannedEndDate: this.ModifyDateFormat(projectdata.PlannedEndDate),
        });
        this.userform.get('projectCode').disable();

        this.project = projectdata;
        this.project.UserRole = this.userRole;
    }

    private ModifyDateFormat(date: string): Date {
        if (date != null) {
            return new Date(moment(date).format('MM/DD/YYYY'));
        }
        return null;
    }
    IsValidDate = function (fromDate: any, toDate: any) {
        if (Date.parse(fromDate) <= Date.parse(toDate))
            return true;
        return false;
    }

    saveProject() {
        // this.setRoleIds();
        if (this.project.ActualStartDate != null) {
            this.project.ActualStartDate = moment(this.project.ActualStartDate.toString()).format('YYYY-MM-DD');
        }
        if (this.project.ActualEndDate != null) {
            this.project.ActualEndDate = moment(this.project.ActualEndDate.toString()).format('YYYY-MM-DD');
        }
        if (this.project.PlannedStartDate != null) {
            this.project.PlannedStartDate = moment(this.project.PlannedStartDate.toString()).format('YYYY-MM-DD');
        }
        if (this.project.PlannedEndDate != null) {
            this.project.PlannedEndDate = moment(this.project.PlannedEndDate.toString()).format('YYYY-MM-DD');
        }
        if (this.project.PlannedStartDate && this.project.PlannedEndDate && !this.IsValidDate(this.project.PlannedStartDate, this.project.PlannedEndDate)) {
            this.messageService.add({
                severity: 'warn',
                summary: 'Warning message',
                detail: 'Planned End Date should be greater than PlannedStartDate'
            });
            return;
        }

        if (this.project.ActualStartDate && this.project.ActualEndDate && !this.IsValidDate(this.project.ActualStartDate, this.project.ActualEndDate)) {
            this.messageService.add({
                severity: 'warn',
                summary: 'Warning message',
                detail: 'Actual End Date should be greater than ActualStartDate'
            });
            return;
        }
        if (this.project.ProgramManager) this.project.ManagerId = this.project.ProgramManager.Id;
        if (this.project.ReportingManager) this.project.ReportingManagerId = this.project.ReportingManager.Id;
        if (this.project.Lead) this.project.LeadId = this.project.Lead.Id;
        if (this.currentProjectID == 0 || this.currentProjectID == undefined) {
            this._service.AddProjectDetails(this.project).subscribe((data) => {
                this.messageService.add({
                    severity: 'success',
                    summary: 'Success Message',
                    detail: 'Succesfully created project'
                });
                setTimeout(() => {
                    this._router.navigate(['/talentmanagement/projectsview']);
                }, 500);

                this.userform.reset()
            }, (error) => {
                this.messageService.add({
                    severity: 'error',
                    summary: 'Error message',
                    detail: 'Failed to add Project'
                });
            });
        }

        else if (this.currentProjectID != 0) {
            this._service.updateProjectDetails(this.project).subscribe((data) => {
                this.messageService.add({
                    severity: 'success',
                    summary: 'Success Message',
                    detail: 'Project Updated successfully'
                });
                setTimeout(() => {
                    this._router.navigate(['/talentmanagement/projectsview']);
                }, 500);
            }, (error) => {
                this.messageService.add({
                    severity: 'error',
                    summary: 'Error message',
                    detail: 'Failed to Update Project'
                });
            });
        }
    }

    onCancel() {
        this._router.navigate(['/talentmanagement/projectsview/']);
    }

    onSubmit(projectsData: ProjectsData) {
        this.formSubmitted = true;
        if (this.userform.valid) {
            this.formSubmitted = false;
            this.saveProject();
        }
    }
}


