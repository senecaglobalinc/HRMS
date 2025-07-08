import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, FormGroupDirective, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import * as moment from 'moment';
import { NgxSpinnerService } from 'ngx-spinner';
import { themeconfig } from '../../../../../themeconfig';
import { MasterDataService } from '../../../../core/services/masterdata.service';
import { GenericType } from '../../../master-layout/models/dropdowntype.model';
import { Associate } from '../../../onboarding/models/associate.model';
import { AssociateExit } from '../../Models/associateExit.model';
import { ResignastionService } from '../../Services/resignastion.service';

interface SelectItem {
  value: number;
  label: string;
}

@Component({
  selector: 'app-resignation',
  templateUrl: './resignation.component.html',
  styleUrls: ['./resignation.component.scss']
})
export class ResignationComponent implements OnInit, OnChanges {

  themeConfigInput = themeconfig.formfieldappearances;
  resignationForm: FormGroup;
  reasonList: SelectItem[] = [];
  HRAList: SelectItem[] = [];
  empId: number;
  exitDate: Date = new Date();
  disableInput = false;
  disableExitDate: boolean;
  AssociateDetails: Associate;
  AssociateExit: AssociateExit = new AssociateExit();
  UserRole: string;
  programManagerName = '';
  designationName = '';
  btnLabel = 'Submit';
  showback = false;
  deliverydept = false;
  currentPath: string;
  formSubmitted = false;
  minDate = new Date();
  deptListScreen = false;
  minLWD = new Date();

  @Input() flag: boolean;
  @ViewChild(FormGroupDirective) formGroupDirective: FormGroupDirective;

  @Output() lastWorkingDateEvent = new EventEmitter<Date>();
  @Output() HraSelectionEvent = new EventEmitter<any>();
  myFilter = (d: Date): boolean => {
    const day = d.getDay();
    // Prevent Saturday and Sunday from being selected.
    return day !== 0 && day !== 6;
  }

  constructor(
    private _resignationService: ResignastionService,
    private _router: Router,
    private _snackBar: MatSnackBar,
    private _masterDataService: MasterDataService,
    private spinner: NgxSpinnerService,
    private actRoute: ActivatedRoute) { }
  ngOnChanges(changes: SimpleChanges): void {
    this.flag = changes.flag.currentValue;
    if(this.flag){
      this.getAssociateExitDetails();
    }
  }
  ngOnInit(): void {
    if (sessionStorage.getItem('AssociatePortal_UserInformation') != null) {
      const currentRole = JSON.parse(
        sessionStorage.getItem('AssociatePortal_UserInformation')
      ).roleName;
      this.UserRole = currentRole;
      this.empId = JSON.parse(
        sessionStorage.getItem('AssociatePortal_UserInformation')
      ).employeeId;
    }
    this.actRoute.url.subscribe(url => { this.currentPath = url[0].path; });
    if (this.UserRole === 'HRM' || this.UserRole === 'Program Manager' || this.UserRole === 'Team Lead') {
      this.showback = true;
      this.actRoute.params.subscribe(params => {
        this.empId = params.id;
      });
    }
    if (this.currentPath === 'deptchecklist') {
      this.showback = true;
      this.actRoute.params.subscribe(params => {
        this.empId = params.id;
      });
    }
    this.getAssociateExitDetails();
    this.createResignationForm();
    this.getReason();
  }
 
  getAssociateExitDetails() {
    this._resignationService.getExitDetailsById(this.empId).subscribe((res: any) => {
      if (res) {
        this.AssociateExit = new AssociateExit();
        this.AssociateExit = res;
        res.DateOfJoin = moment(res.DateOfJoin).format('MM/DD/YYYY');
        if (this.AssociateExit.DepartmentId === 1){
          this.deliverydept = true;
        }

        if (res.AssociateExitId) {

          this.resignationForm.patchValue({
            resignationDate: moment(this.AssociateExit.ResignationDate).format('YYYY-MM-DD'),
            exitDate: moment(this.AssociateExit.ExitDate).format('YYYY-MM-DD'),
            reasonId: this.AssociateExit.ReasonId,
            reasonDetail: this.AssociateExit.ReasonDetail,
            associateRemarks: this.AssociateExit.AssociateRemarks,
          });

          this.minLWD = new Date(res.ResignationDate);
         
          if (this.UserRole === 'HRM' || this.UserRole === 'Team Lead') {
            this.resignationForm.controls.resignationDate.disable();
            this.resignationForm.controls.reasonId.disable();
            this.resignationForm.controls.reasonDetail.disable();
            this.resignationForm.controls.associateRemarks.disable();
            if (res.Status === 'RevokeRequested' || this.currentPath === 'PMSubmission') {
              this.resignationForm.controls.exitDate.disable();
            }
          }
          else {
            this.resignationForm.disable();
          }
          this.btnLabel = 'Submit';
          this.disableInput = true;

        }

        else {
          this.disableExitDate = true;
          if(this.AssociateExit.NoticePeriodInDays){
            var date = new Date(this.exitDate.setDate(this.exitDate.getDate() + this.AssociateExit.NoticePeriodInDays))
            if (date.getDay() == 6) {
              var changeddate = date.setDate(date.getDate()-1)
            }
            else if( date.getDay() == 0) {
              changeddate = date.setDate(date.getDate()-2)
            }
            else {
              changeddate = date.setDate(date.getDate())
            }
            this.resignationForm.patchValue({
              resignationDate: moment(new Date()).format('YYYY-MM-DD'),
              exitDate: moment(changeddate).format('YYYY-MM-DD')
            });
          }
          else{
            this._snackBar.open('Error While getting Notice Period Days', 'x', {
              duration: 5000,
              panelClass: ['error-alert'],
              horizontalPosition: 'right',
              verticalPosition: 'top',
            });
          }
        }
      }
    });
  }

  createResignationForm() {
    this.resignationForm = new FormGroup({
      resignationDate: new FormControl(null, [Validators.required]),
      exitDate: new FormControl(null, [Validators.required]),
      reasonId: new FormControl(null, [Validators.required]),
      reasonDetail: new FormControl(null, [Validators.required]),
      associateRemarks: new FormControl(null),
    });

  }

  getReason(): void {
    this._resignationService.getResignationReason().subscribe((res: GenericType[]) => {
      this.reasonList = [];
      this.reasonList.push({ label: '', value: null });
      res.forEach((e: GenericType) => {
        this.reasonList.push({ label: e.Name, value: e.Id });
      });
    });

  }

  clear() {
    setTimeout(() =>
      this.formGroupDirective.resetForm({
        resignationDate: this.resignationForm.get('resignationDate').value,
        exitDate: this.resignationForm.get('exitDate').value
      }), 0);
  }

  submitResignation() {
    this.formSubmitted = true;
    if (this.resignationForm.valid) {
      this.AssociateExit.ResignationDate = this.resignationForm.value.resignationDate;
      this.AssociateExit.ExitDate = this.resignationForm.value.exitDate;
      this.AssociateExit.CalculatedExitDate = this.resignationForm.value.exitDate;
      this.AssociateExit.ReasonId = this.resignationForm.value.reasonId;
      this.AssociateExit.ReasonDetail = this.resignationForm.value.reasonDetail;
      this.AssociateExit.AssociateRemarks = this.resignationForm.value.associateRemarks;
      this.AssociateExit.ExitTypeId = 1;
      this.spinner.show();
      this._resignationService.submitResignation(this.AssociateExit).subscribe(res => {
        if (res) {
          this.spinner.hide();
          this._snackBar.open('Resignation Submitted successfully.', 'x', {
            duration: 1000,
            panelClass: ['success-alert'],
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
          this._router.navigate(['/shared/dashboard']);
        }
      },
        error => {
          this.spinner.hide();
          this._snackBar.open('Unable to submit resignation', 'x', {
            duration: 1000,
            panelClass: ['error-alert'],
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
        });
    }
    else {
      return;
    }
  }

  onBack() {
    if(this.UserRole === 'Team Lead'){
      this._router.navigate(['/shared/dashboard']);
    }
    else{
      this._router.navigate(['/shared/exit-actions']);
    }
  }

  setLWD(newLWD: Date) {
    this.lastWorkingDateEvent.emit(newLWD);
  }
  setHra(hra: any) {
    this.HraSelectionEvent.emit(hra);
  }

}
