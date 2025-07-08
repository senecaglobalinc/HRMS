import { ExitAnalysisService } from './../../Services/exit-analysis.service';
import { Component, OnInit } from '@angular/core';
import { ProjectsData } from '../../../master-layout/models/projects.model';
import * as servicePath from "../../../../core/service-paths";
import { Router, ActivatedRoute } from '@angular/router';
import * as moment from "moment";
import { Subscription } from 'rxjs';
import {MatSnackBar} from '@angular/material/snack-bar';
import * as fileSaver from 'file-saver';
import 'rxjs/Rx' ;
import{ ChecklistService} from '../../Services/checklist.service';
import { ActivityData, ActivityDetails, ActivityList } from '../../Models/activitymodel';
import { themeconfig } from 'src/themeconfig';
import { AssociateExit } from '../../Models/associateExit.model';
import { ResignastionService } from '../../Services/resignastion.service';
import { MasterDataService } from 'src/app/core/services/masterdata.service';
import { Associate } from 'src/app/modules/onboarding/models/associate.model';
import { FormGroup, FormControl, FormArray } from '@angular/forms';
import { CommonDialogComponent } from 'src/app/modules/shared/components/common-dialog/common-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { NgxSpinnerService } from 'ngx-spinner';


interface SelectItem {
  value: number;
  label: string;
}

@Component({
  selector: 'app-clearenceform',
  templateUrl: './clearenceform.component.html',
  styleUrls: ['./clearenceform.component.scss']
})
export class ClearenceformComponent implements OnInit {

  tlfilled:boolean=false;
  tlnotfilled:boolean=false;
  projectId: number;
  actualExitDate: any;
  FileType:string;
  PageSize: number = 0;
  PageDropDown: number[] = [];
  resources = servicePath.API.PagingConfigValue;
  projectStateSubscription: Subscription;
  projectState: string;
  associateState: string;

  btnLabel : string = "Deactivate";
  showButton : boolean = true;

  selectedTabSubscription: Subscription;
 // clientBillingRoleData = [];
  // SOWData: SOW[] = [];
  dashboard: string;
  isDrafted: boolean = false;
  employeeIdSubscription: Subscription;
  canApprove: boolean;
  EmpId: number;
  hideBack: boolean = false;
  showback: boolean = false;
  activityData = [];
  themeConfigInput = themeconfig.formfieldappearances;
  checklist: Map<number,string> = new Map();
  activityType: Map<number,string> = new Map();
  IsRequired: Map<number,boolean> = new Map();
  roleName: string="";
  pageload: boolean = false;
  associateData:AssociateExit;
  employeeId:number;
  activities = []; 
  clearenceForm: FormGroup;
  panelOpenState = false;
  serviceDeptID: number;
departmentId: number;
  projectData: ProjectsData;
  reasonId: number;
  reasonDetail: string;
   rehireEligibility: boolean;
   rehireEligibilityDetail: string;
   impactOnClientDelivery: boolean;
   impactOnClientDeliveryDetail: string;
   selectDisabled = true;
   reasonList: any;
   prevActivity: string;
   previous: string;
   disable=false;
   dialogResponse: boolean;
   currentDate =  moment(new Date()).format("MM/DD/YYYY");
   UpdateExitDateRequired = false;
  constructor(
    private actRoute: ActivatedRoute, private router: Router,
    private _snackBar: MatSnackBar,
    public dialog: MatDialog,
    private checklistService: ChecklistService,
 private resignationservice:ResignastionService,
 private exitanalysis: ExitAnalysisService,
 private spinner: NgxSpinnerService
    ) {
      this.actualExitDate = this.router.getCurrentNavigation().extras.state
      this.resignationservice.getResigReasonDtls().subscribe(data => {   
        this.reasonId = data.ReasonId;
       this.clearenceForm.get('reasonId').setValue(this.reasonId);
        this.reasonDetail = data.ReasonDetail;
        this.clearenceForm.get('reasonDetail').setValue(this.reasonDetail);

        
  
  
        this.rehireEligibility = data.RehireEligibility;
        this.impactOnClientDelivery = data.ImpactOnClientDelivery
        // this.clearenceForm.get('rehireEligibility').setValue(true);
        this.clearenceForm.patchValue({rehireEligibility:this.rehireEligibility})
        this.clearenceForm.patchValue({impactOnClientDelivery: this.impactOnClientDelivery})
        this.rehireEligibilityDetail = data.RehireEligibilityDetail;
        this.impactOnClientDeliveryDetail = data.ImpactOnClientDeliveryDetail
        this.clearenceForm.get('rehireEligibilityDetail').setValue(this.rehireEligibilityDetail);
        this.clearenceForm.get('impactOnClientDeliveryDetail').setValue(this.impactOnClientDeliveryDetail);
  
  
        this.clearenceForm.get('reasonId').updateValueAndValidity();
        this.clearenceForm.get('reasonDetail').updateValueAndValidity();
  
        this.clearenceForm.get('rehireEligibility').updateValueAndValidity();
        this.clearenceForm.get('rehireEligibilityDetail').updateValueAndValidity();
        this.clearenceForm.get('impactOnClientDelivery').updateValueAndValidity();
        this.clearenceForm.get('impactOnClientDeliveryDetail').updateValueAndValidity();
  
      });//this.projectData = new ProjectsData();
     }

  ngOnInit(): void {
    // this.EmpId = parseInt(JSON.parse(sessionStorage["AssociatePortal_UserInformation"]).employeeId);
    // this.actRoute.params.subscribe(params => { this.dashboard = params["id"]; });
    // this.roleName = JSON.parse(sessionStorage["AssociatePortal_UserInformation"]).roleName;
    this.previous = this.exitanalysis.getPreviousUrl();
    this.roleName = JSON.parse(
      sessionStorage["AssociatePortal_UserInformation"]
    ).roleName;

    if(this.roleName =="Operations Head"){
      this.btnLabel = "Approve";
    }
    else if(this.roleName =="HRM"){
      this.btnLabel = "Deactivate";
    }
    else if (this.roleName == "HRA"){
      this.showButton = false;
    }

    this.actRoute.params.subscribe(params => {
      this.employeeId = params["id"];
    });

    
    this.getActivitiesByHRA();
     

    this.createForm();
    this.resignationservice.getResignationReason().subscribe(data=>{
      this.reasonList = data;
      // this.reasonList = data
    })

    this.getRemarks();

  }
  

  
  createForm() {

    this.clearenceForm = new FormGroup({
      reasonId: new FormControl({value: (this.reasonId)? this.reasonId : null, disabled : true}),
      reasonDetail: new FormControl({value: (this.reasonDetail)? this.reasonDetail : null,disabled : true}),
      rehireEligibility: new FormControl({value : null, disabled : true}),
      rehireEligibilityDetail: new FormControl({value: (this.rehireEligibilityDetail)? this.rehireEligibilityDetail : null,disabled : true}),
      remarksByHRM : new FormControl({value : null,disabled : this.roleName=='HRA' || this.roleName=='Operations Head'}),
      remarksByOpsHead : new FormControl({value : null,disabled : this.roleName=='HRA'}),
      remarksByHRA : new FormControl(null),
      impactOnClientDelivery : new FormControl({value : null, disabled : true}),
      impactOnClientDeliveryDetail : new FormControl({value: (this.impactOnClientDeliveryDetail)? this.impactOnClientDeliveryDetail : null,disabled : true}),
    });
  }
  

  getClosureActivityByDepartment(departmentId: number){
    
    this.spinner.show();
    this.checklistService.GetExitActivitiesByDepartment(this.employeeId,departmentId)
      .toPromise().then((activitylist: ActivityList[])=>{
        this.spinner.hide();
        for(let i=0;i<activitylist.length;i++){
          this.checklist.set(activitylist[i].ActivityId,activitylist[i].Description);
          this.activityType.set(activitylist[i].ActivityId,activitylist[i].ActivityType);
          this.IsRequired.set(activitylist[i].ActivityId,activitylist[i].IsRequired);
        }
        
      }).catch(
      error=>{
        this.spinner.hide();
        this._snackBar.open("Error while getting the Exit Activities by Department", 'x', {
          duration: 2000,
          horizontalPosition: 'right',
          verticalPosition: 'top',
        })
      });
  }
  
  getActivitiesByHRA(){
    this.spinner.show();
    this.checklistService.getActivitiesByHRA(this.employeeId).toPromise()
     .then((response: ActivityData[])=>{
      this.spinner.hide();
      this.activityData= response;
      let departmentIDs = this.activityData.map((ad)=>{ return ad['DepartmentId']})

      for(let i=0;i<this.activityData.length;i++){
        this.getClosureActivityByDepartment(departmentIDs[i]);
      }
     }).catch(error=>{
      this.spinner.hide();
     
   });
  }
  canShowTitle(element: any, index: number) {
   
    let activity_details = this.activityType;
    if(index==0)
    {
      this.prevActivity = this.activityType.get(element.ActivityId);
      return true;
      
    }
    
    if (this.activityType.get(element.ActivityId) !== this.prevActivity) {
      this.prevActivity = this.activityType.get(element.ActivityId);
      return true;
    }
    return false;
  }

  SubmitForAssociateExit() {
    //Validation
    if (this.actualExitDate === this.currentDate) {
      this.HRMSubmit();
    }
    else {
      /* popup will be shown when last working date is not current date */
      const dialogRef = this.dialog.open(CommonDialogComponent, {
        width: '300px',
        disableClose: true,
        data: {
          heading: 'Confirmation',
          message: 'The last working date is not matched. Do you want to proceed?',
        },
      });

      dialogRef.afterClosed().subscribe((result) => {
        this.dialogResponse = result;
        this.UpdateExitDateRequired = true;
        if (this.dialogResponse == true) {
          /* If accepting confirmation  means last working is updated with current day*/
          this.HRMSubmit();
        }
      });
    }
  }

  HRMSubmit(){
    let SubmitData = {
      "employeeId": this.employeeId,
      "userRole": this.roleName,
      "remarksByHRA": this.clearenceForm.value.remarksByHRA,
      "remarksByHRM": this.clearenceForm.value.remarksByHRM,
      "remarksByOperationsHead": this.clearenceForm.value.remarksByOpsHead,
      "UpdateExitDateRequired": this.UpdateExitDateRequired,
    }
    this.associateData = new AssociateExit();
    this.associateData.ReasonId = this.clearenceForm.value.reasonId;
    this.associateData.ReasonDetail = this.clearenceForm.value.reasonDetail;
    this.associateData.RehireEligibility = this.clearenceForm.value.rehireEligibility;
    this.associateData.RehireEligibilityDetail = this.clearenceForm.value.rehireEligibilityDetail;
    this.associateData.ImpactOnClientDelivery = this.clearenceForm.value.impactOnClientDelivery;
    this.associateData.ImpactOnClientDeliveryDetail = this.clearenceForm.value.impactOnClientDeliveryDetail;
    this.spinner.show();
    this.checklistService.ExitClearance(SubmitData).toPromise()
      .then(() => {
        this.spinner.hide();
        this._snackBar.open("Deactivated Successfully", 'x', {
          duration: 1000,
          panelClass: ['success-alert'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
        }).afterDismissed().subscribe(res => {
          this.onBack();
        });
      }).catch(error => {
        this.spinner.hide();
        this._snackBar.open("Error while approving!", 'x', {
          duration: 1000,
          panelClass: ['error-alert'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
      });
  }
  onBack() {
    this.router.navigate(['shared/exit-actions']);
    // this.showback = false;
  }

  getRemarks(){
    this.spinner.show();
    this.checklistService.GetExitClearanceRemarks(this.employeeId).toPromise().then((res:any)=>{
      this.spinner.hide();
      if(res){
        this.clearenceForm.patchValue({
          remarksByHRM : (res.RemarksByHRM) ? res.RemarksByHRM : "",
          remarksByHRA : (res.RemarksByHRA) ? res.RemarksByHRA : "",
          remarksByOpsHead : (res.RemarksByOperationsHead) ? res.RemarksByOperationsHead : ""
        });
      }
    });
    if(this.previous == '/associateexit/associateexitinformation'){
      this.clearenceForm.disable()
      this.disable = true
    }
  }
}