import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA ,MatDialogRef, MatDialog} from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { themeconfig } from 'src/themeconfig';
import { KtForm } from '../../models/kt-form.model';
import { KtFormService } from '../../services/kt-form.service';




@Component({
  selector: 'app-KT-plan-dialog',
  templateUrl: './KT-plan-dialog.component.html',
  styleUrls: ['./KT-plan-dialog.component.scss']
})
export class KTPlanDialogComponent implements OnInit {

  KtFormValues: KtForm;
  
  UpdatektPlan:FormGroup;
  themeConfigInput = themeconfig.formfieldappearances;
  constructor(@Inject(MAT_DIALOG_DATA) public data: {EmployeeId: number},
  private _KtFormService: KtFormService,
  private _snackBar: MatSnackBar,
  private fb: FormBuilder,
  public dialog: MatDialog,
  private dialogRef: MatDialogRef<KTPlanDialogComponent>,
  private _router: Router,) {
  }

  ngOnInit(): void {
    this.UpdatektPlan = new FormGroup({
      Remarks: new FormControl(null,[Validators.required])
    });
  }
  UpdateKTPlan()
  {
    this.KtFormValues = new KtForm();
    if(this.UpdatektPlan.valid)
    {
      this.KtFormValues.EmployeeId=this.data.EmployeeId;
      this.KtFormValues.TransitionNotRequired=true;
      this.KtFormValues.Remarks=this.UpdatektPlan.value.Remarks;
      this.KtFormValues.Type="TLComplete";
    
   this._KtFormService.updateKtTasks(this.KtFormValues).subscribe(res=>{
    if (res) {
      if (this.KtFormValues.Type == 'Save') {       
        this._snackBar.open('Transition Plan updated successfully.', 'x', {
          duration: 1000,
              panelClass:['success-alert'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
        this._router.navigate(['/shared/dashboard']);
      } else {
        this._snackBar.open(
          'Transition Plan Updated successfully.',
          'x',
          {
            panelClass:['success-alert'],

            duration: 1000,
            horizontalPosition: 'right',
            verticalPosition: 'top',
          }
        );
      
      }
    }
  },  
    (error) => {
      this._snackBar.open('Unable to update Transition Plan', 'x', {
        duration: 1000,
           panelClass:['error-alert'],
        horizontalPosition: 'right',
        verticalPosition: 'top',
      });
    }
   )  
    this.close();   
   
  }

  
  else{
    this.UpdatektPlan.markAllAsTouched();
    this._snackBar.open('Please correct the field highlighted in red color !', 'x', {
      duration: 1000,
         panelClass:['error-alert'],
      horizontalPosition: 'right',
      verticalPosition: 'top',
    });
  }
}
close() {
  this.dialogRef.close();
  // this._router.navigate(['/shared/dashboard']);
}
}

