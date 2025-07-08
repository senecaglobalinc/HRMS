import { Component, Inject, OnInit, ViewEncapsulation } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import * as moment from 'moment';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { themeconfig } from '../../../../app/../themeconfig';
import { MatSnackBar } from '@angular/material/snack-bar';
@Component({
  selector: 'app-remarks-teamlead',
  templateUrl: './remarks-teamlead.component.html',
  styleUrls: ['./remarks-teamlead.component.scss'],
})
export class RemarksTeamleadComponent implements OnInit {
  public closeProjectData: FormGroup;
  themeConfigInput = themeconfig.formfieldappearances;
  constructor(
    private dialogRef: MatDialogRef<RemarksTeamleadComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private _snackBar: MatSnackBar
  ) {
    this.closeProjectData = new FormGroup({
      rejectRemarks: new FormControl(null, Validators.required),
    });
  }

  ngOnInit(): void {
    this.dialogRef.updateSize('50%', '40%');
  }
  onNoClick(): void {
    this.dialogRef.close();
  }

  save() {
    this.dialogRef.close(this.closeProjectData.value);
  }

  close() {
    this.dialogRef.close();
  }
}
