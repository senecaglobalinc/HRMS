import { Component, OnInit } from '@angular/core';
import { ViewChild } from '@angular/core';
import { Qualification } from '../../models/education.model';
import { Associate } from '../../models/associate.model';
import { Inject } from '@angular/core';
// import { Http } from '@angular/http';
import { EducationService } from '../../services/education.service';
import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
import { Injector } from '@angular/core';
import { AppInjector } from '../../../shared/injector';
import * as moment from 'moment';
import { CommonService } from '../../../../core/services/common.service';
import { ProfessionalReferences, EmploymentDetails } from '../../models/employmentdetails.model';
import { EmploymentService } from '../../services/employment.service';
import { themeconfig } from '../../../../../themeconfig';
import { MatTable, MatTableDataSource } from '@angular/material/table';
import {
  MatSnackBar,
  MatSnackBarHorizontalPosition,
  MatSnackBarVerticalPosition,
} from '@angular/material/snack-bar';
import { FormGroup, Validators, FormBuilder, FormArray, FormControl } from '@angular/forms';
import { DateAdapter, MAT_DATE_LOCALE, MAT_DATE_FORMATS } from '@angular/material/core';
import { CommonDialogComponent } from '../../../shared/components/common-dialog/common-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { MessageDialogComponent } from 'src/app/modules/project-life-cycle/components/message-dialog/message-dialog.component';
import { employeeDetails } from 'src/app/modules/master-layout/models/talentrequisitionhistory.model';
import { NgxSpinnerService } from 'ngx-spinner';
// import {MomentDateAdapter} from '@angular/material-moment-adapter';

export const MY_FORMATS = {
  parse: {
    dateInput: 'LL',
  },
  display: {
    dateInput: 'YYYY-MM-DD',
    monthYearLabel: 'YYYY',
    dateA11yLabel: 'LL',
    monthYearA11yLabel: 'YYYY',
  },
};
@Component({
  selector: 'app-associate-employment',
  templateUrl: './associate-employment.component.html',
  styleUrls: ['./associate-employment.component.scss'],
  providers: [


    //  {provide: MAT_DATE_FORMATS, useValue: MY_FORMATS},
  ],
})

export class AssociateEmploymentComponent implements OnInit {
  constructor(public dialog: MatDialog, private snackBar: MatSnackBar, private _injector: Injector = AppInjector(),
    @Inject(EmploymentService) private _service: EmploymentService,
    // tslint:disable-next-line:variable-name
    @Inject(Router) private _router: Router, private actRoute: ActivatedRoute, public fb: FormBuilder,private spinner: NgxSpinnerService) {
    this._Associate.PrevEmploymentDetails = new Array<EmploymentDetails>();
    // tslint:disable-next-line:max-line-length
    // this._Associate.PrevEmploymentDetails.push({ ServiceFrom: null, ServiceTo: null, Name: '', Designation: '', Address: '', LeavingReason: '' });
    this._Associate.ProfReferences = new Array<ProfessionalReferences>();
    // tslint:disable-next-line:max-line-length
    // this._Associate.ProfReferences.push({ Name: null, CompanyName: null, Designation: '', MobileNo: '', CompanyAddress: '', OfficeEmailAddress: '' });
  }

   id: number;
  _Associate = new Associate();
  themeAppearence = themeconfig.formfieldappearances;
  currentempID: number;
  dataSource: MatTableDataSource<EmploymentDetails>;
  dataSource2: MatTableDataSource<ProfessionalReferences>;
  isProf:boolean = false;
  isEmp: boolean = false;
  isFormSubmitted: boolean = false;
  isProfFormSubmitted : boolean = false;

  @ViewChild('table') table: MatTable<any>;
  @ViewChild('table2') table2: MatTable<any>;
  private _serverURL: string;
  type: string = 'new';
  lastDate: Date;
  index: number;
  yearRange: string;
  emp: any;
  horizontalPosition: MatSnackBarHorizontalPosition = 'right';
  verticalPosition: MatSnackBarVerticalPosition = 'top';
  PreviousEmployer: FormGroup;
  PreviousEmployerArray: FormArray;
  ProfessionalReferencesArray: FormArray;
  ProfessionalReferences: FormGroup;
  newEmployer: FormGroup;
  newProfRef: FormGroup;
  displayedColumnsPrevious: string[] = ['Name', 'Designation', 'Address', 'ServiceFrom', 'ServiceTo', 'LeavingReason', 'Action', 'Delete'];
  displayedColumnsProfReferences: string[] = ['Name', 'Designation', 'CompanyName', 'CompanyAddress', 'OfficeEmailAddress', 'MobileNo', 'ActionProf', 'DeleteProf'];
  dialogResponse: boolean;


  ngOnInit() {
    this.spinner.show()

    this.actRoute.params.subscribe(params => { this.id = params.id; });
    // this.id = 218; // 496
    this.currentempID = this.id;
    this.yearRange = (new Date().getFullYear() - 50) + ':' + (new Date().getFullYear() + 0);
    this.createInitialFormPrev();
    this.getEmploymentdetails(this.currentempID);
    this.getDates();
  }
  createInitialFormPrev() {
    this.PreviousEmployer = new FormGroup({
      PreviousEmployerArray: new FormArray([]),
      ProfessionalReferencesArray: new FormArray([])
    });
  }
  createFormGroupPrev() {

    this.PreviousEmployerArray = new FormArray([]);
    this.ProfessionalReferencesArray = new FormArray([]);
    this._Associate.PrevEmploymentDetails.forEach(PrevEmpDetails => {
      this.PreviousEmployerArray = this.PreviousEmployer.get('PreviousEmployerArray') as FormArray;
      this.PreviousEmployerArray.push(this.BuildForm(PrevEmpDetails));
    });
    this._Associate.ProfReferences.forEach(ProfDetails => {
      this.ProfessionalReferencesArray = this.PreviousEmployer.get('ProfessionalReferencesArray') as FormArray;
      this.ProfessionalReferencesArray.push(this.BuildFormProfRef(ProfDetails));
    });
  }
  get testArrayPrev(): FormArray {
    return this.PreviousEmployer.controls.PreviousEmployerArray as FormArray;
  }
  get testArrayProfRef(): FormArray {
    return this.PreviousEmployer.controls.ProfessionalReferencesArray as FormArray;
  }
  BuildForm(PrevEmpDetails): FormGroup {

    return new FormGroup({
      // tslint:disable-next-line:max-line-length
      Name: new FormControl(PrevEmpDetails.Name, [Validators.required,Validators.pattern('^[a-zA-Z ]*$'), Validators.maxLength(100)]),
      // tslint:disable-next-line:max-line-length
      Address: new FormControl(PrevEmpDetails.Address, [Validators.required,Validators.maxLength(256)]),
      // tslint:disable-next-line:max-line-length
      Designation: new FormControl(PrevEmpDetails.Designation, [Validators.required,Validators.pattern('^[a-zA-Z ]*$'), Validators.maxLength(25)]),
      ServiceFrom: new FormControl(PrevEmpDetails.ServiceFrom,[Validators.required]),
      ServiceTo: new FormControl(PrevEmpDetails.ServiceTo,[Validators.required]),
      LeavingReason: new FormControl(PrevEmpDetails.LeavingReason, [Validators.required,Validators.maxLength(100), Validators.pattern('^[a-zA-Z ]*$')])
    });
  }
  BuildFormProfRef(ProfDetails): FormGroup {

    return new FormGroup({
      Name: new FormControl(ProfDetails.Name, [Validators.required, Validators.pattern('^[a-zA-Z ]*$'), Validators.maxLength(30)]),
      Designation: new FormControl(ProfDetails.Designation, [Validators.required, Validators.pattern('^[a-zA-Z ]*$'), Validators.maxLength(25)]),
      CompanyName: new FormControl(ProfDetails.CompanyName, [Validators.required, Validators.pattern('^[a-zA-Z ]*$'), Validators.maxLength(100)]),
      CompanyAddress: new FormControl(ProfDetails.CompanyAddress, [Validators.required, Validators.maxLength(256)]),
      OfficeEmailAddress: new FormControl(ProfDetails.OfficeEmailAddress, [Validators.required, Validators.pattern('^[A-Za-z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-z0-9]{2,4}$')]),
      MobileNo: new FormControl(ProfDetails.MobileNo, [Validators.required, Validators.pattern('^[0-9]*$')])
    });
  }
  forAddress(event: any) {
    let k: any;
    k = event.charCode;  //         k = event.keyCode;  (Both can be used)
    return ((k > 64 && k < 91) || (k > 96 && k < 123) || k === 8 || k === 32 || k === 92 || k === 35 || (k >= 44 && k <= 57));
  }

  omit_special_char(event: any) {
    let k: any;
    k = event.charCode;  //         k = event.keyCode;  (Both can be used)
    return ((k > 64 && k < 91) || (k > 96 && k < 123) || k === 8 || k === 32 || k === 46 || k === 44 || (k >= 48 && k <= 57));
  }

  onlychar(event: any) {
    let k: any;
    k = event.charCode;  //         k = event.keyCode;  (Both can be used)
    return ((k > 64 && k < 91) || (k > 96 && k < 123) || k === 8 || k === 32);
  }

  onNewEmployment(empdetails: EmploymentDetails, navigator) {
    // tslint:disable-next-line:max-line-length
    let PrevEmploymentLength = this._Associate.PrevEmploymentDetails.length;
    if (!this._Associate.PrevEmploymentDetails[PrevEmploymentLength - 1].ServiceFrom || !this._Associate.PrevEmploymentDetails[PrevEmploymentLength - 1].ServiceTo ||
      !this._Associate.PrevEmploymentDetails[PrevEmploymentLength - 1].Name || !this._Associate.PrevEmploymentDetails[PrevEmploymentLength - 1].Designation ||
      !this._Associate.PrevEmploymentDetails[PrevEmploymentLength - 1].Address || !this._Associate.PrevEmploymentDetails[PrevEmploymentLength - 1].LeavingReason) {
      return;
    }
    this._Associate.PrevEmploymentDetails.push({ ServiceFrom: null, ServiceTo: null, Name: null, Designation: null, Address: null, LeavingReason: null });
    this.PreviousEmployerArray = this.PreviousEmployer.get('PreviousEmployerArray') as FormArray;
    // tslint:disable-next-line:max-line-length
    this.PreviousEmployerArray.push(this.newEmployer = this.BuildForm({ ServiceFrom: null, ServiceTo: null, Name: null, Designation: null, Address: null, LeavingReason: null }));
    this.dynamicValidations('ServiceFrom', this.newEmployer)
    this.dynamicValidations('ServiceTo', this.newEmployer)
    this.dynamicValidations('Name', this.newEmployer)
    this.dynamicValidations('Designation', this.newEmployer)
    this.dynamicValidations('Address', this.newEmployer)
    this.dynamicValidations('LeavingReason', this.newEmployer)
    this.table.renderRows();
    return false;

  }

  dynamicValidations(control: string, newFormValidate: FormGroup){
    newFormValidate.controls[control].setErrors(null);
    newFormValidate.controls[control].clearValidators();
    newFormValidate.controls[control].updateValueAndValidity();       
  }


  onDeleteEmployment(index: number) {
    this.index = index;
    this.openDialog('Confirmation', 'Do you want to Delete ?');
    // this.DeleteEmployment();
    // this.table.renderRows();
  }

  openDialog(Heading, Message): void {
    const dialogRef = this.dialog.open(CommonDialogComponent, {
      disableClose: true,
      hasBackdrop: true,
      width: '300px',
      data: { heading: Heading, message: Message }
    });

    dialogRef.afterClosed().subscribe(result => {
      this.dialogResponse = result;
      if (this.dialogResponse == true) {
        this.DeleteEmployment();
        this.table.renderRows();
        const dialogConf = this.dialog.open(MessageDialogComponent, {
          disableClose: true,
          hasBackdrop: true,
          width: '300px',
          data: { heading: 'Confirmation', message: 'Employee Details deleted successfully' }
        })
      }
    });
  }




  DeleteEmployment() {
    this._Associate.PrevEmploymentDetails.splice(this.index, 1);
    this.PreviousEmployerArray.removeAt(this.index);
    //   this.snackBar.open('Previous Employment Details Deleted Successfully', 'x', {
    //     duration: 5000,
    //     horizontalPosition: this.horizontalPosition,
    //     verticalPosition: this.verticalPosition,
    // });
  }
  OpenConfirmationDialog(type: string) {   // method to open dialog

  }


  onNewProfRef(profdetails: ProfessionalReferences) {
    let profdetailslength = this._Associate.ProfReferences.length;
    if (!this._Associate.ProfReferences[profdetailslength - 1].Name || !this._Associate.ProfReferences[profdetailslength - 1].CompanyName || !this._Associate.ProfReferences[profdetailslength - 1].Designation ||
      !this._Associate.ProfReferences[profdetailslength - 1].MobileNo || !this._Associate.ProfReferences[profdetailslength - 1].CompanyAddress || !this._Associate.ProfReferences[profdetailslength - 1].OfficeEmailAddress) {
      return;
    }
    // tslint:disable-next-line:max-line-length
    this._Associate.ProfReferences.push({ Name: null, CompanyName: null, Designation: null, MobileNo: null, CompanyAddress: null, OfficeEmailAddress: null });
    this.ProfessionalReferencesArray = this.PreviousEmployer.get('ProfessionalReferencesArray') as FormArray;
    // tslint:disable-next-line:max-line-length
    this.ProfessionalReferencesArray.push(this.newProfRef = this.BuildFormProfRef({ Name: null, CompanyName: null, Designation: null, MobileNo: null, CompanyAddress: null, OfficeEmailAddress: null }));
    this.dynamicValidations('MobileNo',this.newProfRef)
    this.dynamicValidations('Name',this.newProfRef)
    this.dynamicValidations('CompanyName',this.newProfRef)
    this.dynamicValidations('Designation',this.newProfRef)
    this.dynamicValidations('CompanyAddress',this.newProfRef)
    this.dynamicValidations('OfficeEmailAddress',this.newProfRef)
    // tslint:disable-next-line:max-line-length
    this.table2.renderRows();
    return false;

  }


  onDeleteProfessionalRef(index: number) {
    this.index = index;
    this.openDialog2('Confirmation', 'Do you want to Delete ?');

  }

  openDialog2(Heading, Message): void {
    const dialogRef = this.dialog.open(CommonDialogComponent, {
      disableClose: true,
      hasBackdrop: true,
      width: '300px',
      data: { heading: Heading, message: Message }
    });

    dialogRef.afterClosed().subscribe(result => {
      this.dialogResponse = result;
      if (this.dialogResponse == true) {
        this.Delete();
        this.table2.renderRows();
        const dialogConf = this.dialog.open(MessageDialogComponent, {
          disableClose: true,
          hasBackdrop: true,
          width: '300px',
          data: { heading: 'Confirmation', message: 'Professional Reference deleted successfully.' }
        })
      }
    });
  }


  Delete() {
    this._Associate.ProfReferences.splice(this.index, 1);
    this.ProfessionalReferencesArray.removeAt(this.index);
  }

  getDates() {
    // tslint:disable-next-line:one-variable-per-declaration
    let date = new Date(), y = date.getFullYear(), m = date.getMonth(), d = date.getDate();
    this.lastDate = new Date(y, m, d);
  }

  validate(toYear: Date, fromYear: Date): number {
    let count = 0;
    if (toYear && fromYear) {
      this._Associate.PrevEmploymentDetails.forEach((details: any) => {
        if (details.ServiceTo == toYear && details.ServiceFrom == fromYear) {
          {
            count++;
          }
        }
      });
    }

    if (count === 2) {
      // this.messageService.add({severity:'warn', summary: 'Warning Message', detail:'You cant select same service duration'});
      this.snackBar.open('You cannot select same service duration', 'x', {
        duration: 5000,
        horizontalPosition: this.horizontalPosition,
        verticalPosition: this.verticalPosition,
      });
    }
    return count;
  }

  clearInput(evt: any, fieldName, i): void {
    if(fieldName=='ServiceFrom'){
      evt.stopPropagation();
      this.testArrayPrev.controls[i].get('ServiceFrom').reset();
    }
    if(fieldName=='ServiceTo'){
      evt.stopPropagation();
      this.testArrayPrev.controls[i].get('ServiceTo').reset();
    }
  }

  getEmploymentdetails = function (empID: number) {
   

    this._Associate.empID = empID;
    this._service.GetEmploymentDetails(this.id).subscribe((res: any) => {
      // tslint:disable-next-line:prefer-for-of
      for (let i = 0; i < res.length; i++) {
        res[i].ServiceFrom = moment(res[i].ServiceFrom).format('YYYY-MM-DD');
        res[i].ServiceTo = moment(res[i].ServiceTo).format('YYYY-MM-DD');
      }
      this._Associate.PrevEmploymentDetails = res;
      if (this._Associate.PrevEmploymentDetails.length === 0) {
        // tslint:disable-next-line:max-line-length
        this._Associate.PrevEmploymentDetails.push({ ServiceFrom: null, ServiceTo: null, Name: '', Designation: '', Address: '', LeavingReason: '' });
      }
      // this.dataSource = new MatTableDataSource(this._Associate.PrevEmploymentDetails);
      this.createInitialFormPrev();
      this.createFormGroupPrev();

    });
    this._service.GetProfReferenceDetails(this.id).subscribe((res: any) => {
      this.spinner.hide()
      this._Associate.ProfReferences = res;
      if (this._Associate.ProfReferences.length !== 0) {
        this.type = 'edit';
      }
      if (this._Associate.ProfReferences.length === 0) {
        // tslint:disable-next-line:max-line-length
        this._Associate.ProfReferences.push({ Name: null, CompanyName: null, Designation: '', MobileNo: '', CompanyAddress: '', OfficeEmailAddress: '' });
      }
      // this.dataSource2 = new MatTableDataSource(this._Associate.ProfReferences);
      this.createInitialFormPrev();
      this.createFormGroupPrev();

    },(error)=>{
      this.spinner.hide()
    });

  };


  // tslint:disable-next-line:only-arrow-functions
  IsValidDate = function (fromDate: any, toDate: any) {
    if (Date.parse(fromDate) < Date.parse(toDate)) {
      return true;
    }
    return false;
  };

  ValidatePrevEmployerdetails = function () {
    // tslint:disable-next-line:prefer-const
    let prevEmployerdetails = this._Associate.PrevEmploymentDetails;

    if (prevEmployerdetails.length === 1 && !prevEmployerdetails[0].Name) {
      return 3;
    }
    for (var count = 0; count < prevEmployerdetails.length; count++) {

      if (!prevEmployerdetails[count].Name || prevEmployerdetails[count].Name.trim().length === 0
        || !prevEmployerdetails[count].Address || prevEmployerdetails[count].Address.trim().length === 0
        || !prevEmployerdetails[count].Designation || prevEmployerdetails[count].Designation.trim().length === 0
        || !prevEmployerdetails[count].ServiceFrom
        || !prevEmployerdetails[count].ServiceTo) {

        if (count === (prevEmployerdetails.length - 1) && !prevEmployerdetails[count].Name) {
          return 3;
        }

        return 3;
      }
      if (!this.IsValidDate(prevEmployerdetails[count].ServiceFrom, prevEmployerdetails[count].ServiceTo)) {
        return 2;
      }
    }
    return 1;
  };

  ValidateProfReference = function () {
    let profReference = this._Associate.ProfReferences;
    if (profReference.length === 1 && !profReference[0].Name) {
      return false;
    }
    for (let count = 0; count < profReference.length; count++) {
      if (!profReference[count].Name || profReference[count].Name.trim().length === 0
        || !profReference[count].Designation || profReference[count].Name.trim().Designation === 0
        || !profReference[count].CompanyName || profReference[count].Name.trim().CompanyName === 0
        || !profReference[count].CompanyAddress || profReference[count].Name.trim().CompanyAddress === 0
        || !profReference[count].OfficeEmailAddress || profReference[count].Name.trim().OfficeEmailAddress === 0
        || !profReference[count].MobileNo) {
        if (count == (profReference.length - 1) && !profReference[count].Name) {
          return false;
        }
        return false;
      }
    };
    return true;
  }
  service(event, i) {
    this._Associate.PrevEmploymentDetails[i].ServiceFrom = moment(new Date(event.value)).format('YYYY-MM-DD');
  }
  serviceTo(event, i) {
    this._Associate.PrevEmploymentDetails[i].ServiceTo = moment(new Date(event.value)).format('YYYY-MM-DD');
  }
  CompanyNameChange(event, i) {
    this._Associate.PrevEmploymentDetails[i].Name = event.target.value;
  }
  AddressChange(event, i) {
    this._Associate.PrevEmploymentDetails[i].Address = event.target.value;
  }
  DesignationChange(event, i) {
    this._Associate.PrevEmploymentDetails[i].Designation = event.target.value;
  }
  LeavingReasonChange(event, i) {
    this._Associate.PrevEmploymentDetails[i].LeavingReason = event.target.value;
  }

  onSaveorUpdate(emp: Associate, isNew: boolean) {
    var today: any = new Date();
    today = today.getFullYear() + '/' + (today.getMonth() + 1) + '/' + today.getDate();
    var IsValidPrevEmployerdetail = this.ValidatePrevEmployerdetails();
    if (this.newEmployer !== undefined){
      this.isFormSubmitted = true;
      this.setRequiredValidations('ServiceFrom', this.newEmployer)
      this.setRequiredValidations('ServiceTo', this.newEmployer)
      this.setRequiredValidations('Name', this.newEmployer)
      this.setRequiredValidations('Designation', this.newEmployer)
      this.setRequiredValidations('Address', this.newEmployer)
      this.setRequiredValidations('LeavingReason', this.newEmployer)
    }
    if (this.newProfRef !== undefined){
      this.isProfFormSubmitted = true;
      this.setRequiredValidations('MobileNo',this.newProfRef)
      this.setRequiredValidations('Name',this.newProfRef)
      this.setRequiredValidations('CompanyName',this.newProfRef)
      this.setRequiredValidations('Designation',this.newProfRef)
      this.setRequiredValidations('CompanyAddress',this.newProfRef)
      this.setRequiredValidations('OfficeEmailAddress',this.newProfRef)
    }
    if (IsValidPrevEmployerdetail === 2) {
      // this.messageService.add({severity:'warn', summary: 'Warning Message', detail:'Invalid service duration'});
      this.snackBar.open('Invalid service duration', 'x', {
        duration: 5000,
        horizontalPosition: this.horizontalPosition,
        verticalPosition: this.verticalPosition,
      });
      return true;
    }

    if (IsValidPrevEmployerdetail === 3) {

      //  this.messageService.add({severity:'warn', summary: 'Warning Message', detail:'Please complete Employer Reference details'});
      this.snackBar.open('Please complete Previous Employer details', 'x', {
        duration: 5000,
        horizontalPosition: this.horizontalPosition,
        verticalPosition: this.verticalPosition,
      });
      return true;
    }

    if (!this.ValidateProfReference()) {
      //  this.messageService.add({severity:'warn', summary: 'Warning Message', detail:'Please complete Professional Reference details'});
      this.snackBar.open('Please complete Professional Reference details', 'x', {
        duration: 5000,
        horizontalPosition: this.horizontalPosition,
        verticalPosition: this.verticalPosition,
      });
      return true;
    }

    var profReference = this._Associate.ProfReferences;
    for (var count = 0; count < profReference.length; count++) {
      if (!this.validateEmail(profReference[count].OfficeEmailAddress)) {

        //  this.messageService.add({severity:'warn', summary: 'Warning Message', detail:'Please enter valid email address. For ex:- abc@xyz.com'});
        return false;
      }
      if (profReference[count].MobileNo.length < 10) {

        //  this.messageService.add({severity:'warn', summary: 'Warning Message', detail:'Please enter valid contact number.'});
        return false;
      }
    }

    var prevEmployerdetails = this._Associate.PrevEmploymentDetails;
    for (var count = 0; count < prevEmployerdetails.length; count++) {

      if (!this.IsValidDate(prevEmployerdetails[count].ServiceFrom, today)) {
        //  this.messageService.add({severity:'warn', summary: 'Warning Message', detail:'From date should not be greater than today date.'});
        this.snackBar.open('From date should not be greater than today date.', 'x', {
          duration: 5000,
          horizontalPosition: this.horizontalPosition,
          verticalPosition: this.verticalPosition,
        });
        return false;
      }
      if (!this.IsValidDate(prevEmployerdetails[count].ServiceTo, today)) {
        //  this.messageService.add({severity:'warn', summary: 'Warning Message', detail:'To date should not be greater than or equal to today date.'});
        this.snackBar.open('To date should not be greater than or equal to today date.', 'x', {
          duration: 5000,
          horizontalPosition: this.horizontalPosition,
          verticalPosition: this.verticalPosition,
        });
        return false;
      }
    }

    this._Associate.EmpId = this.currentempID;
    this._service.SaveEmploymentDetails(this._Associate).subscribe((data) => {
      if (isNew) {
        this.snackBar.open('Employment details saved successfully.', 'x', {
          duration: 5000,
          horizontalPosition: this.horizontalPosition,
          verticalPosition: this.verticalPosition,
        });
      }
      else{
        this.snackBar.open('Employment details updated successfully.', 'x', {
          duration: 5000,
          horizontalPosition: this.horizontalPosition,
          verticalPosition: this.verticalPosition,
        });
      }
      this.getEmploymentdetails(this.currentempID);
    }, (error) => {
      // this.messageService.add({severity:'error', summary: 'Error Message', detail:'Failed to save employment details!'});
      this.snackBar.open('Failed to save employment details!', 'x', {
        duration: 5000,
        panelClass:['error-alert'],
        horizontalPosition: this.horizontalPosition,
        verticalPosition: this.verticalPosition,
      });
    });
  
    return false;
  }

  setRequiredValidations(formControl:string, newDetails: FormGroup){
    newDetails.controls[formControl].setValidators([Validators.required])
    newDetails.controls[formControl].updateValueAndValidity();
      if (formControl === 'LeavingReason'){
        this.isFormSubmitted = false;
        newDetails.controls[formControl].setValidators([Validators.required,Validators.maxLength(100), Validators.pattern('^[a-zA-Z ]*$')])
        newDetails.controls[formControl].updateValueAndValidity();
      }  
      else if (formControl === 'Designation'){
        newDetails.controls[formControl].setValidators([Validators.required,Validators.pattern('^[a-zA-Z ]*$'), Validators.maxLength(25)])
        newDetails.controls[formControl].updateValueAndValidity();
      }

      else if (formControl === 'Address'){
        newDetails.controls[formControl].setValidators([Validators.required,Validators.maxLength(256)])
        newDetails.controls[formControl].updateValueAndValidity();
      }

      else if (formControl === 'Name'){
        newDetails.controls[formControl].setValidators([Validators.required,Validators.pattern('^[a-zA-Z ]*$'), Validators.maxLength(100)])
        newDetails.controls[formControl].updateValueAndValidity();
      }
      else if (formControl === 'OfficeEmailAddress'){
        this.isProfFormSubmitted = false 
        newDetails.controls[formControl].setValidators([Validators.required, Validators.pattern('^[A-Za-z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-z0-9]{2,4}$')])
        newDetails.controls[formControl].updateValueAndValidity();
      }   
      else if (formControl === 'CompanyName'){
        newDetails.controls[formControl].setValidators([Validators.required,Validators.pattern('^[a-zA-Z ]*$'), Validators.maxLength(100)])
        newDetails.controls[formControl].updateValueAndValidity();
      }
      else if (formControl === 'MobileNo'){
        newDetails.controls[formControl].setValidators([Validators.required,Validators.pattern('^[0-9]*$')])
        newDetails.controls[formControl].updateValueAndValidity();
      }
  }

  onlyForNumbers(event: any) {
    var keys = {
      'escape': 27, 'backspace': 8, 'tab': 9, 'enter': 13,
      '0': 48, '1': 49, '2': 50, '3': 51, '4': 52, '5': 53, '6': 54, '7': 55, '8': 56, '9': 57
    };
    for (var index in keys) {
      if (!keys.hasOwnProperty(index)) continue;
      if (event.charCode == keys[index] || event.keyCode == keys[index]) {
        return; //default event
      }
    }
    event.preventDefault();
  }
  validateEmail(email: any) {
    var filter = /^[\w\-\.\+]+\@[a-zA-Z0-9\.\-]+\.[a-zA-z0-9]{2,4}$/;
    if (filter.test(email)) {
      return true;
    }
    else {
      return false;
    }
  }

  onlyStrings(event: any) {
    let k: any;
    k = event.charCode;  //         k = event.keyCode;  (Both can be used)
    return ((k > 64 && k < 91) || (k > 96 && k < 123) || k === 8 || k === 32 || k === 44 || k === 46);
  }

}
