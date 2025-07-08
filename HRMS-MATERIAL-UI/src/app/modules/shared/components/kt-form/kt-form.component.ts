import { ExitAnalysisService } from './../../../AssociateExit/Services/exit-analysis.service';
import { ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import {FormArray,FormBuilder,FormControl,FormGroup,Validators,} from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatSort } from '@angular/material/sort';
import { MatTable, MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute, Router } from '@angular/router';
import * as moment from 'moment';
import { startWith, map, takeUntil } from 'rxjs/operators';
import { Observable } from 'rxjs/Rx';
import { themeconfig } from '../../../../../themeconfig';
import { CommonService } from '../../../../core/services/common.service';
import { MasterDataService } from '../../../../core/services/masterdata.service';
import { ActivityList } from '../../../AssociateExit/Models/activitymodel';
import {Activity,AssociateExit,TransitionActivityDetails,} from '../../../AssociateExit/Models/associateExit.model';
import { ResignastionService } from '../../../AssociateExit/Services/resignastion.service';
import { GenericType } from '../../../master-layout/models/dropdowntype.model';
import { Associate } from '../../../onboarding/models/associate.model';
import { KtForm, UpdateTransitionPlan } from '../../models/kt-form.model';
import { KtFormService } from '../../services/kt-form.service';
import { CommonDialogComponent } from '../common-dialog/common-dialog.component';
import { NgxSpinnerService } from 'ngx-spinner';
import { EmployeeStatusService } from 'src/app/modules/admin/services/employeestatus.service';
import { KtWarningDialogComponent } from '../kt-warning-dialog/kt-warning-dialog.component';

interface SelectItem {
  value: number;
  label: string;
}

@Component({
  selector: 'app-kt-form',
  templateUrl: './kt-form.component.html',
  styleUrls: ['./kt-form.component.scss'],
  providers: [EmployeeStatusService]
})
export class KtFormComponent implements OnInit {
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  @ViewChild(MatTable) table: MatTable<any>;

  displayedColumns: string[] = ['Task','StartDate','EndDate','Status','Remarks','Action',];
  removeat = []
  themeConfigInput = themeconfig.formfieldappearances;
  dataSource = [];
  transitionToName:any;

  AssociateExit: AssociateExit = new AssociateExit();
  ActivityList: Activity[];
  KtFormValues: KtForm;
  KtPlan: KtForm;
  programManagerName: string = '';
  designationName: string = '';
  dialogResponse: boolean;
  added = false;
  numberOfActivities: number = 0;
  OthersIndex: number = null;
  showProject: boolean = false;
  KtPlanInProgress: boolean = false;
  currentPath: string;
  TeamLeadScreen: boolean = false;
  finalSubmission: boolean = false;
  submitDisable: boolean = false;
  btnLabel = 'Submit';
  showback = true;
  empId: number;
  projectId: number;
  deleteFromBackend: boolean = false;
  associatesList: SelectItem[] = [];
  filteredAssociates: Observable<any>;
  formSubmitted: boolean = false;
  KtForm: FormGroup;
  updateTransitionDetails = [];

  minKTPlanDate: Date;
  maxKTPlanDate: Date;

  activityMap = new Map();
  activityIdMap = new Map();
  disable:boolean = false;
  previous: string;
  roleName: string;
  defaultSelectedValue:any=null;
  updateTransition: boolean;
  formInvalid = false;
  ktStartDate : Date;
  ktEndDate : Date;
  activityStatusDD1= [{ "StatusId": "0", "StatusName": "Initiate" }];
  activityStatusDD2=[{ "StatusId": "1", "StatusName": "In Progress" }, { "StatusId": "2", "StatusName": "Completed" }]

  constructor(
    private actRoute: ActivatedRoute,
    private _masterDataService: MasterDataService,
    private _resignationService: ResignastionService,
    private _KtFormService: KtFormService,
    private _commonService: CommonService,
    private _snackBar: MatSnackBar,
    private fb: FormBuilder,
    public dialog: MatDialog,
    private _router: Router,
    private cd: ChangeDetectorRef,
    private exitanalysis: ExitAnalysisService,
    private spinner: NgxSpinnerService,
    private _service: EmployeeStatusService,
    public warningDialog : MatDialog
  ) {}

  ngOnInit(): void {
    this.actRoute.url.subscribe((url) => {
      this.currentPath = url[1]['path'];
    });
    this.roleName = JSON.parse(
      sessionStorage.AssociatePortal_UserInformation
    ).roleName;
    this.previous = this.exitanalysis.getPreviousUrl();
    if (this.currentPath == 'TeamLead') {
      this.TeamLeadScreen = true;
    }
    this.actRoute.params.subscribe((params) => {
      this.empId = params['empid'];
      this.projectId = params['projectid'];
    });
     this.getResignationDetails();
     this.createKtForm();
    //  this.getAssociateList();
  }

  getResignationDetails() {
    this.spinner.show();
    this._resignationService
      .getExitDetailsById(this.empId)
      .subscribe((res: any) => {
        this.spinner.hide();
        if (res) {
          this.AssociateExit = new AssociateExit();
          this.AssociateExit = res;
          this.minKTPlanDate = res.ResignationDate;
          this.maxKTPlanDate = res.ExitDate;
          res.DateOfJoin = moment(res.DateOfJoin).format('MM/DD/YYYY');
          res.ExitDate = moment(res.ExitDate).format('MM/DD/YYYY');
          if (this.AssociateExit.DepartmentId == 1) this.showProject = true;
          this.getAssociateList();
        }
      });
  }

  createKtForm() {
    this.KtForm = new FormGroup({
      transitionTo: new FormControl(null,[Validators.required]),
      others: new FormControl(null),
      startDate: new FormControl(null, [Validators.required]),
      endDate: new FormControl(null, [Validators.required]),
      knowledgeTransferred: new FormControl(null),
      knowledgeTransferredRemarks: new FormControl(null),
      activityType: new FormArray([this.CreateNewActivityTypeForm()]),
    });
  }

  CreateNewActivityTypeForm(): FormGroup {
    return this.fb.group({
      activityTypeId: new FormControl(''),
      updateTransitionDetail: new FormArray([this.CreateNewActivityForm()]),
    });
  }

  CreateNewActivityForm(): FormGroup {
    return this.fb.group({
      StartDate: new FormControl(null),
      EndDate: new FormControl(null),
      Status: new FormControl(null),
      Remarks: new FormControl(null),
      ActivityId: new FormControl(null),
      ActivityDescription: new FormControl(null),
      IsAdded:true,     
      StatusNotChangedToCompleted:true,
      StatusOptions: new FormControl(this.activityStatusDD1)
    });
  }

  onSelectMandataory(taskValue):void
{
  this.KtForm.markAsUntouched();
  if((taskValue.controls['ActivityId'].value!==null && taskValue.controls['ActivityId'].value!==undefined) || taskValue.controls['ActivityId'].value === -1){
    taskValue.controls['StartDate'].setValidators(Validators.required);
    taskValue.controls['StartDate'].updateValueAndValidity();
    taskValue.controls['EndDate'].setValidators(Validators.required);
    taskValue.controls['EndDate'].updateValueAndValidity();
    taskValue.controls['Status'].setValidators(Validators.required);
    taskValue.controls['Status'].setValue("0");
    taskValue.controls['Status'].updateValueAndValidity();
    taskValue.controls['StatusOptions'].setValue(this.activityStatusDD1)
  }else{
    taskValue.controls['StartDate'].setValue(null);
    taskValue.controls['EndDate'].setValue(null);
    taskValue.controls['Status'].setValue(null);
    taskValue.controls['StartDate'].clearValidators();
    taskValue.controls['StartDate'].updateValueAndValidity();
    taskValue.controls['EndDate'].clearValidators();
    taskValue.controls['EndDate'].updateValueAndValidity();
    taskValue.controls['Status'].clearValidators();
    taskValue.controls['Status'].updateValueAndValidity();

  }
  
}
  getAssociateList(): void {
    this.spinner.show();
    if(this.AssociateExit.DepartmentId==1)
    {   
    this._masterDataService
      .GetAllAssociateListByProject(this.projectId)
      .subscribe((associateResponse: GenericType[]) => {
        this.spinner.hide();
        associateResponse.forEach((associateResponse: GenericType) => {
          if(associateResponse.Id != this.empId){
            this.associatesList.push({
              label: associateResponse.Name,
              value: associateResponse.Id,
            });
          }
        });
        this.getKtTasks();
        this.getKtPlan();
        this.filteredAssociates = this.KtForm.get('transitionTo').valueChanges.pipe(
          startWith(''),
          map((value) => this._filterAssociates(value))
        );
      }),
      (error: any) => {
        this.spinner.hide();
        if (error._body != undefined && error._body != '')
          this._snackBar.open(error.error, 'x', {
            
            duration: 1000,
            panelClass: ['Failed to get Associate List'],
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
      };
    }
    else{
      this._service
      .GetAssociatesByDepartment(this.AssociateExit.DepartmentId)
      .subscribe((associateResponse: GenericType[]) => {
        this.spinner.hide();
        associateResponse.forEach((associateResponse: GenericType) => {
          if(associateResponse.Id != this.empId){
            this.associatesList.push({
              label: associateResponse.Name,
              value: associateResponse.Id,
            });
          }
        });
        this.getDepartmentHead();
        this.getKtTasks();
        this.getKtPlan();
        this.filteredAssociates = this.KtForm.get('transitionTo').valueChanges.pipe(
          startWith(''),
          map((value) => this._filterAssociates(value))
        );
      }),
      (error: any) => {
        this.spinner.hide();
        if (error._body != undefined && error._body != '')
          this._snackBar.open(error.error, 'x', {
            
            duration: 1000,
            panelClass: ['Failed to get Associate List'],
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
      };    
        
    }

  }

  private getDepartmentHead()
  {
    this._service.GetDepartmentHeadByDepartment(this.AssociateExit.DepartmentId)
    .subscribe((deptHead: GenericType) => {
      this.spinner.hide();
      this.associatesList.push({
        label: deptHead.Name,
            value: deptHead.Id,
      });
      this.filteredAssociates = this.KtForm.get('transitionTo').valueChanges.pipe(
        startWith(''),
        map((value) => this._filterAssociates(value))
      );
}),
(error: any) => {
  this.spinner.hide();
  if (error._body != undefined && error._body != '')
    this._snackBar.open(error.error, 'x', {
      
      duration: 1000,
      panelClass: ['Failed to get Associate List'],
      horizontalPosition: 'right',
      verticalPosition: 'top',
    });
};
  }

  private _filterAssociates(value) {
    let filterValue;
    if (typeof value === 'string') {
      filterValue = value.toLowerCase();
    } else if (value !== null) {
      filterValue = value.label;
    } else {
      return this.associatesList;
    }

    return this.associatesList.filter((option) =>
      option.label.toLowerCase().includes(filterValue)
    );
  }

  getKtTasks() {
    const unSelectedArray:TransitionActivityDetails={ActivityId:null,Description:'Select'}
    this.spinner.show();
    this._KtFormService.getKtTasks().subscribe((res: Activity[]) => {
      this.spinner.hide();
      this.ActivityList = new Array<Activity>();
      this.ActivityList = res;

      this.ActivityList.forEach((activityType: Activity, index) => {
        if(activityType.ActivityType !== 'Others'){
          activityType.TransitionActivityDetails.push(unSelectedArray);
        }
        if (activityType.ActivityType == 'Others') this.OthersIndex = index;
        activityType.TransitionActivityDetails.forEach(
          (activity: TransitionActivityDetails) => {
            this.activityMap.set(activity.ActivityId, index);
            this.activityIdMap.set(activity.ActivityId, activity.Description);
          }
        );
      });

      let i = 1;
      for (; i < this.ActivityList.length; i++) {
        const add = this.KtForm.get('activityType') as FormArray;
        add.push(this.CreateNewActivityTypeForm());
      }
    });
  }

  getKtPlan() {
    this.createKtForm();
    this.KtPlan = new KtForm();
    this.spinner.show();
    this._KtFormService
      .getKtPlan(this.empId, this.projectId)
      .subscribe((res) => {
        this.spinner.hide();
        if (res) {
          this.KtPlan = res;

          if (this.KtPlan.Status == 'KTPlanSubmitted') {
            if (this.TeamLeadScreen) {
              this.finalSubmission = true;
              this.btnLabel = 'Completed & Verified';
            }
          }

           if(this.previous == "/associateexit/associateexitinformation"){
             this.KtForm.disable();
           }

          this.KtPlan.UpdateTransitionDetail.forEach(
            (activity: UpdateTransitionPlan) => {
              if (activity.ActivityId <=1) {
                var i = this.OthersIndex;
              } else i = this.activityMap.get(activity.ActivityId);
              const a = this.KtForm['controls']['activityType']['controls'][
                i
              ].get('updateTransitionDetail') as FormArray;
              if (!a.value[0].ActivityId) {
                if (this.KtPlan.Status == 'KTPlanInProgress') {
                  a.controls[0].patchValue({
                    StartDate: activity.StartDate,
                    EndDate: activity.EndDate,
                    Status: this.roleName === 'Associate'? null :activity.Status,
                    Remarks: activity.Remarks,
                    ActivityId: activity.ActivityId,
                    ActivityDescription: activity.ActivityDescription,
                    IsAdded:false,
                    StatusOptions: this.roleName === 'Associate'? this.activityStatusDD2:this.activityStatusDD1
                  });
                }
                else if(this.KtPlan.Status == 'KTPlanSubmitted' && this.roleName=='Associate'){
                  a.controls[0].patchValue({
                    StartDate: activity.StartDate,
                    EndDate: activity.EndDate,
                    Status: (activity.Status == '0') ? null: activity.Status,
                    Remarks: activity.Remarks,
                    ActivityId: activity.ActivityId,
                    ActivityDescription: activity.ActivityDescription,
                    IsAdded:false,
                    StatusOptions: this.activityStatusDD2
                  });
                }
                else if(this.KtPlan.Status == 'KTPlanSubmitted' && this.roleName=='Team Lead'){
                  a.controls[0].patchValue({
                    StartDate: activity.StartDate,
                    EndDate: activity.EndDate,
                    Status: activity.Status,
                    Remarks: activity.Remarks,
                    ActivityId: activity.ActivityId,
                    ActivityDescription: activity.ActivityDescription,
                    IsAdded:false,
                    StatusOptions: (activity.Status == '0') ? this.activityStatusDD1:this.activityStatusDD2
                  });
                }
                else if(this.KtPlan.Status == 'KTPlanCompleted'){
                  a.controls[0].patchValue({
                    StartDate: activity.StartDate,
                    EndDate: activity.EndDate,
                    Status: activity.Status,
                    Remarks: activity.Remarks,
                    ActivityId: activity.ActivityId,
                    ActivityDescription: activity.ActivityDescription,
                    IsAdded:false,
                    StatusOptions: this.activityStatusDD2
                  });
                }
                else{
                  a.controls[0].patchValue({
                    StartDate: activity.StartDate,
                    EndDate: activity.EndDate,
                    Status: activity.Status,
                    Remarks: activity.Remarks,
                    ActivityId: activity.ActivityId,
                    ActivityDescription: activity.ActivityDescription,
                    IsAdded:false,
                    StatusOptions: this.activityStatusDD2
                  });
                }
                a.controls[0]['controls']['StartDate'].setValidators(Validators.required);
                a.controls[0]['controls']['StartDate'].updateValueAndValidity();
                a.controls[0]['controls']['EndDate'].setValidators(Validators.required);
                a.controls[0]['controls']['EndDate'].updateValueAndValidity();
                a.controls[0]['controls']['Status'].setValidators(Validators.required);
                a.controls[0]['controls']['Status'].updateValueAndValidity();
            //    a.disable()
              } else {
                a.push(this.CreateNewActivityForm());
                if (this.KtPlan.Status == 'KTPlanInProgress') {
                  a.controls[a.value.length - 1].patchValue({
                    StartDate: activity.StartDate,
                    EndDate: activity.EndDate,
                    Status: this.roleName === 'Associate'? null :activity.Status,
                    Remarks: activity.Remarks,
                    ActivityId: activity.ActivityId,
                    ActivityDescription: activity.ActivityDescription,
                    IsAdded:false,
                    StatusOptions: this.roleName === 'Associate'? this.activityStatusDD2:this.activityStatusDD1
                  });
                }
                else if(this.KtPlan.Status == 'KTPlanSubmitted' && this.roleName=='Associate'){
                  a.controls[a.value.length - 1].patchValue({
                    StartDate: activity.StartDate,
                    EndDate: activity.EndDate,
                    Status: (activity.Status == '0') ? null: activity.Status,
                    Remarks: activity.Remarks,
                    ActivityId: activity.ActivityId,
                    ActivityDescription: activity.ActivityDescription,
                    IsAdded:false,
                    StatusOptions: this.activityStatusDD2
                  });
                }
                else if(this.KtPlan.Status == 'KTPlanSubmitted' && this.roleName=='Team Lead'){
                  a.controls[a.value.length - 1].patchValue({
                    StartDate: activity.StartDate,
                    EndDate: activity.EndDate,
                    Status: activity.Status,
                    Remarks: activity.Remarks,
                    ActivityId: activity.ActivityId,
                    ActivityDescription: activity.ActivityDescription,
                    IsAdded:false,
                    StatusOptions: (activity.Status == '0') ? this.activityStatusDD1:this.activityStatusDD2
                  });
                }
                else if(this.KtPlan.Status == 'KTPlanCompleted'){
                  a.controls[a.value.length - 1].patchValue({
                    StartDate: activity.StartDate,
                    EndDate: activity.EndDate,
                    Status: activity.Status,
                    Remarks: activity.Remarks,
                    ActivityId: activity.ActivityId,
                    ActivityDescription: activity.ActivityDescription,
                    IsAdded:false,
                    StatusOptions: this.activityStatusDD2
                  });
                }
                else{
                  a.controls[a.value.length - 1].patchValue({
                    StartDate: activity.StartDate,
                    EndDate: activity.EndDate,
                    Status: activity.Status,
                    Remarks: activity.Remarks,
                    ActivityId: activity.ActivityId,
                    ActivityDescription: activity.ActivityDescription,
                    IsAdded:false
                    
                  });
                }
           //     a.disable()
           a.controls[a.value.length - 1]['controls']['StartDate'].setValidators(Validators.required);
          a.controls[a.value.length - 1]['controls']['StartDate'].updateValueAndValidity();
          a.controls[a.value.length - 1]['controls']['EndDate'].setValidators(Validators.required);
          a.controls[a.value.length - 1]['controls']['EndDate'].updateValueAndValidity();
          a.controls[a.value.length - 1]['controls']['Status'].setValidators(Validators.required);
          a.controls[a.value.length - 1]['controls']['Status'].updateValueAndValidity();
              }
            }
          );
          this.associatesList.forEach((e) => {
            if (e.value === this.KtPlan.TransitionTo) {
              this.transitionToName = e.label;
            }
          });
          let obj={label:this.transitionToName, value: this.KtPlan.TransitionTo}
          this.KtForm.patchValue({
            transitionTo: obj,
            others: this.KtPlan.Others,
            startDate: this.KtPlan.StartDate,
            endDate: this.KtPlan.EndDate,
          });
          if (this.KtPlan.Status == 'KTPlanInProgress') {
            this.KtPlanInProgress = true;
          }
          if (this.KtPlan.Status === 'KTPlanCompleted') {
            this.KtForm.disable();
            this.submitDisable = true;
          }
          if (this.roleName === 'Associate') {
            (<any>Object).values(this.KtForm['controls']['activityType']['controls']).forEach((control, index, object) => {
              (<any>Object).values(control.controls.updateTransitionDetail['controls']).forEach(ctrl => {
                if (ctrl.controls['ActivityId'].value === null) {
                  control.controls.updateTransitionDetail.removeAt(ctrl)
                }
              })
              if (control.controls.updateTransitionDetail.length === 0) {
                this.removeat.push(index)
              }
            });
            for (var i = this.removeat.length - 1; i >= 0; i--){
              this.ActivityList.splice(this.removeat[i], 1);
              this.activitytypes.removeAt(this.removeat[i])
            }
          }
        }
})
}
  get activitytypes() {
    return this.KtForm.get('activityType') as FormArray;
  }
  panelOpened(item, i) {
    this.dataSource = this.KtForm['controls']['activityType']['controls'][i]['controls']['updateTransitionDetail']['controls'];
  }

  onNewTask(indexActivityType, indexAcitivity) {
    this.added = true;
    const add = this.KtForm['controls']['activityType']['controls'][
      indexActivityType
    ].get('updateTransitionDetail') as FormArray;
    add.push(this.CreateNewActivityForm());


    this.cd.detectChanges();
    return false;
  }

  onDelete(indexActivityType, indexActivity) {
    this.openDialog(
      'Confirmation',
      'Do you want to Delete ?',
      indexActivityType,
      indexActivity
    );
  }

  openDialog(Heading, Message, indexActivityType, indexActivity): void {
    const dialogRef = this.dialog.open(CommonDialogComponent, {
      width: '300px',
      disableClose: true,
      data: { heading: Heading, message: Message },
    });

    dialogRef.afterClosed().subscribe((result) => {
      this.dialogResponse = result;
      if (this.dialogResponse == true) {
        let activityId = this.KtForm.value.activityType[indexActivityType]
          .updateTransitionDetail[indexActivity].ActivityId;
        this.spinner.show();
        this._KtFormService
          .getKtPlan(this.empId, this.projectId)
          .toPromise()
          .then((res) => {
            this.spinner.hide();
            if (res) {
              this.KtPlan = res;
              for (let activity of this.KtPlan.UpdateTransitionDetail) {
                if (activity.ActivityId == activityId) {
                  this.deleteFromBackend = true;
                  break;
                }
              }
              if (this.deleteFromBackend) {
                this.spinner.show();
                this._KtFormService
                  .deleteKtTasks(this.empId, this.projectId, activityId)
                  .subscribe((res: any) => {
                    this.spinner.hide();
                    if (res) {
                      this._snackBar.open(
                        'Activity Deleted successfully.',
                        'x',
                        {
                          duration: 1000,
                          panelClass:['success-alert'],
                          horizontalPosition: 'right',
                          verticalPosition: 'top',
                        }
                      );
                    }
                  });
              } else {
                this._snackBar.open('Activity Deleted successfully.', 'x', {
                  duration: 1000,
                  panelClass: ['success-alert'],
                  horizontalPosition: 'right',
                  verticalPosition: 'top',
                });
              }
              this.deleteFromBackend = false;
            }
          });

        const remove = this.KtForm['controls']['activityType']['controls'][
          indexActivityType
        ].get('updateTransitionDetail') as FormArray;
        remove.removeAt(indexActivity);
        this.added = false;
        // this.table.renderRows();
      }
    });
  }

  clearField(fieldName) {
    if (fieldName == 'transitionTo') {
      this.KtForm.controls.transitionTo.setValue(null);
    }
  }

  save() {
    if(Date.parse(this.KtForm.value.startDate)>Date.parse(this.KtForm.value.endDate)){
      this._snackBar.open(
        "start date should not greater than end date.",
        "",
        {
          duration: 1000,
          horizontalPosition: "right",
          verticalPosition: "top",
        }
      );
      return;
    }
    this.KtFormValues = new KtForm();
    this.KtFormValues.Type = 'Save';
    this.updateKtTasks();
  }

  setActivityId(item:any){
    item.controls['ActivityId'].patchValue(-1);
  }

  submitDialog(update:boolean = false) {
    this.updateTransition = update;
    if(Date.parse(this.KtForm.value.startDate)>Date.parse(this.KtForm.value.endDate)){
      this._snackBar.open(
        "start date should not greater than end date.",
        "",
        {
          duration: 1000,
          horizontalPosition: "right",
          verticalPosition: "top",
        }
      );
      return;
    }

    const dialogRef = this.dialog.open(CommonDialogComponent, {
      width: '300px',
      disableClose:true,
      data: {
        heading: 'Confirmation',
        message: 'Are you sure you want to Submit?',
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      this.dialogResponse = result;
      if (this.dialogResponse == true) {
        this.submit();
      }
    });
  }

  submit() {
    this.KtFormValues = new KtForm();
    if (!this.KtPlan.Status ||(this.KtPlan.Status=="KTPlanInProgress" &&  this.roleName=="Team Lead")) this.KtFormValues.Type = 'TLSubmit';
    else if (this.roleName == 'Associate')
      this.KtFormValues.Type = 'AssociateSubmit';
    else if(this.updateTransition)
      this.KtFormValues.Type = 'TLSubmit'  
    else if ((this.KtFormValues.Status = 'KTPlanSubmitted') && this.roleName=="Team Lead")
      this.KtFormValues.Type = 'TLComplete';

    this.updateKtTasks();
  }
  displayFn(user: any) {
    return user && user.label ? user.label : '';
  }
  updateKtTasks() {
    this.formInvalid = false;
    this.formSubmitted = true;
    this.updateTransitionDetails=[];
    this.KtFormValues.UpdateTransitionDetail=[];
    if(!this.KtForm.touched && this.KtFormValues.Type != 'TLComplete'){
      this._snackBar.open(
        "No changes to update!",
        "",
        {
          duration: 1000,
          horizontalPosition: "right",
          verticalPosition: "top",
        }
      );
      return;
    }
    else{
      this.formInvalid = false;
      (<any>Object).values(this.KtForm['controls']['activityType']['controls']).forEach(control => {
        control.markAsTouched();
        (<any>Object).values(control.controls.updateTransitionDetail['controls']).forEach(ctrl => {
          if(ctrl.controls['ActivityId'].value != null){
             
             let startDate= new Date(ctrl.controls['StartDate'].value)
             let endDate = new Date(ctrl.controls['EndDate'].value)
             let mainStartDate = new Date(this.KtForm.value.startDate);
             let mainEndDate = new Date(this.KtForm.value.endDate);

             if(!((startDate>=mainStartDate&&startDate<=mainEndDate)&&
             (endDate>=startDate&&endDate<=mainEndDate))){
               ctrl.controls['StartDate'].markAsTouched();
               ctrl.controls['EndDate'].markAsTouched();
               if(startDate < mainStartDate || startDate > mainEndDate){
                ctrl.controls['StartDate'].setErrors({'incorrect': true});
               }
               if(endDate <startDate || endDate > mainEndDate){
                ctrl.controls['EndDate'].setErrors({'incorrect': true});
               }
               this.formInvalid = true;
             }
           
          }
        })
      });
      
      if(this.formInvalid){
        this._snackBar.open(
          'Please correct the date fields highlighted',
          'x',
          {
            panelClass:['error-alert'],
            duration: 5000,
            horizontalPosition: 'right',
            verticalPosition: 'top',
          }
        );
        return;
      }
      else{
        this.TransitionPlanSubmission();
      }
    }
  }

  TransitionPlanSubmission() {
    if (this.KtForm.valid) {
      this.KtFormValues.EmployeeId = this.empId;
      this.KtFormValues.ProjectId = this.projectId;
      this.KtFormValues.TransitionFrom = this.AssociateExit.EmployeeId;
      this.KtFormValues.TransitionTo = this.KtForm.value.transitionTo.value;
      this.KtFormValues.Others = this.KtForm.value.others;
      this.KtFormValues.StartDate = this.KtForm.value.startDate;
      this.KtFormValues.EndDate = this.KtForm.value.endDate;
      if (this.KtForm.value.knowledgeTransferred)
        this.KtFormValues.KnowledgeTransferred = this.KtForm.value.knowledgeTransferred;
      if (this.KtForm.value.knowledgeTransferredRemarks)
        this.KtFormValues.KnowledgeTransaferredRemarks = this.KtForm.value.knowledgeTransferredRemarks;

      if (this.KtForm.value.activityType[this.OthersIndex] &&
        this.KtForm.value.activityType[this.OthersIndex]
          .updateTransitionDetail[0].ActivityDescription &&
        !this.KtForm.value.activityType[this.OthersIndex]
          .updateTransitionDetail[0].ActivityId
      )
        this.KtForm.value.activityType[
          this.OthersIndex
        ].updateTransitionDetail[0].ActivityId = -1;

      var i = 0;

      while (i < this.KtForm.value.activityType.length) {
        var j = 0;
        while (
          j < this.KtForm.value.activityType[i].updateTransitionDetail.length
        ) {
          if (
            this.KtForm.value.activityType[i].updateTransitionDetail[j]
              .ActivityId
          ) {
            this.updateTransitionDetails.push(
              this.KtForm.value.activityType[i].updateTransitionDetail[j]
            );
          }
          j++;
        }
        i++;
      }
      this.KtFormValues.UpdateTransitionDetail = this.updateTransitionDetails;
      const itemlu: any = this.KtForm['controls']['activityType']['controls'];
      for (let i = 0; i < itemlu.length; i++) {
        if (itemlu[i].controls['updateTransitionDetail'].length > 0) {
          for (let j = 0; j < itemlu[i].controls['updateTransitionDetail'].controls.length; j++) {
            itemlu[i].controls['updateTransitionDetail'].controls[j].controls.IsAdded.setValue(false);
          }
        }
      }

      this.spinner.show();
      this._KtFormService.updateKtTasks(this.KtFormValues).subscribe((res) => {
        this.spinner.hide();
        if (res) {
          if (this.KtFormValues.Type == 'Save') {
            this._snackBar.open('Transition Plan Saved successfully.', 'x', {
              duration: 1000,
              panelClass: ['success-alert'],
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
          } else {
            if (res && res['IsSuccessful'] == true) {
              this._snackBar.open(
                'Transition Plan Submitted successfully.',
                'x',
                {
                  panelClass: ['success-alert'],

                  duration: 1000,
                  horizontalPosition: 'right',
                  verticalPosition: 'top',
                }
              );
              this._router.navigate(['/shared/dashboard']);
            }
            else {
              this._snackBar.open(
                res['Message'],
                'x',
                {
                  panelClass: ['error-alert'],

                  duration: 1000,
                  horizontalPosition: 'right',
                  verticalPosition: 'top',
                }
              );
            }
          }
        }
      }),
        (error) => {
          this.spinner.hide();
          this._snackBar.open('Unable to submit Transition Plan', 'x', {
            duration: 1000,
            panelClass: ['error-alert'],
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
        };
    } else {
      this.KtForm.markAllAsTouched();
      this._snackBar.open('Please correct the fields highlighted', 'x', {
        duration: 1000,
        panelClass: ['error-alert'],
        horizontalPosition: 'right',
        verticalPosition: 'top',
      });
    }
  }
  onBack() {
    if (this.previous == '/associateexit/associateexitinformation')    {
      this._router.navigate(['associateexit/associateexitinformation'])
    }
    else{this._router.navigate(['shared/dashboard']);}
  }
  
  statusChange(activityControl){
    activityControl.controls['StatusNotChangedToCompleted'].setValue(false)
  }

  onDateChange(){
    if(this.KtForm.value.startDate != null && this.KtForm.value.endDate != null){
      const dialogRef = this.warningDialog.open(KtWarningDialogComponent, {
        width: '400px',
        disableClose: true
      });
    }
  }
}