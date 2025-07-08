import { Component, OnInit, Inject, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, FormGroupDirective } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { themeconfig } from 'src/themeconfig';
import { CauseForm } from '../../Models/causeanalysis.model';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ExitAnalysisService } from '../../Services/exit-analysis.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-associate-exit-cause-form',
  templateUrl: './associate-exit-cause-form.component.html',
  styleUrls: ['./associate-exit-cause-form.component.scss']
})
export class AssociateExitCauseFormComponent implements OnInit {
  themeAppearence = themeconfig.formfieldappearances;
  rootcauseform: FormGroup;
 causeForm: CauseForm;
 element: any;
 isView: true;
 editMode: false;
  @ViewChild(FormGroupDirective) formGroupDirective: FormGroupDirective;

  constructor(
    public dialogRef: MatDialogRef<AssociateExitCauseFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private formBuilder: FormBuilder,
    private _snackBar: MatSnackBar,
    private _router: Router,
    private causeservice: ExitAnalysisService,

    ) {
      if (data.element) {
        this.element = this.data.element;
    
      }
      this.isView = this.data.isView;

  }
  ngOnInit(): void {

    this.rootcauseform = this.formBuilder.group({
      rootCause: [((this.element && this.element.RootCause) ? this.element.RootCause : "")],
      actionItem: [((this.element && this.element.ActionItem) ? this.element.ActionItem : "")],
      tagretDate: [((this.element && this.element.TagretDate) ? this.element.TagretDate : "")],
      actualDate: [((this.element && this.element.ActualDate) ? this.element.ActualDate : "")],
      responsibility: [((this.element && this.element.Responsibility) ? this.element.Responsibility : "")],
      remarks: [((this.element && this.element.Remarks) ? this.element.Remarks : "")],
   
    });
  }
  saveCauseForm() {
    this.causeForm = new CauseForm();
   
    this.causeForm.submitType = "save";
    this.update(this.causeForm);
    
  }
  submit(){
    this.causeForm = new CauseForm();
   
    this.causeForm.submitType = "submit";
    this.submitData(this.causeForm);
    
  
  }
  update(causeForm: any)
    {
      causeForm.employeeId = this.element.EmployeeId;
     causeForm.rootCause = this.rootcauseform.value.rootCause;
     causeForm.actionItem = this.rootcauseform.value.actionItem;
      causeForm.responsibility = this.rootcauseform.value.responsibility;
    causeForm.tagretDate = this.rootcauseform.value.tagretDate;
      causeForm.actualDate = this.rootcauseform.value.actualDate;
      causeForm.remarks = this.rootcauseform.value.remarks;

      this.causeservice.CreateExitAnalysis(causeForm).toPromise()
      .then(() => {
        if (this.causeForm.submitType = "save"){
        this._snackBar.open("Save  Successfully", 'x', {
          duration: 1000,
          panelClass: ['success-alert'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
        setTimeout(() => 
        this.formGroupDirective.resetForm(), 0)
      }
    
    }).catch(error => {
        this._snackBar.open("Error while save!", 'x', {
          duration: 1000,
          panelClass: ['error-alert'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
      });
 

  }

  submitData(causeForm: any)
  {
    causeForm.employeeId = this.element.EmployeeId;
   causeForm.rootCause = this.rootcauseform.value.rootCause;
   causeForm.actionItem = this.rootcauseform.value.actionItem;
    causeForm.responsibility = this.rootcauseform.value.responsibility;
  causeForm.tagretDate = this.rootcauseform.value.tagretDate;
    causeForm.actualDate = this.rootcauseform.value.actualDate;
    causeForm.remarks = this.rootcauseform.value.remarks;

    this.causeservice.CreateExitAnalysis(causeForm).toPromise()
    .then(() => {
      if (this.causeForm.submitType = "submit"){
      this._snackBar.open("submit  Successfully", 'x', {
        duration: 1000,
        panelClass: ['success-alert'],
        horizontalPosition: 'right',
        verticalPosition: 'top',
      });
      setTimeout(() => 
      this.formGroupDirective.resetForm(), 0)
    }
  
  }).catch(error => {
      this._snackBar.open("Error while submit!", 'x', {
        duration: 1000,
        panelClass: ['error-alert'],
        horizontalPosition: 'right',
        verticalPosition: 'top',
      });
    });


}

}

