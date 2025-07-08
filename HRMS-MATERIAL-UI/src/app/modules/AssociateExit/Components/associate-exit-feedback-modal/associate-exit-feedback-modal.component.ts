import { Component, Inject, OnInit } from '@angular/core';
import { MatDialog, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AssociateDetailReviewData,AssociateDetailData } from '../../Models/analysis.model';
import { themeconfig } from '../../../../../themeconfig';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AssociateExitDashboardService } from 'src/app/modules/shared/services/associate-exit-dashboard.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { NgxSpinnerService } from 'ngx-spinner';
import { AngularEditorConfig } from '@kolkov/angular-editor';

@Component({
  selector: 'app-associate-exit-feedback-modal',
  templateUrl: './associate-exit-feedback-modal.component.html',
  styleUrls: ['./associate-exit-feedback-modal.component.scss']
})
export class AssociateExitFeedbackModalComponent implements OnInit {
  associateName : string;
  feedbackdetailsdata:AssociateDetailReviewData;
  isEditClickedValue:boolean=false;
  curatedFeedbackData=new AssociateDetailData();
  myForm: FormGroup;
  disableform = false;
  themeConfigInput = themeconfig.formfieldappearances;
  editorConfig1 : AngularEditorConfig  
  = {
    editable: false,
    spellcheck: true,
    height: '0',
    minHeight: '140',
    maxHeight: 'auto',
    width: 'auto',
    minWidth: '0',
    translate: 'yes',
    enableToolbar: true,
    showToolbar: false,
    placeholder: 'Enter text here...',
    defaultParagraphSeparator: '',
    defaultFontName: '',
    defaultFontSize: '',
    fonts: [
      { class: 'arial', name: 'Arial' },
      { class: 'times-new-roman', name: 'Times New Roman' },
      { class: 'calibri', name: 'Calibri' },
      { class: 'comic-sans-ms', name: 'Comic Sans MS' }
    ],
    customClasses: [
      {
        name: 'quote',
        class: 'quote',
      },
      {
        name: 'redText',
        class: 'redText'
      },
      {
        name: 'titleText',
        class: 'titleText',
        tag: 'h1',
      },
    ],
    uploadUrl: 'v1/image',
    uploadWithCredentials: false,
    sanitize: true,
    toolbarPosition: 'top',
    toolbarHiddenButtons: [
      [],
      [
      ]
    ]
  };

  editorConfig2 : AngularEditorConfig  
  = {
    editable: true,
    spellcheck: true,
    height: '0',
    minHeight: '140',
    maxHeight: 'auto',
    width: 'auto',
    minWidth: '0',
    translate: 'yes',
    enableToolbar: true,
    showToolbar: true,
    placeholder: 'Enter text here...',
    defaultParagraphSeparator: '',
    defaultFontName: '',
    defaultFontSize: '',
    fonts: [
      { class: 'arial', name: 'Arial' },
      { class: 'times-new-roman', name: 'Times New Roman' },
      { class: 'calibri', name: 'Calibri' },
      { class: 'comic-sans-ms', name: 'Comic Sans MS' }
    ],
    customClasses: [
      {
        name: 'quote',
        class: 'quote',
      },
      {
        name: 'redText',
        class: 'redText'
      },
      {
        name: 'titleText',
        class: 'titleText',
        tag: 'h1',
      },
    ],
    uploadUrl: 'v1/image',
    uploadWithCredentials: false,
    sanitize: true,
    toolbarPosition: 'top',
    toolbarHiddenButtons: [
      [],
      [
      ]
    ]
  };
  
 
  constructor(public dialog: MatDialog,
    private fb: FormBuilder,
    private associateExitDashboardService:AssociateExitDashboardService,
    private _snackBar: MatSnackBar,
    private spinner: NgxSpinnerService,
    @Inject(MAT_DIALOG_DATA) public data: any,
    ) { }

  ngOnInit(): void {
    
    this.feedbackdetailsdata = this.data.element;
    this.myForm = this.fb.group({
      Feedback: [''],
      Hidefeedback:[''],
      AssociateFeedback:['']
    });
    this.myForm.controls['Hidefeedback'].setValue(this.feedbackdetailsdata.ShowInitialRemarks);
    this.myForm.controls['Feedback'].setValue(this.feedbackdetailsdata.FinalRemarks);
    this.myForm.controls['AssociateFeedback'].setValue(this.feedbackdetailsdata.InitialRemarks);
    this.associateName = this.feedbackdetailsdata.AssociateName;
  }
  close(){
    this.dialog.closeAll()
  }

  updateSelection(e){
    this.myForm.controls['Hidefeedback'].setValue(e.checked);
  }

  isEditClicked(){
    this.isEditClickedValue = !this.isEditClickedValue;
  }

  copyAssociateFeedback()
  {
    this.myForm.controls['Feedback'].setValue(this.feedbackdetailsdata.InitialRemarks);
  }

  submitReport(){  
   this.spinner.show();  
   this.curatedFeedbackData.associateExitInterviewId = this.feedbackdetailsdata.AssociateExitInterviewId;
   this.curatedFeedbackData.associateExitInterviewReviewId = 0;
   this.curatedFeedbackData.finalRemarks = this.myForm.controls['Feedback'].value;
   this.curatedFeedbackData.showInitialRemarks = this.myForm.controls['Hidefeedback'].value;
   console.log(this.curatedFeedbackData);
   this.associateExitDashboardService.create(this.curatedFeedbackData).subscribe(res=>{
    this.spinner.hide(); 
    this.close();
   },
   (error=>{
    this.spinner.hide();  
    this._snackBar.open(error.error.text, 'x', {
      duration: 1000,
      panelClass:['error-alert'],
      horizontalPosition: 'right',
      verticalPosition: 'top',
    });
   })
   )
   this.spinner.hide();
  }

}
