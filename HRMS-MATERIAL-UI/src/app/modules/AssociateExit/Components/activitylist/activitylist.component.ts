import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { from, Subscription } from 'rxjs';
import { ProjectsData } from 'src/app/modules/master-layout/models/projects.model';
import { Associate } from '../../../onboarding/models/associate.model';
import { ActivityData, ActivityDataByDepartment, ActivityDetails, ActivityList } from '../../Models/activitymodel';
import { themeconfig } from 'src/themeconfig';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatSnackBar } from '@angular/material/snack-bar';
import { FormControl, FormGroup, Validators, FormBuilder, FormArray } from '@angular/forms';
import { ChecklistService } from '../../Services/checklist.service';
import { AssociateExit } from '../../Models/associateExit.model';
import { NgxSpinnerService } from 'ngx-spinner';


@Component({
  selector: 'app-activitylist',
  templateUrl: './activitylist.component.html',
  styleUrls: ['./activitylist.component.scss']
})
export class ActivitylistComponent implements OnInit {

  themeConfigInput = themeconfig.formfieldappearances;
  serviceDeptID: number;
  checklist: Map<number, string> = new Map();
  displayedColumns: string[];
  empId: number;
  projectState: string;
  associateStateSubscription: Subscription;
  fullName: string;
  roleName: string;
  AssociateResignation: AssociateExit;
  programManagerName: string = '';
  designationName: string = '';
  exitDate: Date;
  checklistForm: FormGroup;
  projectId: number;
  EmployeeId: number;
  departmentId: number;
  employeeIdSubscription: Subscription;
  projectData: ProjectsData;
  associateData: Associate;
  activityData: ActivityData;
  dashboard: string;
  hideBack: boolean = false;
  showback: boolean = true;
  pageload: boolean = false;
  UserRole: string;
  mangerForm: FormGroup;
  activityType: Map<number,string> = new Map();
  prevActivity: string;
  disableform = false;
  IsRequired: Map<number,boolean> = new Map();
  currentPath: string;

  constructor(
    private checklistService: ChecklistService,
    private router: Router,
    public dialog: MatDialog,
    public fb: FormBuilder,
    private actRoute: ActivatedRoute,
    private _snackBar: MatSnackBar,
    private spinner: NgxSpinnerService

  ) {

  }

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  ngOnInit(): void {
    // this.createForm();
    this.spinner.show();
    this.actRoute.url.subscribe(url => { this.currentPath = url[0].path; });
    this.EmployeeId = JSON.parse(sessionStorage["AssociatePortal_UserInformation"]).EmployeeId;
    this.fullName = JSON.parse(sessionStorage["AssociatePortal_UserInformation"]).fullName;
    this.roleName = JSON.parse(sessionStorage["AssociatePortal_UserInformation"]).roleName;

    if (sessionStorage.getItem("AssociatePortal_UserInformation") != null) {
      const currentRole = JSON.parse(
        sessionStorage.getItem("AssociatePortal_UserInformation")
      ).roleName;
      this.UserRole = currentRole;
      this.empId = JSON.parse(
        sessionStorage.getItem("AssociatePortal_UserInformation")
      ).employeeId;
    }
    if(sessionStorage.getItem("Department") != null){
      this.serviceDeptID = JSON.parse(sessionStorage.getItem("Department")).DepartmentId
    }
    this.actRoute.params.subscribe(params => {

      this.empId = params["id"];
    });

    this.actRoute.params.subscribe(params => { this.dashboard = params["id"]; });
    this.employeeIdSubscription = this.checklistService.GetEmployeeId().subscribe(data => {
      this.EmployeeId = data;
    });
    this.checklistForm = new FormGroup({

    });


     this.GetExitActivitiesByDepartment(this.serviceDeptID); //using promises to handle the data
   //  this.GetActivitiesByEmployeeIdAndDepartmentId(this.serviceDeptID); //using promises to handle the data


  }

  GetExitActivitiesByDepartment(departmentId: number){

    this.checklistService.GetExitActivitiesByDepartment(this.empId,departmentId)
      .toPromise().then((activitylist: ActivityList[])=>{
        this.spinner.hide();
        for(let i=0;i<activitylist.length;i++){
          this.checklist.set(activitylist[i].ActivityId,activitylist[i].Description);
          this.activityType.set(activitylist[i].ActivityId,activitylist[i].ActivityType);
          this.IsRequired.set(activitylist[i].ActivityId,activitylist[i].IsRequired);
        }
        this.Create().then(()=>{
          this.getActivitiesByEmployeeIdAndDepartmentId(this.empId);
        });
      }).catch(
      error=>{
        this.spinner.hide();
        this._snackBar.open("Error while getting the exit activities by department", 'x', {
          panelClass:['error-alert'],
          duration: 1000,
          horizontalPosition: 'right',
          verticalPosition: 'top',
        })
      });
  }


   async getActivitiesByEmployeeIdAndDepartmentId(EmployeeId: number) {
    this.spinner.show();
    await this.checklistService.getActivitiesByEmployeeIdAndDepartmentId(EmployeeId, this.serviceDeptID).toPromise()
      .then((activitydata: ActivityDataByDepartment) => {
        this.spinner.hide();
        this.activityData.Remarks = activitydata.Remarks;
        for(let i=0;i<this.activityData.ActivityDetails.length;i++){
          for(let j=0;j<activitydata.ActivityDetails.length;j++){
            if(this.activityData.ActivityDetails[i].ActivityId!==activitydata.ActivityDetails[j].ActivityId){
              continue;
            }
            this.activityData.ActivityDetails[i]=activitydata.ActivityDetails[j];
            break;
          }
          if(this.activityData.ActivityDetails[i].ActivityValue!=="no"){
           this.activityData.ActivityDetails[i].value_bool=true;
          } else {
           this.activityData.ActivityDetails[i].value_bool = false;
          }
        }
        this.populateFormControl(activitydata.StatusCode);
      }).catch(
        error=>{
          this.spinner.hide();
         this._snackBar.open("Error while Getting Activity Checklist", 'x', {
          panelClass:['error-alert'],

          duration: 1000,
          horizontalPosition: 'right',
          verticalPosition: 'top',
         });
        }
      );
  }

  populateFormControl(status: string){
    for(let i=0;i<this.checklist.size;i++){
      this.checklistForm.addControl(`Checklist${i}`,new FormControl(this.activityData.ActivityDetails[i].value_bool));
      this.checklistForm.addControl(`Comment${i}`,new FormControl(this.activityData.ActivityDetails[i].Remarks));
    }
    this.checklistForm.addControl(`Remarks`,new FormControl(this.activityData.Remarks));
    this.pageload = true;
    this.spinner.hide();
    if(status === 'DepartmentActivityCompleted') {
      this.disableform = true;
      this.checklistForm.disable();
    }
  }

  Save(){
    this.clearValidators();
    this.activityData.type="save";
    this.spinner.show();
    this.UpdateActivityChecklist().then(response=>{
      this.spinner.hide();
      this._snackBar.open("Successfully saved", 'x', {
        panelClass:['success-alert'],

        duration: 1000,
        horizontalPosition: 'right',
        verticalPosition: 'top',
      });
    }).catch(error=>{
      this.spinner.hide();
      this._snackBar.open("Error while saving!", 'x', {
        panelClass:['error-alert'],

        duration: 1000,
        horizontalPosition: 'right',
        verticalPosition: 'top',
      });
    });
  }

  clearValidators(){
    Object.keys(this.checklistForm.controls).forEach(key => {
      this.checklistForm.get(key).clearValidators();
      this.checklistForm.get(key).updateValueAndValidity();
    });
  }

  UpdateActivityChecklist(){

   this.activityData.Remarks = this.checklistForm.get('Remarks').value;
   for(let i=0;i<this.activityData.ActivityDetails.length;i++){
     let comment = this.checklistForm.get(`Comment${i}`).value;
     this.activityData.ActivityDetails[i].Remarks = comment;

     let check = this.checklistForm.get(`Checklist${i}`).value;
     this.activityData.ActivityDetails[i].value_bool = check;
     if(check){
       this.activityData.ActivityDetails[i].ActivityValue="yes";
     } else{
       this.activityData.ActivityDetails[i].ActivityValue="no";
     }

   }
    return this.checklistService.updateActivityChecklist(this.activityData)
      .toPromise();
  }

  Submit(){
    //VALIDATION
    for(let i=0;i<this.activityData.ActivityDetails.length;i++){
         if(!this.checklistForm.get(`Checklist${i}`).value ){
          if(this.IsRequired.get(this.activityData.ActivityDetails[i].ActivityId))
          {
         this.checklistForm.controls[`Checklist${i}`].setValidators(Validators.requiredTrue);
         this.checklistForm.controls[`Checklist${i}`].updateValueAndValidity();
          }
        }
      }
   if(this.checklistForm.valid){
     this.SubmitActivityChecklist();
   }
   else{
    this.checklistForm.markAllAsTouched();
    this._snackBar.open('Please select the required fields', 'x', {
      duration: 1000,
         panelClass:['error-alert'],
      horizontalPosition: 'right',
      verticalPosition: 'top',
    });
   }
  }

  SubmitActivityChecklist(){
    this.activityData.type = "submit";
    this.spinner.show();
    this.UpdateActivityChecklist().then(response=>{
      this.spinner.hide();
      this._snackBar.open("Activities Submitted Successfully", 'x', {
        panelClass:['success-alert'],

        duration: 1000,
        horizontalPosition: 'right',
        verticalPosition: 'top',
      }).afterDismissed().subscribe(res=>{
        this.onBack();
      });
    }).catch(error=>{
      this.spinner.hide();
      this._snackBar.open("Error while submitting!", 'x', {
        panelClass:['error-alert'],

        duration: 1000,
        horizontalPosition: 'right',
        verticalPosition: 'top',
      });
    });
  }

  canShowTitle(element: any, index: number) {

    let activity_details = this.activityType;
    if(index==0)
    {
      this.prevActivity = this.activityType.get(element.ActivityId);
      return true;

    }

    if (this.activityType.get(element.ActivityId) !== this.prevActivity) {
      this.prevActivity = this.activityType.get(element.ActivityId);
      return true;
    }
    return false;
  }


  Create() {

    let actDetails: ActivityDetails[] = [];
    for (let id of this.checklist.keys()) {
      actDetails.push({
        ActivityId: id,
        Value: "no",
        ActivityValue:"no",
        Remarks: "",
        value_bool: false,
        createdBy: this.fullName,
        modifiedBy: "",
        ActivityType: "",
        Description: "",
      });
    }
    this.activityData = {
      EmployeeId: this.empId,
      DepartmentId: this.serviceDeptID,
      Remarks: "",
      type: "create",
      ActivityDetails: actDetails
    }
    return Promise.resolve();

  }

  onBack() {
    if(this.currentPath === 'abscond'){
      this.router.navigate(['associateexit/abscond-dashboard'])
    }
    else{
      this.router.navigate(['shared/exit-actions']);
    }
    this.showback = false;
  }
}
