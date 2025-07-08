import { Component, OnInit, ViewChild, Input, Inject } from '@angular/core';
import { FormBuilder, FormControl, Validators, FormGroup, FormGroupDirective } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { AssociateAllocation, PercentageDropDown, RoleDetails } from '../../../master-layout/models/associateallocation.model';
import * as moment from 'moment';
import { themeconfig } from 'src/themeconfig';
import { AssociateExit } from '../../Models/associateExit.model';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AssociateAllocationService } from 'src/app/modules/TalentMangment/services/associate-allocation.service';
import { TemporaryAllocationReleaseService } from 'src/app/modules/TalentMangment/services/temporary-allocation-release.service';
import { DropDownType } from 'src/app/modules/master-layout/models/dropdowntype.model';
import { ResourceRelease } from 'src/app/modules/TalentMangment/models/resourcerelease.model';
import { ExitAnalysisService } from '../../Services/exit-analysis.service';
import { MasterDataService } from 'src/app/core/services/masterdata.service';
import { ChecklistService } from '../../Services/checklist.service';
import { EmployeeData } from '../../../admin/models/employee.model';
import { EmployeeStatusService } from '../../../admin/services/employeestatus.service';
import { UploadService } from './../../../onboarding/services/upload.service';
import { HttpClient } from '@angular/common/http';
import * as servicePath from '../../../../core/service-paths';
import { environment } from '../../../../../environments/environment';

  interface SelectItem {
    value : number;
    label : string;
  }


@Component({
  selector: 'app-associate-abscond-form',
  templateUrl: './associate-abscond-form.component.html',
  styleUrls: ['./associate-abscond-form.component.scss'],
  providers: [EmployeeStatusService]
})
export class AssociateAbscondFormComponent implements OnInit {
  themeConfigInput = themeconfig.formfieldappearances;
  abscondForm: FormGroup;
  employeesList: DropDownType[] = [];
  employeeId: number;
  associatesList = [];
  roleName: string="";
  employeeDetails: AssociateExit;
  EmployeeId: number;
  //formSubmitted : boolean = false;
  seletedemp: any;
  exittype:any;
  releaseResource: ResourceRelease;
  PMScreen: boolean = false;
  //abs:boolean=false;
  //sep:boolean=false;
  UserRole : string;
  aExitType:string;
  separationForm: FormGroup;
  currentPath: string;
    themeAppearence = themeconfig.formfieldappearances;
    exitReasonsList= [];
    showEvidence: boolean = false;
    formsubmitted:boolean = false;
    _empData: EmployeeData;
    _usersList = [];
    exitreasonlist =[];
    lastWorkingDate: Date;
    terminationForm: FormGroup;
    EmpName: string;
    disableAssociateName: boolean = false;
    openform: boolean = false;
    dashboard: string = "HRMDashbaord";
    @Input() max: any;
    selectedEmpdata=[]
    today = new Date(); 
    AssociateExit : AssociateExit = new AssociateExit();
    previous:string;
    uploaddetails: any;
    id: number;
    filedata: any;
    PAstatus: any;
    selectedEmp:any;
    causeCategory: any;
    uploaddata: any[] = [];
    private _resources = servicePath.API.upload;
    private _serverURL = environment.EmployeeMicroService;
    private selectedUploadDetails: any;
  
    @ViewChild(FormGroupDirective) formGroupDirective: FormGroupDirective;
    @ViewChild('messageToaster') messageToaster: any;
    @ViewChild('fileUpload') fileUpload: any;
    @ViewChild('uploadDialog') uploadDialog: any;
    @ViewChild('fileInput') fileInputVariable: any;
    @ViewChild('fileName') fileNameVariable: any;
    index: number;
    selectedFile: String = "";
    documentDesc: any = { file: "" };
    displayedColumns: string[] = [
      'Sno','DocumentName','Delete'
    ];
    fileGroup: FormGroup;
  constructor(
    private _service: TemporaryAllocationReleaseService,
    private associateservice :ExitAnalysisService,
    private _router: Router,
    private snackBar: MatSnackBar,
    private actRoute: ActivatedRoute,
    private _servicess: EmployeeStatusService,
    private _snackBar: MatSnackBar,
    private fb: FormBuilder,
    private checklist: ChecklistService,
    private _masterDataService: MasterDataService,
    ) 
    {
      this.today.setDate(this.today.getDate());
    }
  ngOnInit(): void {
    this.releaseResource = new ResourceRelease();
    this.actRoute.url.subscribe(url => { this.currentPath = url[0]["path"]; });
    console.log(this.currentPath);
    this.employeeId = JSON.parse(sessionStorage['AssociatePortal_UserInformation']).employeeId;
    this.roleName = JSON.parse(sessionStorage["AssociatePortal_UserInformation"]).roleName;
    if (sessionStorage.getItem("AssociatePortal_UserInformation") != null) {
      const currentRole = JSON.parse(
        sessionStorage.getItem("AssociatePortal_UserInformation")
      ).roleName;
       this.UserRole = currentRole;
    }
      this.getExitReasonsList();
     
        this.actRoute.params.subscribe(params => { this.id = params['id']; });
        this.fileGroup = new FormGroup(
            {
                SelectedFileShow: new FormControl(null),
                uploadselect: new FormControl(null)
            });
      
      this.clearValues();
    
    this.actRoute.data.subscribe((data)=>{
      this.aExitType = data['title']
      if(this.aExitType === 'SBH'){
        this.createSeparationForm();
      }
      if(this.aExitType === 'Abscond'){
        this.AbscondForm();
      }
      if(this.aExitType === 'Termination'){
        this.TerminationForm();
      }
    }) 
    
    if (this.UserRole == 'HRA') {
    this.PMScreen = true;
    }
    //this.getEmployees();
    this.getUsersList();
    this.AbscondForm();

  }


  AbscondForm() {
    this.abscondForm = new FormGroup({
      email: new FormControl(null),
      abscondedFromDate: new FormControl(null),
      abscondedToDate: new FormControl(null),

    });
  }

  createSeparationForm(){
    this.separationForm = new FormGroup({
      exitDate : new FormControl(null, [Validators.required]),
      reasonDetail : new FormControl(null, [Validators.required]),
    });
  }
  TerminationForm(){
    this.terminationForm = new FormGroup({
      EmpName: new FormControl(null),
      causeCategory: new FormControl(null),
      lastWorkingDate: new FormControl(null),
      CauseDetails: new FormControl(null),
  });}
 /* private getEmployees(): void {
    this._service.GetAssociatesToRelease(this.employeeId, this.roleName).subscribe((res: any) => {
        this.associatesList = []
        this.associatesList.push({ label: '', value: null });
        if (res.length > 0) {
            res.forEach((element: any) => {
                if (this.associatesList.findIndex(x => x.label == element.EmpName) === -1)
                    this.associatesList.push({ label: element.EmpName, value: element.EmployeeId });
            });
        }

    },
        (error: any) => {
            this.snackBar.open('Failed to Get Employees!.', 'x', {
                duration: 1000,
                panelClass:['error-alert'],
                horizontalPosition: 'right',
                verticalPosition: 'top',
              });
            // this.Clear();
        }
    );
}*/
getUsersList() {
  this._servicess.GetAssociateNames().subscribe((res: AssociateExit[]) => {
      let dataList: any[] = res;
      this._usersList = [];
      this._usersList.push({ label: '', value: null });
      dataList.forEach(e => {
          this._usersList.push({ label: e.EmpName, value: e.EmpId });
      });
      if (this.employeeId > 0) {
        console.log(this.employeeId);
      }
    },
    (error: any) => {
        this.snackBar.open('Failed to Get Employees!.', 'x', {
            duration: 1000,
            panelClass:['error-alert'],
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
        // this.Clear();
    }
);
}


submit() {
  console.log(this.seletedemp);
  let isValid = true;
  if(isValid === true){
    let SubmitData = {
      "EmployeeId": this.seletedemp,

     }
  this.employeeDetails = new AssociateExit();
  this.employeeDetails.email = this.abscondForm.value.email;
  this.employeeDetails.EmployeeId = this.seletedemp;
  this.employeeDetails.email = this.abscondForm.value.email;
  this.employeeDetails.abscondedFromDate = this.abscondForm.value.abscondedFromDate;
  this.employeeDetails.abscondedToDate = this.abscondForm.value.abscondedToDate;
   this.employeeDetails.ExitTypeId = 2 /* Abscond*/
  this.associateservice.CreateAssociateAbscond(this.employeeDetails).toPromise()
    .then(() => {
     
      this.snackBar.open(" Submitted Successfully", 'x', {
        duration: 1000,
        horizontalPosition: 'right',
        verticalPosition: 'top',
      }).afterDismissed().subscribe(res => {
        // this.onBack();
        this.Clear();

      });
    
    }).catch(error => {
      this.snackBar.open("Error while submit!", 'x', {
        duration: 1000,
        panelClass: ['error-alert'],

        horizontalPosition: 'right',
        verticalPosition: 'top',
      });
    });
  }
}

submitSBH(){ // separation by hr
  // this.employeeDetails = new AssociateExit();
  let postObj = {}
  postObj['EmployeeId'] = this.seletedemp;
  postObj['ExitTypeId'] = 3;
  postObj['ReasonDetail'] = this.separationForm.value.reasonDetail;
  console.log(postObj,'----------------')
  this.associateservice.CreateAssociateAbscond(postObj).toPromise()
    .then(() => {
     
      this.snackBar.open(" Submitted Successfully", 'x', {
        duration: 1000,
        horizontalPosition: 'right',
        verticalPosition: 'top',
      }).afterDismissed().subscribe(res => {
        // this.onBack();
        this.Clear();

      });
    
    }).catch(error => {
      this.snackBar.open("Error while submit!", 'x', {
        duration: 1000,
        panelClass: ['error-alert'],

        horizontalPosition: 'right',
        verticalPosition: 'top',
      });
    });
}
Clear() {
  this.abscondForm;
  this.seletedemp;
}

  
    
  
  getExitReasonsList(){
    this._masterDataService.GetExitReasons().subscribe((data:any[]) => {
      console.log(data);
      this.exitreasonlist.push({label: 'Select Category', value:null})
      data.forEach(e=>{this.exitreasonlist.push({label:e.Name, value: e.Id})})
      console.log(this.exitreasonlist);
    });
  }
  
  
  clearValues = function () {
      setTimeout(() => 
      this.formGroupDirective.resetForm(), 0)
      
  }

  
  
  onSubmit() {
    this.formsubmitted=true;
    if(this.terminationForm.valid){
      console.log(this.selectedEmp);
      //let isValid = true;
    //if(isValid === true){
    let SubmitData = {
      "EmployeeId": this.selectedEmp,
      "ReasonId": this.causeCategory
     }
      this.AssociateExit.EmployeeId =this.selectedEmp;
      this.AssociateExit.ExitDate = this.terminationForm.value.lastWorkingDate;
      this.AssociateExit.ReasonId = this.causeCategory;
      this.AssociateExit.CauseDetails = this.terminationForm.value.CauseDetails;
      this.AssociateExit.ExitType = 'termination';

      this.checklist.SubmitTermination(this.AssociateExit).subscribe(res => {
      if (res) {
        this._snackBar.open('Termination Submitted successfully.', 'x', {
          duration: 1000,
          panelClass:['success-alert'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
          });
        this._router.navigate(["/shared/dashboard"]);
        }
        else if(Error){
          this._snackBar.open('Associate Already Exists', 'x', {
            duration: 1000,
            panelClass:['success-alert'],
            horizontalPosition: 'right',
            verticalPosition: 'top',
      });
    }
      }),
      error => {
        this._snackBar.open('Unable to submit Termination', 'x', {
          duration: 1000,
          panelClass:['error-alert'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
      };
    }
  }

}

