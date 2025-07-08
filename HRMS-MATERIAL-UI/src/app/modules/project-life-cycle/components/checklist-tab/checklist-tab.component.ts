import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { from, Subscription } from 'rxjs';
import { ActivityData, ActivityDetails, ActivityList } from 'src/app/modules/project-life-cycle/models/activities.model';
import { ProjectCreationService } from 'src/app/modules/project-life-cycle/services/project-creation.service';
import { ProjectClosureService } from 'src/app/modules/project-life-cycle/services/project-closure.service';
import { themeconfig } from 'src/themeconfig';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatSnackBar } from '@angular/material/snack-bar';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { NgxSpinnerService } from 'ngx-spinner';


@Component({
  selector: 'app-checklist-tab',
  templateUrl: './checklist-tab.component.html',
  styleUrls: ['./checklist-tab.component.scss']
})
export class ChecklistTabComponent implements OnInit {

  projectId: number;
  projectIdSubscription: Subscription;
  activityData: ActivityData;
  dashboard: string;
  hideBack: boolean = false;
  showback: boolean = true;
  pageload: boolean = false;

  constructor(private projectCreationService: ProjectCreationService,
    private router: Router,
    public dialog: MatDialog,
    private actRoute: ActivatedRoute,
    private _snackBar: MatSnackBar,
    private projectClosureService: ProjectClosureService,
    private spinner: NgxSpinnerService) { 
    }

    themeConfigInput = themeconfig.formfieldappearances;

    serviceDeptID: number;
    checklist: Map<number,string> = new Map();
    displayedColumns: string[];
    EmpId: number;
    projectState: string;
    projectStateSubscription: Subscription;
    fullName: string;
    roleName:string;
    checklistForm: FormGroup;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;  
  
  ngOnInit():void {
    this.spinner.show();
    this.EmpId = JSON.parse(sessionStorage["AssociatePortal_UserInformation"]).employeeId;
    this.fullName = JSON.parse(sessionStorage["AssociatePortal_UserInformation"]).fullName;
    this.roleName = JSON.parse(sessionStorage["AssociatePortal_UserInformation"]).roleName;
    
    switch(this.roleName){
      case("Admin Manager"):{
        this.serviceDeptID=2;
        break;
      }
      case("IT Manager"):{
        this.serviceDeptID=3;
        break;
      }
      case("Finance Manager"):{
        this.serviceDeptID=4;
        break;
      }
      case("Quality and Information Security Manager"):{
        this.serviceDeptID=5;
        break;
      }
    }
    this.actRoute.params.subscribe(params => { this.dashboard = params["id"]; });
    this.projectStateSubscription = this.projectCreationService.GetProjectState().subscribe(data => {
      this.projectState = data;
    });
    this.projectIdSubscription = this.projectCreationService.GetProjectId().subscribe(data => {
      this.projectId = data;
    });

    this.checklistForm = new FormGroup({});
    this.getClosureActivityByDepartment(this.serviceDeptID); //using promises to handle the data
    
  }

  getClosureActivityByDepartment(departmentId: number){
    this.projectClosureService.getClosureActivitiesByDepartment(departmentId)
      .toPromise().then((activitylist: ActivityList[])=>{
        for(let i=0;i<activitylist.length;i++){
          this.checklist.set(activitylist[i].ActivityId,activitylist[i].Description);
        }
        this.CreateActivityChecklist().then(()=>{
          this.getActivitiesByProjectIdAndDepartmentId(this.projectId);
        });       
      }).catch(
      error=>{
        this.spinner.hide();
        this._snackBar.open("Error while getting the closure activities by department", 'x', {
          duration: 1000,
          horizontalPosition: 'right',
          verticalPosition: 'top',
        })
      });
  }

  getActivitiesByProjectIdAndDepartmentId(projectId: number){
    this.projectClosureService.getActivitiesByProjectIdAndDepartmentId(this.projectId,this.serviceDeptID).toPromise()
     .then((activitydata: ActivityData)=>{
       //this.activityData = activitydata;
       this.activityData.Remarks = activitydata.Remarks;
       for(let i=0;i<this.activityData.ActivityDetails.length;i++){
         for(let j=0;j<activitydata.ActivityDetails.length;j++){
           if(this.activityData.ActivityDetails[i].ActivityId!==activitydata.ActivityDetails[j].ActivityId){
             continue;
           }
           this.activityData.ActivityDetails[i]=activitydata.ActivityDetails[j];
           break;
         }
         if(this.activityData.ActivityDetails[i].Value!=="no"){
          this.activityData.ActivityDetails[i].value_bool=true;
         } else {
          this.activityData.ActivityDetails[i].value_bool = false;
         }
       }
       
      this.populateFormControl();
       
     }).catch(
     error=>{
       this.spinner.hide();
       this._snackBar.open("Error while Getting Activity Checklist", 'x', {
        duration: 1000,
        horizontalPosition: 'right',
        verticalPosition: 'top',
      });
     });  
  }

  CreateActivityChecklist(){
    let actDetails: ActivityDetails[] = [];
    for(let id of this.checklist.keys()){
      actDetails.push({
        ActivityId: id,
        Value: "no",
        Remarks: "",
        value_bool: false,
        createdBy: this.fullName,
        modifiedBy: ""
      });
    }
    this.activityData = {
      ProjectId: this.projectId,
      DepartmentId: this.serviceDeptID,
      Remarks: "",
      type: "create",
      ActivityDetails: actDetails
    }
    return Promise.resolve();
  }

  populateFormControl(){
    for(let i=0;i<this.checklist.size;i++){
      this.checklistForm.addControl(`Checklist${i}`,new FormControl(this.activityData.ActivityDetails[i].value_bool));
      this.checklistForm.addControl(`Comment${i}`,new FormControl(this.activityData.ActivityDetails[i].Remarks));
    }
    this.checklistForm.addControl(`Remarks`,new FormControl(this.activityData.Remarks));
    this.pageload = true;
    this.spinner.hide();
  }

  Save(){
    this.clearValidators();
    this.activityData.type="save";
    this.UpdateActivityChecklist().then(response=>{
      this._snackBar.open("Successfully saved", 'x', {
        duration: 1000,
        horizontalPosition: 'right',
        verticalPosition: 'top',
      });
    }).catch(error=>{
      this._snackBar.open("Error while saving!", 'x', {
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
    let remark = this.checklistForm.get('Remarks').value;
    if(remark===""){
      this.activityData.Remarks = null;
    } else{
      this.activityData.Remarks = remark;
    }   
   for(let i=0;i<this.activityData.ActivityDetails.length;i++){
     let comment = this.checklistForm.get(`Comment${i}`).value;
     
     if(comment===""){
      this.activityData.ActivityDetails[i].Remarks = null;
     } else{
      this.activityData.ActivityDetails[i].Remarks = comment;
     }
     let check = this.checklistForm.get(`Checklist${i}`).value;
     this.activityData.ActivityDetails[i].value_bool = check;
     if(check){
       this.activityData.ActivityDetails[i].Value="yes";
     } else{
       this.activityData.ActivityDetails[i].Value="no";
     }
     
   }
    return this.projectClosureService.UpdateActivityChecklist(this.activityData)
      .toPromise();
  }

  Submit(){
    //VALIDATION
    let valid=true
   this.clearValidators();
   for(let i=0;i<this.activityData.ActivityDetails.length;i++){
     if(!this.checklistForm.get(`Checklist${i}`).value || !this.checklistForm.get(`Comment${i}`).value){
      this.checklistForm.controls[`Checklist${i}`].setValidators(Validators.required);
      this.checklistForm.controls[`Comment${i}`].setValidators(Validators.required);
      this.checklistForm.controls[`Comment${i}`].updateValueAndValidity();
      this._snackBar.open("Fill the details before submitting", 'x', {
        duration: 1000,
        horizontalPosition: 'right',
        verticalPosition: 'top',
      });
      valid=false
     }    
   }
   if(this.checklistForm.valid && valid){
     this.SubmitActivityChecklist()
   }
  }

  SubmitActivityChecklist(){
    this.activityData.type = "submit";
    this.UpdateActivityChecklist().then(response=>{
      this._snackBar.open("Activities Submitted Successfully", 'x', {
        duration: 1000,
        horizontalPosition: 'right',
        verticalPosition: 'top',
      }).afterDismissed().subscribe(res=>{
        this.onBack();
      });
    }).catch(error=>{
      this._snackBar.open("Error while submitting!", 'x', {
        duration: 1000,
        horizontalPosition: 'right',
        verticalPosition: 'top',
      });
    });
  }

  onBack() {
    this.router.navigate(['shared/dashboard']);
    this.showback = false;
  }

}
