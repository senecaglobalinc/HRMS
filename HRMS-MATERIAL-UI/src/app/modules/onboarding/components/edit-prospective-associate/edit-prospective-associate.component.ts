import { Component, OnInit, Injector, Inject, ViewChild } from "@angular/core";
// import { Http } from "@angular/http";
import * as moment from "moment";
import {
  FormBuilder,
  Validators,
  FormGroup,
  FormGroupDirective,
  ControlContainer
} from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { EmployeeType } from "../../models/employeetype.model";
import { Grade } from "../../../admin/models/grade.model";
import { Department } from "../../../admin/models/department.model";
import { Designation } from "../../../admin/models/designation.model";
import { Associate } from "../../models/associate.model";
import { GenericType } from "../../../../modules/master-layout/models/dropdowntype.model";
import { ProspectiveAssosiateService } from "../../services/prospective-assosiate.service";
import { MasterDataService } from "../../../../modules/master-layout/services/masterdata.service";
import { ProjectsService } from "../../services/projects.service";
import * as servicePath from "../../../../core/service-paths";
import { NgxSpinnerService } from 'ngx-spinner';
import {
  MatSnackBar,
  MatSnackBarHorizontalPosition,
  MatSnackBarVerticalPosition
} from "@angular/material/snack-bar";
// import { FormGroup, NgForm } from "../../../../../node_modules/@angular/forms";
//import {Technology} from "../../admin/models/designation.model";
//import {HRAdvisor} from "../../admin/models/designation.model";
import { MatDialog } from "@angular/material/dialog";
import { ConfirmCancelComponent } from "../../../master-layout/components/confirm-cancel/confirm-cancel.component";
import { themeconfig } from "../../../../../themeconfig";
import { MatCheckboxChange } from "@angular/material/checkbox";
import { ThrowStmt } from "@angular/compiler";

@Component({
  selector: "app-edit-prospective-associate",
  templateUrl: "./edit-prospective-associate.component.html",
  styleUrls: ["./edit-prospective-associate.component.scss"]
})
export class EditProspectiveAssociateComponent implements OnInit {
  // newAssociate: Associate;
  newAssociate: any;
  filteredDesignationIds: GenericType[] = [];
  designation: Designation;
  componentName: string;
  ProspectiveAssociateList: Array<Associate>;
  empTypes: any[] = [];
  technologies: any[] = [];
  departments: any[] = [];
  hradvisors: any[] = [];
  reportingmanagers: any[] = [];
  designationList = [];
  id: number;
  isRequired: boolean = false;
  isDropped: boolean = false;
  lastDate: any;
  disableReason: boolean = true;
  dropped: boolean = false;
  isShow: boolean = false;
  themeConfigInput = themeconfig.formfieldappearances;
  newAssocaiteForm: FormGroup;
  isSubmitted: boolean = false;
  @ViewChild(FormGroupDirective) formGroupDirective: FormGroupDirective;

  constructor(
    private _router: Router,
    private masterDataService: MasterDataService,
    public dialog: MatDialog,
    private projectsService: ProjectsService,
    private _prospectiveAssociateService: ProspectiveAssosiateService,
    private actRoute: ActivatedRoute,
    private _snackBar: MatSnackBar,
    private _formBuilder: FormBuilder,
    private spinner: NgxSpinnerService
  ) {
    // this.newAssociate = new Associate();
  }

  ngOnInit(): void {
    this.spinner.show();
    this.newAssocaiteForm = this._formBuilder.group({
      FirstName: ["", [Validators.required]],
      MiddleName: [""],
      LastName: ["", [Validators.required]],
      PersonalEmailAddress: ["", [Validators.required, Validators.email, , Validators.pattern(/^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/)]],
      DateOfJoining: ["", [Validators.required]],
      MobileNo: [
        "",
        [Validators.required, Validators.pattern("^[1-9]{1}[0-9]{9}$")]
      ],
      GradeName: ["", [Validators.required]],
      Designation: ["", [Validators.required]],
      EmploymentType: ["", [Validators.required]],
      TechnologyID: ["", [Validators.required]],
      Department: ["", [Validators.required]],
      ManagerId: ["", [Validators.required]],
      HRAdvisorName: ["", [Validators.required]],
      RecruitedBy: [""],
      dropped: [false],
      DropOutReason: ["", [Validators.required]]
    },
      // this.validationDropped()
    );

    this.getDates();
    this.getDesignations();
    this.actRoute.params.subscribe(params => {
      this.id = params["id"];
    });
    this.newAssocaiteForm.controls.GradeName.disable();

    this.projectsService
      .list(servicePath.API.EmployeeType.list)
      .subscribe((res: any[]) => {
        this.empTypes = res;
      });
    this.projectsService
      .GetList(servicePath.API.Technology.activelist)
      .subscribe((res: any[]) => {
        this.technologies = res;
      });
    this.projectsService
      .GetList(servicePath.API.Department.list)
      .subscribe((res: any[]) => {
        this.departments = res;
      });
    this.projectsService
      .GetList(servicePath.API.HRAdvisor.list)
      .subscribe((res: any[]) => (this.hradvisors = res));
    this.masterDataService
      .GetManagersAndCompetencyLeads()
      .subscribe(res => (this.reportingmanagers = res));
    this._prospectiveAssociateService.getPADetailsById(this.id).subscribe(
      (data: Associate) => {
        this.spinner.hide();
        if (data != null) {
          this.newAssociate = data;
          this.newAssocaiteForm.patchValue(data);
          this.newAssocaiteForm.controls.Department.setValue(data.DepartmentId);
          this.newAssocaiteForm.controls["Designation"].setValue({
            Name: this.newAssociate.Designation,
            Id: this.newAssociate.DesignationId
          });

          this.newAssocaiteForm.controls.Department.setValue(data.DepartmentId);
          if (
            this.newAssociate.TechnologyID == null ||
            this.newAssociate.TechnologyID == ""
          ) {
            this.newAssociate.TechnologyID = "";
          }

          if (this.newAssociate.DepartmentId == 1) this.isRequired = true;
          if (
            this.newAssociate.GradeId == null ||
            this.newAssociate.GradeId == ""
          ) {
            this.newAssociate.GradeId = "";
          }
          if (
            this.newAssociate.DesignationId == null ||
            this.newAssociate.DesignationId == 0
          ) {
            this.newAssociate.DesignationId = 0;
          }
          if (
            this.newAssociate.EmploymentType == null ||
            this.newAssociate.EmploymentType == ""
          ) {
            this.newAssociate.EmploymentType = "";
          }
          if (
            this.newAssociate.ManagerId == null ||
            this.newAssociate.ManagerId == "" ||
            this.newAssociate.ManagerId == 0
          ) {
            this.newAssociate.ManagerId = "";
          }
          if (
            this.newAssociate.HRAdvisorName == null ||
            this.newAssociate.HRAdvisorName == ""
          ) {
            this.newAssociate.HRAdvisorName = "";
          }
          if (
            this.newAssociate.DepartmentId == null ||
            this.newAssociate.DepartmentId == ""
          ) {
            this.newAssociate.DepartmentId = "";
          }
          this.newAssocaiteForm.controls.TechnologyID.setValue(this.newAssociate.TechnologyID);
        } else {
          this.spinner.hide();
          this._snackBar.open(
            "Failed to get the prospective associate details.",
            "x",
            {
              duration: 1000,
              panelClass:['error-alert'],
              horizontalPosition: "right",
              verticalPosition: "top"
            }
          );
        }
        //swal("", "Failed to get the prospective associate details", "error");
      },
      (error: any) => {
        this.spinner.hide();
        if (error.error != undefined && error.error != "")
          this._snackBar.open(
            "Failed to get the prospective associate details.",
            "x",
            {
              duration: 1000,
              panelClass:['error-alert'],
              horizontalPosition: "right",
              verticalPosition: "top"
            }
          );

        // this.messageService.add({severity:'error', summary: 'Failure Message', detail:'Failed to get the prospective associate details'});
      }
    );
  }

  validationDropped() {
    return (droppedVal: FormGroup) => {
      return droppedVal.value.dropped && droppedVal.value.DropOutReason
        ? { required: true }
        : null;
    };
  }

  getDesignations(): void {
    this.masterDataService.GetDesignationList().subscribe((data: any[]) => {
      this.designationList = data;
    });
  }

  getDates() {
    var date = new Date(),
      y = date.getFullYear(),
      m = date.getMonth();
    this.lastDate = new Date(y, m + 2, 0);
    // this.startDate= new Date(y, m - 2, 0)
  }

  compareCategoryObjects(object1: any, object2: any) {
    return object1 && object2 && object1.id == object2.id;
  }

  filteredDesignations(event) {
    if (event) {
      let suggestionString = event.target.value.toLowerCase();
      this.filteredDesignationIds = [];
      this.designationList.forEach(v => {
        if (
          this.filteredDesignationIds.findIndex(
            x => x.Name == v.DesignationName
          ) === -1 &&
          v.DesignationName.toLowerCase().indexOf(suggestionString) > -1
        ) {
          this.filteredDesignationIds.push({
            Id: v.DesignationId,
            Name: v.DesignationName
          });
        }
      });
    } else {
      this.filteredDesignationIds = [];
      this.pushFilteredDesignationIds();
    }
  }

  pushFilteredDesignationIds() {
    this.filteredDesignationIds = [];
    for (let i = 0; i < this.designationList.length; i++) {
      this.filteredDesignationIds.push({
        Id: this.designationList[i].DesignationId,
        Name: this.designationList[i].DesignationName
      });
    }
  }

  displayFn(designation: any) {
    return designation && designation ? designation.Name : "";
  }

  openDialog(): void {
    const dialogRef = this.dialog.open(ConfirmCancelComponent, {
      disableClose: true,
      hasBackdrop: true,
      width: "300px",
      data: { route: "/associates/prospectiveassociate" }
    });
  }

  requiredTechnolgy(event: any) {
    var target = event.value;
    if (target == 1) this.isRequired = true;
    else {
      this.isRequired = false;
      this.newAssociate.TechnologyID = "";
    }
  }

  onlychar(event: any) {
    let k: any;
    k = event.charCode; //         k = event.keyCode;  (Both can be used)
    return (k > 64 && k < 91) || (k > 96 && k < 123) || k == 8 || k == 32;
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

  onUpdate() {
    this.isSubmitted = true;
    if (this.newAssocaiteForm.get('dropped').value == true) {
      this.newAssocaiteForm.get('DropOutReason').setValidators([Validators.required]);
      this.isDropped = true;
    }
    else {
      this.newAssocaiteForm.get("DropOutReason").setErrors(null);
      this.newAssocaiteForm.get('DropOutReason').clearValidators();
      this.isDropped = false;
    }
    this.newAssocaiteForm.get('DropOutReason').updateValueAndValidity();
     if(this.newAssocaiteForm.get('Department').value!=1)
      {
      var technology =this.newAssocaiteForm.get('TechnologyID');
      technology.reset();
      technology.clearValidators();
      technology.updateValueAndValidity();
      }
    if (this.newAssocaiteForm.valid) {
      if (this.designation && this.designation.DesignationId) {
        this.newAssociate.DesignationId = this.designation.DesignationId;
      } else {
        this.newAssociate.DesignationId = 0;
        this.newAssociate.Gender = "";
      }
      this.newAssociate.JoinDate = this.newAssocaiteForm.value.DateOfJoining; //,'dd/mm/yyyy',true);
      this.newAssociate.DateOfJoining = this.newAssocaiteForm.value.DateOfJoining;
      this.newAssociate.DepartmentId = this.newAssocaiteForm.value.Department;
      this.newAssociate.DesignationId = this.newAssocaiteForm.value.Designation.Id;
      this.newAssociate.Designation = this.newAssocaiteForm.value.Designation.Name;
      this.newAssociate.dropped = this.newAssocaiteForm.value.dropped;
      this.newAssociate.DropOutReason = this.newAssocaiteForm.value.DropOutReason;
      this.newAssociate.EmploymentType = this.newAssocaiteForm.value.EmploymentType;
      this.newAssociate.FirstName = this.newAssocaiteForm.value.FirstName;
      this.newAssociate.MiddleName = this.newAssocaiteForm.value.MiddleName;
      this.newAssociate.LastName = this.newAssocaiteForm.value.LastName;
      this.newAssociate.HRAdvisorName = this.newAssocaiteForm.value.HRAdvisorName;
      this.newAssociate.ManagerId = this.newAssocaiteForm.value.ManagerId;
      this.newAssociate.MobileNo = this.newAssocaiteForm.value.MobileNo;
      this.newAssociate.PersonalEmailAddress = this.newAssocaiteForm.value.PersonalEmailAddress;
      this.newAssociate.RecruitedBy = this.newAssocaiteForm.value.RecruitedBy;
      this.newAssociate.TechnologyID = this.newAssocaiteForm.value.TechnologyID;
      this.newAssociate.ReasonForDropOut = this.newAssocaiteForm.value.DropOutReason;

      this.newAssociate.EmpName =
        this.newAssocaiteForm.value.FirstName +
          " " +
          this.newAssocaiteForm.value.MiddleName
          ? this.newAssocaiteForm.value.MiddleName + " "
          : "" + this.newAssocaiteForm.value.LastName;
          this.newAssociate.DateofJoining = moment(this.newAssocaiteForm.value.DateOfJoining).format("YYYY-MM-DD");
          this.newAssociate.JoinDate = this.newAssociate.DateofJoining;
          this.newAssociate.DateOfJoining = this.newAssociate.DateofJoining;
      this._prospectiveAssociateService
        .UpdatePADetails(this.newAssociate)
        .subscribe(
          (data: boolean) => {
            if (data != null) {
              let message = "Prospective associate details updated successfully.";
              if (this.isDropped)
                message = "Prospective associate details successfully marked for a drop.";

              this._snackBar.open(
                message, "x",
                {
                  duration: 3000,
                  horizontalPosition: "right",
                  verticalPosition: "top"
                }
              );

              setTimeout(() => {
                this._router.navigate(["/associates/prospectiveassociate"]);
              }, 1000);
            } else {
              this._snackBar.open("Failed to update.", "x", {
                duration: 1000,
                horizontalPosition: "right",
                verticalPosition: "top"
              });
            }

          },
          error => {
            this._snackBar.open(error.error.title, "x", {
              duration: 1000,
              panelClass:['error-alert'],
              horizontalPosition: "right",
              verticalPosition: "top"
            });
            // this.messageService.add({severity:'error', summary: 'Error message', detail:error.error});
          }
        );

      return true;
    }
  }

  getGradeByDesignation(designation) {
    if (designation && designation.Id) {
      this.masterDataService.getGradeByDesignation(designation.Id).subscribe(
        (gradeResponse: Grade) => {
          if (gradeResponse) {
            this.newAssociate.GradeName = gradeResponse.GradeName;
            this.newAssociate.GradeId = gradeResponse.GradeId;
            this.newAssocaiteForm.controls.GradeName.setValue(
              gradeResponse.GradeName
            );
          }
        },
        (error: any) => { }
      );
    } else {
      this.newAssociate.GradeName = "";
    }
  }

  onChange(event: any) {
    event.stopPropagation();
    this.newAssocaiteForm.get("DropOutReason").reset();
    if (this.isSubmitted) {
      this.newAssocaiteForm.get("DropOutReason").setErrors(null);
      this.newAssocaiteForm.get('DropOutReason').clearValidators();
      this.newAssocaiteForm.get('DropOutReason').updateValueAndValidity();
    }
  }
  setDepartmentName(e) {
    this.newAssociate.Department = e.Description;
  }
}
