import { Component, Inject, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { themeconfig } from 'src/themeconfig';
import { AssignRmForNonDeliveryService } from '../../services/assign-rm-for-non-delivery.service';
import { NonDeliveryAssociates } from '../../../onboarding/models/changeRM.model';
import { updateAssociateRMDetais } from '../../../onboarding/models/changeRM.model'
import { Router } from '@angular/router';
import { FormControl, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ReplaySubject, Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

@Component({
  selector: 'app-assign-rm-to-non-delivery',
  templateUrl: './assign-rm-to-non-delivery.component.html',
  styleUrls: ['./assign-rm-to-non-delivery.component.scss']
})
export class AssignRmToNonDeliveryComponent implements OnInit {
  themeConfigInput = themeconfig.formfieldappearances;
  nonDeliveryAssociates:NonDeliveryAssociates[] = [];
  selectedRmId:number;
  updateRmDetailsByEmpOrDept:updateAssociateRMDetais = new updateAssociateRMDetais() ;
  reportingManagerId = new FormControl('',[Validators.required]);
  public filteredAssociatesList: ReplaySubject<NonDeliveryAssociates[]> = new ReplaySubject<NonDeliveryAssociates[]>(1);
  public associatesFilterCtrl = new FormControl(null);
  protected _onDestroy = new Subject<void>();

  constructor(@Inject(MAT_DIALOG_DATA) public data:any,public _service:AssignRmForNonDeliveryService,private router : Router,public dialogRef: MatDialogRef<AssignRmToNonDeliveryComponent>,private _snackBar : MatSnackBar) { }

  ngOnInit(): void {
    this.getAllNonDeliveryAssociates();
  }

  public getAllNonDeliveryAssociates() : void {
   this.nonDeliveryAssociates = [];
    this._service.getNonDeliveryAssociates().subscribe((res:NonDeliveryAssociates[])=>{
      this.nonDeliveryAssociates = res;
      this.filteredAssociatesList.next(this.nonDeliveryAssociates.slice());
      this.associatesFilterCtrl.valueChanges
        .pipe(takeUntil(this._onDestroy))
        .subscribe(() => {
          this.getFilteredAssociates();
        });
    },(error)=>{
      this._snackBar.open(
        'Unable to Fetch Non Delivery Associates',
        'x',
        {
          duration: 3000,
          panelClass: ['error-alert'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
        }
      );

    })
    
  }

  public onRmChange(event) : void{
    this.selectedRmId=event.value;
  }

  public saveRM() : void{
    if(this.reportingManagerId.valid){
    this.dialogRef.close();
    this.updateRmDetailsByEmpOrDept.ReportingManagerId = this.selectedRmId;
    this.updateRmDetailsByEmpOrDept.associates = this.data;
    this._service.assignRmByDeptOrByAssociate(this.updateRmDetailsByEmpOrDept).subscribe((res)=>{
      this._snackBar.open(
        'Successfully Updated Reporting Manager',
        'x',
        {
          duration: 3000,
          panelClass: ['success-alert'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
        }
      );
      this.router.navigate(['/associates/changeRM']);
    },(error)=>{
      this._snackBar.open(
        'Failed Updating Reporting Manager',
        'x',
        {
          duration: 3000,
          panelClass: ['error-alert'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
        }
      );
    })
  }
  else {
    this._snackBar.open(
      'Please Select Reporting Manager',
      'x',
      {
        duration: 3000,
        horizontalPosition: 'right',
        verticalPosition: 'top',
      }
    );
  }
  }

  onNoClick(){
    this.dialogRef.close();
  }

  getFilteredAssociates(){
    if (!this.nonDeliveryAssociates) {
      return;
    }
    // get the search keyword
    let search = this.associatesFilterCtrl.value;
    if (!search) {
      this.filteredAssociatesList.next(this.nonDeliveryAssociates.slice());
      return;
    } else {
      search = search.toLowerCase();
    }
    // filter the non delivery associates list
    this.filteredAssociatesList.next(
      this.nonDeliveryAssociates.filter(project => project.Name.toLowerCase().indexOf(search) > -1)
    );
  }

}
