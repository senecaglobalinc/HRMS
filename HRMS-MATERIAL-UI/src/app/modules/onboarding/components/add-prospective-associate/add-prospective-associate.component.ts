import {Component, OnInit, ViewChild }from '@angular/core';
import {FormBuilder, Validators, FormGroup,FormGroupDirective } from "@angular/forms";
import { Router, ActivatedRoute }from "@angular/router"; 
import {  GenericType} from    "../../../../modules/master-layout/models/dropdowntype.model";
import {Grade}   from "../../../admin/models/grade.model";    
import { Designation } from '../../../admin/models/designation.model';
import * as servicePath  from '../../../../core/service-paths';
import { MasterDataService } from  '../../../../modules/master-layout/services/masterdata.service';
import {  Associate } from  '../../models/associate.model';
import { ProjectsService } from '../../services/projects.service'; 
import {MatDialog} from '@angular/material/dialog';
import {ConfirmCancelComponent} from '../../../master-layout/components/confirm-cancel/confirm-cancel.component'
import * as moment from "moment";
import { themeconfig } from '../../../../../themeconfig';
// import { ConfirmCancelComponent } from '../../../../core/components/confirm-cancel';
import {
  MatSnackBar,
  MatSnackBarHorizontalPosition,
  MatSnackBarVerticalPosition,
} from '@angular/material/snack-bar';


@Component({
  selector: 'app-add-prospective-associate',
  templateUrl: './add-prospective-associate.component.html',
  styleUrls: ['./add-prospective-associate.component.scss']
})
export class AddProspectiveAssociateComponent implements OnInit {

  newAssociate: Associate;
      filteredDesignationIds: any;
      designation: Designation;
      componentName: string;
      empTypes: any[] = [];
      technologies: any[] = [];
      departments: any[] = [];
      hradvisors: any[] = [];
      reportingmanagers: any[] = [];
      isRequired: boolean = false;
      lastDate: any;
      designationList = [];
      desgntnLst = [];
      minDate: Date;
      maxDate: Date;
      themeConfigInput = themeconfig.formfieldappearances;
      newAssocaiteForm: FormGroup;
      isNonDelivery: boolean = false;
      @ViewChild(FormGroupDirective) formGroupDirective: FormGroupDirective;

      

  constructor(private _router: Router,
    private _actRoute: ActivatedRoute,public dialog: MatDialog,
    private _formBuilder: FormBuilder,private _snackBar: MatSnackBar,
    private projectsService: ProjectsService,private masterDataService: MasterDataService) {
      this.newAssociate = new Associate();
     }

     openDialog(): void {
      const dialogRef = this.dialog.open(ConfirmCancelComponent, {
        disableClose: true,
        hasBackdrop: true,
        width: '300px',
        data: {route:'/associates/prospectiveassociate'}
      });
    }

  ngOnInit(): void {
    this.isNonDelivery = false;
    this.newAssociate.MaritalStatus = "";
    this.newAssociate.Gender = "";
    this.newAssociate.GradeId = "";
    this.newAssociate.DesignationId =0;
    this.newAssociate.EmploymentType = "";
    this.newAssociate.TechnologyID = "";
    this.newAssociate.DepartmentId = "";
    this.newAssociate.ManagerId = "";
    this.newAssociate.HRAdvisorName = "";

    
    // this.minDate = new Date(2007, 0, 1);
    // this.maxDate = new Date(2050, 11, 31);
    var date = new Date();
    this.minDate=new Date(date.getFullYear(), date.getMonth()-1,1);
    this.maxDate = new Date(date.getFullYear(), date.getMonth() + 1, 0);

    this.getDates();

    this.projectsService.list(servicePath.API.EmployeeType.list).subscribe((res:any[]) => (this.empTypes = res));
     this.projectsService.GetList(servicePath.API.Technology.activelist).subscribe((res:any[]) => (this.technologies = res));
      
      this.projectsService.GetList(servicePath.API.Department.list).subscribe((res:any[]) => (this.departments =
      res));
      
      this.projectsService.GetList(servicePath.API.HRAdvisor.activelist).subscribe((res:any[]) => (this.hradvisors = res));
    // this.masterDataService
    //   .GetManagersAndCompetencyLeads().subscribe((res: GenericType[]) => {
    //     this.reportingmanagers = res;
    //   });

      this.newAssocaiteForm = this._formBuilder.group({
        firstName: ["", [Validators.required]],
        middleName: [""],
        lastName: ["", [Validators.required]],
        PersonalEmailAddress: ["", [Validators.required,Validators.email, , Validators.pattern(/^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/)]],
        dateOfJoining: ["", [Validators.required]],
        MobileNo: ["", [Validators.required,Validators.pattern('^[1-9]{1}[0-9]{9}$')]],
        ddlgradeName: ["", [Validators.required]],
        ddldesignationID: ["", [Validators.required]],
        ddlemploymentType: ["", [Validators.required]],
        ddltechnology: ["", [Validators.required]],
        ddlDepartmentId: ["", [Validators.required]],
        ddlmanagerId: ["", [Validators.required]],
        ddlHRAdvisorName: ["", [Validators.required]],
        recruitedBy: [""]
      });
      this.newAssocaiteForm.controls.ddlgradeName.disable();
      this.getDesignations();
  }

  getDesignations(): void {
    this.masterDataService.GetDesignationList().subscribe((data: any[]) => {
      this.designationList = data;
    });
  }

  filteredDesignations(event){
    if(event){
      let suggestionString = event.target.value.toLowerCase();
        this.filteredDesignationIds = [];
        this.designationList.forEach((v) => {
          if (this.filteredDesignationIds.findIndex(x => x.Name == v.DesignationName) === -1 && v.DesignationName.toLowerCase().indexOf(suggestionString) > -1) {
            this.filteredDesignationIds.push({ Id: v.DesignationId, Name: v.DesignationName });
          }
        });
    }
    else {
      this.filteredDesignationIds=[];
      this.pushFilteredDesignationIds();
    }

  }

  requiredTechnolgy(event: any) {
    var target = event.value;
    if (target == 1 || target == 7) {
    this.isNonDelivery = false;
    }
    else {
     this.newAssocaiteForm.controls.ddltechnology.setValue(null);
     this.newAssocaiteForm.get('ddltechnology').clearValidators();
     this.newAssocaiteForm.controls['ddltechnology'].updateValueAndValidity();
      this.isNonDelivery = true;
    }
    if(target!=null)
    {
    this.masterDataService
    .GetManagersAndCompetencyLeads(target).subscribe((res: GenericType[]) => {
      this.reportingmanagers = res;
    });
  }

  }
  pushFilteredDesignationIds(){
    this.filteredDesignationIds=[];
    for (let i = 0; i < this.designationList.length; i++) {
      this.filteredDesignationIds.push({ Id: this.designationList[i].DesignationId, Name: this.designationList[i].DesignationName });
    }
  }

  getDates() {
    var date = new Date(),
      y = date.getFullYear(),
      m = date.getMonth();
    this.lastDate = new Date(y, m + 2, 0);
  
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

  onCancel() {
    // this.confirmationService.confirm({
    //     message: 'Are you sure that cancel this?',
    //     accept: () => {
    //       this._router.navigate(["/associates/prospectiveassociate"]);
    //     },
        
    //       reject : ()=>{
    //           this.onCancel();
          
    //      }
    // });
    
  }

  onClear() {
    this.newAssocaiteForm.reset();
    setTimeout(() => this.formGroupDirective.resetForm(), 0);
  }

  clearInput(evt: any, fieldName): void {
    if(fieldName=='dateOfJoining'){
      evt.stopPropagation();
      this.newAssocaiteForm.get('dateOfJoining').reset();
    }
  }

  displayFn(designation: any) {
    return designation && designation ? designation.Name : '';
  }

  getGradeByDesignation(designation) {
    if(designation && designation.Id){
      this.masterDataService.getGradeByDesignation(designation.Id).subscribe((gradeResponse:Grade)=>{
        if(gradeResponse){
          
       this.newAssociate.GradeName=gradeResponse.GradeName;
       this.newAssociate.GradeId=gradeResponse.GradeId;
       this.newAssocaiteForm.controls.ddlgradeName.setValue(gradeResponse.GradeName);


        }
      },
      (error: any) => {
        }
      );
    }
    else{
       this.newAssociate.GradeName="";
    }
    }

    filteredDesignation(value: any): void {
      let suggestionString = value;
      this.masterDataService.GetDesignationByString(suggestionString).subscribe(
        (desginationResponse) => {
        
          this.desgntnLst = desginationResponse;
          this.designationList = []
          this.desgntnLst.push({label: "Select Designation", value: null})

          this.desgntnLst.forEach(element => {
            this.designationList.push({
              label: element.DesignationName,
              value: element.DesignationId
            });
          });
          return this.designationList;
        }
        
        // (error: any) => {
        //   if (error._body != undefined && error._body != "")
        //     this._commonService
        //       .LogError(this.componentName, error._body)
        //       .then((data: any) => {});
        // }
      );
    }

    
  onCreate() {
    
    if(this.newAssocaiteForm.valid){
      
    if (this.newAssocaiteForm.value.ddldesignationID) {
    this.newAssociate.DesignationId = this.newAssocaiteForm.value.ddldesignationID.Id;
  }else
  {
      this.newAssociate.DesignationId =0;
      this.newAssociate.Gender="";
  }
  this.newAssociate.FirstName = this.newAssocaiteForm.value.firstName;
  this.newAssociate.LastName = this.newAssocaiteForm.value.lastName;
  this.newAssociate.MiddleName = this.newAssocaiteForm.value.middleName;
  this.newAssociate.EmploymentType = this.newAssocaiteForm.value.ddlemploymentType;
  this.newAssociate.TechnologyID = this.newAssocaiteForm.value.ddltechnology;
  this.newAssociate.DepartmentId = this.newAssocaiteForm.value.ddlDepartmentId;
  this.newAssociate.ManagerId = this.newAssocaiteForm.value.ddlmanagerId;
  this.newAssociate.HRAdvisorName = this.newAssocaiteForm.value.ddlHRAdvisorName;
  this.newAssociate.PersonalEmailAddress = this.newAssocaiteForm.value.PersonalEmailAddress;
  this.newAssociate.MobileNo = this.newAssocaiteForm.value.MobileNo;
  this.newAssociate.RecruitedBy = this.newAssocaiteForm.value.recruitedBy;
  this.newAssociate.DateofJoining = moment(this.newAssocaiteForm.value.dateOfJoining).format("YYYY-MM-DD");
  this.newAssociate.JoinDate = this.newAssocaiteForm.value.dateOfJoining;

  this.newAssociate.JoinDate = new Date(this.newAssociate.DateofJoining);

  
  this.projectsService.create(this.newAssociate).subscribe((res:any) => {
    this.newAssociate.Id = res;
   
    if(res != null){
      this._snackBar.open(
        'Prospective associate details saved successfully.',
        'x',
        {
          duration: 1000,
          horizontalPosition: 'right',
          verticalPosition: 'top',
        }
      );
      this.onClear();
      setTimeout(() => 
            {
              this._router.navigate(["/associates/prospectiveassociate"]);
            }, 1000);
    }
    else{
      this._snackBar.open(
        'Failed to save prospective associate details.',
        'x',
        {
          duration: 1000,
          panelClass:['error-alert'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
        }
      );
    }
    },
     error => {
      this._snackBar.open(
        error.error,
        'x',
        {
          duration: 1000,
          panelClass:['error-alert'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
        }
      );
     }
  );
}
}
 

}
