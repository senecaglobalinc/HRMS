import { Component, OnInit } from '@angular/core';
import { ProjectCreationService } from '../../services/project-creation.service';
import { ProjectsData } from '../../../master-layout/models/projects.model';
import * as servicePath from "../../../../core/service-paths";
import { Router, ActivatedRoute } from '@angular/router';
import * as moment from "moment";
import { Subscription } from 'rxjs';
import { ProjectClosureReport} from '../../models/projects.model';
import{ProjectClosureService} from '../../services/project-closure.service';
import {MatSnackBar} from '@angular/material/snack-bar';
import * as fileSaver from 'file-saver';
import 'rxjs/Rx' ;
import { ActivityData, ActivityList } from '../../models/activities.model';
import { themeconfig } from 'src/themeconfig';
import { NgxSpinnerService } from 'ngx-spinner';
import { take } from 'rxjs/operators';
import { MatDialog } from '@angular/material/dialog';
import { RemarksTeamleadComponent } from '../remarks-teamlead.component';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import{ProjectClosureReject} from '../../models/projects.model'

@Component({
  selector: 'app-pm-closure-dashboard',
  templateUrl: './pm-closure-dashboard.component.html',
  styleUrls: ['./pm-closure-dashboard.component.scss']
})
export class PMClosureDashboardComponent implements OnInit {
  submitdisbale:boolean=false;
  tlfilled:boolean=false;
  tlnotfilled:boolean=false;
  displayCloseProject = false;
  projectId: number;
  FileType:string;
  PageSize: number = 0;
  PageDropDown: number[] = [];
  resources = servicePath.API.PagingConfigValue;
  projectStateSubscription: Subscription;
  projectState: string;
  selectedTabSubscription: Subscription;
  clientBillingRoleData = [];
  // SOWData: SOW[] = [];
  projectclosurereport: ProjectClosureReport;
  dashboard: string;
  isDrafted: boolean = false;
  projectIdSubscription: Subscription;
  canApprove: boolean;
  EmpId: number;
  hideBack: boolean = false;
  showback: boolean = false;
  activityData = [];
  themeConfigInput = themeconfig.formfieldappearances;
  checklist: Map<number,string> = new Map();
  roleName: string="";
  pageload: boolean = false;
  projectData: ProjectsData;
  projectclosurereject:ProjectClosureReject;
  closeProjectData : FormGroup;
  closingProjectData:ProjectClosureReport;
  constructor(private projectCreationService: ProjectCreationService,
    private actRoute: ActivatedRoute, private router: Router,
    private _snackBar: MatSnackBar,
    private projectClosureService: ProjectClosureService,
    private spinner: NgxSpinnerService, private dialog?: MatDialog) {//this.projectData = new ProjectsData();
    this.projectclosurereport=new ProjectClosureReport();
  this.projectclosurereject=new ProjectClosureReject(); }

  ngOnInit(): void {
    
    this.spinner.show();
    this.CreateCloseProjectForm();
    this.EmpId = parseInt(JSON.parse(sessionStorage["AssociatePortal_UserInformation"]).employeeId);
    this.actRoute.params.subscribe(params => { this.dashboard = params["id"]; });
    this.roleName = JSON.parse(sessionStorage["AssociatePortal_UserInformation"]).roleName;
    let winpath = window.location.pathname.split(";")[0].split("/").reverse()[0];
    this.showback = true;
    // if(winpath === 'PMDashboard' || window.location.pathname === '/project/view/DHDashboard' || window.location.pathname === '/project/view/projectDashboard' || window.location.pathname === '/project/viewteamlead/TLDashboard' ){
    //   this.showback = true;
    // }

    this.projectStateSubscription = this.projectCreationService.GetProjectState().subscribe(data => {
      this.projectState = data;
    });
    this.projectIdSubscription = this.projectCreationService.GetProjectId().subscribe(data => {
      this.projectId = data;
    });
    this.GetProjectDetails(); //to get the project state. The code above doesn't work
    this.selectedTabSubscription = this.projectCreationService.GetSelectedTab().subscribe(data => {
      if (data == 3)
        this.hideBack = true;
      this.GetTeamLeadData();  // we need to get refreshed data every time 
    });
    
    for(let i=2;i<=5;i++){
      this.getClosureActivityByDepartment(i);
    }
    this.getActivitiesByProjectIdForPM();
  }
  CreateCloseProjectForm(){
    this.closeProjectData = new FormGroup({
      
      'rejectRemarks' : new FormControl(null, Validators.required)
    })
  }
  getClosureActivityByDepartment(departmentId: number){
    this.projectClosureService.getClosureActivitiesByDepartment(departmentId)
      .toPromise().then((activitylist: ActivityList[])=>{
        for(let i=0;i<activitylist.length;i++){
          this.checklist.set(activitylist[i].ActivityId,activitylist[i].Description);
        }
      }).catch(
      error=>{
        this.spinner.hide();
        this._snackBar.open("Error while getting the closure activities by department", 'x', {
          duration: 2000,
          horizontalPosition: 'right',
          verticalPosition: 'top',
        })
      });
  }

  getActivitiesByProjectIdForPM(){
    this.projectClosureService.GetActivitiesByProjectIdForPM(this.projectId).toPromise()
     .then((response: ActivityData[])=>{
      this.activityData= response;
      for(let department of this.activityData){
        if(department===''||department===null || department.StatusDescription!=="Completed"){
          this.submitdisbale=true;
          break;
        }
       }
      this.spinner.hide();
     }).catch(error=>{
      this.spinner.hide();
      // this._snackBar.open('An error has occurred. Please contact administration.', 'x', {
      //   duration: 2000,
      //   horizontalPosition: 'right',
      //   verticalPosition: 'top',
      // });
   });
  }

  GetTeamLeadData(){
    if (this.projectId > 0) {
      this.GetProjectTeamLeadInfoByID(this.projectId);
    }
  }
  GetProjectTeamLeadInfoByID(curProjectID: number){
    this.projectClosureService.GetClosureReportByProjectID(curProjectID).toPromise().then((res:ProjectClosureReport ) => {
      this.projectclosurereport=res[0];
      if(this.projectclosurereport.StatusId===24)//StatusId 25 -> Completed or In Progress
      {
        this.tlfilled=false;
        this.tlnotfilled=true;
        this.submitdisbale=true;
      }
      else{
        this.tlfilled=true;
        this.tlnotfilled=false;
      }
    }).catch(error=>{
      this.spinner.hide();
      this.pageload = true;
      // this._snackBar.open('An error has occurred. Please contact administration.', 'x', {
      //   duration: 2000,
      //   horizontalPosition: 'right',
      //   verticalPosition: 'top',
      // });
   });;
  }

  downloadclient(){
   
    this.FileType="ClientFeedback";
    
     this.projectClosureService.DownloadProjectClosureFileUpload(this.FileType,this.projectId).subscribe(response => {
       let blob:any = new Blob([response], { type: 'application/pdf' });
      
       if (window.navigator && window.navigator.msSaveOrOpenBlob) {
         window.navigator.msSaveOrOpenBlob(blob);
         return;
       }
       const url = window.URL.createObjectURL(blob);
       
     fileSaver.saveAs(blob, this.projectclosurereport.ClientFeedbackFile);
     }), error => console.log('Error downloading the file'),
                  () => console.info('File downloaded successfully');
   }
   
   downloaddelivery(){
     this.FileType="DeliveryPerformance";
    
     this.projectClosureService.DownloadProjectClosureFileUpload(this.FileType,this.projectId).subscribe(response => {
       let blob:any = new Blob([response], { type: 'application/pdf' });
       
       if (window.navigator && window.navigator.msSaveOrOpenBlob) {
         window.navigator.msSaveOrOpenBlob(blob);
         return;
       }
       const url = window.URL.createObjectURL(blob);
       
     fileSaver.saveAs(blob, this.projectclosurereport.DeliveryPerformanceFile);
     }), error => console.log('Error downloading the file'),
                  () => console.info('File downloaded successfully');
   
   }
 
   Reject(){
     
    // this.projectClosureService.Reject(this.projectId).toPromise().then((res:any)=>{
    //   this._snackBar.open("Project Closure report rejected", 'x', {
    //     duration: 1000,
    //     horizontalPosition: 'right',
    //     verticalPosition: 'top',
    //   }).afterDismissed().toPromise().then(()=>{
    //     this.GetTeamLeadData();
    //   });
    //   }).catch((error)=>{
    //     this._snackBar.open("Error while rejecting", 'x', {
    //       duration: 1000,
    //       horizontalPosition: 'right',
    //       verticalPosition: 'top',
    //     });
    //   });
  
            this.displayCloseProject = true;
            const dialogRef = this.dialog.open(RemarksTeamleadComponent, {
              disableClose: true,
              hasBackdrop: true,
              data: {rejectRemarks:this.projectclosurereport.rejectRemarks},

            });
  
            dialogRef.afterClosed().subscribe(result => {
              
              
              if(result.rejectRemarks){
                
                this.closeProjectData.value.rejectRemarks = result.rejectRemarks;
               
               // this.closeProject();
               var createObj = new ProjectClosureReport();
               createObj.ProjectId=this.projectId;
               createObj.rejectRemarks=this.closeProjectData.value.rejectRemarks ;
               this.projectClosureService.Reject(createObj).subscribe(
                (res:any) => {
                  if(res>0){
                    this._snackBar.open('Project Closure Rejected', 'x', {
                          duration: 3000,
                          horizontalPosition: 'right',
                          verticalPosition: 'top',
                        });
                        this.GetTeamLeadData();
                   
                  }
                  else{ 
                    this._snackBar.open('Error occured while rejecting', 'x', {
                      duration: 3000,
                      horizontalPosition: 'right',
                      verticalPosition: 'top',
                    });
                  }
                 });
              }  
            });
             this.closingProjectData = this.projectclosurereport;
            
              this.closeProjectData.patchValue({
               
                'rejectRemarks' : new String(this.projectclosurereport.rejectRemarks)
              })

           
          }

   SubmitForClosureByDH(){
     //Validation
     let isValid = true;
    
     for(let department of this.activityData){
      if(department===null || department.StatusDescription!=="Completed"){
        isValid = false;
        break;
      }
     }
     if(isValid && this.tlfilled){
      let projectSubmitData = {
        "projectId": this.projectId,
        "status": "Approve",
        "employeeId": this.EmpId
       }
      
     this.projectClosureService.ApproveOrRejectClosureByDH(projectSubmitData).toPromise()
      .then(()=>{
        this._snackBar.open("Approved Successfully", 'x', {
          duration: 1000,
          horizontalPosition: 'right',
          verticalPosition: 'top',
        }).afterDismissed().subscribe(res=>{
          this.onBack();
        });
      }).catch(error=>{
        this._snackBar.open("Error while approving!", 'x', {
          duration: 1000,
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
      });
    } else {
      this._snackBar.open("All departments activities and team lead data not received!", 'x', {
        duration: 3000,
        horizontalPosition: 'right',
        verticalPosition: 'top',
      });
    }
   }
   
   SubmitForClosureApproval(){
     //Validation
     let isValid = true;
    
     for(let department of this.activityData){
      if(department===null || department.StatusDescription!=="Completed"){
        isValid = false;
        break;
      }
     }
     if(isValid && this.tlfilled){
      let projectSubmitData = {
        "projectId": this.projectId,
        "userRole": this.roleName,
        "employeeId": this.EmpId
       }
      
       this.projectClosureService.SubmitForClosureApproval(projectSubmitData).toPromise()
        .then(res=>{
          this._snackBar.open("Successfully submitted", 'x', {
            duration: 1000,
            horizontalPosition: 'right',
            verticalPosition: 'top',
          }).afterDismissed().subscribe(res=>{
            this.onBack();
          });;
        }).catch(error=>{
          this._snackBar.open("Error while submitting!", 'x', {
            duration: 1000,
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
        });
     } else{
      this._snackBar.open("All departments activities and team lead data not received!", 'x', {
        duration: 3000,
        horizontalPosition: 'right',
        verticalPosition: 'top',
      });
     }
     
   }

  onBack(){
    this.showback = false;
    this.router.navigate(['/dashboard']);
  }

  GetProjectDetails() {
    if (this.projectId > 0) {
      this.GetProjectByID(this.projectId);
    }
  }
  private GetProjectByID(currentProjectID: number): void {
    this.projectCreationService
      .GetProjectDetailsbyID(currentProjectID)
      .toPromise().then(
        (projectdata: ProjectsData) => {
          this.projectData = projectdata;
          this.projectState=this.projectData.ProjectState;
          this.pageload = true;
        }).catch(
        error => {
          this._snackBar.open("Error while getting the project details", 'x', {
            duration: 1000,
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
        }
      );
  }

}
