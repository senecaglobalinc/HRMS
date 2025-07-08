import {Component, Inject, OnInit, ViewEncapsulation} from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import * as moment from 'moment';
import { FormGroup, FormControl, Validators } from '@angular/forms';


@Component({
    selector: 'reject-dialog',
    templateUrl: './reject-dialog.component.html',
    styleUrls: ['./reject-dialog.component.css']
})
export class RejectDialogComponent implements OnInit {

    public RejectForm : FormGroup;
    rejectReason : string;


    constructor(
        private dialogRef: MatDialogRef<RejectDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public data: any  ) {

        this.RejectForm = new FormGroup({
            'rejectReason' : new FormControl(this.rejectReason,Validators.required)
          })


    }

    ngOnInit() {

    }
    onNoClick(): void {
            this.dialogRef.close(null);
      }

    save() {
        if (this.RejectForm.invalid){
            return;
        }
        else{
        this.dialogRef.close(this.RejectForm.value);
        }
    }

    close() {
        this.dialogRef.close();
    }

}
