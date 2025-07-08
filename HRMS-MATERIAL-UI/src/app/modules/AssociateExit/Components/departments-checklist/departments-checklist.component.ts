import { Component, OnInit } from '@angular/core';
import { ProjectsData } from '../../../master-layout/models/projects.model';
import * as servicePath from "../../../../core/service-paths";
import { Router, ActivatedRoute } from '@angular/router';
import * as moment from "moment";
import { Subscription } from 'rxjs';
import {MatSnackBar} from '@angular/material/snack-bar';
import * as fileSaver from 'file-saver';
import 'rxjs/Rx' ;
import{ ChecklistService} from '../../Services/checklist.service';
import { ActivityData, ActivityDetails, ActivityList } from '../../Models/activitymodel';
import { themeconfig } from 'src/themeconfig';
import { AssociateExit } from '../../Models/associateExit.model';
import { ResignastionService } from '../../Services/resignastion.service';
import { MasterDataService } from 'src/app/core/services/masterdata.service';
import { Associate } from 'src/app/modules/onboarding/models/associate.model';
import { FormGroup, FormControl } from '@angular/forms';

interface ExitType {
  value : string;
  viewValue : string;
}

interface Reason {
  value: string;
  viewValue: string;
}


@Component({
  selector: 'app-departments-checklist',
  templateUrl: './departments-checklist.component.html',
  styleUrls: ['./departments-checklist.component.scss']
})
export class DepartmentsChecklistComponent implements OnInit {
  tlfilled:boolean=false;
  tlnotfilled:boolean=false;

  projectId: number;
  FileType:string;
  PageSize: number = 0;
  PageDropDown: number[] = [];
  resources = servicePath.API.PagingConfigValue;
  projectStateSubscription: Subscription;
  projectState: string;
  associateState: string;

  selectedTabSubscription: Subscription;
  clientBillingRoleData = [];
  // SOWData: SOW[] = [];
  dashboard: string;
  isDrafted: boolean = false;
  employeeIdSubscription: Subscription;
  canApprove: boolean;
  EmpId: number;
  hideBack: boolean = false;
  showback: boolean = false;
  activityData = [];
  themeConfigInput = themeconfig.formfieldappearances;
  checklist: Map<number,string> = new Map();
  roleName: string="";
  pageload: boolean = false;
  associateData:Associate;
  EmployeeId:number;
  activities = []; 
  clearenceForm: FormGroup;
  panelOpenState = false;

  projectData: ProjectsData;


  constructor(
    private actRoute: ActivatedRoute, private router: Router,
    private _snackBar: MatSnackBar,
    private checklistService: ChecklistService,
 private resignationservice:ResignastionService,
    ) {//this.projectData = new ProjectsData();
     }

  ngOnInit(): void {
    this.createForm();



    this.activities = 
    [{ id: 1, naming: ' HRA ',data:'No Data'}, 
    { id: 2, naming: 'Delivery Head' ,data:'No Data'}, 
    { id: 3, naming: 'Admin Service' , data:'No Data'},  
    { id: 4, naming: 'Quality and Security Service' ,data:'No Data'},  
    { id: 6, naming: 'Finance Service' , data:'No Data'}, 
    ]; 


    this.EmpId = parseInt(JSON.parse(sessionStorage["AssociatePortal_UserInformation"]).employeeId);
    this.actRoute.params.subscribe(params => { this.dashboard = params["id"]; });
    this.roleName = JSON.parse(sessionStorage["AssociatePortal_UserInformation"]).roleName;
    let winpath = window.location.pathname.split(";")[0].split("/").reverse()[0];
    this.showback = true;
    // if(winpath === 'PMDashboard' || window.location.pathname === '/project/view/DHDashboard' || window.location.pathname === '/project/view/projectDashboard' || window.location.pathname === '/project/viewteamlead/TLDashboard' ){
    //   this.showback = true;
    // }

    this.employeeIdSubscription = this.checklistService.GetEmployeeId().subscribe(data => {
      this.EmployeeId = data;
    });

   
    this.getExitDetailsById(); //to get the project state. The code above doesn't work
    this.selectedTabSubscription = this.checklistService.GetSelectedTab().subscribe(data => {
      if (data == 3)
        this.hideBack = true;
    });
    
    for(let i=2;i<=5;i++){
      this.getClosureActivityByDepartment(i);
    }
  }


  expandPanel(matExpansionPanel, event): void {
    event.stopPropagation(); // Preventing event bubbling
    
    if (!this._isExpansionIndicator(event.target)) {
      matExpansionPanel.close(); // Here's the magic
    }
  }
  
  private _isExpansionIndicator(target: EventTarget): boolean {
    const expansionIndicatorClass = 'mat-expansion-indicator';

    return (target['classList'] && target['classList'].contains(expansionIndicatorClass) );
  }
  
  createForm(){

    this.clearenceForm = new FormGroup({
  
      reasonleaving: new FormControl(''),
      exittype : new FormControl(''),
     
      hrsignature: new FormControl(''),
    });
  }
  reason: Reason[] = [
    {value: 'r-0', viewValue: 'Lack of overseas opportunity'},
    {value: 'r-1', viewValue: 'Lack of challenging work/New technology'},
    {value: 'r-2', viewValue: 'Leadership/team behavioral issues'}
  ];

  exittype: ExitType[] = [ 
    {value: 'r-0', viewValue: 'Resignation'},
    {value: 'r-1', viewValue: 'Discharge'},
    {value: 'r-2', viewValue: 'Retirement'}
  ];

  getClosureActivityByDepartment(departmentId: number){
    this.checklistService.GetClosureActivitiesByDepartment(departmentId)
      .toPromise().then((activitylist: ActivityList[])=>{
        for(let i=0;i<activitylist.length;i++){
          this.checklist.set(activitylist[i].ActivityId,activitylist[i].Description);
        }
      }).catch(
      error=>{
        this._snackBar.open("Error while getting the closure activities by department", 'x', {
          panelClass:['error-alert'],

          duration: 2000,
          horizontalPosition: 'right',
          verticalPosition: 'top',
        })
      });
  }

  onBack(){
    this.showback = false;
    this.router.navigate(['shared/exit-actions']);
  }

  getExitDetailsById() {
    if (this.EmpId > 0) {
      this.GetExitDetailsById(this.EmpId);
    }
  }
  private GetExitDetailsById(empId : number): void {
    this.resignationservice
      .getExitDetailsById(empId)
      .toPromise().then(
        (associateData: Associate) => {
          this.associateData = associateData;
          this.associateState=this.associateData.associateState;
          this.pageload = true;
        }).catch(
        error => {
          this._snackBar.open("Error while getting the project details", 'x', {
            panelClass:['error-alert'],

            duration: 1000,
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
        }
      );
  }
}
