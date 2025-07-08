import { Component, OnInit, ViewChild, EventEmitter, Output, Input } from '@angular/core';
import { Associate, ContactDetails } from '../models/associate.model';
import { GenericType } from '../../../models/dropdowntype.model';
import { ActivatedRoute, Router } from '../../../../../node_modules/@angular/router';
import { MasterDataService } from '../../../services/masterdata.service';
import { PersonalService } from '../services/personal.service';
import * as  moment from 'moment';
import { CommonService } from '../../../services/common.service';
import { Grade } from '../../admin/models/grade.model';
import { MessageService, SelectItem } from 'primeng/api';
import { PersonalDetails } from '../../shared/utility/enums';
import { KraRoleData } from '../../../models/kraRoleData.model';
import { RoleData, DepartmentDetails } from 'src/app/models/role.model';
import { RoleService } from '../../admin/services/role.service';
import { Designation } from "../../admin/models/designation.model";

@Component({
  selector: 'app-personal-details',
  templateUrl: './personal-details.component.html',
  styleUrls: ['./personal-details.component.scss'],
  providers: [CommonService, MessageService,]
})
export class PersonalDetailsComponent implements OnInit {
  id: number;
  @Input() type: string = 'new';
  currentempID: number;
  _Associate: Associate;
  _Contacts: ContactDetails;
  _CurrentContacts: ContactDetails;
  _PermanentContacts: ContactDetails;
  managers: any[];
  ddlCountries: any[];
  ddlBloodGrp: any[];
  ddlBGVStatus: any[];
  ddlGender: any[];
  ddlMaritalStatus: any[];
  ddlDepartmentDetails: SelectItem[] = [];
  ddlGradesDetails: any[];
  ddlHRAdvisors: any[];
  ddlEmpTypes: any[];
  ddltechnologies: any[];
  valueKey: string = "Country";
  valueKey1: string = "BloodGroup";
  valueKey2: string = "BGVStatus";
  valueKey3: string = "Gender";
  valueKey4: string = "MaritalStatus";
  isRequired: boolean = false;
  isDisabled: boolean = false;
  todisableKRA: boolean = false;
  public kraRoleList: SelectItem[] = [];
  lastDate: Date;
  birthDate: Date;
  filteredDesignationIds: GenericType[] = [];
  private componentName: string;
  designation: Designation;
  zipLength: number = 6;
  roleNames: SelectItem[] = [];
  deptId: number;

  buttonType: string;
  @Output() onAssociateSave = new EventEmitter();
  @ViewChild("pANNumberInput") pANNumberInputVariable: any;
  @ViewChild("passportNumberInput") passportNumberVariable: any;
  @ViewChild("pfNumberInput") pfNumberVariable: any;
  pattern: string;
  constructor(
    private messageService: MessageService,
    private _service: PersonalService,
    private _commonService: CommonService,
    private actRoute: ActivatedRoute,
    private masterDataService: MasterDataService,
    private router: Router,
    private rolesServiceObj: RoleService
  ) {
    this._Associate = new Associate();
    this.pattern = "^[0-9]+(.[0-9]{1,2})?$";
  }
  ngOnInit() {
    this.rolesServiceObj.selectedDepartment.subscribe(data => {
      this._Associate.DepartmentId = data;
      //this.deptId =  this._Associate.DepartmentId; i have commented
    });

    this.getDepartmentList();
    this.actRoute.params.subscribe(params => { this.id = params["id"]; });
    this.actRoute.params.subscribe(params => { this.type = params["type"]; });

    this.currentempID = this.id;
    this._Contacts = new ContactDetails();
    this.kraRoleList = [];
    this.kraRoleList.push({ label: "", value: null });
    this.getKraRoles();
    this.GetHRAdvisors();
    this.GetEmpTypes();
    this.getTechnologies();
    this.getCountries(this.valueKey);
    this.getBloodGroup(this.valueKey1);
    this.getBGVStatus(this.valueKey2);
    this.getGender(this.valueKey3);
    this.getMaritalStatus(this.valueKey4);
    this.GetManagersAndLeads();
    this.getDates();
    if (this.type == "new") {
      this.GetPersonalDetails();
    }
    else {
      this.GetEmployeePersonalDetails();
    }
  }

  getDepartmentList(): void {
    this.rolesServiceObj.getDepartmentList().subscribe((res: DepartmentDetails[]) => {
      this.ddlDepartmentDetails.push({ label: 'Select Department', value: -1 });
      res.forEach(element => {
        this.ddlDepartmentDetails.push({ label: element.Description, value: element.DepartmentId });
      });
    });
  }

  private getKraRoles() {
    this.masterDataService.GetKraRoles().subscribe(
      (res: KraRoleData[]) => {
        this.kraRoleList = [];
        this._Associate.KRARoleId = null;
        this.kraRoleList.push({ label: "Select KRA Role", value: null });
        res.forEach((element: KraRoleData) => {
          this.kraRoleList.push({
            label: element.KRARoleName,
            value: element.KRARoleID
          });
        });
      },

      (error: any) => {
        this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Failed to get KRA Roles list.' });
      }
    );
  }

  onlyNumbers(event: any) {
    this._commonService.onlyNumbers(event);
  }


  getBloodGroup(valueKey1: string) {
    this._commonService.GetBusinessValues(valueKey1).subscribe((res: any) => {
      this.ddlBloodGrp = res;
    });
  }

  getBGVStatus(valueKey2: string) {
    this._commonService.GetBusinessValues(valueKey2).subscribe((res: any) => {
      this.ddlBGVStatus = res;
    });
  }

  getGender(valueKey3: string) {
    this._commonService.GetBusinessValues(valueKey3).subscribe((res: any) => {
      this.ddlGender = res;
    });
  }

  getMaritalStatus(valueKey4: string) {
    this._commonService.GetBusinessValues(valueKey4).subscribe((res: any) => {
      this.ddlMaritalStatus = res;
    });
  }



  filteredDesignation(event: any): void {
    let suggestionString = event.query;
    this.masterDataService.GetDesignationByString(suggestionString).subscribe(
      (desginationResponse: GenericType[]) => {
        this.filteredDesignationIds = [];
        this.filteredDesignationIds = desginationResponse;
      },
      (error) => {
        if (error.error != undefined && error.error != "")
          this._commonService
            .LogError(this.componentName, error.error)
            .subscribe((data: any) => { });
      }
    );
  }

  getGradeByDesignation(designation: Designation) {
    if (this.designation && this.designation.DesignationId) {
      this.masterDataService.getGradeByDesignation(designation.DesignationId).subscribe(
        (gradeResponse: Grade) => {
          this._Associate.GradeName = gradeResponse.GradeName;
          this._Associate.GradeId = gradeResponse.GradeId;
        },
        (error: any) => {
          if (error.error != undefined && error.error != "")
            this._commonService
              .LogError(this.componentName, error.error)
              .subscribe((data: any) => { });
        }
      );
    }
    else {
      this._Associate.GradeName = "";
      this._Associate.GradeId = null;
    }
  }

  GetEmployeePersonalDetails(): void {
    this._service.GetEmployeePersonalDetails(this.id)
      .subscribe((res: any) => {
        if (res.DateofBirth) res.DateofBirth = moment(res.DateofBirth).format("YYYY-MM-DD");
        if (res.JoinDate) res.JoinDate = moment(res.JoinDate).format("YYYY-MM-DD");
        if (res.BgvinitiatedDate)
          res.BgvinitiatedDate = moment(res.BgvinitiatedDate).format("YYYY-MM-DD");
        if (res.BgvcompletionDate)
          res.BgvcompletionDate = moment(res.BgvcompletionDate).format("YYYY-MM-DD");
        if (res.EmploymentStartDate) res.EmploymentStartDate = moment(res.EmploymentStartDate).format("YYYY-MM-DD");

        this._Associate = res;
        //this._Associate.Designation = res.designation;
        if (this._Associate.Designation && this._Associate.DesignationId) {
          this.designation = new Designation();
          this.designation.DesignationId = this._Associate.DesignationId;
          this.designation.DesignationName = this._Associate.Designation;
        }

        //Assign dates
        this._Associate.Birthdate = res.DateofBirth;
        this._Associate.DateofJoining = res.JoinDate;
        this._Associate.BgvStartDate = res.BgvinitiatedDate;
        this._Associate.BgvCompletionDate = res.BgvcompletionDate;
        this._Associate.StartDateofEmployment = res.EmploymentStartDate;

        this.disableBGVDates(this._Associate.Bgvstatus);
        this.disableKRA(this._Associate.Bgvstatus);

        if (this._Associate.DepartmentId == 1) this.isRequired = true;

        if (
          this._Associate.TechnologyId == null ||
          this._Associate.TechnologyId == ""
        ) {
          this._Associate.TechnologyId = "";
        }

        if (this._Associate.contactDetails != null && this._Associate.contactDetails.length != 0) {
          this.type = "edit";
          for (var i = 0; i < this._Associate.contactDetails.length; i++) {
            if (this._Associate.contactDetails[i].addressType == "CurrentAddress") {
              this._Contacts.currentAddCity = this._Associate.contactDetails[i].currentAddCity;
              this._Contacts.currentAddCountry = this._Associate.contactDetails[i].currentAddCountry;
              this._Contacts.currentAddress1 = this._Associate.contactDetails[i].currentAddress1;
              this._Contacts.currentAddress2 = this._Associate.contactDetails[i].currentAddress2;
              this._Contacts.currentAddState = this._Associate.contactDetails[i].currentAddState;
              this._Contacts.currentAddZip = this._Associate.contactDetails[i].currentAddZip;
            }
            if (this._Associate.contactDetails[i].addressType == "PermanentAddress") {
              this._Contacts.permanentAddCity = this._Associate.contactDetails[i].permanentAddCity;
              this._Contacts.permanentAddCountry = this._Associate.contactDetails[i].permanentAddCountry;
              this._Contacts.permanentAddress1 = this._Associate.contactDetails[i].permanentAddress1;
              this._Contacts.permanentAddress2 = this._Associate.contactDetails[i].permanentAddress2;
              this._Contacts.permanentAddState = this._Associate.contactDetails[i].permanentAddState;
              this._Contacts.permanentAddZip = this._Associate.contactDetails[i].permanentAddZip;
            }
          }
          if (this._Contacts.currentAddCity != null ||
            this._Contacts.currentAddCountry != null ||
            this._Contacts.currentAddress1 != null ||
            this._Contacts.currentAddress2 != null ||
            this._Contacts.currentAddState != null ||
            this._Contacts.currentAddZip != null) {
            if (this._Contacts.currentAddCity == this._Contacts.permanentAddCity
              && this._Contacts.currentAddCountry == this._Contacts.permanentAddCountry
              && this._Contacts.currentAddress1 == this._Contacts.permanentAddress1
              && this._Contacts.currentAddress2 == this._Contacts.permanentAddress2
              && this._Contacts.currentAddState == this._Contacts.permanentAddState
              && this._Contacts.currentAddZip == this._Contacts.permanentAddZip) {
              this._Contacts.address = true;
            }
          }
        }
      });
  }

  GetPersonalDetails(): void {
    this._service.GetPersonalDetails(this.id).subscribe((res: Associate) => {
      if (res.DateofJoining)
        res.DateofJoining = moment(res.DateofJoining).format("YYYY-MM-DD");
      this._Associate = res;
      this._Associate.DateofJoining = res.DateofJoining;
      this._Associate.ReportingManager = res.ReportingManager;
      this._Associate.TechnologyId = res.TechnologyID;
      this._Associate.Hradvisor = res.HRAdvisorName;
      if (this._Associate.Designation && this._Associate.DesignationId) {
        this.designation = new Designation();
        this.designation.DesignationId = this._Associate.DesignationId;
        this.designation.DesignationName = this._Associate.Designation;
      }
      if (this._Associate.DepartmentId == 1) this.isRequired = true;

      if (
        this._Associate.TechnologyId == null ||
        this._Associate.TechnologyId == ""
      ) {
        this._Associate.TechnologyId = "";
      }
      this.ddlEmpTypes.forEach(empType =>
        {
          if(empType.EmpType == res.EmploymentType)
          this._Associate.EmployeeTypeId = empType.EmployeeTypeId;
        });
    });
    
  }
  GetManagersAndLeads() {
    this.masterDataService
      .GetManagersAndCompetencyLeads()
      .subscribe((res: GenericType[]) => {
        this.managers = res;
      });
  }



  GetHRAdvisors() {
    this._service.GetHRAdvisors().subscribe((res: any) => {
      this.ddlHRAdvisors = res;
    });
  }

  GetEmpTypes() {
    this._service.GetEmpTypes().subscribe((res: any) => {
      this.ddlEmpTypes = res;
    });
  }

  getTechnologies() {
    this._service.GetTechnologies().subscribe((res: any) => {
      this.ddltechnologies = res;
    });
  }


  getCountries(valueKey: string) {
    this._commonService.GetBusinessValues(valueKey).subscribe((res: any) => {
      this.ddlCountries = res;
    });
  }



  onSave() {
    this.buttonType = "Save";
  }

  onUpdate() {
    this.buttonType = "Update";
  }

  onSubmit() {
    if (this.designation && this.designation.DesignationId) {
      this._Associate.DesignationId = this.designation.DesignationId;
    } else {
      this._Associate.DesignationId = 0;
      this._Associate.Gender = "";
    }
    if (this.buttonType == "Save") {
      this.onSavePersonalDetails();
    } else if (this.buttonType == "Update") {
      this.onUpdatePersonalDetails();
    }
  }

  validateInputData(): boolean {
    var today: any = new Date();
    let mm = today.getMonth() + 1;
    let dd = today.getDate();
    if (dd < 10) {
      dd = '0' + dd;
    }

    if (mm < 10) {
      mm = '0' + mm;
    }
    today = today.getFullYear() + "-" + (mm) + "-" + dd;

    if (this._Associate.Birthdate == "" || this._Associate.Birthdate == undefined) {
      this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: 'Please enter Date of birth' });
      return false;
    }

    if (moment(this._Associate.DateofBirth).isSameOrAfter(new Date())) {
      this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: 'Date of birth should be less than today' });
      return false;
    }

    if (this._Associate.AadharNumber != "" && this._Associate.AadharNumber != undefined) {
      if (this._Associate.AadharNumber.includes("000000000000")) {
        this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: 'Please enter valid Aadhar Number' });
        return false;
      }
    }

    if (this._Associate.DateofJoining == "" || this._Associate.DateofJoining == undefined) {
      this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: 'Please enter joining date' });
      return false;
    }

    // if (this._Associate.KRARoleId == 0 || this._Associate.KRARoleId == undefined) {
    //   this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: 'Please select KRA Role' });
    //   return false;
    // }

    if (Date.parse(this._Associate.Birthdate) > Date.parse(this._Associate.DateofJoining)) {
      this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: "employee's birth date should not greater than joining date" });
      return false;
    }

    if (this._Associate.StartDateofEmployment == "" || this._Associate.StartDateofEmployment == undefined) {
      this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: 'Please enter employment start date' });

      return;
    }

    if (this._Associate.EmploymentStartDate > today.toString()) {
      this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: "employement start date should not greater than today's date " });
      return false;
    }

    if (Date.parse(this._Associate.StartDateofEmployment) < Date.parse(this._Associate.Birthdate)) {
      this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: "employement start date should be greater than birth date" });
      return false;
    }

    if (Date.parse(this._Associate.StartDateofEmployment) > Date.parse(this._Associate.DateofJoining)) {
      this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: "employement start date should not greater than joining date" });
      return false;
    }

    if (this._Associate.PassportDateValidUpto != undefined && this._Associate.PassportDateValidUpto != "") {
      if (!this.IsValidDate(this._Associate.DateofBirth, Date.parse(this._Associate.PassportDateValidUpto))) {
        this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: "Passport Valid Date should be greater than or equal to Birth Date" });
        return false;
      }
    }

    if (this._Associate.Pfnumber != "" && this._Associate.Pfnumber != undefined) {
      let i = 0, j = 0;
      for (i = 0; i < this._Associate.Pfnumber.length; i++) {
        if (this._Associate.Pfnumber[i].includes("0")) {
          j++;
        }
      }
      if (i == j) {
        this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: "Please enter valid PF Number" });
        return false;
      }
    }

    if (this._Associate.BgvStartDate != "" && this._Associate.BgvStartDate != undefined) {
      if (!this.IsValidDate(this._Associate.DateofJoining, this._Associate.BgvStartDate)) {
        this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: "BGV Start Date should be greater than joining Date" });
        return false;
      }
    }

    if (this._Associate.Bgvstatus == "Verified" && this._Associate.Bgvstatus != undefined) {

      if (this._Associate.BgvStartDate == "" || this._Associate.BgvStartDate == undefined ||
        this._Associate.BgvCompletionDate == "" || this._Associate.BgvCompletionDate == undefined) {

        this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: "Please enter BGV Start Date and BGV Completed Date" });
        return false;
      }
    }

    if (this._Associate.BgvStartDate != "" && this._Associate.BgvStartDate != undefined &&
      this._Associate.BgvCompletionDate != "" && this._Associate.BgvCompletionDate != undefined) {

      if (!this.IsValidDate(this._Associate.BgvCompletionDate, this._Associate.BgvCompletionDate)) {
        this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: "BGV Completed Date should be greater than BGV Start Date" });
        return false;
      }
    }

    if (this._Associate.DesignationId == 0 || this._Associate.DesignationId == null) {
      this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: "Please select Designation" });
      return false;
    }

    return true;
  }

  onSavePersonalDetails() {
    if (this._Contacts.address == true) {
      let isValid = this.validateAddresses(this._Contacts.address);
      if (isValid == false) {
        this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: 'Please enter current address details' });
        return false;
      }
    }

    this._Associate.contacts = this._Contacts;
    this._Associate.JoinDate = new Date(this._Associate.DateofJoining);
    this._Associate.DateofBirth = new Date(this._Associate.DateofBirth);
    this._Associate.BgvinitiatedDate = new Date(this._Associate.BgvStartDate);
    this._Associate.BgvcompletionDate = new Date(this._Associate.BgvCompletionDate);
    this._Associate.EmploymentStartDate = new Date(this._Associate.StartDateofEmployment);

    let validData = this.validateInputData();

    if (this.selectedDate(this._Associate.DateofBirth) == false) return;

    if (validData) {
      this._service.SavePersonalDetails(this._Associate).subscribe(
        (data: any) => {
          if (data.EmployeeId > 0) {
            this.type = "edit";
            this.onAssociateSave.emit(this.type);
            this.currentempID = data.EmployeeId;
            this.messageService.add({ severity: 'success', summary: 'Success Message', detail: "Personal details saved successfully" });

            setTimeout(() => {
              this.router.navigate([
                "/associates/prospectivetoassociate/edit/" +
                data.EmployeeId +
                "/" +
                "profile"
              ]);
            }, 1000);

          }
          else if (data.length > 0) {
            let errorMessage = this.validatePersonalDetails(data);
            this.messageService.add({ severity: 'error', summary: 'Error Message', detail: errorMessage + " already exists" });

          }
        },
        error => {
          if (error.error != undefined && error.error != "")
            this.messageService.add({ severity: 'error', summary: 'Error Message', detail: error.error });
          else
            this.messageService.add({ severity: 'error', summary: 'Error Message', detail: "Failed to save personal details" });
        });
    }
  }

  onUpdatePersonalDetails() {
    this._Associate.EmployeeId = this.currentempID;
    if (this._Contacts.address == true) {
      let isValid = this.validateAddresses(this._Contacts.address);
      if (isValid == false) {
        this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: 'Please enter current address details' });
        return false;
      }
    }
    this._Associate.contacts = this._Contacts;
    this._Associate.JoinDate = new Date(this._Associate.DateofJoining);
    this._Associate.DateofBirth = new Date(this._Associate.Birthdate);
    this._Associate.BgvinitiatedDate = new Date(this._Associate.BgvStartDate);
    this._Associate.BgvcompletionDate = new Date(this._Associate.BgvCompletionDate);
    this._Associate.EmploymentStartDate = new Date(this._Associate.StartDateofEmployment);

    if (this.selectedDate(this._Associate.DateofBirth) == false) return;

    let validData = this.validateInputData();

    if (validData) {
      this._service.UpdatePersonalDetails(this._Associate).subscribe(
        (data: any) => {
          if (data.IsSuccessful == true) {
            this.type = "edit";
            this.messageService.add({ severity: 'success', summary: 'Success Message', detail: 'Personal details updated successfully' });

          }
          else if (data.length > 0) {
            let errorMessage = this.validatePersonalDetails(data);
            this.messageService.add({ severity: 'error', summary: 'Error Message', detail: errorMessage + " already exists" });
          }
        },
        (error) => {
          if (error != undefined && error != "")
            this.messageService.add({ severity: 'error', summary: 'Error Message', detail: error.error });
          else
            this.messageService.add({ severity: 'error', summary: 'Error Message', detail: 'Failed to update personal details' });

        }
      );
    }
  }

  private validatePersonalDetails(resultData: any): string {
    if (resultData.length > 0) {
      let errorMessage: string = '';
      resultData.forEach((ele: any) => {
        if (ele.Id == PersonalDetails.PersonalEmailAddress)
          errorMessage != '' ? errorMessage += 'Personal Email Id' : errorMessage = 'Personal Email Id';
        if (ele.Id == PersonalDetails.MobileNo)
          errorMessage != '' ? errorMessage += ', Mobile Number' : errorMessage = 'Mobile Number';
        if (ele.Id == PersonalDetails.AadharNumber)
          errorMessage != '' ? errorMessage += ', Aadhar Number' : errorMessage = 'Aadhar Number';
        if (ele.Id == PersonalDetails.PANNumber)
          errorMessage != '' ? errorMessage += ', PAN Number' : errorMessage = 'PAN Number';
        if (ele.Id == PersonalDetails.UANNumber)
          errorMessage != '' ? errorMessage += ', UAN Number' : errorMessage = 'UAN Number';
        if (ele.Id == PersonalDetails.PFNumber)
          errorMessage != '' ? errorMessage += ', PF Number' : errorMessage = 'PF Number';
        if (ele.Id == PersonalDetails.PassportNumber)
          errorMessage != '' ? errorMessage += ', Passport Number' : errorMessage = 'Passport Number';
      });
      return errorMessage;
    }
  }

  private validateAddresses(isaddress: boolean): boolean {
    if (isaddress) {
      if ((this._Contacts.currentAddCity != null && this._Contacts.currentAddCity != "" && this._Contacts.currentAddCity.trim().length > 0)
        || (this._Contacts.currentAddCountry != null && this._Contacts.currentAddCountry != "")
        || (this._Contacts.currentAddress1 != null && this._Contacts.currentAddress1 != "" && this._Contacts.currentAddress1.trim().length > 0)
        || (this._Contacts.currentAddress2 != null && this._Contacts.currentAddress2 != "" && this._Contacts.currentAddress2.trim().length > 0)
        || (this._Contacts.currentAddState != null && this._Contacts.currentAddState != "" && this._Contacts.currentAddState.trim().length > 0)
        || (this._Contacts.currentAddZip != null && this._Contacts.currentAddZip != "")) {

        this._Contacts.permanentAddCity = this._Contacts.currentAddCity;
        this._Contacts.permanentAddCountry = this._Contacts.currentAddCountry;
        this._Contacts.permanentAddress1 = this._Contacts.currentAddress1;
        this._Contacts.permanentAddress2 = this._Contacts.currentAddress2;
        this._Contacts.permanentAddState = this._Contacts.currentAddState;
        this._Contacts.permanentAddZip = this._Contacts.currentAddZip;
        return true;
      }
      else {
        this._Contacts.permanentAddCity = "";
        this._Contacts.permanentAddCountry = "";
        this._Contacts.permanentAddress1 = "";
        this._Contacts.permanentAddress2 = "";
        this._Contacts.permanentAddState = "";
        this._Contacts.permanentAddZip = "";
        this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: 'Please enter current address details' });

        return false;
      }
    }
  }




  phoneNumbers(event: any) {
    var keys = {
      escape: 27,
      backspace: 8,
      tab: 9,
      enter: 13,
      "-": 45,
      "0": 48,
      "1": 49,
      "2": 50,
      "3": 51,
      "4": 52,
      "5": 53,
      "6": 54,
      "7": 55,
      "8": 56,
      "9": 57
    };
    for (var index in keys) {
      if (!keys.hasOwnProperty(index)) continue;
      if (event.charCode == keys[index] || event.keyCode == keys[index]) {
        return; //default event
      }
    }
    event.preventDefault();
  }

  omit_special_char(event: any) {
    let k: any;
    k = event.charCode; //         k = event.keyCode;  (Both can be used)
    return (
      (k > 64 && k < 91) ||
      (k > 96 && k < 123) ||
      k == 8 ||
      k == 32 ||
      (k >= 48 && k <= 57)
    );
  }

  onlychar(event: any) {
    let k: any;
    k = event.charCode; //         k = event.keyCode;  (Both can be used)
    return (k > 64 && k < 91) || (k > 96 && k < 123) || k == 8 || k == 32;
  }

  forAddress(event: any) {
    let k: any;
    k = event.charCode; //         k = event.keyCode;  (Both can be used)
    return (
      (k > 64 && k < 91) ||
      (k > 96 && k < 123) ||
      k == 8 ||
      k == 32 ||
      k == 92 ||
      k == 35 ||
      (k >= 44 && k <= 57)
    );
  }

  forPfNumber(event: any) {
    let k: any;
    k = event.charCode; //         k = event.keyCode;  (Both can be used)
    return (
      (k > 64 && k < 91) ||
      (k > 96 && k < 123) ||
      (k > 47 && k < 58) ||
      k == 8 ||
      k == 47
    );
  }

  requiredTechnolgy(event: any) {
    var target = event.target.value;
    if (target == 1) {
      this.isRequired = true;
    } else {
      this.isRequired = false;
      this._Associate.TechnologyId = "";
    }
  }

  selectedDate(selectedDate): Boolean {
    var today: any = new Date();
    today = today.getFullYear();
    selectedDate = moment(selectedDate).toDate();
    var selDate = selectedDate.getFullYear();
    var diff = Math.round(Math.abs(selDate - today));
    if (diff >= 100 || diff < 18) {
      this.messageService.add({ severity: 'warn', summary: 'failed Message', detail: 'Please select a valid birth date' });
      return false;
    }
  }
  disableBGVDates(event: any) {
    var bgvstatus = event;
    this._Associate.Bgvstatus = bgvstatus;
    if (bgvstatus == "NotVerified") {
      this._Associate.BgvStartDate = "";
      this._Associate.BgvCompletionDate = "";
      this.isDisabled = true;
    } else {
      this.isDisabled = false;
    }
  }

  disableKRA(event: any) {
    if (event == "Verified")
      this.todisableKRA = true;
    else
      this.todisableKRA = false;
  }

  IsValidDate = function (fromDate: any, toDate: any) {
    if (Date.parse(fromDate) <= Date.parse(toDate)) return true;
    return false;
  };

  getDates() {
    var date = new Date(),
      y = date.getFullYear(),
      m = date.getMonth(),
      d = date.getDate();
    this.lastDate = new Date(y, m + 2, 0);
    this.birthDate = new Date(y, m, d - 1);
  }
  upperCase() {
    this._Associate.Pannumber = this.pANNumberInputVariable.nativeElement.value.toUpperCase();
    this._Associate.PassportNumber = this.passportNumberVariable.nativeElement.value.toUpperCase();
    this._Associate.Pfnumber = this.pfNumberVariable.nativeElement.value.toUpperCase();
  }

  setZipLength(event: any) {
    // this._Contacts.currentAddZip = null;
    if (event.target.value == "India")
      this.zipLength = 6;
    else this.zipLength = 5;
  }


  sameAddress(isaddress: boolean) {
    if (isaddress == true) {
      this.validateAddresses(isaddress);
    }
    else {
      this._Contacts.permanentAddCity = "";
      this._Contacts.permanentAddCountry = "";
      this._Contacts.permanentAddress1 = "";
      this._Contacts.permanentAddress2 = "";
      this._Contacts.permanentAddState = "";
      this._Contacts.permanentAddZip = "";
    }
  }

  onlyForNumbers(event: any) {
    var keys = {
      escape: 27,
      backspace: 8,
      tab: 9,
      enter: 13,
      "0": 48,
      "1": 49,
      "2": 50,
      "3": 51,
      "4": 52,
      "5": 53,
      "6": 54,
      "7": 55,
      "8": 56,
      "9": 57
    };
    for (var index in keys) {
      if (!keys.hasOwnProperty(index)) continue;
      if (event.charCode == keys[index] || event.keyCode == keys[index]) {
        return; //default event
      }
    }
    event.preventDefault();
  }

}
