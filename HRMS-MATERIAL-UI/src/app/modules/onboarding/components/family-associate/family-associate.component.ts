import { Component, OnInit, ViewChild } from '@angular/core';
import { ProspectiveToAssociateComponent } from '../prospective-to-associate/prospective-to-associate.component';
import { EmergencyContactDetail, Relation } from '../../models/family.model';
import { Router, ActivatedRoute } from '@angular/router';
import { CommonService } from '../../../master-layout/services/common.service';
import * as moment from 'moment';
import { FamilyService } from '../../services/family.service';
import { Associate } from '../../models/associate.model';
import { FormGroup, FormControl, Validators, FormGroupDirective, FormArray, FormBuilder } from '@angular/forms';
import { NavService } from '../../../master-layout/services/nav.service';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatSnackBar } from '@angular/material/snack-bar';
import { themeconfig } from '../../../../../themeconfig';
import { MatTable } from '@angular/material/table';
import {MatDialog} from '@angular/material/dialog';
import { CommonDialogComponent } from '../../../shared/components/common-dialog/common-dialog.component';
import { DatePipe } from '@angular/common';

import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-family-associate',
  templateUrl: './family-associate.component.html',
  styleUrls: ['./family-associate.component.scss'],
  providers: [CommonService]
})
export class FamilyAssociateComponent implements OnInit {
  minDate: Date;
  maxDate: Date;
  relationshipForm: FormGroup;
  dialogResponse:boolean;
  relationshipFormData: any;
  emergencydetails: FormGroup;
  savedmsg:string
  // emergencytwodetails: FormGroup;
  added = false;
  id: number;
  currentempID: number;
  isPrimary: boolean;
  _Associate = new Associate();
  AssociateRelationsInfo: Array<Relation>;
  relationsForm: FormGroup;
  // contactDetailsOne = new EmergencyContactDetail();
  // contactDetailsTwo = new EmergencyContactDetail();
  relationvalue: any[];
  themeAppearence = themeconfig.formfieldappearances;
  selectedFamilyDetails: any;
  type: string = "new";
  valueKey: string = "Relation";
  valueKey1: string = "Country";
  ddlRelations: any[];
  ddlCountries: any[];
  birthDate: Date;
  shortYearCutoff: any = '+10';
  mobileErrorMessage: string;
  formSubmitted: boolean = false;
  firstcontactzipLength: number;
  secondcontactzipLength: number;
  rship;
  relationtable: FormArray;
  isClearClicked:Boolean=false;

  //isNew:boolean=true;
  @ViewChild('familyDialog') familyDialog: any;
  @ViewChild(FormGroupDirective) formGroupDirective: FormGroupDirective;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  @ViewChild(MatTable) table: MatTable<any>;
  submitAttempt: boolean = false;
  //dataSource: MatTableDataSource<any>;
  dataSource = new MatTableDataSource();
  // dataSource = this.AssociateRelationsInfo;
  displayedColumns: string[] = ['S.no', 'RelationShip', 'Name', 'DateOfBirth', 'Occupation', 'Action'];
  constructor(public dialog: MatDialog,private formBuilder: FormBuilder, private _snackBar: MatSnackBar, private _commonService: CommonService, private _service: FamilyService, private router: Router, private actRoute: ActivatedRoute, private datePipe: DatePipe, private spinner: NgxSpinnerService) {
    this.AssociateRelationsInfo = new Array<Relation>();


    const currentYear = new Date().getFullYear();
    this.minDate = new Date(currentYear - 100, 0, 1);
    this.maxDate = new Date();
  }

  ngOnInit(): void {
    this.spinner.show()
    this.actRoute.params.subscribe(params => { this.id = params['id']; });
    this.currentempID = Math.floor(this.id);


    this.relationshipForm = new FormGroup({
      contactDetailsOne: this.createContactsForm(),
      contactDetailsTwo: this.createContactsForm(),
      RelationsInfo: new FormArray([this.CreateNewRelationshipForm()]),
      EmployeeId: new FormControl(null, [Validators.required]),
    });

    this.getRelations(this.valueKey);
    this.getCountries(this.valueKey1);
    this.getFamilydetails(this.currentempID);
    this.getDates();

    this.isPrimary = false;

    this._Associate.contactDetailsOne = new EmergencyContactDetail();
    this._Associate.contactDetailsTwo = new EmergencyContactDetail();
    this.relationshipForm.controls['EmployeeId'].setValue(this.currentempID);
    this.dataSource.data = this.relationshipForm['controls']['RelationsInfo']['controls'];
  }

  CreateNewRelationshipForm(): FormGroup {
    return this.formBuilder.group({
      Id: new FormControl(0),
      BirthDate: new FormControl(null),
      Name: new FormControl('', [Validators.required]),
      Occupation: new FormControl('', [Validators.required]),
      RelationShip: new FormControl('', [Validators.required]),
      DateOfBirth: new FormControl('', [Validators.required]),
      EmployeeId: new FormControl(this.currentempID, [Validators.required]),
    });
  }

  addNewRelationshipForm() {
    (this.relationshipForm.controls['RelationsInfo'] as FormArray).push(this.CreateNewRelationshipForm());
  }

  createContactsForm() {
    return new FormGroup({
      Id: new FormControl(0),
      IsPrimary: new FormControl(null),
      EmployeeId: new FormControl(this.currentempID, [Validators.required]),
      ContactType: new FormControl(null),
      ContactName: new FormControl(null, [Validators.required]),
      Relationship: new FormControl(null, [Validators.required]),
      AddressLine1: new FormControl(null, [Validators.required, Validators.maxLength(200)]),
      AddressLine2: new FormControl(null, [Validators.maxLength(200)]),
      City: new FormControl(null, [Validators.required]),
      State: new FormControl(null, [Validators.required]),
      Country: new FormControl(null, [Validators.required]),
      PostalCode: new FormControl(null, [Validators.required,Validators.pattern('^[0-9]*$')]),
      TelePhoneNo: new FormControl(null, [Validators.pattern('^(?:(?:\\+|0{0,2})91(\\s*[\\ -]\\s*)?|[0]?)?[789]\\d{9}|(\\d[ -]?){10}\\d$'), Validators.minLength(11), Validators.maxLength(12)]),
      MobileNo: new FormControl(null, [Validators.required, Validators.minLength(10), Validators.maxLength(10)]),
      EmailAddress: new FormControl(null, [Validators.maxLength(50)]),
    });
  }

  onClear() {
    this.isClearClicked=true;
    this.relationshipForm.reset();
    setTimeout(() => this.formGroupDirective.resetForm(), 0);
    this.formSubmitted=false;
  }
  getRelations(valueKey: string) {
    this._commonService.GetBusinessValues(valueKey).subscribe((res: any) => {
      this.ddlRelations = res
    });
  }
  compareCategoryObjects(object1: any, object2: any) {
    return object1 && object2 && object1.id == object2.id;
  }
  getCountries(valueKey1: string) {
    this._commonService.GetBusinessValues(valueKey1).subscribe((res: any) => {
      this.ddlCountries = res
    });
  }

  selectedDate(selectedDate: Date): Boolean {
    var today: any = new Date();
    today = today.getFullYear();
    selectedDate = moment(selectedDate).toDate();
    var selDate = selectedDate.getFullYear();
    var diff = Math.round(Math.abs(selDate - today));
    if (diff >= 100) {

      this._snackBar.open('Please select a valid date', 'x', {
        duration: 1000,
        horizontalPosition: 'right',
        verticalPosition: 'top',
      });

      return;
    }
  }
  getDates() {
    var date = new Date(),
      y = date.getFullYear(),
      m = date.getMonth(),
      d = date.getDate();
    this.birthDate = new Date(y, m, d - 1);
  }
  checkValidations = function () {
    this.submitAttempt = false;

    if (this._Associate.contactDetailsOne.IsPrimary == true) {
      // if (this._Associate.contactDetailsOne.ContactName == undefined || this._Associate.contactDetailsOne.ContactName == "" ||
      //   this._Associate.contactDetailsOne.Relationship == undefined || this._Associate.contactDetailsOne.Relationship == "" ||
      //   this._Associate.contactDetailsOne.AddressLine1 == undefined || this._Associate.contactDetailsOne.AddressLine1 == "" ||
      //   this._Associate.contactDetailsOne.City == undefined || this._Associate.contactDetailsOne.City == "" ||
      //   this._Associate.contactDetailsOne.State == undefined || this._Associate.contactDetailsOne.State == "" ||
      //   this._Associate.contactDetailsOne.PostalCode == undefined || this._Associate.contactDetailsOne.PostalCode == "" ||
      //   this._Associate.contactDetailsOne.Country == undefined || this._Associate.contactDetailsOne.Country == "" ||
      //   this._Associate.contactDetailsOne.MobileNo == undefined || this._Associate.contactDetailsOne.MobileNo == "")

      if (this.emergencydetails.value.ContactOnefirstcontactname || this.emergencydetails.value.ContactOnerelation || this.emergencydetails.value.ContactOneAddressLine1 || this.emergencydetails.value.ContactOneCity ||
        this.emergencydetails.value.ContactOneState || this.emergencydetails.value.ContactOnecountry || this.emergencydetails.value.ContactOneZip || this.emergencydetails.value.ContactOneMobileNumber)
        this.submitAttempt = false;
      else {
        this.submitAttempt = true;
      }

    }
    else if (this._Associate.contactDetailsTwo.IsPrimary == true) {

      // if (this._Associate.contactDetailsTwo.ContactName == undefined || this._Associate.contactDetailsTwo.ContactName == "" ||
      //   this._Associate.contactDetailsTwo.Relationship == undefined || this._Associate.contactDetailsTwo.AddressLine1 == undefined ||
      //   this._Associate.contactDetailsTwo.AddressLine1 == "" || this._Associate.contactDetailsTwo.City == undefined ||
      //   this._Associate.contactDetailsTwo.City == "" || this._Associate.contactDetailsTwo.State == undefined ||
      //   this._Associate.contactDetailsTwo.State == "" || this._Associate.contactDetailsTwo.PostalCode == undefined ||
      //   this._Associate.contactDetailsTwo.Country == undefined || this._Associate.contactDetailsTwo.MobileNo == undefined ||
      //   this._Associate.contactDetailsTwo.ContactName == "" || this._Associate.contactDetailsTwo.Relationship == "" ||
      //   this._Associate.contactDetailsTwo.PostalCode == null || this._Associate.contactDetailsTwo.Country == "" ||
      //   this._Associate.contactDetailsTwo.MobileNo == "")
      if (this.emergencydetails.value.ContactTwofirstcontactname || this.emergencydetails.value.ContactTworelation || this.emergencydetails.value.ContactTwoAddressLine1 || this.emergencydetails.value.ContactTwoCity ||
        this.emergencydetails.value.ContactTwoState || this.emergencydetails.value.ContactTwocountry || this.emergencydetails.value.ContactTwoZip || this.emergencydetails.value.ContactTwoMobileNumber)

        this.submitAttempt = false;
      else {
        this.submitAttempt = true;
      }
    }
    if (this.emergencydetails.value.ContactOneTelephoneNumber && this.emergencydetails.value.ContactOneTelephoneNumber.length! >= 10)
      this.submitAttempt = false;
    else {
      this.submitAttempt = true;
    }
    if (this.emergencydetails.value.ContactOneMobileNumber && this.emergencydetails.value.ContactOneMobileNumber.length >= 10)
      this.submitAttempt = false;
    else {
      this.submitAttempt = true;
    }
    if (this.emergencydetails.value.ContactOneTelephoneNumber && this.emergencydetails.value.ContactOneTelephoneNumber.length >= 10)
      this.submitAttempt = false;
    else {
      this.submitAttempt = true;
    }
    if (this.emergencydetails.value.ContactTwoMobileNumber && this.emergencydetails.value.ContactTwoMobileNumber.length >= 10)
      this.submitAttempt = false;
    else {
      this.submitAttempt = true;
    }
    if (this.emergencydetails.value.ContactOneZip && this.emergencydetails.value.ContactOneZip.length == this.firstcontactzipLength)
      this.submitAttempt = false;
    else {
      this.submitAttempt = true;
    }
    if (this.emergencydetails.value.ContactTwoZip && this.emergencydetails.value.ContactTwoZip.length == this.firstcontactzipLength)
      this.submitAttempt = false;
    else {
      this.submitAttempt = true;
    }

  }
  //this.emergencydetails.controls.ContactTwoZip.setvalue(x)

  getFamilydetails = function (empID: number) {
    this._service.GetFamilyDetails(this.id).subscribe((res: any) => {
     this.spinner.hide()
      if (res.Item.RelationsInfo.length != 0 || res.Item.contactDetailsOne || res.Item.contactDetailsTwo) {
        this.type = "edit";
        //this.isNew=false;
      }
      for (var i = 0; i < res.Item.RelationsInfo.length; i++) {

        res.Item.RelationsInfo[i].BirthDate = res.Item.RelationsInfo[i].DateOfBirth.replace("T11:00:00+05:30", '');
        if(!this.formSubmitted){
        if (i >= 1) {
          this.addNewRelationshipForm();
        }
      }

        this.relationshipForm['controls']['RelationsInfo']['controls'][i]['controls']['Id'].setValue(res.Item.RelationsInfo[i].Id);
        this.relationshipForm['controls']['RelationsInfo']['controls'][i]['controls']['EmployeeId'].setValue(this.currentempID);
        this.relationshipForm['controls']['RelationsInfo']['controls'][i]['controls']['BirthDate'].setValue(res.Item.RelationsInfo[i].BirthDate);
        this.relationshipForm['controls']['RelationsInfo']['controls'][i]['controls']['Name'].setValue(res.Item.RelationsInfo[i].Name);
        this.relationshipForm['controls']['RelationsInfo']['controls'][i]['controls']['Occupation'].setValue(res.Item.RelationsInfo[i].Occupation);
        this.relationshipForm['controls']['RelationsInfo']['controls'][i]['controls']['RelationShip'].setValue(parseInt(res.Item.RelationsInfo[i].RelationShip));
        this.relationshipForm['controls']['RelationsInfo']['controls'][i]['controls']['DateOfBirth'].setValue(res.Item.RelationsInfo[i].BirthDate);
      }
      this._Associate.RelationsInfo = res.Item.RelationsInfo;


      if (res.Item.contactDetailsOne != null && res.Item.contactDetailsOne != undefined) {
        this._Associate.contactDetailsOne = res.Item.contactDetailsOne;
        this.relationshipForm.controls['contactDetailsOne']['controls']['Id'].setValue(this._Associate.contactDetailsOne.Id);
        this.relationshipForm.controls['contactDetailsOne']['controls']['EmployeeId'].setValue(this.currentempID);
        this.relationshipForm.controls['contactDetailsOne']['controls']['ContactType'].setValue(this._Associate.contactDetailsOne.ContactType);
        this.relationshipForm.controls['contactDetailsOne']['controls']['AddressLine1'].setValue(this._Associate.contactDetailsOne.AddressLine1);
        this.relationshipForm.controls['contactDetailsOne']['controls']['AddressLine2'].setValue(this._Associate.contactDetailsOne.AddressLine2);
        this.relationshipForm.controls['contactDetailsOne']['controls']['City'].setValue(this._Associate.contactDetailsOne.City);
        this.relationshipForm.controls['contactDetailsOne']['controls']['ContactName'].setValue(this._Associate.contactDetailsOne.ContactName);
        this.relationshipForm.controls['contactDetailsOne']['controls']['Country'].setValue(this._Associate.contactDetailsOne.Country);
        this.relationshipForm.controls['contactDetailsOne']['controls']['EmailAddress'].setValue(this._Associate.contactDetailsOne.EmailAddress);
        this.relationshipForm.controls['contactDetailsOne']['controls']['IsPrimary'].setValue(this._Associate.contactDetailsOne.IsPrimary);
        this.relationshipForm.controls['contactDetailsOne']['controls']['MobileNo'].setValue(this._Associate.contactDetailsOne.MobileNo);
        this.relationshipForm.controls['contactDetailsOne']['controls']['PostalCode'].setValue(this._Associate.contactDetailsOne.PostalCode);
        this.relationshipForm.controls['contactDetailsOne']['controls']['Relationship'].setValue(this._Associate.contactDetailsOne.Relationship);
        this.relationshipForm.controls['contactDetailsOne']['controls']['State'].setValue(this._Associate.contactDetailsOne.State);
        this.relationshipForm.controls['contactDetailsOne']['controls']['TelePhoneNo'].setValue(this._Associate.contactDetailsOne.TelePhoneNo);
       
        if (this._Associate.contactDetailsOne.Country == "India")
          this.firstcontactzipLength = 6;
        else this.firstcontactzipLength = 5;
      }
      if (res.Item.contactDetailsTwo != null && res.Item.contactDetailsTwo != undefined) {
        this._Associate.contactDetailsTwo = res.Item.contactDetailsTwo;
        this.relationshipForm.controls['contactDetailsTwo']['controls']['Id'].setValue(this._Associate.contactDetailsTwo.Id);
        this.relationshipForm.controls['contactDetailsTwo']['controls']['EmployeeId'].setValue(this.currentempID);
        this.relationshipForm.controls['contactDetailsTwo']['controls']['ContactType'].setValue(this._Associate.contactDetailsTwo.ContactType);
        this.relationshipForm.controls['contactDetailsTwo']['controls']['AddressLine1'].setValue(this._Associate.contactDetailsTwo.AddressLine1);
        this.relationshipForm.controls['contactDetailsTwo']['controls']['AddressLine2'].setValue(this._Associate.contactDetailsTwo.AddressLine2);
        this.relationshipForm.controls['contactDetailsTwo']['controls']['City'].setValue(this._Associate.contactDetailsTwo.City);
        this.relationshipForm.controls['contactDetailsTwo']['controls']['ContactName'].setValue(this._Associate.contactDetailsTwo.ContactName);
        this.relationshipForm.controls['contactDetailsTwo']['controls']['Country'].setValue(this._Associate.contactDetailsTwo.Country);
        this.relationshipForm.controls['contactDetailsTwo']['controls']['EmailAddress'].setValue(this._Associate.contactDetailsTwo.EmailAddress);
        this.relationshipForm.controls['contactDetailsTwo']['controls']['IsPrimary'].setValue(this._Associate.contactDetailsTwo.IsPrimary);
        this.relationshipForm.controls['contactDetailsTwo']['controls']['MobileNo'].setValue(this._Associate.contactDetailsTwo.MobileNo);
        this.relationshipForm.controls['contactDetailsTwo']['controls']['PostalCode'].setValue(this._Associate.contactDetailsTwo.PostalCode);
        this.relationshipForm.controls['contactDetailsTwo']['controls']['Relationship'].setValue(this._Associate.contactDetailsTwo.Relationship);
        this.relationshipForm.controls['contactDetailsTwo']['controls']['State'].setValue(this._Associate.contactDetailsTwo.State);
        this.relationshipForm.controls['contactDetailsTwo']['controls']['TelePhoneNo'].setValue(this._Associate.contactDetailsTwo.TelePhoneNo);

        if (this._Associate.contactDetailsTwo.Country == "India")
          this.secondcontactzipLength = 6;
        else this.secondcontactzipLength = 5;


        this.dataSource.data = this.relationshipForm['controls']['RelationsInfo']['controls'];
        this.relationshipForm.value;

      }
      if (this._Associate.RelationsInfo.length == 0)
        this._Associate.RelationsInfo.push({
          BirthDate: null,
          Name: "",
          Occupation: "",
          RelationShip: ""
        });
      if (this._Associate.contactDetailsOne == null)
        this._Associate.contactDetailsOne = new EmergencyContactDetail();
      if (this._Associate.contactDetailsTwo == null)
        this._Associate.contactDetailsTwo = new EmergencyContactDetail();
    },(error)=>{
      this.spinner.hide()
    });


  }



  updateSelection(isprimaryname: string) {
        if (isprimaryname == 'contactDetailsOne') {
      this.relationshipForm.controls['contactDetailsTwo']['controls']['IsPrimary'].setValue(false);
      this.relationshipForm.controls['contactDetailsOne']['controls']['IsPrimary'].setValue(true);
    }
    else if (isprimaryname == 'contactDetailsTwo') {
      this.relationshipForm.controls['contactDetailsTwo']['controls']['IsPrimary'].setValue(true);
      this.relationshipForm.controls['contactDetailsOne']['controls']['IsPrimary'].setValue(false);
    }
  };


  clearInput(evt: any, fieldName, i): void {
    if(fieldName=='DateOfBirth'){
      evt.stopPropagation();
      this.relationshipForm.controls['RelationsInfo']['controls'][i].get('DateOfBirth').reset();
    }
  }

  onNewRelation(index) {
    
    this.added = true;
    if(this.relationshipForm.controls.RelationsInfo.valid){
      const add = this.relationshipForm.get('RelationsInfo') as FormArray;
      add.push(this.CreateNewRelationshipForm());
      this.table.renderRows();
      return false;
    }
    else{
      return false;
    }

  }

 

  onDelete(i) {
    this.openDialog('Confirmation','Do you want to Delete ?',i);
 }


  onRelationChange(event: any, index: number) {
    let relationShip = event.value;
    let count = 0;
    if( this.relationshipForm['controls']['RelationsInfo']['controls'][index]['controls']['EmployeeId']['value']==null)
    { this.relationshipForm['controls']['RelationsInfo']['controls'][index]['controls']['Id'].setValue(index);
      this.relationshipForm['controls']['RelationsInfo']['controls'][index]['controls']['EmployeeId'].setValue(this.currentempID);
     }

    if (relationShip) {

      let i, j: number;
      i = 0, j = 1;
      // do {
      //   let relationshipName = this.AssociateRelationsInfo[i].relationship;
      //   if (relationshipName == "Mother" || relationshipName == "Father" || relationshipName == "Spouse" ||
      //     relationshipName == "Mother-in-Law" || relationshipName == "Father-in-Law") {
      //     j = i + 1;
      //     while (j < this.AssociateRelationsInfo.length + 1) {
      //       if (this.AssociateRelationsInfo[j].relationship &&
      //         (relationshipName == this.AssociateRelationsInfo[j].relationship)) {
      //         event.target.value = "";
      //         this.messageService.add({ severity: 'warn', summary: 'Warn Message', detail: 'You cant select same relationship twice' });
      //         return;
      //       }
      //       j++;
      //     }
      //   }
      //   else i++;
      // } while (i < this.AssociateRelationsInfo.length)
      if (relationShip == "Mother" || relationShip == "Father" || relationShip == "Spouse" ||
        relationShip == "Mother-in-Law" || relationShip == "Father-in-Law") {
        for (i = 0; i < this.AssociateRelationsInfo.length; i++) {
          if (i != index) {
            let relationshipName = this.AssociateRelationsInfo[i].RelationShip;
            if (relationShip == relationshipName) {
              event.target.value = "";
              this.AssociateRelationsInfo[index].RelationShip = "";
              // this.messageService.add({ severity: 'warn', summary: 'Warn Message', detail: 'You cant select same relationship twice' });
              this._snackBar.open('You cant select same relationship twice', 'x', {
                duration: 1000,
                horizontalPosition: 'right',
                verticalPosition: 'top',
              });
              return;
            }
          }
        }
      }
    }
  }

  onSaveorUpdate() {
    if(this.isClearClicked){
      this.relationshipForm.controls['EmployeeId'].setValue(this.currentempID);
      this.relationshipForm.controls['contactDetailsOne']['controls']['Id'].setValue(0);
      this.relationshipForm.controls['contactDetailsOne']['controls']['EmployeeId'].setValue(this.currentempID);
      this.relationshipForm.controls['contactDetailsTwo']['controls']['Id'].setValue(0);
      this.relationshipForm.controls['contactDetailsTwo']['controls']['EmployeeId'].setValue(this.currentempID);
    }

    this.formSubmitted=true;
    this.relationshipFormData = this.relationshipForm.value;
    this.type;
      for (var i = 0; i < this.relationshipFormData.RelationsInfo.length; i++){
        // this.relationshipFormData.RelationsInfo[i].DateOfBirth = this.relationshipFormData.RelationsInfo[i].DateOfBirth.toString().substring(0, 10);
        var temp=this.relationshipFormData.RelationsInfo[i].DateOfBirth
        var set=this.datePipe.transform(temp,"yyyy-MM-dd")
        this.relationshipFormData.RelationsInfo[i].DateOfBirth=set;
        this.relationshipFormData.RelationsInfo[i].BirthDate=this.relationshipFormData.RelationsInfo[i].DateOfBirth
      }
    this.relationshipFormData.contactDetailsOne.EmployeeId = this.currentempID;
    this.relationshipFormData.contactDetailsTwo.EmployeeId = this.currentempID;
    if(!this.relationshipForm['controls']['contactDetailsOne']['controls']['IsPrimary'].value && !this.relationshipForm['controls']['contactDetailsTwo']['controls']['IsPrimary'].value){
      this._snackBar.open('Please select atleast one primary', 'x', {
        duration: 1000,
        horizontalPosition: 'right',
        verticalPosition: 'top',
      });
      return false;
    }

    if (this.relationshipForm['controls']['contactDetailsOne']['controls']['IsPrimary'].value && !this.relationshipForm['controls']['contactDetailsTwo']['controls']['IsPrimary'].value) {
      this.removemyValidators('contactDetailsTwo');
      this.DynamicValidators('contactDetailsOne');
    }
    if (!this.relationshipForm['controls']['contactDetailsOne']['controls']['IsPrimary'].value && this.relationshipForm['controls']['contactDetailsTwo']['controls']['IsPrimary'].value) {
      this.removemyValidators('contactDetailsOne');
      this.DynamicValidators('contactDetailsTwo')
    }
    // this.checkValidations();
    if (this.relationshipForm.valid) {
      this._service.SaveFamilyDetails(this.relationshipFormData).subscribe((data) => {
        if(this.type == "new"){
          this.savedmsg ='Family details saved successfully'
      }
      else{
        this.savedmsg ='Family details updated successfully'
      }
        this.submitAttempt = false;
        this.isClearClicked=false;
        this._snackBar.open(this.savedmsg, 'x', {
          duration: 1000,
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });

      //  this.onClear();
        this.getFamilydetails(this.currentempID);
      }, (error) => {
        this._snackBar.open(error.error, 'x', {
          duration: 1000,
          panelClass:['error-alert'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
      });
    }
    else {
      this._snackBar.open("Please correct the fields highlighted in red colour",
        "!", {
          duration: 1000,
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
    }
  }
  OpenConfirmationDialog(editMode: number) {   // method to open dialog
    // this.confirmationService.confirm({
    //   message: 'Do you want to Delete ?',
    //   accept: () => {
    //     this.Delete();
    //   },
    //   reject: () => {

    //   }
    // });
  }



  onCancel() {
    this.familyDialog.nativeElement.close();
  }

  onlyNumbers(event: any) {
    this._commonService.onlyNumbers(event);
  }

  onlychar(event: any) {
    let k: any;
    k = event.charCode;
    // k = event.keyCode; (Both can be used)
    return ((k > 64 && k < 91) || (k > 96 && k < 123) || k == 8 || k == 32);
  }
  forAddress(event: any) {
    let k: any;
    k = event.charCode;
    // k = event.keyCode; (Both can be used)
    return ((k > 64 && k < 91) || (k > 96 && k < 123) || k == 8 || k == 32 || k == 92 || k == 35 || (k >= 44 && k <= 57));
  }

  onlyForNumbers(event: any) {
    var keys = { 'escape': 27, 'backspace': 8, 'tab': 9, 'enter': 13, '0': 48, '1': 49, '2': 50, '3': 51, '4': 52, '5': 53, '6': 54, '7': 55, '8': 56, '9': 57 };
    for (var index in keys) {
      if (!keys.hasOwnProperty(index))
        continue;
      if (event.charCode == keys[index] || event.keyCode == keys[index]) {
        return; //default event
      }
    }
    event.preventDefault();
  };

  phoneNumbers(event: any) {
    var keys = { 'escape': 27, 'backspace': 8, 'tab': 9, 'enter': 13, '-': 45, '0': 48, '1': 49, '2': 50, '3': 51, '4': 52, '5': 53, '6': 54, '7': 55, '8': 56, '9': 57 };
    for (var index in keys) {
      if (!keys.hasOwnProperty(index))
        continue;
      if (event.charCode == keys[index] || event.keyCode == keys[index]) {
        return; //default event
      }
    }
    event.preventDefault();
  };

  setfirstcontactzipLength(event: any) {
    const removeFrom = this.relationshipForm.get('contactDetailsOne') as FormArray;
    if (event.value == "India") {
      this.firstcontactzipLength = 6
      removeFrom['controls']['PostalCode'].setValidators([Validators.required,Validators.minLength(6), Validators.maxLength(6),Validators.pattern('^[0-9]*$')]);
      removeFrom['controls']['PostalCode'].updateValueAndValidity();
    } 
    else {
      this.firstcontactzipLength=5
      removeFrom['controls']['PostalCode'].setValidators([Validators.required,Validators.minLength(5), Validators.maxLength(5),Validators.pattern('^[0-9]*$')]);
      removeFrom['controls']['PostalCode'].updateValueAndValidity();
    }
  }

  setsecondcontactzipLength(event: any) {
    const removeFrom = this.relationshipForm.get('contactDetailsTwo') as FormArray;
    if (event.value == "India") {
      this.secondcontactzipLength = 6
      removeFrom['controls']['PostalCode'].setValidators([Validators.required,Validators.minLength(6), Validators.maxLength(6),Validators.pattern('^[0-9]*$')]);
      removeFrom['controls']['PostalCode'].updateValueAndValidity();
    } 
    else {
      this.secondcontactzipLength=5
      removeFrom['controls']['PostalCode'].setValidators([Validators.required,Validators.minLength(5), Validators.maxLength(5),Validators.pattern('^[0-9]*$')]);
      removeFrom['controls']['PostalCode'].updateValueAndValidity();
    }
  }

  

  openDialog(Heading,Message,index): void {
    const dialogRef = this.dialog.open(CommonDialogComponent, {
      disableClose: true,
      hasBackdrop: true,
      width: '300px',
      data: {heading: Heading, message: Message}
    });

    dialogRef.afterClosed().subscribe(result => {
       this.dialogResponse = result;
       if(this.dialogResponse==true){
        const remove = this.relationshipForm.get('RelationsInfo') as FormArray;
        remove.removeAt(index);
        this.added = false;
        this.table.renderRows();
      }
    });
  }


  removemyValidators(formName){
    const removeFrom = this.relationshipForm.get(formName) as FormArray;
    removeFrom['controls']['AddressLine1'].setValidators([Validators.maxLength(200)]);
    removeFrom['controls']['AddressLine1'].updateValueAndValidity();
    removeFrom['controls']['City'].setValidators([]);
    removeFrom['controls']['City'].updateValueAndValidity();
    removeFrom['controls']['ContactName'].setValidators([]);
    removeFrom['controls']['ContactName'].updateValueAndValidity();
    removeFrom['controls']['Country'].setValidators([]);
    removeFrom['controls']['Country'].updateValueAndValidity();
    removeFrom['controls']['EmployeeId'].setValidators([]);
    removeFrom['controls']['EmployeeId'].updateValueAndValidity();
    removeFrom['controls']['EmployeeId'].setValidators([]);
    removeFrom['controls']['EmployeeId'].updateValueAndValidity();
    removeFrom['controls']['MobileNo'].setValidators([Validators.minLength(10), Validators.maxLength(10)]);
    removeFrom['controls']['MobileNo'].updateValueAndValidity();
    removeFrom['controls']['PostalCode'].setValidators([Validators.pattern('^[0-9]*$')]);
    removeFrom['controls']['PostalCode'].updateValueAndValidity();
    removeFrom['controls']['Relationship'].setValidators([]);
    removeFrom['controls']['Relationship'].updateValueAndValidity();
    removeFrom['controls']['State'].setValidators([]);
    removeFrom['controls']['State'].updateValueAndValidity();
  }
  DynamicValidators(formName){
    const addvalidation = this.relationshipForm.get(formName) as FormArray;
    addvalidation['controls']['AddressLine1'].setValidators([Validators.required,Validators.maxLength(200)]);
    addvalidation['controls']['AddressLine1'].updateValueAndValidity();
    addvalidation['controls']['City'].setValidators([Validators.required]);
    addvalidation['controls']['City'].updateValueAndValidity();
    addvalidation['controls']['ContactName'].setValidators([Validators.required]);
    addvalidation['controls']['ContactName'].updateValueAndValidity();
    addvalidation['controls']['Country'].setValidators([Validators.required]);
    addvalidation['controls']['Country'].updateValueAndValidity();
    addvalidation['controls']['EmployeeId'].setValue(this.currentempID);
    addvalidation['controls']['EmployeeId'].setValidators([Validators.required]);
    addvalidation['controls']['EmployeeId'].updateValueAndValidity();
    addvalidation['controls']['MobileNo'].setValidators([Validators.required,Validators.minLength(10), Validators.maxLength(10)]);
    addvalidation['controls']['MobileNo'].updateValueAndValidity();
    addvalidation['controls']['PostalCode'].setValidators([Validators.required,Validators.pattern('^[0-9]*$')]);
    addvalidation['controls']['PostalCode'].updateValueAndValidity();
    addvalidation['controls']['Relationship'].setValidators([Validators.required]);
    addvalidation['controls']['Relationship'].updateValueAndValidity();
    addvalidation['controls']['State'].setValidators([Validators.required]);
    addvalidation['controls']['State'].updateValueAndValidity();
  }
}

