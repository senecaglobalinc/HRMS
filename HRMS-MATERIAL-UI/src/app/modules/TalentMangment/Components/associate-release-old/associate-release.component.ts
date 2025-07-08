import { Component, OnInit, ViewChild } from '@angular/core';
import * as moment from 'moment';
import { ResourceReleaseService } from '../../services/resource-release.service';
import { ActivatedRoute } from '@angular/router';
import { ResourceRelease } from '../../models/resourcerelease.model';
import { TemporaryAllocationReleaseService } from '../../services/temporary-allocation-release.service';
import { AssociateAllocationService } from '../../services/associate-allocation.service';
import { MasterDataService } from 'src/app/modules/master-layout/services/masterdata.service';
import { themeconfig } from 'src/themeconfig';
import {FormControl, FormGroup, FormBuilder, Validators, FormGroupDirective } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';


@Component({
    selector: 'app-associate-release',
    templateUrl: './associate-release.component.html',
    styleUrls: ['./associate-release.component.scss']
})
export class AssociateReleaseComponent implements OnInit {
    themeConfigInput = themeconfig.formfieldappearances;
    releaseResource: ResourceRelease;
    releaseFormSubmitted = false;
    firstDate: Date;
    lastDate: Date;
    IsValidDate: boolean;
    releaseForm: FormGroup;
    invalid: boolean;
    projectsList = [];
    talentpoolsList= [];
    associatesList = [];
    associatesProjectsList: any[] = [];
    projectDetails: ResourceRelease[] = [];
    componentName: string;
    hideTrainingProject = true;
    employeeId: any;
    roleName: any;
    isEligiblePrimary = false;
    disableddlProject = false;
    remainingProjects : any = [];
    isPrimaryProject: boolean = false;
    primaryProjectId: any;
    //NotifyAll = true;
    ddlPrimaryProjectId: any;
  
    @ViewChild(FormGroupDirective) formGroupDirective: FormGroupDirective;
  
  
    constructor(private _service: ResourceReleaseService,
       private masterDataService: MasterDataService,
        private formBuilder: FormBuilder,
        private _tempAllocationReleaseService: TemporaryAllocationReleaseService,
        private actRoute: ActivatedRoute,
        private snackBar: MatSnackBar,
        private _allocateService: AssociateAllocationService) {
        this.componentName = this.actRoute.routeConfig.component.name;
    }
  
   
    ngOnInit() {
      this.releaseResource = new ResourceRelease();
      this.employeeId = JSON.parse(sessionStorage['AssociatePortal_UserInformation']).employeeId;
      this.roleName = JSON.parse(sessionStorage['AssociatePortal_UserInformation']).roleName;
      
    //  this.releaseResource.NotifyAll = true;
      this.isEligiblePrimary= false;

      this.releaseForm = new FormGroup({
        EmployeeId: new FormControl(null, [Validators.required]),
        ProjectId: new FormControl(null, [Validators.required]),
        ReleaseDate: new FormControl('', [Validators.required]),
        talentpool: new FormControl(null, ),
        NotifyAll:new FormControl(false)
     });
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
              this.snackBar.open('Failed to Get Employees!.', 'x', {
                  duration: 1000,
                  panelClass:['error-alert'],
                  horizontalPosition: 'right',
                  verticalPosition: 'top',
                });
              this.Clear();
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
              this.snackBar.open('Failed to Get Employees!.', 'x', {
                  duration: 1000,
                  panelClass:['error-alert'],
                  horizontalPosition: 'right',
                  verticalPosition: 'top',
                });
              this.Clear();
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
  
   getAndfillProjectData(employeeId: number): void {
      if (employeeId != null && employeeId != undefined) {
          let list = this.associatesProjectsList.filter((project: any) => {
              return (project.EmployeeId == employeeId && project.ProjectId == this.releaseResource.ProjectId);
          });
          if (list && list.length > 0) {
              this.releaseResource.EffectiveDate = list[0].EffectiveDate;
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
  
    //   if (this.isPrimaryProject && this.remainingProjects.length > 1) {
         
  
    //       this.isEligiblePrimary = true;
         
    //   }
  }
  
  private release(releaseResource: ResourceRelease): void {
      if (releaseResource != null && releaseResource != undefined) {
        if (releaseResource.NotifyAll != true) releaseResource.NotifyAll = false;
          this.releaseResource.TalentRequisitionId = 1;
          this._tempAllocationReleaseService.TemporaryReleaseAssociate(this.releaseResource).subscribe((data: any) => {
              if (data.IsSuccessful == true) {
                  this.snackBar.open('Associate released from the project successfully!', 'x', {
                      duration: 1000,
                      horizontalPosition: 'right',
                      verticalPosition: 'top',
                    });
                  this.releaseFormSubmitted = false;
                  this.Clear();
                  this.getEmployees();
              }
              else {
                  this.snackBar.open('Failed to release associate from project.', 'x', {
                      duration: 1000,
                      panelClass:['error-alert'],
                      horizontalPosition: 'right',
                      verticalPosition: 'top',
                  });
                  this.Clear();
                  return;
              }
          }, (error) => {
              this.snackBar.open('Failed to release associate from project.', 'x', {
                  duration: 1000,
                  panelClass:['error-alert'],
                  horizontalPosition: 'right',
                  verticalPosition: 'top',
              });
              this.Clear();
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
  
  clearInput(evt: any): void {
      evt.stopPropagation();
      this.releaseResource.releaseDate='';
  }

  
  private getDates(): void {
      var date = new Date(), y = date.getFullYear(), m = date.getMonth();
      this.firstDate = new Date(y, m, 1);
      this.lastDate = new Date();
    }


    private CheckDates = function (fromDate: any, toDate: any): boolean {
        if ((fromDate != null || fromDate != undefined) && (toDate != null && toDate != undefined)) {
            if (Date.parse(fromDate) <= Date.parse(toDate)) {
                this.invalid = false;
                return true
            }
            else {

                this.snackBar.open('Invalid Release date, please check associate`s allocations.', 'x', {
                                      duration: 1000,
                                      horizontalPosition: 'right',
                                      verticalPosition: 'top',
                                    });
                // this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: 'Invalid Release date, please check associate`s allocations.' });
                this.invalid = true;
                return false
            }

        }
    }
  
//   private CheckDates = function (fromDate: any, toDate: any): boolean {
//       if ((fromDate != null || fromDate != undefined) && (toDate != null && toDate != undefined)) {
//           if (Date.parse(fromDate) <= Date.parse(toDate)) {
//               this.invalid = false;
//               return true
//           }
//           else {
  
//               this.snackBar.open('Invalid Release date, please check associate`s allocations.', 'x', {
//                   duration: 1000,
//                   panelClass:['error-alert'],
//                   horizontalPosition: 'right',
//                   verticalPosition: 'top',
//                 });
//               this.Clear();
//               this.invalid = true;
//               return false
//           }
  
//       }
//   }
  Clear = function (): void {
      //this.releaseResource.MakePrimaryProjectId = null;
     // this.isEligiblePrimary= false;
      this.hideTrainingProject = true;
      this.releaseResource.ProjectId = null;
      this.releaseResource.ProjectName = null;
      this.releaseResource.employeeId = null;
      this.releaseResource.NotifyAll = false;
      this.resetValues();
      this.releaseForm.reset();
      this.releaseForm.get('EmployeeId').setErrors(null);
      this.releaseForm.get('EmployeeId').clearValidators();
      this.releaseForm.get('EmployeeId').updateValueAndValidity();
      this.releaseForm.get('ReleaseDate').setErrors(null);
      this.releaseForm.get('ReleaseDate').clearValidators();
      this.releaseForm.get('ReleaseDate').updateValueAndValidity();
    //   setTimeout(() => this.formGroupDirective.resetForm(), 0);
      this.projectsList = [];
      this.projectsList.push({ label: 'Select Project', value: 0 });
      this.disableddlProject = true;
  }
  resetValues = function (): void {
      this.releaseFormSubmitted = false;
      this.releaseResource.releaseDate = "";
      this.releaseResource.talentpool = "";
  }

  submit(): void {
      if(this.releaseResource.employeeId == null){
        this.snackBar.open('Please Select the Employee', 'x', {
            duration: 1000,
            horizontalPosition: 'right',
            verticalPosition: 'top',
        });
        return;
      }
      this.releaseForm.get('ReleaseDate').setValidators([Validators.required])
      this.releaseForm.get('ReleaseDate').updateValueAndValidity();
    this.isPrimary();
    this.releaseResource.remainingProjects = [];
    if (this.isPrimaryProject && this.remainingProjects.length > 1) {
        this.isEligiblePrimary = true;
        this.remainingProjects.forEach((element: any) => {
            if (element.value != null) {
                this.releaseResource.remainingProjects.push(element.value);   
            }
        });
        this.releaseAssociate(this.releaseResource);
    }
    else {
        this.releaseAssociate(this.releaseResource);
    }
}
  
  releaseAssociate(releaseResource: ResourceRelease) {
//       // this.rel
//       if ( this.remainingProjects.length > 0 && !releaseResource.MakePrimaryProjectId ){
  
//   this.isEligiblePrimary = false;
  
//       //     this.snackBar.open('Please select one primary project.', 'x', {
//       //         duration: 1000,
//       //         panelClass:['error-alert'],
//       //         horizontalPosition: 'right',
//       //         verticalPosition: 'top',
//       //       });
//       //    this.Clear();
         
//       //     return false;
//       }
//   else if (releaseResource.MakePrimaryProjectId > 0)
//           releaseResource.IsPrimary = true;
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
      this.releaseResource.remainingProjects = null;
  }
  cancel() {
      this.isEligiblePrimary = false;
      this.isPrimaryProject = false;
      this.releaseResource.remainingProjects = null;
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
          if(res.length > 0){
            this.primaryProjectId = res[0].ProjectId;
          }
      });
      return this.primaryProjectId;
  }
  }