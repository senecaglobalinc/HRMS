import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { NgxSpinnerService } from 'ngx-spinner';
import { Observable } from 'rxjs';
import { themeconfig } from 'src/themeconfig';
import { AbscondService } from '../../Services/abscond.service';
import { CreateAbscond } from '../../Models/abscond.model'
import { startWith, map } from 'rxjs/operators';
import * as moment from 'moment';
import { ActivatedRoute, Router } from '@angular/router';

interface SelectItem {
  value: number;
  label: string;
}
@Component({
  selector: 'app-abscond-details',
  templateUrl: './abscond-details.component.html',
  styleUrls: ['./abscond-details.component.scss']
})
export class AbscondDetailsComponent implements OnInit {
  abscondForm: FormGroup;
  themeConfigInput = themeconfig.formfieldappearances;
  associatesList: SelectItem[] = [];
  filteredAssociates: Observable<any>;
  leadId: number;
  deptId: number;
  currentDate = new Date();
  roleName: string;
  associateId: number;
  AbscondDetails: CreateAbscond = new CreateAbscond();
  Abscondobj = [];
  disablefields = false;
  disableforHRM = false;
  disablebuttons = false;
  constructor(private actRoute: ActivatedRoute, private abscondService: AbscondService, private _snackBar: MatSnackBar,
    private spinner: NgxSpinnerService, private _router: Router) { }

  ngOnInit(): void {
    this.actRoute.params.subscribe(params => {
      this.associateId = params.id;
    });
    this.roleName = JSON.parse(
      sessionStorage.AssociatePortal_UserInformation
    ).roleName;
    this.leadId = JSON.parse(
      sessionStorage.AssociatePortal_UserInformation
    ).employeeId;
    this.deptId = JSON.parse(
      sessionStorage.AssociatePortal_UserInformation
    ).departmentId;
    this.AbscondForm();
    this.getAssociateList();
    this.getAbscondDetailsById();
  }
  getAbscondDetailsById() {
    this.abscondService.GetAbscondDetailByAssociateId(this.associateId).subscribe(res => {
      this.Abscondobj = res['Item']
      if (this.Abscondobj) {
        this.abscondForm.controls['associateId'].setValue({
          label: this.Abscondobj['AssociateName'],
          value: this.Abscondobj['AssociateId']
        })
        this.abscondForm.controls['absentFromDate'].setValue(this.Abscondobj['AbsentFromDate'])
        this.abscondForm.controls['absentToDate'].setValue(this.Abscondobj['AbsentToDate'])
        this.abscondForm.controls['remarksByTL'].setValue(this.Abscondobj['RemarksByTL'])
        this.abscondForm.controls['remarksByHRA'].setValue(this.Abscondobj['RemarksByHRA'])
        this.abscondForm.controls['remarksByHRM'].setValue(this.Abscondobj['RemarksByHRM'])
        this.abscondForm.controls['isAbscond'].setValue(this.Abscondobj['IsAbscond'])
        this.disablefields = true;
        if (this.roleName === 'HRM') {
          this.disableforHRM = true;
        }
        if(this.Abscondobj['StatusId'] === 43){
          this.abscondForm.disable();
        }
        if(this.Abscondobj['StatusId'] === 43 || this.Abscondobj['StatusId'] === 40 ){
          this.disablebuttons = true;
        }
        if((this.Abscondobj['StatusId'] === 40 || this.Abscondobj['StatusId'] === 34) && this.roleName === 'HRA'){
          this.abscondForm.disable();
          this.disablebuttons = true;
        }
      }
    })
  }

  displayFn(user: any) {
    return user && user.label ? user.label : '';
  }

  AbscondForm() {
    this.abscondForm = new FormGroup({
      associateId: new FormControl(null),
      absentFromDate: new FormControl(null),
      absentToDate: new FormControl(null),
      remarksByTL: new FormControl(null, Validators.required),
      remarksByHRA: new FormControl(null),
      remarksByHRM: new FormControl(null),
      isAbscond: new FormControl(false, [])
    });
    if (this.roleName === 'HRA') {
      this.abscondForm.controls['remarksByHRA'].setValidators(Validators.required);
      this.abscondForm.controls['remarksByHRA'].updateValueAndValidity();
    }
    if (this.roleName === 'HRM') {
      this.abscondForm.controls['remarksByHRM'].setValidators(Validators.required);
      this.abscondForm.controls['remarksByHRM'].updateValueAndValidity();
    }
  }

  getAssociateList(): void {
    this.spinner.show();
    this.abscondService
      .GetAssociateByLead(this.leadId, this.deptId)
      .subscribe(associateResponse => {
        this.spinner.hide();
        associateResponse['Items'].forEach(res => {
          this.associatesList.push({
            label: res.AssociateName,
            value: res.AssociateId,
          });
        });
        this.filteredAssociates = this.abscondForm.get('associateId').valueChanges.pipe(
          startWith(''),
          map((value) => this._filterAssociates(value))
        );
      }),
      (error: any) => {
        this.spinner.hide();
        if (error._body != undefined && error._body != '')
          this._snackBar.open(error.error, 'x', {

            duration: 1000,
            panelClass: ['Failed to get Associate List'],
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
      };
  }

  private _filterAssociates(value) {
    let filterValue;
    if (typeof value === 'string') {
      filterValue = value.toLowerCase();
    } else if (value !== null) {
      filterValue = value.label;
    } else {
      return this.associatesList;
    }

    return this.associatesList.filter((option) =>
      option.label.toLowerCase().includes(filterValue)
    );
  }
  Submit() {
    this.AbscondDetails.associateId = this.abscondForm.value.associateId.value;
    this.AbscondDetails.absentFromDate = this.abscondForm.value.absentFromDate;
    this.AbscondDetails.absentToDate = this.abscondForm.value.absentToDate;
    this.AbscondDetails.remarksByTL = this.abscondForm.value.remarksByTL;
    this.AbscondDetails.remarksByHRA = this.abscondForm.value.remarksByHRA;
    this.AbscondDetails.remarksByHRM = this.abscondForm.value.remarksByHRM
    this.AbscondDetails.isAbscond = this.abscondForm.value.isAbscond;
    if (this.roleName === 'HRA') {
      this.acknowledgeByHRA()
    }
    if (this.roleName === 'HRM') {
      this.ConfirmAbscond()
    }

  }
  acknowledgeByHRA() {
    if (this.abscondForm.value.remarksByHRA) {
      this.abscondService.AcknowledgeAbscond(this.AbscondDetails).subscribe(res => {
        if (res && res['IsSuccessful'] == true) {
          this._snackBar.open('Abscond Acknowledged Successfully.', 'x', {
            duration: 2000,
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
          this._router.navigate(['associateexit/abscond-dashboard']);
        }
        else {
          this._snackBar.open(res['Message'], 'x', {
            duration: 2000,
            panelClass: ['error-alert'],
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
        }
      },
        (error: any) => {
          this._snackBar.open('Error occured while submitting!.', 'x', {
            duration: 2000,
            panelClass: ['error-alert'],
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
        })
    }
    else {
      return;
    }

  }
  ConfirmAbscond() {
    if (this.abscondForm.value.remarksByHRM) {
      this.abscondService.ConfirmAbscond(this.AbscondDetails).subscribe(res => {
        if (res && res['IsSuccessful'] == true) {
          if(this.AbscondDetails.isAbscond === true){
            this._snackBar.open('Abscond Confirmed Successfully.', 'x', {
              duration: 2000,
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
          }
          else{
            this._snackBar.open('Abscond Disproved Successfully.', 'x', {
              duration: 2000,
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
          }
          this._router.navigate(['associateexit/abscond-dashboard']);
        }
        else {
          this._snackBar.open(res['Message'], 'x', {
            duration: 2000,
            panelClass: ['error-alert'],
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
        }
      },
        (error: any) => {
          this._snackBar.open('Error occured while submitting!.', 'x', {
            duration: 2000,
            panelClass: ['error-alert'],
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
        })
    }
    else {
      return;
    }

  }
  clear() {
    if (this.roleName === 'HRA') {
      this.abscondForm.controls['remarksByHRA'].setValue(null);
    }
    if (this.roleName === 'HRM') {
      this.abscondForm.controls['remarksByHRM'].setValue(null);
    }
  }
  onBack(){
    this._router.navigate(['associateexit/abscond-dashboard'])
  }
  AbscondClearance(){
    this.AbscondDetails.associateId = this.abscondForm.value.associateId.value;
    this.AbscondDetails.absentFromDate = this.abscondForm.value.absentFromDate;
    this.AbscondDetails.absentToDate = this.abscondForm.value.absentToDate;
    this.AbscondDetails.remarksByTL = this.abscondForm.value.remarksByTL;
    this.AbscondDetails.remarksByHRA = this.abscondForm.value.remarksByHRA;
    this.AbscondDetails.remarksByHRM = this.abscondForm.value.remarksByHRM
    this.AbscondDetails.isAbscond = this.abscondForm.value.isAbscond;
    this.abscondService.AbscondClearance(this.AbscondDetails).subscribe(res => {
      if(res && res['IsSuccessful'] == true){
        this._snackBar.open('Clearance given Successfully', 'x', {
          duration: 2000,
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
        this._router.navigate(['associateexit/abscond-dashboard']);
      }
      else{
        this._snackBar.open(res['Message'], 'x', {
          duration: 2000,
          panelClass: ['error-alert'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
      }
    },
    (error) => {
      this._snackBar.open('Error occured while submitting!', 'x', {
        duration: 2000,
        panelClass: ['error-alert'],
        horizontalPosition: 'right',
        verticalPosition: 'top',
      });
    })
  }
}
