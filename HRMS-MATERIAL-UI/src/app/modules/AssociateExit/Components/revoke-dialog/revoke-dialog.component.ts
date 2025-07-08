import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { themeconfig } from '../../../../../themeconfig';

@Component({
  selector: 'app-revoke-dialog',
  templateUrl: './revoke-dialog.component.html',
  styleUrls: ['./revoke-dialog.component.scss']
})
export class RevokeDialogComponent implements OnInit {

  revokeResignation: FormGroup;
  themeConfigInput = themeconfig.formfieldappearances;
  submitType:string;
  

  constructor(private dialogRef: MatDialogRef<RevokeDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) { }



  ngOnInit(): void {
    this.submitType = this.data.value;
    this.revokeResignation = new FormGroup({
      RevokeReason: new FormControl(null, [Validators.required]),
    });

  }

  onNoClick(): void {
    this.dialogRef.close();
  }

  save() : void {
    if(this.revokeResignation.valid)
    this.dialogRef.close(this.revokeResignation.value);
    
  }
}
