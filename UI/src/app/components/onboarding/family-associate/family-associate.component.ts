import { Component, OnInit, ViewChild } from '@angular/core';
import { EmergencyContactDetail, Relation } from '../models/family.model';
import { Router, ActivatedRoute } from '@angular/router';
import { CommonService } from '../../../services/common.service';
import * as moment from 'moment';
import { MessageService } from 'primeng/api';
import { FamilyService } from '../services/family.service';
import { Associate } from 'src/app/components/onboarding/models/associate.model';
import { ConfirmationService } from 'primeng/api';

@Component({
  selector: 'app-family-associate',
  templateUrl: './family-associate.component.html',
  styleUrls: ['./family-associate.component.scss'],
  providers: [MessageService, ConfirmationService, CommonService]
})
export class FamilyAssociateComponent implements OnInit {
  id: number;
  currentempID: number;
  isPrimary: boolean;
  _Associate = new Associate();
  relationsInfo: Array<Relation>;
  // contactDetailsOne = new EmergencyContactDetail();
  // contactDetailsTwo = new EmergencyContactDetail();
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
  //isNew:boolean=true;
  @ViewChild('familyDialog') familyDialog: any;
  submitAttempt: boolean = false;
  @ViewChild('messageToaster') messageToaster: any;


  constructor(private messageService: MessageService, private _commonService: CommonService, private confirmationService: ConfirmationService, private _service: FamilyService, private router: Router, private actRoute: ActivatedRoute) { }

  ngOnInit() {
    this.actRoute.params.subscribe(params => { this.id = params['id']; });
    // this.id = 214;
    this.currentempID = this.id;
    this._Associate.contactDetailsOne = new EmergencyContactDetail();
    this._Associate.contactDetailsTwo = new EmergencyContactDetail();   

    this.getRelations(this.valueKey);
    this.getCountries(this.valueKey1);
    this.getFamilydetails(this.currentempID);
    this.getDates(); 
    
  }

  //   OpenConfirmationDialog() {   // method to open dialog
  //   this.confirmationService.confirm({
  //     message: 'Do you want to clear ?',
  //     accept: () => {
  //       this.onClear();
  //     },
  //     reject: () => {

  //     }
  //   });
  // }
  onClear() {
    this.formSubmitted = false;
    this._Associate.RelationsInfo = new Array<Relation>();

    this._Associate.RelationsInfo.push({
      BirthDate: null,
      Name: "",
      Occupation: "",
      RelationShip: ""
    });
    this._Associate.contactDetailsOne = new EmergencyContactDetail();
    this._Associate.contactDetailsTwo = new EmergencyContactDetail();
    this._Associate.contactDetailsOne.IsPrimary = false;
    this._Associate.contactDetailsTwo.IsPrimary = false;
  }

  getRelations(valueKey: string) {
    this._commonService.GetBusinessValues(valueKey).subscribe((res: any) => {
      this.ddlRelations = res
    });
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
      this.messageService.add({
        severity: 'warn',
        summary: 'Warn Message',
        detail: 'Please select a valid date'
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
      if (this._Associate.contactDetailsOne.ContactName == undefined || this._Associate.contactDetailsOne.ContactName == "" ||
        this._Associate.contactDetailsOne.Relationship == undefined || this._Associate.contactDetailsOne.Relationship == "" ||
        this._Associate.contactDetailsOne.AddressLine1 == undefined || this._Associate.contactDetailsOne.AddressLine1 == "" ||
        this._Associate.contactDetailsOne.City == undefined || this._Associate.contactDetailsOne.City == "" ||
        this._Associate.contactDetailsOne.State == undefined || this._Associate.contactDetailsOne.State == "" ||
        this._Associate.contactDetailsOne.PostalCode == undefined || this._Associate.contactDetailsOne.PostalCode == "" ||
        this._Associate.contactDetailsOne.Country == undefined || this._Associate.contactDetailsOne.Country == "" ||
        this._Associate.contactDetailsOne.MobileNo == undefined || this._Associate.contactDetailsOne.MobileNo == "")

        this.submitAttempt = true;

    }
    else if (this._Associate.contactDetailsTwo.IsPrimary == true) {
      if (this._Associate.contactDetailsTwo.ContactName == undefined || this._Associate.contactDetailsTwo.ContactName == "" ||
        this._Associate.contactDetailsTwo.Relationship == undefined || this._Associate.contactDetailsTwo.AddressLine1 == undefined ||
        this._Associate.contactDetailsTwo.AddressLine1 == "" || this._Associate.contactDetailsTwo.City == undefined ||
        this._Associate.contactDetailsTwo.City == "" || this._Associate.contactDetailsTwo.State == undefined ||
        this._Associate.contactDetailsTwo.State == "" || this._Associate.contactDetailsTwo.PostalCode == undefined ||
        this._Associate.contactDetailsTwo.Country == undefined || this._Associate.contactDetailsTwo.MobileNo == undefined ||
        this._Associate.contactDetailsTwo.ContactName == "" || this._Associate.contactDetailsTwo.Relationship == "" ||
        this._Associate.contactDetailsTwo.PostalCode == null || this._Associate.contactDetailsTwo.Country == "" ||
        this._Associate.contactDetailsTwo.MobileNo == "")

        this.submitAttempt = true;
    }
    if (this._Associate.contactDetailsOne.TelePhoneNo && this._Associate.contactDetailsOne.TelePhoneNo.length < 10)
      this.submitAttempt = true;
    if (this._Associate.contactDetailsOne.MobileNo && this._Associate.contactDetailsOne.MobileNo.length < 10)
      this.submitAttempt = true;
    if (this._Associate.contactDetailsTwo.TelePhoneNo && this._Associate.contactDetailsTwo.TelePhoneNo.length < 10)
      this.submitAttempt = true;
    if (this._Associate.contactDetailsTwo.MobileNo && this._Associate.contactDetailsTwo.MobileNo.length < 10)
      this.submitAttempt = true;
    if (this._Associate.contactDetailsOne.PostalCode && this._Associate.contactDetailsOne.PostalCode.length != this.firstcontactzipLength)
      this.submitAttempt = true;
    if (this._Associate.contactDetailsTwo.PostalCode && this._Associate.contactDetailsTwo.PostalCode.length != this.secondcontactzipLength)
      this.submitAttempt = true;

  }

  getFamilydetails = function (empID: number) {
    this._service.GetFamilyDetails(this.id).subscribe((res: any) => {

      if (res.Item.RelationsInfo.length != 0 || res.Item.contactDetailsOne || res.Item.contactDetailsTwo) {
        this.type = "edit";
        //this.isNew=false;
      }
      for (var i = 0; i < res.Item.RelationsInfo.length; i++) {
        res.Item.RelationsInfo[i].BirthDate = res.Item.RelationsInfo[i].DateOfBirth.replace("T11:00:00+05:30", '')
      }
      this._Associate.RelationsInfo = res.Item.RelationsInfo;
      if (res.Item.contactDetailsOne != null && res.Item.contactDetailsOne != undefined) {
        this._Associate.contactDetailsOne = res.Item.contactDetailsOne;
        if (this._Associate.contactDetailsOne.Country == "India")
          this.firstcontactzipLength = 6;
        else this.firstcontactzipLength = 5;
      }
      if (res.Item.contactDetailsTwo != null && res.Item.contactDetailsTwo != undefined) {
        this._Associate.contactDetailsTwo = res.Item.contactDetailsTwo;
        if (this._Associate.contactDetailsTwo.Country == "India")
          this.secondcontactzipLength = 6;
        else this.secondcontactzipLength = 5;
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
    });
  }

  updateSelection(isprimaryname: string) {
    this.formSubmitted = false;
    if (isprimaryname == 'firstPrimary') {
      this._Associate.contactDetailsTwo.IsPrimary = false;
      this._Associate.contactDetailsOne.IsPrimary = true;
    }
    else if (isprimaryname == 'secondPrimary') {
      this._Associate.contactDetailsTwo.IsPrimary = true;
      this._Associate.contactDetailsOne.IsPrimary = false;
    }
  };

  onNewRelation(rShip: Relation) {
    this._Associate.RelationsInfo.push({
      Id: 0,
      BirthDate: null,
      Name: "",
      Occupation: "",
      RelationShip: ""
    });
  }

  onRelationChange(event: any, index: number) {
    let relationShip = event.target.value;
    let count = 0;
    if (relationShip) {

      let i, j: number;
      i = 0, j = 1;

      // do {
      //   let relationshipName = this._Associate.relationsInfo[i].relationship;
      //   if (relationshipName == "Mother" || relationshipName == "Father" || relationshipName == "Spouse" ||
      //     relationshipName == "Mother-in-Law" || relationshipName == "Father-in-Law") {
      //     j = i + 1;
      //     while (j < this._Associate.relationsInfo.length + 1) {
      //       if (this._Associate.relationsInfo[j].relationship &&
      //         (relationshipName == this._Associate.relationsInfo[j].relationship)) {
      //         event.target.value = "";
      //         this.messageService.add({ severity: 'warn', summary: 'Warn Message', detail: 'You cant select same relationship twice' });
      //         return;
      //       }
      //       j++;
      //     }
      //   }
      //   else i++;
      // } while (i < this._Associate.relationsInfo.length)
      
        if (relationShip == "Mother" || relationShip == "Father" || relationShip == "Spouse" ||
          relationShip == "Mother-in-Law" || relationShip == "Father-in-Law") {
          for (i = 0; i < this._Associate.RelationsInfo.length; i++) {
            if (i != index) {
              let relationshipName = this._Associate.RelationsInfo[i].RelationShip;
              if (relationShip == relationshipName) {
                event.target.value = "";
                this._Associate.RelationsInfo[index].RelationShip = "";
                this.messageService.add({ severity: 'warn', summary: 'Warn Message', detail: 'You cant select same relationship twice' });
                return;
              }
            }
          }
        }
    }
  }

  onSaveorUpdate(emp: Associate) {
    this.formSubmitted = true;
    for (var i = 0; i < this._Associate.RelationsInfo.length; i++) {
      if (this._Associate.RelationsInfo[i].Name == "" || this._Associate.RelationsInfo[i].Name.trim().length == 0 ||
        this._Associate.RelationsInfo[i].RelationShip == "" || this._Associate.RelationsInfo[i].RelationShip == null 
        || this._Associate.RelationsInfo[i].Occupation == "" || this._Associate.RelationsInfo[i].Occupation.trim().length == 0 
        || this._Associate.RelationsInfo[i].BirthDate == null) {
        this.messageService.add({
          severity: 'warn',
          summary: 'Warn Message',
          detail: 'Please enter relationship details'
        });
        return
      }
      else
        this._Associate.RelationsInfo[i].DateOfBirth = this._Associate.RelationsInfo[i].BirthDate;
    }
    this.checkValidations();
    this.isPrimary = false;
    if (this.submitAttempt == true) {
      this.messageService.add({
        severity: 'warn',
        summary: 'Warn Message',
        detail: 'Please complete contact details'
      });

      return
    }
    if (this.selectedDate(this._Associate.DateofBirth) == false)
      return;
    if (this._Associate.contactDetailsOne.EmailAddress) {
      var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
      var isValid = re.test(this._Associate.contactDetailsOne.EmailAddress);
      if (!isValid) {
        this.messageService.add({
          severity: 'warn',
          summary: 'Warn Message',
          detail: 'Please enter valid email format'
        });

        return
      }
    }
    if (this._Associate.contactDetailsTwo.EmailAddress) {
      var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
      var isValid = re.test(this._Associate.contactDetailsTwo.EmailAddress);

      if (!isValid) {
        this.messageService.add({
          severity: 'warn',
          summary: 'Warn Message',
          detail: 'Please enter valid email format'
        });

        return
      }
    }
    if (this._Associate.contactDetailsOne.IsPrimary == true) {
      this.isPrimary = true;
    }
    if (this._Associate.contactDetailsTwo.IsPrimary == true) {
      this.isPrimary = true;
    }
    if (this.isPrimary == false) {
      this.messageService.add({ severity: 'warn', summary: 'Warn Message', detail: 'Please select a primary contact' });
      return;
    }
    this._Associate.EmployeeId = this.currentempID;
    this._service.SaveFamilyDetails(this._Associate).subscribe((data) => {
      this.submitAttempt = false;
      this.messageService.add({
        severity: 'success',
        summary: 'Success Message',
        detail: 'Family details saved successfully'
      });

      this.onClear();
      this.getFamilydetails(this.currentempID);
    }, (error) => {
      this.messageService.add({
        severity: 'error',
        summary: 'Error Message',
        detail: 'Failed to save family details'
      });

    });

  }
  OpenConfirmationDialog(editMode: number) {   // method to open dialog
    this.confirmationService.confirm({
      message: 'Do you want to Delete ?',
      accept: () => {
        this.Delete();
      },
      reject: () => {

      }
    });
  }
  onDelete(selectedRelationship: any) {
    this.OpenConfirmationDialog(0);
    //  this.familyDialog.nativeElement.open();
    this.selectedFamilyDetails = selectedRelationship;
  }

  Delete() {
    this._Associate.RelationsInfo.splice(this._Associate.RelationsInfo.indexOf(this.selectedFamilyDetails), 1);
    //this.familyDialog.nativeElement.close();
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
    if (event.target.value == "India")
      this.firstcontactzipLength = 6;
    else this.firstcontactzipLength = 5;
  }

  setsecondcontactzipLength(event: any) {
    if (event.target.value == "India")
      this.secondcontactzipLength = 6;
    else this.secondcontactzipLength = 5;
  }

}