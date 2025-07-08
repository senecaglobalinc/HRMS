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

interface SelectItem {
  value: number;
  label: string;
}
@Component({
  selector: 'app-abscond',
  templateUrl: './abscond.component.html',
  styleUrls: ['./abscond.component.scss']
})
export class AbscondComponent implements OnInit {
  abscondForm: FormGroup;
  themeConfigInput = themeconfig.formfieldappearances;
  associates : any;
  associatesList: SelectItem[] = [];
  filteredAssociates: Observable<any>;
  leadId: number;
  deptId: number;
  currentDate = new Date();
  roleName: string;
  createAbscond: CreateAbscond = new CreateAbscond();
  constructor(private abscondService: AbscondService, private _snackBar: MatSnackBar,
    private spinner: NgxSpinnerService) { }

  ngOnInit(): void {
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
  }
  displayFn(user: any) {
    return user && user.label ? user.label : '';
  }
  clearField(fieldName, event) {
    if (fieldName == 'associateId') {
      event.stopPropagation();
      this.abscondForm.controls.associateId.setValue('');
    }
  }
  AbscondForm() {
    this.abscondForm = new FormGroup({
      associateId: new FormControl(null, Validators.required),
      absentFromDate: new FormControl(null, Validators.required),
      absentToDate: new FormControl(null, Validators.required),
      remarksByTL: new FormControl(null, Validators.required)
    });
  }

  getAssociateList(): void {
    this.spinner.show();
    this.abscondService
      .GetAssociateByLead(this.leadId, this.deptId)
      .subscribe(associateResponse => {
        this.spinner.hide();
        this.associates = associateResponse['Items'];
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
  GetSelectedAssociate(value){
    let obj =this.associates.find(ele => ele.AssociateId == value);
    if(obj.AssociateExitFlag){
      this._snackBar.open('Already Associate raised resignation!', 'x', {
        duration: 2000,
        panelClass: ['error-alert'],
        horizontalPosition: 'right',
        verticalPosition: 'top',
      });
      this.abscondForm.reset();
      this.abscondForm.controls.associateId.setValue('');
    }

  }
  CreateAbscondRequest() {
    this.abscondForm.markAllAsTouched();
    if (this.abscondForm.valid) {
      this.createAbscond.associateId = this.abscondForm.value.associateId.value;
      this.createAbscond.absentFromDate = moment(this.abscondForm.value.absentFromDate).format('YYYY-MM-DD')
      this.createAbscond.absentToDate = moment(this.abscondForm.value.absentToDate).format('YYYY-MM-DD')
      this.createAbscond.remarksByTL = this.abscondForm.value.remarksByTL;
      this.createAbscond.tlId = this.leadId;
      this.abscondService.CreateAbscond(this.createAbscond).subscribe(res => {
        if (res && res['IsSuccessful'] == true) {
          this._snackBar.open('Marked as Abscond Successfully.', 'x', {
            duration: 2000,
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
          this.clear();
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
    else{
      return;
    }
  }
  clear() {
    this.abscondForm.reset();
  }
}
