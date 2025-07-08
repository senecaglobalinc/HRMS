import { Component, Inject, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import * as moment from 'moment';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { themeconfig } from '../../../../../themeconfig';
import { MatSnackBar } from '@angular/material/snack-bar';
import { CdkTextareaAutosize } from '@angular/cdk/text-field';
@Component({
  selector: 'date-dialog',
  templateUrl: './date-dialog.component.html',
  styleUrls: ['./date-dialog.component.css'],
})
export class DateDialogComponent implements OnInit {
  public closeProjectData: FormGroup;
  public projectName: any;
  public startDate: any;
  public validFromErrorMessage: string;
  public isgreater = false;
  themeConfigInput = themeconfig.formfieldappearances;
  minClosureStartDate = new Date();
  maxClosureEndDate = new Date();
  constructor(
    private dialogRef: MatDialogRef<DateDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private _snackBar: MatSnackBar
  ) {
    this.closeProjectData = new FormGroup({
      ActualEndDate: new FormControl(null, Validators.required),
      Remarks: new FormControl(null, Validators.required),
    });
  }
  
  @ViewChild('autosize', {static: false}) autosize: CdkTextareaAutosize;

  ngOnInit() {
    this.dialogRef.updateSize('50%', '65%');
    if (this.data.BillingEndDate != null) {
      let date = new Date(
        moment(this.data.BillingEndDate).format('YYYY-MM-DD')
      );
      this.closeProjectData.patchValue({
        ActualEndDate: date,
      });
    }
    this.projectName = this.data.ProjectName;
    this.startDate = this.data.StartDate;
    var date = new Date(), year = date.getFullYear(), month = date.getMonth();
    this.minClosureStartDate = new Date(year, month, 1);
    this.maxClosureEndDate = new Date();
  }

  clearInput(evt: any, fieldName): void {
    if(fieldName=='ActualEndDate'){
      evt.stopPropagation();
      this.closeProjectData.get('ActualEndDate').reset();
    }
  }

  validFromYear(datevalue: any) {
    this.validFromErrorMessage = '';

    if (Date.parse(datevalue) < Date.parse(this.startDate)) {
      this._snackBar.open('End Date should be greater than Start Date', 'x', {
        duration: 3000,
        horizontalPosition: 'right',
        verticalPosition: 'top',
      });
      this.validFromErrorMessage = 'End Date should be greater than Start Date';
      this.isgreater = true;
      return false;
    }
    if (
      this.validFromErrorMessage == '' ||
      this.validFromErrorMessage == null
    ) {
      this.isgreater = false;
    }
  }
  onNoClick(): void {
    this.dialogRef.close();
  }

  save() {
    if (this.isgreater) {
      return;
    }
    this.dialogRef.close(this.closeProjectData.value);
  }

  close() {
    this.dialogRef.close();
  }
}
