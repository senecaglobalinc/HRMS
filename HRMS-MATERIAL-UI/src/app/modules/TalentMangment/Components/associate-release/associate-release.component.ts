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
import { Observable } from 'rxjs';
import { map, startWith } from "rxjs/operators";
import { NgxSpinnerService } from 'ngx-spinner';


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
    releaseToProjectsList = [];
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
    disableddlReleaseTo = false;
    remainingProjects : any = [];
    isPrimaryProject: boolean = false;
    primaryProjectId: any;
    //NotifyAll = true;
    ddlPrimaryProjectId: any;
    filteredEmployee : Observable<any>;
  
    @ViewChild(FormGroupDirective) formGroupDirective: FormGroupDirective;
  
  
    constructor(private _service: ResourceReleaseService,
       private masterDataService: MasterDataService,
        private formBuilder: FormBuilder,
        private _tempAllocationReleaseService: TemporaryAllocationReleaseService,
        private actRoute: ActivatedRoute,
        private snackBar: MatSnackBar,
        private spinner: NgxSpinnerService,
        private _allocateService: AssociateAllocationService) {
        this.componentName = this.actRoute.routeConfig.component.name;
    }
  
   
    ngOnInit() {
      this.spinner.show();
      this.employeeId = JSON.parse(sessionStorage['AssociatePortal_UserInformation']).employeeId;
      this.roleName = JSON.parse(sessionStorage['AssociatePortal_UserInformation']).roleName;
      
      this.isEligiblePrimary= false;

      this.releaseForm = new FormGroup({
        employeeId: new FormControl(null, [Validators.required]),
        ProjectId: new FormControl(null, [Validators.required]),
        releaseDate: new FormControl('', [Validators.required]),
        talentpool: new FormControl(null, ),
        NotifyAll:new FormControl(false),
        EffectiveDate: new FormControl(null, ),
        PrimaryProjectId: new FormControl(null, ),
        TalentRequisitionId: new FormControl(null, ),
        releaseProjectId: new FormControl(null, ),
        remainingProjects: new FormControl(null, ),
        reportingManagerId: new FormControl(null, ),
        ProjectName : new FormControl(null, ),

     });
      this.getDates();
      this.getEmployees();
  }
  displayFn(user: any) {
    return user && user.label ? user.label : '';
  }

  private getEmployees(): void {
      this._tempAllocationReleaseService.GetAssociatesToRelease(this.employeeId, this.roleName).subscribe((res: any) => {
        this.spinner.hide();
          this.associatesList = []
        //   this.associatesList.push({ label: '', value: null });
          this.projectsList = [];
          this.projectsList.push({ label: '', value: null });
          if (res.length > 0) {
              res.forEach((element: any) => {
                  if (this.associatesList.findIndex(x => x.label == element.EmpName) === -1)
                      this.associatesList.push({ label: element.EmpName, value: element.EmployeeId });
              });
              this.filteredEmployee = this.releaseForm.valueChanges.pipe(
                startWith(''),
                map((value) => this._filterEmployee(value.employeeId))
              );
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
  
   getProjectList(employeeId): void {
    this.projectDetails = [];
    this.spinner.show();
      this.masterDataService.GetAssociateProjectsForRelease(employeeId).subscribe((projectResponse: any[]) => {
        this.spinner.hide()
          this.projectDetails = projectResponse;
          this.talentpoolsList = [];
          this.talentpoolsList.push({ label: '', value: null });
          this.projectDetails.forEach((element: any) => {
              if (element.ProjectName.indexOf("Talent Pool") != -1 && this.talentpoolsList.findIndex(x => x.label == element.ProjectName) == -1)
                  this.talentpoolsList.push({ label: element.ProjectName, value: element.ProjectId });
          });
          this.getProjectsByEmployeeId(employeeId);
      },
          (error: any) => {
            this.spinner.hide()
              this.snackBar.open('Failed to Get Employees!.', 'x', {
                  duration: 1000,
                  panelClass:['error-alert'],
                  horizontalPosition: 'right',
                  verticalPosition: 'top',
                });
              this.Clear();
          });
  }
  
  getProjectsByEmployeeId(employeeId: any): void {
      this.primaryProjectId = this.getPrimaryProjectId(employeeId);
      let list = this.projectDetails
      if (employeeId != null && employeeId != undefined) {
          this.releaseForm.controls['releaseDate'].setValue(null);
          this.releaseForm.controls['ProjectId'].setValue(null);
          this.projectsList = [];
          this.projectsList.push({ label: '', value: null });
          if (list && list.length > 0) {
              list.forEach((element: ResourceRelease) => {
                  if (this.projectsList.findIndex(x => x.label == element.ProjectName) == -1) {
                      this.projectsList.push({ label: element.ProjectName, value: element.ProjectId })
                  };
                  if (element.ProjectName.indexOf("Training") == -1) {
                      this.hideTrainingProject = false;
                      this.releaseForm.controls['ProjectId'].setValue(element.ProjectId);
                      this.releaseForm.controls['ProjectName'].setValue(element.ProjectName);
                      this.releaseForm.controls['releaseProjectId'].setValue(null);
                  }
                  else {
                    this.releaseForm.controls['ProjectId'].setValue(element.ProjectId);
                    this.hideTrainingProject = true;
                  } 
              });
              if (list.length == 1) {
                  this.releaseForm.controls['ProjectId'].setValue(this.projectsList[1].value);
                  this.disableddlProject = true;
                  this.getAndfillProjectData(employeeId);
                  this.getAssociatepoolProject(employeeId, this.projectsList[1].value, this.roleName);
              }
              else {
                this.disableddlProject = false;
                this.releaseForm.controls['ProjectId'].setValue(this.projectsList[0].value);
                this.getAssociatepoolProject(employeeId, this.projectsList[1].value, this.roleName);
              } 
          }
      }
  }
  
   getAndfillProjectData(employeeId: number): void {
      if (employeeId != null && employeeId != undefined) {
        let list = this.projectDetails;
          if (list && list.length > 0) {
           var selectedProjectId= this.releaseForm.controls["ProjectId"].value;
          var selectedProjectEffectiveDate="";
           list.forEach(item=>
            {
                if(item.ProjectId ==selectedProjectId)
                {
                    selectedProjectEffectiveDate=item.EffectiveDate;
                }

            });          
              this.releaseForm.controls['EffectiveDate'].setValue(selectedProjectEffectiveDate);
          }
          if (this.hideTrainingProject == false) {
              let projectManager = this.projectDetails.filter((project: ResourceRelease) => {
                  return project.ProjectId == this.releaseForm.value.releaseProjectId;
              });
              if (projectManager && projectManager.length > 0)
                  this.releaseForm.controls['reportingManagerId'].setValue(projectManager[0].reportingManagerId);
          }
      }
      this.loadRemainingProjects(this.releaseForm.value.ProjectId);
      this.isPrimary();
  }
  
  private release(releaseResource: ResourceRelease): void {
      if (releaseResource != null && releaseResource != undefined) {
        if (releaseResource.NotifyAll != true) releaseResource.NotifyAll = false;
          releaseResource.TalentRequisitionId = 1;
          this.spinner.show();
          this._tempAllocationReleaseService.TemporaryReleaseAssociate(releaseResource).subscribe((data: any) => {
              if (data.IsSuccessful == true) {
                  this.snackBar.open('Associate released from the project successfully!', 'x', {
                      duration: 1000,
                      horizontalPosition: 'right',
                      verticalPosition: 'top',
                    });
                  this.releaseFormSubmitted = false;
                  this.Clear();
              }
              else if(data.Message!=null && data.Message.includes("Invalid release date please verify the last working day"))
                {
                    this.snackBar.open('Invalid release date, Please verify the last working day.', 'x', {
                        duration: 1000,
                        panelClass:['error-alert'],
                        horizontalPosition: 'right',
                        verticalPosition: 'top',
                    });
                    this.spinner.hide();
                }                
                else{
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
  
  private getAssociatepoolProject(EmployeeId: number, projectId: number, roleName: string) {
      if (EmployeeId != null && EmployeeId != undefined) {
          this._service.GetAssociateReleaseToProjects(EmployeeId, projectId, roleName).subscribe((res: any) => {
            this.releaseToProjectsList = [];
            if (res.length > 0) {
                res.forEach((element: any) => {
                  this.releaseToProjectsList.push({ label: element.ProjectName, value: element.ProjectId });
                });

                
                
                this.disableddlReleaseTo = true;
                if(this.releaseToProjectsList && this.releaseToProjectsList.length > 1)
                {
                this.disableddlReleaseTo = false;
                }
                else if(this.releaseToProjectsList && this.releaseToProjectsList.length == 1)
                {                    
                    this.releaseForm.controls['releaseProjectId'].setValue(res[0].ProjectId);
                    this.releaseForm.controls['reportingManagerId'].setValue(res[0].ReportingManagerId);
                }
            }

          },
          (error) => {

          });
      }
      else {
         // this.releaseForm.controls['talentpool'].setValue("");
          this.releaseForm.controls['releaseProjectId'].setValue(null);
          this.releaseForm.controls['reportingManagerId'].setValue(null);
      }
  }
  
  clearInput(evt: any, control): void {
      if (control == 'releaseDate'){
        evt.stopPropagation();
        this.releaseForm.get('releaseDate').reset();
      }
      if(control == 'employeeId'){
        evt.stopPropagation();
        this.releaseForm.reset();
        this.projectsList = [];
        this.releaseToProjectsList = [];
      }
      
  }

  
  private getDates(): void {
      var date = new Date(), y = date.getFullYear(), m = date.getMonth();
    //   this.firstDate = new Date(y, m, 1);
    this.firstDate = new Date(y, m, 0);
    if(this.firstDate.getDay()===6){
      this.firstDate.setDate(this.firstDate.getDate() - 1);
    }
    else if(this.firstDate.getDay()===0){
        this.firstDate.setDate(this.firstDate.getDate() - 2);
      }

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
  
  Clear = function (): void {
    // this.spinner.show();
      this.hideTrainingProject = true;
      this.releaseFormSubmitted = false;
      this.releaseForm.reset();
      setTimeout(() => this.formGroupDirective.resetForm(), 0);
      this.projectsList = [];      
      this.projectsList.push({ label: 'Select Project', value: 0 });
      this.disableddlProject = true;
      this.releaseToProjectsList = [];
      this.disableddlReleaseTo = true;
      this.getEmployees();   
    //   this.getProjectList();   
  }
  

  submit(): void {
    this.releaseFormSubmitted = true;
    this.isPrimary();
    let remainingPro = [];
    if(this.releaseForm.valid){
        if (this.isPrimaryProject && this.remainingProjects.length > 1) {
            this.isEligiblePrimary = true;
            this.remainingProjects.forEach((element: any) => {
                if (element.value != null) {
                    remainingPro.push(element.value);   
                }
            });
            this.releaseForm.controls['remainingProjects'].setValue(remainingPro);
            this.releaseAssociate(this.releaseForm.value);
        }
        else {
            this.releaseAssociate(this.releaseForm.value);
        }
    }
    else return;
    
}
private _filterEmployee(value) {
    let filterValue;
    if (value) {
        if(typeof value === 'string'){
            filterValue = value.toLowerCase();
        }
        else{
            filterValue = value.label.toLowerCase();
        }
        
      return this.associatesList.filter((option) =>
        option.label.toLowerCase().includes(filterValue)
      );
    } else {
      return this.associatesList;
    }
  }
  releaseAssociate(releaseResource: any) {
      releaseResource.employeeId = releaseResource.employeeId.value;
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
      this.releaseForm.controls['remainingProjects'].setValue(null);
  }
 
  loadRemainingProjects(projectId) {
      this.remainingProjects = this.projectsList.filter(item => item.value != projectId);
  }
  setPrimaryKey(remainingProjectslen) {
      if (this.primaryProjectId != this.releaseForm.value.ProjectId) {
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
      if (this.primaryProjectId == this.releaseForm.value.ProjectId) {
          this.isPrimaryProject = true;
      }
  }
  getPrimaryProjectId(employeeId) {
      this._allocateService.GetEmployeePrimaryAllocationProject(employeeId).subscribe((res: any) => {
          if(res){
            this.primaryProjectId = res.ProjectId;
          }
      });
      return this.primaryProjectId;
  }
  }