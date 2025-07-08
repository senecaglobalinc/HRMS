import { Component, Injector, OnInit, Inject, ViewChild } from "@angular/core";
import { Http } from "@angular/http";
import * as moment from "moment";
import { ActivatedRoute, Router } from "@angular/router";
import { EmployeeType } from "../models/employeetype.model";
import { Grade } from "../../admin/models/grade.model";
import { Department } from "../../admin/models/department.model";
import { Designation } from "../../admin/models/designation.model";
//import {Technology} from "../../admin/models/designation.model";
//import {HRAdvisor} from "../../admin/models/designation.model";
import { GenericType } from "../../../models/dropdowntype.model";
import { Associate } from "../models/associate.model";
import { ProspectiveassociateService } from "../services/prospectiveassociate.service";
import { MasterDataService } from "../../../services/masterdata.service";
import * as servicePath from '../../../service-paths';
import { ProjectsService } from '../../onboarding/services/projects.service';
import { MessageService } from 'primeng/api';
import { FormGroup, NgForm } from "../../../../../node_modules/@angular/forms";
import { ConfirmationService } from 'primeng/api';

@Component({
  selector: 'app-edit-prospective-associate',
  templateUrl: './edit-prospective-associate.component.html',
  styleUrls: ['./edit-prospective-associate.component.scss'],
  providers: [MessageService, ConfirmationService]



})
export class EditProspectiveAssociateComponent implements OnInit {
  newAssociate: Associate;
  filteredDesignationIds: GenericType[] = [];
  designation: Designation;
  componentName: string;
  ProspectiveAssociateList: Array<Associate>;
  empTypes: any[] = [];
  technologies: any[] = [];
  departments: any[] = [];
  hradvisors: any[] = [];
  reportingmanagers: any[] = [];
  id: number;
  isRequired: boolean = false;
  lastDate: any;
  disableReason: boolean = true;
  dropped: boolean = false;

  constructor(
    private _router: Router,
    private masterDataService: MasterDataService,
    private projectsService: ProjectsService,
    private _prospectiveAssociateService: ProspectiveassociateService,
    private actRoute: ActivatedRoute,
    private messageService: MessageService,
    private confirmationService: ConfirmationService

  ) {

    this.newAssociate = new Associate();
  }


  ngOnInit() {
    this.getDates();
    this.actRoute.params.subscribe(params => { this.id = params["id"]; });
    this.projectsService.list(servicePath.API.EmployeeType.list).subscribe((res: any[]) => (this.empTypes = res));
    this.projectsService.GetList(servicePath.API.Technology.list).subscribe((res: any[]) => (this.technologies = res));
    this.projectsService.GetList(servicePath.API.Department.list).subscribe((res: any[]) => (this.departments =
      res));
    this.projectsService.GetList(servicePath.API.HRAdvisor.list).subscribe((res: any[]) => (this.hradvisors = res));
    this.masterDataService.GetManagersAndCompetencyLeads().subscribe(res => (this.reportingmanagers = res));
    this._prospectiveAssociateService
      .getPADetailsById(this.id)
      .subscribe((data: Associate) => {
        debugger
        if (data != null) {
          data.DateofJoining = moment(data.JoiningDate).format("YYYY-MM-DD");
          this.newAssociate = data;
          if (this.newAssociate.Designation && this.newAssociate.DesignationId) {
            this.designation = new Designation();
            this.designation.DesignationId = this.newAssociate.DesignationId;
            this.designation.DesignationName = this.newAssociate.Designation;
          }
          if (this.newAssociate.TechnologyID == null || this.newAssociate.TechnologyID == "") {
            this.newAssociate.TechnologyID = "";
          }

          if (this.newAssociate.DepartmentId == 1) this.isRequired = true;

          if (this.newAssociate.GradeId == null || this.newAssociate.GradeId == "") {
            this.newAssociate.GradeId = "";
          }
          if (this.newAssociate.DesignationId == null || this.newAssociate.DesignationId == 0) {
            this.newAssociate.DesignationId = 0;
          }
          if (this.newAssociate.EmploymentType == null || this.newAssociate.EmploymentType == "") {
            this.newAssociate.EmploymentType = "";
          }
          if (this.newAssociate.ManagerId == null || this.newAssociate.ManagerId == "" || this.newAssociate.ManagerId == 0) {
            this.newAssociate.ManagerId = "";
          }
          if (this.newAssociate.HRAdvisorName == null || this.newAssociate.HRAdvisorName == "") {
            this.newAssociate.HRAdvisorName = "";
          }
          if (this.newAssociate.DepartmentId == null || this.newAssociate.DepartmentId == "") {
            this.newAssociate.DepartmentId = "";
          }
        }
        else
          this.messageService.add({ severity: 'error', summary: 'Failure Message', detail: 'Failed to get the prospective associate details' });
        //swal("", "Failed to get the prospective associate details", "error");
      },
        (error: any) => {
          if (error.error != undefined && error.error != "")
            this.messageService.add({ severity: 'error', summary: 'Failure Message', detail: 'Failed to get the prospective associate details' });

        }
      );
  }

  filteredDesignation(event: any): void {
    let suggestionString = event.query;
    this.masterDataService.GetDesignationByString(suggestionString).subscribe(
      (desginationResponse: GenericType[]) => {
        this.filteredDesignationIds = [];
        this.filteredDesignationIds = desginationResponse;
      }
    );
  }

  getGradeByDesignation(designation: Designation) {
    if (designation && designation.DesignationId) {
      this.masterDataService.getGradeByDesignation(designation.DesignationId).subscribe((gradeResponse: Grade) => {
        if (gradeResponse) {
          this.newAssociate.GradeName = gradeResponse.GradeName;
          this.newAssociate.GradeId = gradeResponse.GradeId;
        }
      },
      );
    }
    else {
      this.newAssociate.GradeName = "";
    }
  }

  // onlyNumbers(event: any) {
  //   this._commonService.onlyNumbers(event);
  // }

  getDates() {
    var date = new Date(),
      y = date.getFullYear(),
      m = date.getMonth();
    this.lastDate = new Date(y, m + 2, 0);
    // this.startDate= new Date(y, m - 2, 0)
  }
  onCancel() {
    this.confirmationService.confirm({
      message: 'Are you sure you want to cancel it?',
      accept: () => {
        this._router.navigate(["/associates/prospectiveassociate"]);
      },

      reject: () => {
        this.onCancel();

      }
    });
  }
  // onCancel() {
  //   this._router.navigate(["/associates/prospectiveassociate"]);
  // }

  requiredTechnolgy(event: any) {
    var target = event.target.value;
    if (target == 1) this.isRequired = true;
    else {
      this.isRequired = false;
      this.newAssociate.TechnologyID = "";
    }
  }

  onUpdate() {
    if (this.dropped == true) {
      if (this.newAssociate.ReasonForDropOut) {
        this.newAssociate.ReasonForDropOut = this.newAssociate.ReasonForDropOut
          .trim()
          .replace(/  +/g, " ");
      }
      if (this.newAssociate.ReasonForDropOut == "") {
        this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: 'Please Enter valid reason' });
        //swal("", "Please Enter valid reason", "warning");
        return false;
      }
    }
    if (this.designation && this.designation.DesignationId) {
      this.newAssociate.DesignationId = this.designation.DesignationId;
    } else {
      this.newAssociate.DesignationId = 0;
      this.newAssociate.Gender = "";
    }
    this.newAssociate.JoinDate = new Date(this.newAssociate.DateofJoining); //,'dd/mm/yyyy',true);

    if (
      this.newAssociate.FirstName.trim().length == 0 ||
      this.newAssociate.LastName.trim().length == 0
    ) {
      this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: 'Please complete prospective associate details' });
      //swal("", "Please complete prospective associate details", "warning");
      return false;
    }
    if (this.newAssociate.DesignationId == 0) {
      this.messageService.add({ severity: 'warn', summary: 'Warning Message', detail: 'Please select designation' });
      //swal("", "Please select designation.", "warning");
      return false;
    }
    this._prospectiveAssociateService.UpdatePADetails(this.newAssociate).subscribe((data: boolean) => {
      // swal(
      //   "",
      //   "Prospective associate details updated successfully.",
      //   "success"
      // );
      if (data != null) {
        this.messageService.add({ severity: 'success', summary: 'success Message', detail: 'Prospective associate details updated successfully.' });
        setTimeout(() => {
          this._router.navigate(["/associates/prospectiveassociate"]);
        }, 1000);
      }
      else {
        this.messageService.add({ severity: 'error', summary: 'Failure Message', detail: 'Failed to update' });

      }
      // this._router.navigate(["/associates/prospectiveassociate"]);

    },
      error => {
        this.messageService.add({ severity: 'error', summary: 'Error message', detail: error.error });
      }
    );

    return true;
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

  updateDropOutReason() {
    if (this.dropped == true) this.disableReason = true;
    else {
    this.disableReason = false;
      this.newAssociate.ReasonForDropOut = null;
    }
  }
}

