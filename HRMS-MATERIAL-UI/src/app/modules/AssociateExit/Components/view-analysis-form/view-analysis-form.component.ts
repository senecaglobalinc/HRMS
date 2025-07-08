import { Component, OnInit, ViewChild, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { themeconfig } from 'src/themeconfig';
import { FormControl, FormGroup, FormBuilder, FormGroupDirective } from '@angular/forms';
import { ExitAnalysisService } from '../../Services/exit-analysis.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { CauseForm } from '../../Models/causeanalysis.model';

@Component({
  selector: 'app-view-analysis-form',
  templateUrl: './view-analysis-form.component.html',
  styleUrls: ['./view-analysis-form.component.scss']
})
export class ViewAnalysisFormComponent implements OnInit {
  themeAppearence = themeconfig.formfieldappearances;
  rootcauseform: FormGroup;
  causeForm: CauseForm;
  element: any;
  isView: true;
  editMode: false;
  @ViewChild(FormGroupDirective) formGroupDirective: FormGroupDirective;

  constructor(
    public dialogRef: MatDialogRef<ViewAnalysisFormComponent>,
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

  update(causeForm: any) {
    causeForm.employeeId = this.element.EmployeeId;
    causeForm.rootCause = this.rootcauseform.value.rootCause.disable();
    causeForm.actionItem = this.rootcauseform.value.actionItem.disable();
    causeForm.responsibility = this.rootcauseform.value.responsibility.disable();
    causeForm.tagretDate = this.rootcauseform.value.tagretDate.disable();
    causeForm.actualDate = this.rootcauseform.value.actualDate.disable();
    causeForm.remarks = this.rootcauseform.value.remarks.disable();

    this.causeservice.CreateExitAnalysis(causeForm).toPromise()
      .then(() => {
        if (this.causeForm.submitType = "save") {
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
}