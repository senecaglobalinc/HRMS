import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import * as moment from 'moment';
import { themeconfig } from '../../../../../themeconfig';
import { CommonService } from '../../../../core/services/common.service';

@Component({
  selector: 'app-close-cbr-dialog',
  templateUrl: './close-cbr-dialog.component.html',
  styleUrls: ['./close-cbr-dialog.component.scss']
})
export class CloseCbrDialogComponent implements OnInit {

  closeRoleData: FormGroup;
  themeConfigInput = themeconfig.formfieldappearances;
  submitted = false;

  minCBRCloseDate = new Date();
  maxCBRCloseDate = new Date();

  constructor(private dialogRef: MatDialogRef<CloseCbrDialogComponent>,  
    private _commonService: CommonService,
    private _snackBar: MatSnackBar,
    @Inject(MAT_DIALOG_DATA) public data: any  ) { }

  ngOnInit(): void {
    this.createCloseRoleform();
  }

  clearInput(evt: any, fieldName): void {
    if(fieldName=='BillingEndDate'){
      evt.stopPropagation();
      this.closeRoleData.get('BillingEndDate').reset();
    }
  }
  createCloseRoleform() {
    var date = new Date(), y = date.getFullYear(), m = date.getMonth();
    this.minCBRCloseDate = new Date(y, m, 1); //new Date(this.closableRole.StartDate);
    this.closeRoleData = new FormGroup({
      BillingEndDate: new FormControl(null, [Validators.required]),
      Reason: new FormControl(null, [Validators.required])
    });

    if (this.data.BillingEndDate != null) {
      let date = new Date(moment(this.data.BillingEndDate).format("YYYY-MM-DD"));
      this.closeRoleData.patchValue({
        BillingEndDate: date
      })
    }
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

  save() {
    this.submitted = true;
    if(this.closeRoleData.value.BillingEndDate!=null)
      { 
        this.closeRoleData.value.BillingEndDate = new Date(moment(this.closeRoleData.value.BillingEndDate).format("YYYY-MM-DD"));
          if (
            !this._commonService.IsValidDate(
              this.data.BillingStartDate,
              this.closeRoleData.value.BillingEndDate
            )
          ) {
            this._snackBar.open('Client Billing Start Date should be less than or equal to Client Billing End Date', 'x', {
              duration: 1000,
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
            return;
        }

      if(this.closeRoleData.valid)
       this.dialogRef.close(this.closeRoleData.value);
      }
}

}
