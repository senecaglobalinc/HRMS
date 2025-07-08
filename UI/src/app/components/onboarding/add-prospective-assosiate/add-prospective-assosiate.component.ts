import {Component, OnInit }from '@angular/core';
import {FormBuilder, Validators, FormGroup } from "@angular/forms";
import { Router, ActivatedRoute }from "@angular/router";    
import {  GenericType} from    "../../../models/dropdowntype.model";   
import {Grade}   from "../../admin/models/grade.model"; 
import * as servicePath  from '../../../service-paths';  
import { MasterDataService } from  "../../../services/masterdata.service";     
import {  Associate } from  'src/app/components/onboarding/models/associate.model';  
import { ProjectsService } from '../../onboarding/services/projects.service'; 
import { MessageService } from 'primeng/api';
import {ConfirmationService} from 'primeng/api';
import { Designation } from '../../admin/models/designation.model';
   

    
@Component({selector: 'app-add-prospective-assosiate', 
            templateUrl: './add-prospective-assosiate.component.html',
            styleUrls: ['./add-prospective-assosiate.component.scss'],
            providers : [MessageService,ConfirmationService]
     
        })
    
    export class AddProspectiveAssosiateComponent implements OnInit {
      newAssociate: Associate;
      filteredDesignationIds: GenericType[];
      designation: Designation;
      componentName: string;
      empTypes: any[] = [];
      technologies: any[] = [];
      departments: any[] = [];
      hradvisors: any[] = [];
      reportingmanagers: any[] = [];
      isRequired: boolean = false;
      lastDate: any;
      
   
      newAssocaiteForm: FormGroup;
     
    constructor(
      private _router: Router,
    private _actRoute: ActivatedRoute,
    private _formBuilder: FormBuilder,
    private projectsService: ProjectsService,
    private masterDataService: MasterDataService,
    private messageService : MessageService,
    private confirmationService: ConfirmationService
      
           
    ) {
       
      this.newAssociate = new Associate();
         
    }
    ngOnInit() { 
 
    this.newAssociate.MaritalStatus = "";
    this.newAssociate.Gender = "";
    this.newAssociate.GradeId = "";
    this.newAssociate.DesignationId =0;
    this.newAssociate.EmploymentType = "";
    this.newAssociate.TechnologyID = "";
    this.newAssociate.DepartmentId = "";
    this.newAssociate.ManagerId = "";
    this.newAssociate.HRAdvisorName = "";
    this.getDates();

    // this.projectsService.list(servicePath.API.PAssociate.list).subscribe((res:any[]) => (this.empTypes =
    //   res));
     this.projectsService.list(servicePath.API.EmployeeType.list).subscribe((res:any[]) => (this.empTypes = res));
     this.projectsService.GetList(servicePath.API.Technology.list).subscribe((res:any[]) => (this.technologies = res));
      
      this.projectsService.GetList(servicePath.API.Department.list).subscribe((res:any[]) => (this.departments =
      res));
      
      this.projectsService.GetList(servicePath.API.HRAdvisor.list).subscribe((res:any[]) => (this.hradvisors = res));
    this.masterDataService
      .GetManagersAndCompetencyLeads().subscribe((res: GenericType[]) => {
        this.reportingmanagers = res;
      });
    this.newAssocaiteForm = this._formBuilder.group({
      firstName: ["", [Validators.required]],
      middleName: ["", [Validators.required]],
      lastName: ["", [Validators.required]],
      PersonalEmailAddress: ["", [Validators.required]],
      dateOfJoining: ["", [Validators.required]],
      MobileNo: ["", [Validators.required]],
      ddlgradeName: ["", [Validators.required]],
      ddldesignationID: ["", [Validators.required]],
      ddlemploymentType: ["", [Validators.required]],
      ddltechnology: [""],
      ddlDepartmentId: ["", [Validators.required]],
      ddlmanagerId: ["", [Validators.required]],
      ddlHRAdvisorName: ["", [Validators.required]],
      recruitedBy: [""]
    });
  }

  onCreate() {
      if (this.designation && this.designation.DesignationId) {
      this.newAssociate.DesignationId = this.designation.DesignationId;
    }else
    {
        this.newAssociate.DesignationId =0;
        this.newAssociate.Gender="";
    }
    if (
      this.newAssociate.DateofJoining == "" ||
      this.newAssociate.DateofJoining == undefined
    ) {
      this.messageService.add({severity:'warn', summary: 'Warning Message', detail:'Please enter joining date'});
      return false;
    }

    if (
      this.newAssociate.FirstName.trim().length == 0 ||
      this.newAssociate.LastName.trim().length == 0
    ) {
      this.messageService.add({severity:'warn', summary: 'Warning Message', detail:'Please complete prospective associate details'});
     // swal("", "Please complete prospective associate details", "warning");
      return false;
    }

    this.newAssociate.JoinDate = new Date(this.newAssociate.DateofJoining);
    if(this.newAssociate.DesignationId==0){
       this.messageService.add({severity:'warn', summary: 'Warning Message', detail:'Please select designation'});
      return false;
    }
    
    this.projectsService.create(this.newAssociate).subscribe((res:any) => {
      this.newAssociate.Id = res;
      if(res != null){
        this.messageService.add({severity:'success', summary: 'success Message', detail:'Prospective associate details saved successfully.'});
        this.onClear();
        setTimeout(() => 
              {
                this._router.navigate(["/associates/prospectiveassociate"]);
              }, 1000);
      }
      else{
        this.messageService.add({severity:'error', summary: 'failed Message', detail:'Failed to save prospective associate details'});
      }
      },
       error => {
        this.messageService.add({severity:'error', summary: 'Error message', detail:error.error}); 
       }
    );
  }


onCancel() {
  this.confirmationService.confirm({
      message: 'Are you sure that cancel this?',
      accept: () => {
        this._router.navigate(["/associates/prospectiveassociate"]);
      },
      
        reject : ()=>{
            this.onCancel();
        
       }
  });
}

  // onCancel() {
  //   this._router.navigate(["/associates/prospectiveassociate"]);
  // }

  getDates() {
    var date = new Date(),
      y = date.getFullYear(),
      m = date.getMonth();
    this.lastDate = new Date(y, m + 2, 0);
  
  }

  filteredDesignation(event: any): void {
    let suggestionString = event.query;
    this.masterDataService.GetDesignationByString(suggestionString).subscribe(
      (desginationResponse: GenericType[]) => {
        this.filteredDesignationIds = [];
        this.filteredDesignationIds = desginationResponse;
      },
      // (error: any) => {
      //   if (error._body != undefined && error._body != "")
      //     this._commonService
      //       .LogError(this.componentName, error._body)
      //       .then((data: any) => {});
      // }
    );
  }

  getGradeByDesignation(designation: Designation) {
  if(designation && designation.DesignationId){
    this.masterDataService.getGradeByDesignation(designation.DesignationId).subscribe((gradeResponse:Grade)=>{
      if(gradeResponse){
     this.newAssociate.GradeName=gradeResponse.GradeName;
     this.newAssociate.GradeId=gradeResponse.GradeId;
      }
    },
    // (error: any) => {
    //     if (error._body != undefined && error._body != "")
    //       this._commonService
    //         .LogError(this.componentName, error._body)
    //         .then((data: any) => {});
    //   }
    );
  }
  else{
     this.newAssociate.GradeName="";
  }
  }

  // onClear() {
  //   this.confirmationService.confirm({
  //       message: 'Are you sure that you want clear?',
  //       accept: () => {
  //         this.newAssocaiteForm.reset();
  //       },
        
  //         reject : ()=>{
  //             this.onCancel();
          
  //        }
  //   });
  // }
  onClear() {
    this.newAssocaiteForm.reset();
    
    }

  onlyNumbers(event: any) {
   // this._commonService.onlyNumbers(event);
  }

  requiredTechnolgy(event: any) {
    var target = event.target.value;
    if (target == 1) this.isRequired = true;
    else {
      this.isRequired = false;
      this.newAssociate.TechnologyID = "";
    }
  }

  onFiltersChanged(event: any) {}

  selected(grid: any) {}

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
}

    
    
    
    
    