import { ExitAnalysisService } from './../../Services/exit-analysis.service';
import { Component, OnInit, QueryList, ViewChild, ViewChildren } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, FormGroupDirective, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Observable, ReplaySubject, Subject, Subscription } from 'rxjs';
import { themeconfig } from '../../../../../themeconfig';
import { MasterDataService } from '../../../../core/services/masterdata.service';
import { DropDownType, GenericType } from '../../../master-layout/models/dropdowntype.model';
// import { ReportsData } from '../../models/reportsdata.model';
// import { ReportsFilterData, UtilizationReportFilterData } from '../../models/reportsfilter.model';
import { ProjectsData } from '../../../master-layout/models/projects.model';
import * as moment from "moment";
// import { ResourceReportService } from '../../services/resource-report.service';
import { NavService } from '../../../master-layout/services/nav.service';
import { ProjectCreationService } from '../../../project-life-cycle/services/project-creation.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ResignastionService } from '../../Services/resignastion.service';
import { FeedBackDetails } from '../../Models/associateExit.model';
import {editorConfig} from '../../../../core/angularEditorConfiguratioan';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmationDialogComponent } from 'src/app/modules/shared/components/confirmation-dialog/confirmation-dialog.component';
import { NgxSpinnerService } from 'ngx-spinner';

interface SelectItem {
  value: number;
  label: string;
}

// interface Reason {
//   value: string;
//   viewValue: string;
// }

@Component({
  selector: 'app-associate-exit-feedback-form',
  templateUrl: './associate-exit-feedback-form.component.html',
  styleUrls: ['./associate-exit-feedback-form.component.scss']
})
export class AssociateExitFeedbackFormComponent implements OnInit {
  themeConfigInput = themeconfig.formfieldappearances;
  projectId: number;
  filteredBanksMulti: ReplaySubject<any[]> = new ReplaySubject<any[]>(1);
  filteredcols: any[];
  displaycolsfields: any[];
  displaycols = [];
  private _onDestroy = new Subject<void>();

  projectData: ProjectsData;

  editorConfig = editorConfig;


  afterSearchFilter: boolean = false;
  cols: any[] = [];
  columnOptions: any[] = [];
  PageSize: number = 5;
  PageDropDown: number[] = [];
  // private resources = servicePath.API.PagingConfigValue;
  componentName: string;
  selectDisabled =true;
  EmployeeId: number;
  feedbackdetails: FeedBackDetails;
  reasonList: SelectItem[] = [];
  showback: boolean = false;
  reasonId: number;
  reasonDetail: string;
  projectsList: SelectItem[] = [];
  exitFeedbackForm: FormGroup;
  projectIdSubscription: Subscription;
  disable:boolean = false;
  previous:string;
  dialogResponse: boolean;
  disableform = false;
  formsubmitted = false;
  disableSubmit = false;


  @ViewChild(FormGroupDirective) formGroupDirective: FormGroupDirective;

  constructor(private _masterDataService: MasterDataService, private _snackBar: MatSnackBar,
    private projectCreationService: ProjectCreationService,
    private actRoute: ActivatedRoute,
    private _formBuilder: FormBuilder,
    private _resignationService: ResignastionService,
    private router: Router,
    private exitanalysis: ExitAnalysisService,
    private spinner: NgxSpinnerService,
    public dialog: MatDialog,

    public navService: NavService) {
    this._resignationService.getResigReasonDtls().subscribe(data => {
      this.reasonId = data.ReasonId;
     this.exitFeedbackForm.get('reasonId').setValue(this.reasonId);
    
      this.reasonDetail = data.ReasonDetail;
      this.exitFeedbackForm.get('reasonDetail').setValue(this.reasonDetail);
      this.exitFeedbackForm.get('reasonId').updateValueAndValidity();
      this.exitFeedbackForm.get('reasonDetail').updateValueAndValidity();

    });


    this.navService.currentSearchBoxData.subscribe(responseData => {
      // this.applyFilter(responseData);
    })
    // this.PageSize = this.resources.PageSize;
    // this.PageDropDown = this.resources.PageDropDown;
  }

  ngOnInit(): void {
    this.previous = this.exitanalysis.getPreviousUrl();

    this.actRoute.params.subscribe(params => {
      this.EmployeeId = params["id"];
    });
    this.createexitFeedbackForm();
    this.getReason();
    editorConfig.editable = true;

    this._resignationService.getResignationReason().subscribe((res: any) => {
      if (res) {
        this.feedbackdetails = new FeedBackDetails();
        this.feedbackdetails.reasonDetail = res.ReasonDetail;
        this.feedbackdetails.reasonId = res.ReasonId;
      }
    });

   
  }
  createexitFeedbackForm() {
    this.exitFeedbackForm = new FormGroup({
      alternateAddress: new FormControl(null, Validators.required),
      alternateEmail: new FormControl(null, [Validators.required, Validators.email, Validators.pattern(/^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/)]),
      alternateMobileNo: new FormControl(null, [Validators.required, Validators.pattern('[1-9]{1}[0-9]{9}')]),
      remarks: new FormControl(null, [Validators.required]),
      reasonId: new FormControl({value: (this.reasonId)? this.reasonId : null, disabled : true}),
      reasonDetail: new FormControl({value: (this.reasonDetail)? this.reasonDetail : null, disabled : true}),
      notify: new FormControl(false)

    });
    this.getExitInterviewDetails();
  }
  getReason(): void {
    this.spinner.show();
    this._resignationService.getResignationReason().subscribe((res: GenericType[]) => {
      this.spinner.hide();
      this.reasonList = [];
      this.reasonList.push({ label: '', value: null });
      res.forEach((res: GenericType) => {
        this.reasonList.push({ label: res.Name, value: res.Id });
      })
    })

  }
  onlyForNumbers(event: any) {
    var keys = {
      escape: 27,
      backspace: 8,
      tab: 9,
      enter: 13,
      "0": 48,
      "1": 49,
      "2": 50,
      "3": 51,
      "4": 52,
      "5": 53,
      "6": 54,
      "7": 55,
      "8": 56,
      "9": 57,
    };
    for (var index in keys) {
      if (!keys.hasOwnProperty(index)) continue;
      if (event.charCode == keys[index] || event.keyCode == keys[index]) {
        return; //default event
      }
    }
    event.preventDefault();
  }

  getExitInterviewDetails(){
    this.spinner.show();
    this._resignationService.getExitInterview(this.EmployeeId).subscribe((res:any)=>{
      this.spinner.hide();
      if(res){
        this.exitFeedbackForm.patchValue({
          alternateAddress: res.AlternateAddress,
          alternateEmail: res.AlternateEmail,
          alternateMobileNo: res.AlternateMobileNo,
          remarks: res.Remarks,
        });
        this.exitFeedbackForm.disable();
        editorConfig.editable = false;
        this.disableform = true;

      }
    })
    if(this.previous == '/associateexit/associateexitinformation'){
      this.exitFeedbackForm.disable();
      this.exitFeedbackForm.controls.remarks.disable()
      this.disable=true;
    }

  }


  submitFeedBack() {
    this.formsubmitted = true;
    if (this.exitFeedbackForm.valid) {
      this.disableSubmit = true;
      this.feedbackdetails = new FeedBackDetails();
      this.feedbackdetails.employeeId = this.EmployeeId;
      this.feedbackdetails.alternateMobileNo = this.exitFeedbackForm.value.alternateMobileNo;
      this.feedbackdetails.alternateAddress = this.exitFeedbackForm.value.alternateAddress;
      this.feedbackdetails.alternateEmail = this.exitFeedbackForm.value.alternateEmail;
      this.feedbackdetails.reasonId = this.exitFeedbackForm.value.reasonId;
      this.feedbackdetails.reasonDetail = this.exitFeedbackForm.value.reasonDetail;
      this.feedbackdetails.remarks = this.exitFeedbackForm.value.remarks;
      this.feedbackdetails.IsNotified = this.exitFeedbackForm.value.notify;
      if (this.feedbackdetails.IsNotified) {
        const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
          width: '300px',
          disableClose: true,
          data: { heading: 'Confirmation', message: 'Are you sure to share the feedback?' }
        });

        dialogRef.afterClosed().subscribe(result => {
          this.dialogResponse = result;
          if (this.dialogResponse === true) {
            this.submitFeedbackData();
          }
          else{
            this.disableSubmit = false;
            return;
          }
        })
      }
      else{
        this.submitFeedbackData();
      }
    }
    else {
      this.disableSubmit = false;
      return;
    }
  }

  submitFeedbackData() {
    this.spinner.show();
    this._resignationService.CreateExitFeedback(this.feedbackdetails).toPromise()
      .then(() => {
        this.spinner.hide();
        this._snackBar.open("Exit feedback Submitted Successfully", 'x', {
          duration: 1000,
          horizontalPosition: 'right',
          verticalPosition: 'top',
        }).afterDismissed().subscribe(res => {
          this.onBack();
        });
      }).catch(error => {
        this.spinner.hide();
        this._snackBar.open("Error while submit!", 'x', {
          duration: 1000,
          panelClass: ['error-alert'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
        this.disableSubmit = false;
      });
  }
  onBack() {
    this.router.navigate(['shared/exit-actions']);
    this.showback = false;
  }

  clear() {
    this.exitFeedbackForm.reset({
      reasonId: this.exitFeedbackForm.get('reasonId').value,
      reasonDetail: this.exitFeedbackForm.get('reasonDetail').value,
    });
    setTimeout(() => this.formGroupDirective.resetForm({
      reasonId: this.exitFeedbackForm.get('reasonId').value,
      reasonDetail: this.exitFeedbackForm.get('reasonDetail').value,
    }), 0);
    this.formsubmitted = false;
    this.disableSubmit = false;
  }


}