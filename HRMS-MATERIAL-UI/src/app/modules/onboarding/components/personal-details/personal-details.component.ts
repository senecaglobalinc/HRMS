import {
  Component,
  OnInit,
  ViewChild,
  EventEmitter,
  Output,
  Input,
} from "@angular/core";
import { MatSnackBar } from "@angular/material/snack-bar";
import { ActivatedRoute, Router } from "@angular/router";
import {
  FormBuilder,
  FormGroup,
  FormControl,
  Validators,
  FormArray,
} from "@angular/forms";
import * as moment from "moment";
import { Observable } from "rxjs";
import { map, startWith } from "rxjs/operators";

import { PersonalService } from "../../services/personal.service";
import { PersonalDetails } from "../../../master-layout/utility/enums";
import { themeconfig } from "../../../../../themeconfig";
import { Associate, ContactDetails } from "../../models/associate.model";
import { GenericType } from "../../../master-layout/models/dropdowntype.model";
import { Designation } from "../../../../modules/admin/models/designation.model";
import { CommonService } from "../../../../core/services/common.service";
import { MasterDataService } from "../../../master-layout/services/masterdata.service";
import { RoleService } from "../../../../modules/admin/services/role.service";
import { DepartmentDetails } from "../../../master-layout/models/role.model";
import { KraRoleData } from "../../../master-layout/models/kraRoleData.model";
import { Grade } from "../../../../modules/admin/models/grade.model";
import { AssignReportingManagerService } from "../../../project-life-cycle/services/assign-reporting-manager.service";
import { NgxSpinnerService } from "ngx-spinner";

@Component({
  selector: "app-personal-details",
  templateUrl: "./personal-details.component.html",
  styleUrls: ["./personal-details.component.scss"],
})
export class PersonalDetailsComponent implements OnInit {
  minDate: Date;
  maxDate: Date;
  themeConfigInput = themeconfig.formfieldappearances;
  personelDetailsForm: FormGroup;
  contactDetails: FormArray;
  allAssociatesList: any[] = [];
  allGradessList: any;
  RoleTypesByGradeList:any;
  isCheckedSameAddress: boolean;
  isProspectivetoAssociateEdit: boolean = false;
  isProspectivetoAssociateNew: boolean = false;
  isAssociateInformationEdit: boolean = false;
  AsoociateDataFormData: any;
  id: number;
  @Input() type: string = "new";
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
  ddlDepartmentDetails: any[] = [];
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
  public kraRoleList: any[] = [];
  lastDate: Date;
  birthDate: Date;
  filteredDesignationIds: Observable<any>;
  allDesignationsList: any[] = [];
  private componentName: string;
  designation: Designation;
  zipLengthone: number;
  zipLengthtwo: number;
  roleNames: any[] = [];
  deptId: number;
  isInEditMode: boolean;

  today = new Date();

  buttonType: string;
  @Output() onAssociateSave = new EventEmitter();
  @ViewChild("pANNumberInput") pANNumberInputVariable: any;
  @ViewChild("passportNumberInput") passportNumberVariable: any;
  @ViewChild("pfNumberInput") pfNumberVariable: any;
  pattern: string;
  maxJoiningDate: Date;
  minJoiningDate: Date;
  pageload: boolean=false;

  constructor(
    private fb: FormBuilder,
    private _service: PersonalService,
    private _commonService: CommonService,
    private actRoute: ActivatedRoute,
    private _snackBar: MatSnackBar,
    private masterDataService: MasterDataService,
    private router: Router,
    private rolesServiceObj: RoleService,
    private spinner: NgxSpinnerService
  ) {
    this._Associate = new Associate();
    this.pattern = "^[0-9]+(.[0-9]{1,2})?$";


    const currentYear = new Date().getFullYear();
    this.minDate = new Date(currentYear - 100, 0, 1);
    this.maxDate = new Date(currentYear - 18, 11, 31);


    this.today.setDate(this.today.getDate());


  }

  ngOnInit(): void {
    this.spinner.show()
    this.type == "edit"
      ? (this.isInEditMode = true)
      : (this.isInEditMode = false);
    this.createPersonelDetailsForm();

    this.rolesServiceObj.selectedDepartment.subscribe((data) => {
      this._Associate.DepartmentId = data;
      this.deptId = this._Associate.DepartmentId;
    });
    this.getDepartmentList();
    this.getAllGradesList();

    this.actRoute.params.subscribe((params) => {
      this.id = params["id"];
    });
    this.actRoute.params.subscribe((params) => {
      this.type = params["type"];
    });

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
    var date = new Date();
    this.minJoiningDate = new Date(date.getFullYear(), date.getMonth(), 1);
    this.maxJoiningDate = new Date(date.getFullYear(), date.getMonth() + 1, 0);
    this.getDates();


    this.getAllDesignationsList();

    this.filteredDesignationIds = this.personelDetailsForm.valueChanges.pipe(
      startWith(""),
      map((value) => this._filter(value))
    );
  }

  createPersonelDetailsForm() {
    this.personelDetailsForm = new FormGroup({
      ID: new FormControl(null),
      Id: new FormControl(null),
      EmpId: new FormControl(null),
      EmpCode: new FormControl(null),
      EmploymentType: new FormControl(null),
      Designation: new FormControl(null, [Validators.required]),
      TechnologyId: new FormControl(null),
      GradeName: new FormControl({ value: null, disabled: this.isInEditMode }, [
        Validators.required,
      ]),
      KRARoleId: new FormControl(null),
      contacts: this.createContactsForm(),
      contactDetails: this.fb.array([
        this.createCurrentAddress(),
        this.createPermanentAdd(),
      ]),
      RelationsInfo: new FormControl(null),
      contactDetailsOne: new FormControl(null),
      contactDetailsTwo: new FormControl(null),
      EmployeeId: new FormControl(null),
      EmployeeCode: new FormControl(
        { value: null, disabled: this.isInEditMode },
        [Validators.required, Validators.pattern('[ANC][0-9]*'), Validators.minLength(5), Validators.maxLength(5)]
      ),
      FirstName: new FormControl({ value: null, disabled: this.isInEditMode }, [
        Validators.required, Validators.pattern('^[a-zA-Z]+$')
      ]),
      MiddleName: new FormControl({ value: null, disabled: this.isInEditMode }, [Validators.pattern('^[a-zA-Z]+$')
      ]),
      //  MiddleName: new FormControl({ value: null, disabled: this.isInEditMode }),
      LastName: new FormControl({ value: null, disabled: this.isInEditMode }, [
        Validators.required, Validators.pattern('^[a-zA-Z]+$')
      ]),
      Photograph: new FormControl(null),
      AccessCardNo: new FormControl(null),
      Gender: new FormControl(null, [Validators.required]),
      GradeId: new FormControl(null),
      DesignationId: new FormControl(null),
      MaritalStatus: new FormControl(null),
      Qualification: new FormControl(null),
      TelephoneNo: new FormControl(null, [Validators.pattern('^(?:(?:\\+|0{0,2})91(\\s*[\\ -]\\s*)?|[0]?)?[789]\\d{9}|(\\d[ -]?){10}\\d$'), Validators.minLength(11)]),
      MobileNo: new FormControl(null, [Validators.required, Validators.pattern('[1-9]{1}[0-9]{9}')]),
      WorkEmailAddress: new FormControl(null),
      PersonalEmailAddress: new FormControl(null, [Validators.required, Validators.email, Validators.pattern(/^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/)]),
      DateofBirth: new FormControl(null),
      JoinDate: new FormControl(null),
      ConfirmationDate: new FormControl(null),
      RelievingDate: new FormControl(null),
      BloodGroup: new FormControl(null, [Validators.required]),
      Nationality: new FormControl(null, [Validators.pattern('^[a-zA-Z ]*$')]),
      Pannumber: new FormControl(null, [Validators.required,Validators.pattern('^[A-Z]{5}[0-9]{4}[A-Z]{1}$'), Validators.minLength(10)]),
      PassportNumber: new FormControl(null, [Validators.minLength(8), Validators.pattern('^[A-PR-WYa-pr-wy][1-9]\\d\\s?\\d{4}[1-9]$')]),
      PassportIssuingOffice: new FormControl(null, [Validators.pattern('^[a-zA-Z ]*$')]),
      PassportDateValidUpto: new FormControl(null),
      ReportingManager: new FormControl(null, [Validators.required]),
      ProgramManager: new FormControl(null),
      DepartmentId: new FormControl(null, [Validators.required]),
      DocumentsUploadFlag: new FormControl(null),
      CubicalNumber: new FormControl(null),
      AlternateMobileNo: new FormControl(null),
      StatusId: new FormControl(null),
      BgvinitiatedDate: new FormControl(null),
      BgvCompletionDate: new FormControl(null),
      BgvcompletionDate: new FormControl(null),
      BgvstatusId: new FormControl(null),
      Experience: new FormControl(null),
      CompetencyGroup: new FormControl(null),
      BgvtargetDate: new FormControl(null),
      EmployeeTypeId: new FormControl(null, [Validators.required]),
      UserId: new FormControl(null),
      ResignationDate: new FormControl(null),
      Bgvstatus: new FormControl(null),
      BgvStatus: new FormControl(null),
      Paid: new FormControl(null),
      Hradvisor: new FormControl(null, [Validators.required]),
      Uannumber: new FormControl(null, [Validators.minLength(12)]),
      AadharNumber: new FormControl(null, [Validators.minLength(12)]),
      Pfnumber: new FormControl(null, [Validators.minLength(22)]),
      Remarks: new FormControl(null),
      EmploymentStartDate: new FormControl(null),
      CareerBreak: new FormControl(null, [Validators.pattern('^[0-9]+$'), Validators.minLength(1)]),
      TotalExperience: new FormControl(null),
      ExperienceExcludingCareerBreak: new FormControl(null),
      CurrentUser: new FormControl(null),
      CreatedDate: new FormControl(null),
      ModifiedDate: new FormControl(null),
      SystemInfo: new FormControl(null),
      IsActive: new FormControl(null),
      CreatedBy: new FormControl(null),
      ModifiedBy: new FormControl(null),
      Birthdate: new FormControl(null, [Validators.required]),
      DateofJoining: new FormControl(null, [Validators.required]),
      BgvStartDate: new FormControl(null),
      StartDateofEmployment: new FormControl(null, [Validators.required]),
      RoleTypeId: new FormControl(null)
    });
  }

  clearInput(evt: any, fieldName): void {
    if (fieldName == 'BgvCompletionDate') {
      evt.stopPropagation();
      this.personelDetailsForm.get('BgvCompletionDate').reset();
    }
    if (fieldName == 'BgvStartDate') {
      evt.stopPropagation();
      this.personelDetailsForm.get('BgvStartDate').reset();
    }
    if (fieldName == 'DateofJoining') {
      evt.stopPropagation();
      this.personelDetailsForm.get('DateofJoining').reset();
    }
    if (fieldName == 'StartDateofEmployment') {
      evt.stopPropagation();
      this.personelDetailsForm.get('StartDateofEmployment').reset();
    }
    if (fieldName == 'PassportDateValidUpto') {
      evt.stopPropagation();
      this.personelDetailsForm.get('PassportDateValidUpto').reset();
    }
    if (fieldName == 'Birthdate') {
      evt.stopPropagation();
      this.personelDetailsForm.get('Birthdate').reset();
    }
  }
  createCurrentAddress() {
    return this.fb.group({
      ID: [],
      addressType: ["CurrentAddress"],
      currentAddCity: [],
      currentAddCountry: [],
      currentAddState: [],
      currentAddZip: [],
      currentAddress1: [],
      currentAddress2: [],
      permanentAddCity: [],
      permanentAddCountry: [],
      permanentAddState: [],
      permanentAddZip: [],
      permanentAddress1: [],
      permanentAddress2: [],
    });
  }

  createContactsForm() {
    return this.fb.group({
      currentAddCity: [],
      currentAddCountry: [],
      currentAddState: [],
      currentAddZip: [],
      currentAddress1: [],
      currentAddress2: [],
      permanentAddCity: [],
      permanentAddCountry: [],
      permanentAddState: [],
      permanentAddZip: [],
      permanentAddress1: [],
      permanentAddress2: [],
      address: this.isCheckedSameAddress,
    });
  }

  createPermanentAdd() {
    return this.fb.group({
      ID: [],
      addressType: ["PermanentAddress"],
      currentAddCity: [],
      currentAddCountry: [],
      currentAddState: [],
      currentAddZip: [],
      currentAddress1: [],
      currentAddress2: [],
      permanentAddCity: [],
      permanentAddCountry: [],
      permanentAddState: [],
      permanentAddZip: [],
      permanentAddress1: [],
      permanentAddress2: [],
    });
  }

  getDepartmentList(): void {
    this.rolesServiceObj
      .getDepartmentList()
      .subscribe((res: DepartmentDetails[]) => {
        this.ddlDepartmentDetails.push({
          label: "Select Department",
          value: -1,
        });
        res.forEach((element) => {
          this.ddlDepartmentDetails.push({
            label: element.Description,
            value: element.DepartmentId,
          });
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
            value: element.KRARoleID,
          });
        });
      },
      (error: any) => {
        // this._snackBar.open("Failed to get KRA Roles list.", "", {
        //   duration: 1000,
        //   horizontalPosition: "right",
        //   verticalPosition: "top",
        // });
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

  getGradeByDesignation(designation) {
    this.personelDetailsForm.controls.DesignationId.setValue(designation.DesignationId);
    this.personelDetailsForm.controls.RoleTypeId.reset();
    this.filteredDesignationIds = null;
    if (this.designation && this.designation.DesignationId) {
      this.masterDataService
        .getGradeByDesignation(designation.DesignationId)
        .subscribe(
          (gradeResponse: Grade) => {
            // this._Associate.GradeName = gradeResponse.GradeName;
            // this._Associate.GradeId = gradeResponse.GradeId;
            this.personelDetailsForm.controls.GradeId.setValue(
              gradeResponse.GradeId
            );
            this.personelDetailsForm.controls.GradeName.setValue(gradeResponse.GradeName);
            this.getRoleTypesByGrade(gradeResponse.GradeId);
          },
          (error: any) => {
            if (error.error != undefined && error.error != "")
              this._commonService
                .LogError(this.componentName, error.error)
                .subscribe((data: any) => { });
          }
        );
    } else {
      // this._Associate.GradeName = "";
      // this._Associate.GradeId = null;
      // this.personelDetailsForm.controls.GradeName.setValue(null);
    }
  }

  GetEmployeePersonalDetails(): void {

    this._service.GetEmployeePersonalDetails(this.id).subscribe((res: any) => {
      this.spinner.hide()
      if (res.DateofBirth && res.DateofBirth !== null)
        res.DateofBirth = moment(res.DateofBirth).format("YYYY-MM-DD");
      if (res.JoinDate)
        res.JoinDate = moment(res.JoinDate).format("YYYY-MM-DD");
      if (res.BgvinitiatedDate)
        res.BgvinitiatedDate = moment(res.BgvinitiatedDate).format(
          "YYYY-MM-DD"
        );
      if (res.BgvcompletionDate)
        res.BgvcompletionDate = moment(res.BgvcompletionDate).format(
          "YYYY-MM-DD"
        );
      if (res.EmploymentStartDate)
        res.EmploymentStartDate = moment(res.EmploymentStartDate).format(
          "YYYY-MM-DD"
        );

      this._Associate = res;
      
      this._Associate.RoleTypeId={
        value:res.RoleTypeId,
        label:res.RoleTypeName
        
      }
      this.GetManagersAndLead(this._Associate.DepartmentId);
      this.personelDetailsForm.controls.ID.setValue(res.ID);

      //this._Associate.Designation = res.designation;
      if (this._Associate.Designation && this._Associate.DesignationId) {
        this.designation = new Designation();
        this.designation.DesignationId = this._Associate.DesignationId;
        this.designation.DesignationName = this._Associate.Designation;

        this.getGradeByDesignation(this.designation);
      }

      //Assign dates
      this._Associate.Birthdate = res.DateofBirth;

      this._Associate.DateofJoining = res.JoinDate;
      this._Associate.BgvStartDate = res.BgvinitiatedDate;
      this._Associate.BgvCompletionDate = res.BgvcompletionDate;
      this._Associate.StartDateofEmployment = res.EmploymentStartDate;
      this._Associate.CareerBreak = res.CareerBreak;

      this.disableBGVDates(this._Associate.Bgvstatus);
      this.disableKRA(this._Associate.Bgvstatus);

      if (this._Associate.DepartmentId == 1) this.isRequired = true;

      if (
        this._Associate.TechnologyId == null ||
        this._Associate.TechnologyId == ""
      ) {
        this._Associate.TechnologyId = "";
      }

      this.personelDetailsForm.controls.TechnologyId.setValue(
        this._Associate.TechnologyId
      );

      if (
        this._Associate.contactDetails != null &&
        this._Associate.contactDetails.length != 0
      ) {
        this.type = "edit";
        for (var i = 0; i < this._Associate.contactDetails.length; i++) {
          if (
            this._Associate.contactDetails[i].addressType == "CurrentAddress"
          ) {
            this._Contacts.currentAddCity = this._Associate.contactDetails[
              i
            ].currentAddCity;
            this._Contacts.currentAddCountry = this._Associate.contactDetails[
              i
            ].currentAddCountry;
            this._Contacts.currentAddress1 = this._Associate.contactDetails[
              i
            ].currentAddress1;
            this._Contacts.currentAddress2 = this._Associate.contactDetails[
              i
            ].currentAddress2;
            this._Contacts.currentAddState = this._Associate.contactDetails[
              i
            ].currentAddState;
            this._Contacts.currentAddZip = this._Associate.contactDetails[
              i
            ].currentAddZip;
          }
          if (
            this._Associate.contactDetails[i].addressType == "PermanentAddress"
          ) {
            this._Contacts.permanentAddCity = this._Associate.contactDetails[
              i
            ].permanentAddCity;
            this._Contacts.permanentAddCountry = this._Associate.contactDetails[
              i
            ].permanentAddCountry;
            this._Contacts.permanentAddress1 = this._Associate.contactDetails[
              i
            ].permanentAddress1;
            this._Contacts.permanentAddress2 = this._Associate.contactDetails[
              i
            ].permanentAddress2;
            this._Contacts.permanentAddState = this._Associate.contactDetails[
              i
            ].permanentAddState;
            this._Contacts.permanentAddZip = this._Associate.contactDetails[
              i
            ].permanentAddZip;
          }
        }
        if (
          this._Contacts.currentAddCity != null ||
          this._Contacts.currentAddCountry != null ||
          this._Contacts.currentAddress1 != null ||
          this._Contacts.currentAddress2 != null ||
          this._Contacts.currentAddState != null ||
          this._Contacts.currentAddZip != null
        ) {
          if (
            this._Contacts.currentAddCity == this._Contacts.permanentAddCity &&
            this._Contacts.currentAddCountry ==
            this._Contacts.permanentAddCountry &&
            this._Contacts.currentAddress1 ==
            this._Contacts.permanentAddress1 &&
            this._Contacts.currentAddress2 ==
            this._Contacts.permanentAddress2 &&
            this._Contacts.currentAddState ==
            this._Contacts.permanentAddState &&
            this._Contacts.currentAddZip == this._Contacts.permanentAddZip
          ) {
            this._Contacts.address = true;
          }
        }
      }
      this.assignPersonalDetails();
    },(error)=>{
      this.spinner.hide()
    });
  }

  GetPersonalDetails(): void {
    let DateOfJoining: Date;
    this._service.GetPersonalDetails(this.id).subscribe((res) => {
      this.spinner.hide()
      if (res.DateofJoining) {
        res.DateofJoining = moment(res.DateofJoining).format("YYYY-MM-DD");
      }
      this._Associate = res;
      this._Associate.DateofJoining = res.DateOfJoining;
      this._Associate.ReportingManager = res.ReportingManager;
      this._Associate.ManagerId = res.ManagerId;
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
      this.ddlEmpTypes.forEach((empType) => {
        if (empType.EmpType == res.EmploymentType)
          this._Associate.EmployeeTypeId = empType.EmployeeTypeId;
      });
      this.assignPersonalDetails();
    },(error)=>{
      this.spinner.hide()
    });
  }
  GetManagersAndLeads() {
    this.masterDataService
      .GetManagersAndCompetencyLeads()
      .subscribe((res: GenericType[]) => {
        this.managers = res;
        if (res) {
          if (this.type == "new") {
            this.GetPersonalDetails();
          } else {
            this.GetEmployeePersonalDetails();
          }
        }
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

  getAssociateDsgId() {
    if (this.designation && this.designation.DesignationId) {
      this._Associate.DesignationId = this.designation.DesignationId;
    } else {
      this._Associate.DesignationId = 0;
      //this._Associate.Gender = "";
    }
  }

  validateInputData(): boolean {
    var today: any = new Date();
    let mm = today.getMonth() + 1;
    let dd = today.getDate();
    if (dd < 10) {
      dd = "0" + dd;
    }

    if (mm < 10) {
      mm = "0" + mm;
    }
    today = today.getFullYear() + "-" + mm + "-" + dd;

    if (!this.personelDetailsForm.value.Birthdate) {
      this._snackBar.open("Please enter Date of birth", "", {
        duration: 1000,
        horizontalPosition: "right",
        verticalPosition: "top",
      });
      return false;
    }

    if (
      moment(this.personelDetailsForm.value.Birthdate).isSameOrAfter(new Date())
    ) {
      this._snackBar.open("Date of birth should be less than today", "", {
        duration: 1000,
        horizontalPosition: "right",
        verticalPosition: "top",
      });
      return false;
    }

    if (this.personelDetailsForm.value.AadharNumber) {
      if (this.personelDetailsForm.value.AadharNumber.includes("000000000000")) {
        this._snackBar.open("Please enter valid Aadhar Number", "", {
          duration: 1000,
          horizontalPosition: "right",
          verticalPosition: "top",
        });
        return false;
      }
    }

    if (
      this.personelDetailsForm.value.DateofJoining == "" ||
      this.personelDetailsForm.value.DateofJoining == undefined
    ) {
      this._snackBar.open("Please enter joining date", "", {
        duration: 1000,
        horizontalPosition: "right",
        verticalPosition: "top",
      });
      return false;
    }
    if (
      Date.parse(this.personelDetailsForm.value.Birthdate) >
      Date.parse(this.personelDetailsForm.value.DateofJoining)
    ) {
      this._snackBar.open(
        "Employee's birth date should not greater than joining date",
        "",
        {
          duration: 1000,
          horizontalPosition: "right",
          verticalPosition: "top",
        }
      );
      return false;
    }

    if (
      this.personelDetailsForm.value.StartDateofEmployment == "" ||
      this.personelDetailsForm.value.StartDateofEmployment == undefined
    ) {
      this._snackBar.open("Please enter employment start date", "", {
        duration: 1000,
        horizontalPosition: "right",
        verticalPosition: "top",
      });
      return false;
    }

    if (this.personelDetailsForm.value.EmploymentStartDate > today.toString()) {
      this._snackBar.open(
        "Employement start date should not greater than today's date ",
        "",
        {
          duration: 1000,
          horizontalPosition: "right",
          verticalPosition: "top",
        }
      );
      return false;
    }

    if (
      Date.parse(this.personelDetailsForm.value.StartDateofEmployment) <
      Date.parse(this.personelDetailsForm.value.Birthdate)
    ) {
      this._snackBar.open(
        "Employement start date should be greater than birth date",
        "",
        {
          duration: 1000,
          horizontalPosition: "right",
          verticalPosition: "top",
        }
      );
      return false;
    }
    if (
      Date.parse(this.personelDetailsForm.value.StartDateofEmployment) >
      Date.parse(this.personelDetailsForm.value.DateofJoining)
    ) {
      this._snackBar.open(
        "Employement start date should not be greater than joining date",
        "",
        {
          duration: 1000,
          horizontalPosition: "right",
          verticalPosition: "top",
        }
      );
      return false;
    }

    if (this.personelDetailsForm.value.PassportDateValidUpto) {
      if (
        Date.parse(this.personelDetailsForm.value.DateofBirth) >
        Date.parse(this.personelDetailsForm.value.PassportDateValidUpto)
      ) {
        this._snackBar.open(
          "Passport Valid Date should be greater than or equal to Birth Date",
          "",
          {
            duration: 1000,
            horizontalPosition: "right",
            verticalPosition: "top",
          }
        );
        return false;
      }
    }

    if (this.personelDetailsForm.value.Pfnumber) {
      let i = 0,
        j = 0;
      for (i = 0; i < this.personelDetailsForm.value.Pfnumber.length; i++) {
        if (this.personelDetailsForm.value.Pfnumber[i].includes("0")) {
          j++;
        }
      }
      if (i == j) {
        this._snackBar.open("Please enter valid PF Number", "", {
          duration: 1000,
          horizontalPosition: "right",
          verticalPosition: "top",
        });
        return false;
      }
    }
    if (this.personelDetailsForm.value.BgvStartDate) {
      if (
        !this.IsValidDate(
          this.personelDetailsForm.value.DateofJoining,
          this.personelDetailsForm.value.BgvStartDate
        )
      ) {
        this._snackBar.open(
          "BGV Start Date should be greater than joining Date",
          "",
          {
            duration: 1000,
            horizontalPosition: "right",
            verticalPosition: "top",
          }
        );
        return false;
      }

      //   if (
      //     Date.parse(this.personelDetailsForm.value.BgvStartDate) >
      //     Date.parse(this.personelDetailsForm.value.BgvCompletionDate)
      //   )
      //  {
      //   this._snackBar.open(
      //     "BGV Start Date should be greater than joining Date",
      //     "",
      //     {
      //       duration: 1000,
      //       horizontalPosition: "right",
      //       verticalPosition: "top",
      //     }
      //   );
      //   return false;
      // }
    }

    if (
      this.personelDetailsForm.value.Bgvstatus == "Verified" &&
      this.personelDetailsForm.value.Bgvstatus != undefined
    ) {
      if (
        this.personelDetailsForm.value.BgvStartDate == "" ||
        this.personelDetailsForm.value.BgvStartDate == undefined ||
        this.personelDetailsForm.value.BgvCompletionDate == "" ||
        this.personelDetailsForm.value.BgvCompletionDate == undefined
      ) {
        this._snackBar.open(
          "Please enter BGV Start Date and BGV Completed Date",
          "",
          {
            duration: 1000,
            horizontalPosition: "right",
            verticalPosition: "top",
          }
        );
        return false;
      }

    }

    if (
      this.personelDetailsForm.value.BgvStartDate &&
      this.personelDetailsForm.value.BgvCompletionDate
    ) {
      if (
        !this.IsValidDate(this.personelDetailsForm.value.BgvStartDate,
          this.personelDetailsForm.value.BgvCompletionDate

        )
      ) {
        this._snackBar.open(
          "BGV Completed Date should be greater than BGV Start Date",
          "",
          {
            duration: 1000,
            horizontalPosition: "right",
            verticalPosition: "top",
          }
        );
        return false;
      }
      if (
        Date.parse(this.personelDetailsForm.value.BgvStartDate) >=
        Date.parse(this.personelDetailsForm.value.BgvCompletionDate)
      ) {
        this._snackBar.open(
          "BGV Completed Date should be greater than BGV Start Date",
          "",
          {
            duration: 1000,
            horizontalPosition: "right",
            verticalPosition: "top",
          }
        );
        return false;
      }
    }

    if (!this.personelDetailsForm.value.Designation) {
      this._snackBar.open("Please select Designation", "", {
        duration: 1000,
        horizontalPosition: "right",
        verticalPosition: "top",
      });
      return false;
    }

    return true;
  }

  onSavePersonalDetails() {

    this.AsoociateDataFormData = this.personelDetailsForm.value;
    if(this.AsoociateDataFormData.RoleTypeId!=null)
    {
    this.AsoociateDataFormData.RoleTypeId = this.AsoociateDataFormData.RoleTypeId.value
    }
    else{
      this.AsoociateDataFormData.RoleTypeId = this.AsoociateDataFormData.RoleTypeId
    }
    this.AsoociateDataFormData.Id = this._Associate.Id;
    this.AsoociateDataFormData.ID = this._Associate.Id;
    this.AsoociateDataFormData.EmployeeId = 0;
    this.AsoociateDataFormData.EmpId = this.currentempID;
    this.AsoociateDataFormData.DateofBirth = this.AsoociateDataFormData.Birthdate;
    this.AsoociateDataFormData.BgvStatus = this.AsoociateDataFormData.Bgvstatus;

    this.AsoociateDataFormData.BgvcompletionDate = this.AsoociateDataFormData.BgvCompletionDate;
    this.AsoociateDataFormData.BgvinitiatedDate = this.AsoociateDataFormData.BgvStartDate;


    this.AsoociateDataFormData.EmpCode = this.AsoociateDataFormData.EmployeeCode;

    this.AsoociateDataFormData.contactDetails[0].ID = 0;
    this.AsoociateDataFormData.contactDetails[1].ID = 0;
    this.AsoociateDataFormData.contacts.currentAddCity = this.AsoociateDataFormData.contactDetails[0].currentAddCity;
    this.AsoociateDataFormData.contacts.currentAddCountry = this.AsoociateDataFormData.contactDetails[0].currentAddCountry;
    this.AsoociateDataFormData.contacts.currentAddress1 = this.AsoociateDataFormData.contactDetails[0].currentAddress1;
    this.AsoociateDataFormData.contacts.currentAddress2 = this.AsoociateDataFormData.contactDetails[0].currentAddress2;
    this.AsoociateDataFormData.contacts.currentAddState = this.AsoociateDataFormData.contactDetails[0].currentAddState;
    this.AsoociateDataFormData.contacts.currentAddZip = this.AsoociateDataFormData.contactDetails[0].currentAddZip;
    this.AsoociateDataFormData.contacts.permanentAddCity = this.AsoociateDataFormData.contactDetails[1].permanentAddCity;
    this.AsoociateDataFormData.contacts.permanentAddCountry = this.AsoociateDataFormData.contactDetails[1].permanentAddCountry;
    this.AsoociateDataFormData.contacts.permanentAddress1 = this.AsoociateDataFormData.contactDetails[1].permanentAddress1;
    this.AsoociateDataFormData.contacts.permanentAddress2 = this.AsoociateDataFormData.contactDetails[1].permanentAddress2;
    this.AsoociateDataFormData.contacts.permanentAddState = this.AsoociateDataFormData.contactDetails[1].permanentAddState;
    this.AsoociateDataFormData.contacts.permanentAddZip = this.AsoociateDataFormData.contactDetails[1].permanentAddZip;
    this.AsoociateDataFormData.contacts.address = this.isCheckedSameAddress;

    this.AsoociateDataFormData.EmpName = this.AsoociateDataFormData.FirstName + " " + this.AsoociateDataFormData.LastName;
    this.AsoociateDataFormData.EncryptedMobileNumber = null;
    this.AsoociateDataFormData.RoleName = null;
    this.AsoociateDataFormData.WorkEmail = null;
    this.AsoociateDataFormData.Dob = null;
    this.AsoociateDataFormData.Department = null;
    this.AsoociateDataFormData.ProjectId = 0;
    this.AsoociateDataFormData.joiningStatusID = null;
    this.AsoociateDataFormData.ProjectName = null;
    this.AsoociateDataFormData.LeadId = 0;
    this.AsoociateDataFormData.ManagerId = null;
    this.AsoociateDataFormData.ManagerFirstName = null;
    this.AsoociateDataFormData.ManagerLastName = null;
    this.AsoociateDataFormData.ManagerName = null;
    this.AsoociateDataFormData.LeadFirstName = null;
    this.AsoociateDataFormData.LeadLastName = 0;
    this.AsoociateDataFormData.LeadName = null;
    this.AsoociateDataFormData.EmailAddress = null;
    this.AsoociateDataFormData.Year = null;
    this.AsoociateDataFormData.RoleId = 0;
    this.AsoociateDataFormData.LastWorkingDate = null;
    this.AsoociateDataFormData.DropOutReason = null;
    delete this.AsoociateDataFormData.ID;

    this.getAssociateDsgId();
    if (this._Contacts.address == true) {
      let isValid = this.validateAddresses(this._Contacts.address);
      if (isValid == false) {
        this._snackBar.open("Please enter current address details", "", {
          duration: 1000,
          horizontalPosition: "right",
          verticalPosition: "top",
        });
        return false;
      }
    }
    this._Associate.contacts = this._Contacts;
    this._Associate.JoinDate = new Date(this._Associate.DateofJoining);
    this._Associate.DateofBirth = new Date(this._Associate.DateofBirth);
    this._Associate.BgvinitiatedDate = new Date(this._Associate.BgvStartDate);
    this._Associate.BgvcompletionDate = new Date(
      this._Associate.BgvCompletionDate
    );
    this._Associate.EmploymentStartDate = new Date(
      this._Associate.StartDateofEmployment
    );
    this.AsoociateDataFormData.TechnologyId = this._Associate.TechnologyId;
    this.AsoociateDataFormData.JoinDate = this._Associate.JoinDate;
    this.AsoociateDataFormData.StartDateofEmployment = this._Associate.StartDateofEmployment;
    this.AsoociateDataFormData.EmploymentStartDate = this._Associate.StartDateofEmployment;
    let validData = this.validateInputData();

    if (this.selectedDate(this.AsoociateDataFormData.Birthdate) == false)
      return;
    if (this.validateInputData() == false) {
      this.personelDetailsForm.valid == false;
      return;

    }

    if (this.personelDetailsForm.valid) {
      this._service
        .SavePersonalDetails(this.AsoociateDataFormData)
        .subscribe(
          (data: any) => {
            if (data.EmployeeId > 0) {
              this.type = "edit";
              this.onAssociateSave.emit(this.type);
              this.currentempID = data.EmployeeId;
              this._snackBar.open("Personal details saved successfully", "", {
                duration: 1000,
                horizontalPosition: "right",
                verticalPosition: "top",
              });
              setTimeout(() => {
                this.router.navigate([
                  "/associates/prospectivetoassociate/edit/" +
                  data.EmployeeId +
                  "/" +
                  "profile",
                ]);
              }, 1000);
            } else if (data.length > 0) {
              let errorMessage = this.validatePersonalDetails(data);
              this._snackBar.open(errorMessage + " already exists", "", {
                duration: 1000,
                panelClass:['error-alert'],
                horizontalPosition: "right",
                verticalPosition: "top",
              });
            }
          },
          (error) => {
            if (error.error != undefined && error.error != "") {
              this._snackBar.open(error.error, "", {
                duration: 1000,
                panelClass:['error-alert'],
                horizontalPosition: "right",
                verticalPosition: "top",
              });
            } else {
              this._snackBar.open("Failed to save personal details", "", {
                duration: 1000,
                panelClass:['error-alert'],
                horizontalPosition: "right",
                verticalPosition: "top",
              });
            }
          }
        );
    } else {
      this._snackBar.open(
        "Please correct the fields highlighted in red colour",
        "!",
        {
          duration: 1000,
          horizontalPosition: "right",
          verticalPosition: "top",
        }
      );
    }
  }

  onUpdatePersonalDetails() {
    this.personelDetailsForm.controls.EmployeeId.setValue(this.currentempID);

    if ((window.location.href.indexOf("associates/prospectivetoassociate/edit/") > -1) && (window.location.href.indexOf("/list") > -1) || (window.location.href.indexOf("associates/prospectivetoassociate/edit/") > -1) && (window.location.href.indexOf("/profileupdate") > -1) || (window.location.href.indexOf("associates/prospectivetoassociate/edit/") > -1) && (window.location.href.indexOf("/EPU") > -1)) {
      this.AsoociateDataFormData = this.personelDetailsForm.getRawValue();
      if(this.AsoociateDataFormData.RoleTypeId!=null)
    {
    this.AsoociateDataFormData.RoleTypeId = this.AsoociateDataFormData.RoleTypeId.value
    }
    else{
      this.AsoociateDataFormData.RoleTypeId = this.AsoociateDataFormData.RoleTypeId
    }
      this.AsoociateDataFormData.BgvStatus = this.AsoociateDataFormData.Bgvstatus;
      this.AsoociateDataFormData.BgvcompletionDate = this.AsoociateDataFormData.BgvCompletionDate;
      this.AsoociateDataFormData.BgvinitiatedDate = this.AsoociateDataFormData.BgvStartDate;
      this.AsoociateDataFormData.DateofBirth = this.AsoociateDataFormData.Birthdate;
      this.AsoociateDataFormData.EmploymentStartDate = this.AsoociateDataFormData.StartDateofEmployment;
      this.AsoociateDataFormData.JoinDate = this.AsoociateDataFormData.DateofJoining;
      this.AsoociateDataFormData.contacts.currentAddress1 = this.AsoociateDataFormData.contactDetails[0].currentAddress1;
      this.AsoociateDataFormData.contacts.currentAddress2 = this.AsoociateDataFormData.contactDetails[0].currentAddress2;
      this.AsoociateDataFormData.contacts.currentAddCity = this.AsoociateDataFormData.contactDetails[0].currentAddCity;
      this.AsoociateDataFormData.contacts.currentAddState = this.AsoociateDataFormData.contactDetails[0].currentAddState;
      this.AsoociateDataFormData.contacts.currentAddCountry = this.AsoociateDataFormData.contactDetails[0].currentAddCountry;
      this.AsoociateDataFormData.contacts.currentAddZip = this.AsoociateDataFormData.contactDetails[0].currentAddZip;
      this.AsoociateDataFormData.contacts.permanentAddress1 = this.AsoociateDataFormData.contactDetails[1].permanentAddress1;
      this.AsoociateDataFormData.contacts.permanentAddress2 = this.AsoociateDataFormData.contactDetails[1].permanentAddress2;
      this.AsoociateDataFormData.contacts.permanentAddCity = this.AsoociateDataFormData.contactDetails[1].permanentAddCity;
      this.AsoociateDataFormData.contacts.permanentAddState = this.AsoociateDataFormData.contactDetails[1].permanentAddState;
      this.AsoociateDataFormData.contacts.permanentAddCountry = this.AsoociateDataFormData.contactDetails[1].permanentAddCountry;
      this.AsoociateDataFormData.contacts.permanentAddZip = this.AsoociateDataFormData.contactDetails[1].permanentAddZip;
      this.AsoociateDataFormData.contacts.permanentAddZip = this.AsoociateDataFormData.contactDetails[1].permanentAddZip;
      this.AsoociateDataFormData.contacts.address = this.isCheckedSameAddress;
      delete this.AsoociateDataFormData['Id'];
      delete this.AsoociateDataFormData['EmpId'];
      delete this.AsoociateDataFormData['EmpCode'];
      this.AsoociateDataFormData.contactDetails = null;
    } else if ((window.location.href.indexOf("associates/prospectivetoassociate/edit/") > -1) && (window.location.href.indexOf("/profileupdate") > -1)) {
      this.AsoociateDataFormData = this.personelDetailsForm.getRawValue();

      if (this._Associate.contactDetails) {
        this.AsoociateDataFormData.contactDetails[0].ID = this._Associate.contactDetails[0].ID;
        this.AsoociateDataFormData.contactDetails[1].ID = this._Associate.contactDetails[1].ID;
      }
      else {
        this.AsoociateDataFormData.contactDetails[0].ID = 0;
        this.AsoociateDataFormData.contactDetails[1].ID = 0;
      }


      delete this.AsoociateDataFormData['Id'];
      delete this.AsoociateDataFormData['EmpId'];
      delete this.AsoociateDataFormData['EmpCode'];
    }
    let validdata = this.validateInputData();
    if (this.validateInputData() == false) {
      this.personelDetailsForm.valid == false;
      return;

    }
    if (this.selectedDate(this.AsoociateDataFormData.Birthdate) == false) return;

    if (this.personelDetailsForm.valid) {
      this._service.UpdatePersonalDetails(this.AsoociateDataFormData).subscribe(
        (data: any) => {
          if (data.IsSuccessful == true) {
            this.type = "edit";
            this._snackBar.open("Personal details updated successfully", "", {
              duration: 1000,
              horizontalPosition: "right",
              verticalPosition: "top",
            });
          } else if (data.length > 0) {
            let errorMessage = this.validatePersonalDetails(data);
            this._snackBar.open(errorMessage + " already exists", "", {
              duration: 1000,
              panelClass:['error-alert'],
              horizontalPosition: "right",
              verticalPosition: "top",
            });
          }
        },
        (error) => {
          if (error != undefined && error != "") {
            this._snackBar.open(error.error, "", {
              duration: 1000,
              panelClass:['error-alert'],
              horizontalPosition: "right",
              verticalPosition: "top",
            });
          } else {
            this._snackBar.open("Failed to update personal details", "", {
              duration: 1000,
              panelClass:['error-alert'],
              horizontalPosition: "right",
              verticalPosition: "top",
            });
          }
        }
      );
    } else {
      this._snackBar.open(
        "Please correct the fields highlighted in red colour",
        "!",
        {
          duration: 1000,
          horizontalPosition: "right",
          verticalPosition: "top",
        }
      );
    }
  }

  private validatePersonalDetails(resultData: any): string {
    if (resultData.length > 0) {
      let errorMessage: string = "";
      resultData.forEach((ele: any) => {
        if (ele.Id == PersonalDetails.PersonalEmailAddress)
          errorMessage != ""
            ? (errorMessage += "Personal Email Id")
            : (errorMessage = "Personal Email Id");
        if (ele.Id == PersonalDetails.MobileNo)
          errorMessage != ""
            ? (errorMessage += ", Mobile Number")
            : (errorMessage = "Mobile Number");
        if (ele.Id == PersonalDetails.AadharNumber)
          errorMessage != ""
            ? (errorMessage += ", Aadhar Number")
            : (errorMessage = "Aadhar Number");
        if (ele.Id == PersonalDetails.PANNumber)
          errorMessage != ""
            ? (errorMessage += ", PAN Number")
            : (errorMessage = "PAN Number");
        if (ele.Id == PersonalDetails.UANNumber)
          errorMessage != ""
            ? (errorMessage += ", UAN Number")
            : (errorMessage = "UAN Number");
        if (ele.Id == PersonalDetails.PFNumber)
          errorMessage != ""
            ? (errorMessage += ", PF Number")
            : (errorMessage = "PF Number");
        if (ele.Id == PersonalDetails.PassportNumber)
          errorMessage != ""
            ? (errorMessage += ", Passport Number")
            : (errorMessage = "Passport Number");
      });
      return errorMessage;
    }
  }

  private validateAddresses(isaddress: boolean): boolean {
    if (isaddress) {
      if (
        (this._Contacts.currentAddCity != null &&
          this._Contacts.currentAddCity != "" &&
          this._Contacts.currentAddCity.trim().length > 0) ||
        (this._Contacts.currentAddCountry != null &&
          this._Contacts.currentAddCountry != "") ||
        (this._Contacts.currentAddress1 != null &&
          this._Contacts.currentAddress1 != "" &&
          this._Contacts.currentAddress1.trim().length > 0) ||
        (this._Contacts.currentAddress2 != null &&
          this._Contacts.currentAddress2 != "" &&
          this._Contacts.currentAddress2.trim().length > 0) ||
        (this._Contacts.currentAddState != null &&
          this._Contacts.currentAddState != "" &&
          this._Contacts.currentAddState.trim().length > 0) ||
        (this._Contacts.currentAddZip != null &&
          this._Contacts.currentAddZip != "")
      ) {

        this.personelDetailsForm
          .get("contactDetails")
        ["controls"][1].controls.permanentAddress1.setValue(
          this._Contacts.currentAddress1
        );
        this.personelDetailsForm
          .get("contactDetails")
        ["controls"][1].controls.permanentAddress2.setValue(
          this._Contacts.currentAddress2
        );
        this.personelDetailsForm
          .get("contactDetails")
        ["controls"][1].controls.permanentAddCity.setValue(
          this._Contacts.currentAddCity
        );
        this.personelDetailsForm
          .get("contactDetails")
        ["controls"][1].controls.permanentAddState.setValue(
          this._Contacts.currentAddState
        );
        this.personelDetailsForm
          .get("contactDetails")
        ["controls"][1].controls.permanentAddCountry.setValue(
          this._Contacts.currentAddCountry
        );
        this.personelDetailsForm
          .get("contactDetails")
        ["controls"][1].controls.permanentAddZip.setValue(
          this._Contacts.currentAddZip
        );
        return true;
      } else {
        this._Contacts.permanentAddCity = "";
        this._Contacts.permanentAddCountry = "";
        this._Contacts.permanentAddress1 = "";
        this._Contacts.permanentAddress2 = "";
        this._Contacts.permanentAddState = "";
        this._Contacts.permanentAddZip = "";
        this._snackBar.open("Please enter current address details", "", {
          duration: 1000,
          horizontalPosition: "right",
          verticalPosition: "top",
        });
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
      "9": 57,
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
      this._snackBar.open("Please select a valid birth date", "", {
        duration: 1000,
        horizontalPosition: "right",
        verticalPosition: "top",
      });
      return false;
    }
  }
  disableBGVDates(event: any) {
    if (event) {
      var bgvstatus = event.value ? event.value : event;
      this._Associate.Bgvstatus = bgvstatus;
      if (bgvstatus == "NotVerified") {
        this.personelDetailsForm.controls.BgvStartDate.setValidators([]);
        this.personelDetailsForm.controls.BgvCompletionDate.setValidators([]);
        this.personelDetailsForm.controls.BgvStartDate.setValue("");
        this.personelDetailsForm.controls.BgvCompletionDate.setValue("");
        this.isDisabled = true;
      } else {
        this.isDisabled = false;
        this.personelDetailsForm.controls.BgvStartDate.setValidators([Validators.required]);
        this.personelDetailsForm.controls.BgvCompletionDate.setValidators([Validators.required]);
      }
    } else {
      this.isDisabled = false;
    }
  }

  disableKRA(event: any) {
    if (event == "Verified") this.todisableKRA = true;
    else this.todisableKRA = false;
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

  setZipLengthone(event: any) {
    // this._Contacts.currentAddZip = null;
    if (event.value == "India") {
      this.zipLengthone = 6;
    }
    else { this.zipLengthone = 5; }
  }
  setZipLengthtwo(event: any) {
    // this._Contacts.currentAddZip = null;
    if (event.value == "India") {
      this.zipLengthtwo = 6;

    }
    else {
      this.zipLengthtwo = 5;
    }
  }
  sameAddress(e) {
    if (e.checked == true) {
      this.personelDetailsForm
        .get("contactDetails")
      ["controls"][1].controls.permanentAddress1.setValue(
        this.personelDetailsForm.get("contactDetails")["controls"][0].controls
          .currentAddress1.value
      );
      this.personelDetailsForm
        .get("contactDetails")
      ["controls"][1].controls.permanentAddress2.setValue(
        this.personelDetailsForm.get("contactDetails")["controls"][0].controls
          .currentAddress2.value
      );
      this.personelDetailsForm
        .get("contactDetails")
      ["controls"][1].controls.permanentAddCity.setValue(
        this.personelDetailsForm.get("contactDetails")["controls"][0].controls
          .currentAddCity.value
      );
      this.personelDetailsForm
        .get("contactDetails")
      ["controls"][1].controls.permanentAddState.setValue(
        this.personelDetailsForm.get("contactDetails")["controls"][0].controls
          .currentAddState.value
      );
      this.personelDetailsForm
        .get("contactDetails")
      ["controls"][1].controls.permanentAddCountry.setValue(
        this.personelDetailsForm.get("contactDetails")["controls"][0].controls
          .currentAddCountry.value
      );
      this.personelDetailsForm
        .get("contactDetails")
      ["controls"][1].controls.permanentAddZip.setValue(
        this.personelDetailsForm.get("contactDetails")["controls"][0].controls
          .currentAddZip.value
      );
      this.isCheckedSameAddress = true;
    } else {
      this.personelDetailsForm
        .get("contactDetails")
      ["controls"][1].controls.permanentAddress1.setValue("");
      this.personelDetailsForm
        .get("contactDetails")
      ["controls"][1].controls.permanentAddress2.setValue("");
      this.personelDetailsForm
        .get("contactDetails")
      ["controls"][1].controls.permanentAddCity.setValue("");
      this.personelDetailsForm
        .get("contactDetails")
      ["controls"][1].controls.permanentAddState.setValue("");
      this.personelDetailsForm
        .get("contactDetails")
      ["controls"][1].controls.permanentAddCountry.setValue("");
      this.personelDetailsForm
        .get("contactDetails")
      ["controls"][1].controls.permanentAddZip.setValue("");
      this.isCheckedSameAddress = false;
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
      "9": 57,
    };
    for (var index in keys) {
      if (!keys.hasOwnProperty(index)) continue;
      if (event.charCode == keys[index] || event.keyCode == keys[index]) {
        return; //default event
      }
    }
    event.preventDefault();
  }

  assignPersonalDetails() {
    this.personelDetailsForm.controls.FirstName.setValue(
      this._Associate.FirstName
    );
    this.personelDetailsForm.controls.MiddleName.setValue(
      this._Associate.MiddleName
    );
    this.personelDetailsForm.controls.LastName.setValue(
      this._Associate.LastName
    );
    this.personelDetailsForm.controls.Gender.setValue(this._Associate.Gender);
    this.personelDetailsForm.controls.Birthdate.setValue(
      this._Associate.Birthdate
    );
    this.personelDetailsForm.controls.DateofBirth.setValue(
      this._Associate.DateofBirth
    );
    this.personelDetailsForm.controls.MaritalStatus.setValue(
      this._Associate.MaritalStatus
    );
    this.personelDetailsForm.controls.BloodGroup.setValue(
      this._Associate.BloodGroup
    );
    this.personelDetailsForm.controls.Nationality.setValue(
      this._Associate.Nationality
    );
    this.personelDetailsForm.controls.TelephoneNo.setValue(
      this._Associate.TelephoneNo
    );
    this.personelDetailsForm.controls.MobileNo.setValue(
      this._Associate.MobileNo
    );
    this.personelDetailsForm.controls.PersonalEmailAddress.setValue(
      this._Associate.PersonalEmailAddress
    );
    this.personelDetailsForm.controls.AadharNumber.setValue(
      this._Associate.AadharNumber
    );
    this.personelDetailsForm.controls.Pannumber.setValue(
      this._Associate.Pannumber
    );
    this.personelDetailsForm.controls.PassportNumber.setValue(
      this._Associate.PassportNumber
    );
    this.personelDetailsForm.controls.PassportIssuingOffice.setValue(
      this._Associate.PassportIssuingOffice
    );
    this.personelDetailsForm.controls.PassportDateValidUpto.setValue(
      this._Associate.PassportDateValidUpto
    );
    this.personelDetailsForm.controls.Uannumber.setValue(
      this._Associate.Uannumber
    );
    this.personelDetailsForm.controls.Pfnumber.setValue(
      this._Associate.Pfnumber
    );
    this.personelDetailsForm.controls.StartDateofEmployment.setValue(
      this._Associate.StartDateofEmployment
    );
    this.personelDetailsForm.controls.EmploymentStartDate.setValue(
      this._Associate.StartDateofEmployment
    );
    this.personelDetailsForm.controls.EmployeeCode.setValue(
      this._Associate.EmployeeCode
    );

    this.personelDetailsForm.controls.CareerBreak.setValue(
      this._Associate.CareerBreak
    );

    this.personelDetailsForm.controls.DateofJoining.setValue(
      this._Associate.DateofJoining
    );
    this.personelDetailsForm.controls.JoinDate.setValue(
      this._Associate.DateofJoining
    );

    this.personelDetailsForm.controls.Hradvisor.setValue(
      this._Associate.Hradvisor
    );
    this.personelDetailsForm.controls.Designation.setValue(
      this._Associate.Designation
    );
    this.personelDetailsForm.controls.GradeName.setValue(
      this._Associate.GradeName
    );
    this.personelDetailsForm.controls.GradeId.setValue(this._Associate.GradeId);
    if (this._Associate.RoleTypeId != null){
      this.personelDetailsForm.controls.RoleTypeId.setValue(
        this._Associate.RoleTypeId
      ); 
    }   
    this.getRoleTypesByGrade(this._Associate.GradeId);
    this.personelDetailsForm.controls.UserId.setValue(this._Associate.UserId);
    this.personelDetailsForm.controls.EmployeeTypeId.setValue(
      this._Associate.EmployeeTypeId
    );
    this.personelDetailsForm.controls.EmploymentType.setValue(
      this._Associate.EmploymentType
    );
    this.personelDetailsForm.controls.DepartmentId.setValue(
      this._Associate.DepartmentId
    );
    this.personelDetailsForm.controls.TechnologyId.setValue(
      this._Associate.TechnologyId
    )

    if (this._Associate.ReportingManager == null) {
      this.personelDetailsForm.controls.ReportingManager.setValue(
        this._Associate.ManagerId
      );
      this._Associate.ReportingManager = this._Associate.ManagerId;
    }
    else {
      this.personelDetailsForm.controls.ReportingManager.setValue(
        this._Associate.ReportingManager
      );
    }

    let isRMidIsExist;
    if (this.managers) {
      let rmID = this._Associate.ReportingManager;
      isRMidIsExist = this.managers.some(function (el) {
        return el.EmpId == rmID;
      });
    }
    if (isRMidIsExist) {
      this.personelDetailsForm.controls.ReportingManager.setValue(
        this._Associate.ReportingManager
      );
    }
    else {
      this.personelDetailsForm.controls.ReportingManager.setValue(null);
    }
    this.personelDetailsForm.controls.Bgvstatus.setValue(
      this._Associate.Bgvstatus
    );

    this.personelDetailsForm.controls.BgvStartDate.setValue(
      this._Associate.BgvStartDate
    );
    this.personelDetailsForm.controls.BgvCompletionDate.setValue(
      this._Associate.BgvCompletionDate
    );

    this.personelDetailsForm.controls.DesignationId.setValue(
      this._Associate.DesignationId
    );
    this.personelDetailsForm
      .get("contacts")
    ["controls"].currentAddress1.setValue(this._Contacts.currentAddress1);
    this.personelDetailsForm
      .get("contacts")
    ["controls"].currentAddress2.setValue(this._Contacts.currentAddress2);
    this.personelDetailsForm
      .get("contacts")
    ["controls"].currentAddCity.setValue(this._Contacts.currentAddCity);
    this.personelDetailsForm
      .get("contacts")
    ["controls"].currentAddState.setValue(this._Contacts.currentAddState);
    this.personelDetailsForm
      .get("contacts")
    ["controls"].currentAddCountry.setValue(this._Contacts.currentAddCountry);
    this.personelDetailsForm
      .get("contacts")
    ["controls"].currentAddZip.setValue(this._Contacts.currentAddZip);
    this.personelDetailsForm
      .get("contacts")
    ["controls"].permanentAddress1.setValue(this._Contacts.permanentAddress1);
    this.personelDetailsForm
      .get("contacts")
    ["controls"].permanentAddress2.setValue(this._Contacts.permanentAddress2);
    this.personelDetailsForm
      .get("contacts")
    ["controls"].permanentAddCity.setValue(this._Contacts.permanentAddCity);
    this.personelDetailsForm
      .get("contacts")
    ["controls"].permanentAddState.setValue(this._Contacts.permanentAddState);
    this.personelDetailsForm
      .get("contacts")
    ["controls"].permanentAddCountry.setValue(
      this._Contacts.permanentAddCountry
    );
    this.personelDetailsForm
      .get("contacts")
    ["controls"].permanentAddZip.setValue(this._Contacts.permanentAddZip);

    if (this._Associate.contactDetails) {
      this.personelDetailsForm
        .get("contactDetails")
      ["controls"][0].controls.ID.setValue(
        this._Associate.contactDetails[0].ID
      );
      this.personelDetailsForm
        .get("contactDetails")
      ["controls"][0].controls.currentAddress1.setValue(
        this._Contacts.currentAddress1
      );
      this.personelDetailsForm
        .get("contactDetails")
      ["controls"][0].controls.currentAddress2.setValue(
        this._Contacts.currentAddress2
      );
      this.personelDetailsForm
        .get("contactDetails")
      ["controls"][0].controls.currentAddCity.setValue(
        this._Contacts.currentAddCity
      );
      this.personelDetailsForm
        .get("contactDetails")
      ["controls"][0].controls.currentAddState.setValue(
        this._Contacts.currentAddState
      );
      this.personelDetailsForm
        .get("contactDetails")
      ["controls"][0].controls.currentAddCountry.setValue(
        this._Contacts.currentAddCountry
      );
      this.personelDetailsForm
        .get("contactDetails")
      ["controls"][0].controls.currentAddZip.setValue(
        this._Contacts.currentAddZip
      );
      this.personelDetailsForm
        .get("contactDetails")
      ["controls"][1].controls.ID.setValue(
        this._Associate.contactDetails[1].ID
      );
      this.personelDetailsForm
        .get("contactDetails")
      ["controls"][1].controls.permanentAddress1.setValue(
        this._Contacts.permanentAddress1
      );
      this.personelDetailsForm
        .get("contactDetails")
      ["controls"][1].controls.permanentAddress2.setValue(
        this._Contacts.permanentAddress2
      );
      this.personelDetailsForm
        .get("contactDetails")
      ["controls"][1].controls.permanentAddCity.setValue(
        this._Contacts.permanentAddCity
      );
      this.personelDetailsForm
        .get("contactDetails")
      ["controls"][1].controls.permanentAddState.setValue(
        this._Contacts.permanentAddState
      );
      this.personelDetailsForm
        .get("contactDetails")
      ["controls"][1].controls.permanentAddCountry.setValue(
        this._Contacts.permanentAddCountry
      );
      this.personelDetailsForm
        .get("contactDetails")
      ["controls"][1].controls.permanentAddZip.setValue(
        this._Contacts.permanentAddZip
      );
    }

    if (
      this.personelDetailsForm.get("contactDetails")["controls"][0].controls
        .currentAddress1.value
    ) {
      if (
        this.type == "edit" &&
        this.personelDetailsForm.get("contactDetails")["controls"][0].controls
          .currentAddress1.value ==
        this.personelDetailsForm.get("contactDetails")["controls"][1].controls
          .permanentAddress1.value
      ) {
        this.isCheckedSameAddress = true;
      } else {
        this.isCheckedSameAddress = false;
      }
    }

    this.personelDetailsForm.updateValueAndValidity();
  }

  private _filter(value) {
    let filterValue;
    if (value && value.Designation) {
      filterValue = value.Designation.toLowerCase();
      return this.allDesignationsList.filter((option) =>
        option.DesignationName.toLowerCase().includes(filterValue)
      );
    } else {
      return this.allDesignationsList;
    }
  }

  getAllDesignationsList() {
    this._service.getAllDesignationsList().subscribe((resp: GenericType[]) => {
      this.allDesignationsList = [];
      this.allDesignationsList = resp;
    });
  }

  clearFields(e, event: any) {
    if (e == "Designation") {
      event.stopPropagation();
      this.personelDetailsForm.controls.Designation.reset();
      this.RoleTypesByGradeList = [];
      this.personelDetailsForm.controls.RoleTypeId.reset();
    }
    if (e == "RoleTypeId") {
      event.stopPropagation();
      this.personelDetailsForm.controls.RoleTypeId.reset();
    }
    this.getFilteredDesignationsIds();

  }

  getFilteredDesignationsIds() {
    this.filteredDesignationIds = this.personelDetailsForm.valueChanges.pipe(
      startWith(""),
      map((value) => this._filter(value))
    );
  }

  getAllGradesList() {
    this._service.getAllGradesList().subscribe((resp: GenericType[]) => {
      this.allGradessList = [];
      this.allGradessList = resp;
    });
  }
  displayFn(user: any) {
    return user && user.label ? user.label : '';
  }
  
  getRoleTypesByGrade(grade) {
    return;
    //COMMENTED BY KALYAN PENUMUTCHU ON 25MAY2024 AS PART OF AWS CLOUD MIGRATION. THIS NEEDS TO BE OPENED UP ONCE KRA MODULE IS DEPLOYED TO CLOUD
    /*this.masterDataService.GetRoleTypesByGrade(grade).subscribe((resp: GenericType[]) => {
      this.RoleTypesByGradeList = [];
      resp['Items'].forEach(element => {
        this.RoleTypesByGradeList.push({label:element.RoleTypeName, value: element.RoleTypeId})
      
      });
    },(error) => {
      this._snackBar.open(error.error), "", {
        duration: 1000,
        panelClass:['error-alert'],
        horizontalPosition: "right",
        verticalPosition: "top",
      }
    }); */
  }

  getGradeNameAndID(e) {
    this.personelDetailsForm.controls.GradeId.setValue(e.GradeId);
    this.getRoleTypesByGrade(e.GradeId)
  }

  getEmployementTypeeNameNID(e) {
    this.personelDetailsForm.controls.EmploymentType.setValue(
      e.EmployeeTypeCode
    );
  }

  showTechnologyField(e) {
    if (e.value === 1) {
      this.isRequired = true
      this.personelDetailsForm.controls.TechnologyId.setValidators([Validators.required]);
    }
    else {
      this.isRequired = false
      this.personelDetailsForm.controls.TechnologyId.setValidators([]);
      this.personelDetailsForm.controls.TechnologyId.setValue(null
      );
    }
    this.personelDetailsForm.controls['ReportingManager'].reset();
    this.GetManagersAndLead(this.personelDetailsForm.controls.DepartmentId.value);

    
  }
 async GetManagersAndLead(departmentId:number) {
   await this.masterDataService
      .GetManagersAndCompetencyLeads(departmentId)
      .subscribe((res: GenericType[]) => {
        this.managers = res;
         this.personelDetailsForm.controls.ReportingManager.setValue(this._Associate.ReportingManager);
        
      });
    }

    changeTextToUppercase(field) {
       let value = this.personelDetailsForm.controls[field].value.toUpperCase();
       this.personelDetailsForm.controls.Pannumber.setValue(value);
    }
}
