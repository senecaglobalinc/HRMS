import { Component, OnInit, QueryList, ViewChild, ViewChildren } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, FormGroupDirective, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Observable, ReplaySubject, Subject, Subscription } from 'rxjs';
import { themeconfig } from '../../../../../themeconfig';
import { MasterDataService } from '../../../../core/services/masterdata.service';
import { DropDownType, GenericType } from '../../../master-layout/models/dropdowntype.model';
// import { ReportsData } from '../../models/reportsdata.model';
// import { ReportsFilterData, UtilizationReportFilterData } from '../../models/reportsfilter.model';
import { ProjectsData } from '../../../master-layout/models/projects.model';
import * as moment from "moment";
// import { ResourceReportService } from '../../services/resource-report.service';
import { NavService } from '../../../master-layout/services/nav.service';
import { ProjectCreationService } from '../../../project-life-cycle/services/project-creation.service';

interface SelectItem {
  value : number;
  label : string;
}

@Component({
  selector: 'app-hrmassociate-exit',
  templateUrl: './hrmassociate-exit.component.html',
  styleUrls: ['./hrmassociate-exit.component.scss']
})
export class HrmassociateExitComponent implements OnInit {

  themeConfigInput = themeconfig.formfieldappearances;
  projectId: number;
  filteredBanksMulti: ReplaySubject<any[]> = new ReplaySubject<any[]>(1);
  filteredcols: any[];
  displaycolsfields: any[];
  displaycols = [];
  private _onDestroy = new Subject<void>();

  projectData: ProjectsData;
  

  afterSearchFilter: boolean = false;
  cols: any[] = [];
  columnOptions: any[] = [];
  PageSize: number = 5;
  PageDropDown: number[] = [];
 // private resources = servicePath.API.PagingConfigValue;
  componentName: string;
 
  projectsList: SelectItem[] = [];
  associateexitform: FormGroup;
  projectIdSubscription: Subscription;

  
  @ViewChild(FormGroupDirective) formGroupDirective: FormGroupDirective;

  constructor(private _masterDataService: MasterDataService,private _snackBar: MatSnackBar,
    private projectCreationService: ProjectCreationService,
    public navService: NavService) {
    
    this.navService.currentSearchBoxData.subscribe(responseData => {
      // this.applyFilter(responseData);
    })
   // this.PageSize = this.resources.PageSize;
   // this.PageDropDown = this.resources.PageDropDown;
   }

  ngOnInit(): void {
    this.createForm();
    this.projectIdSubscription = this.projectCreationService.GetProjectId().subscribe(data => {
      this.projectId = data;
    });
    console.log(this.projectId);
    this.GetProjectDetails();  // we need to get refreshed data every time 
    
   
  }

  GetProjectDetails() {
    if (this.projectId > 0) {
      this.GetProjectByID(this.projectId);
      // this.GetClientBillingRolesByProjectId(this.projectId);
      // this.GetSOWDetailsById(this.projectId);
    }
  }
  private GetProjectByID(currentProjectID: number): void {
    this.projectCreationService
      .GetProjectDetailsbyID(currentProjectID)
      .subscribe(
        (projectdata: ProjectsData) => {
          this.projectData = projectdata;
          this.PopulateForm();
        },
        error => {
          this._snackBar.open("Error while getting the project details", 'x', {
            duration: 1000,
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
          // this.messageService.add({
          //   severity: "error",
          //   summary: "Error message",
          //   detail: "Error while getting the project details"
          // });
        }
      );
  }
    ngOnDestroy() {
      this._onDestroy.next();
      this._onDestroy.complete();
    }
   

  createForm(){

    this.associateexitform = new FormGroup({
      EmployeeId: new FormControl(''),
      EmployeeName: new FormControl(''),
      DateofJoin: new FormControl(''),
      DesignationId: new FormControl(''),
      TotalService: new FormControl(''),
      ProjectId: new FormControl(''),
      ExperienceId: new FormControl(''),
      GradeId: new FormControl(''),
      Department: new FormControl(''),
      Technology: new FormControl(''),
      ReportingManager: new FormControl(''),
      

    });
  }

  PopulateForm(): void {
    // if (this.updatedProjectData != null) {
      this.associateexitform.patchValue({
      
      EmployeeId: this.projectData.ProjectCode,
      EmployeeName: this.projectData.ProjectName,
        ClientId: this.projectData.ClientId,
        ReportingManager: this.projectData.ManagerId,
        ProjectId: this.projectData.ProjectTypeId,
        ProjectStateId: this.projectData.ProjectStateId,
        PracticeAreaId: this.projectData.PracticeAreaId,
        // ActualStartDate: this.ModifyDateFormat(
        //   this.projectData.ActualStartDate
        // ),
        // ActualEndDate: this.ModifyDateFormat(
        //   this.projectData.ActualEndDate
        // ),
        // DomainId: this.projectData.DomainId,
        // ProgramManagerName: this.projectData.ManagerName
      });
    // }
  }

  clearField(fieldName) {
   
    if(fieldName=='EmployeeId'){
    this.associateexitform.controls.EmployeeId.setValue(null);
    }
    if(fieldName=='PracticeAreaId'){
      this.associateexitform.controls.PracticeAreaId.setValue(null);
    } 
    if(fieldName=='ProjectId'){
      this.associateexitform.controls.ProjectId.setValue(null);
    } 
    if(fieldName=='ProgramManagerId'){
      this.associateexitform.controls.ProgramManagerId.setValue(null);
    } 
    if(fieldName=='ClientId'){
      this.associateexitform.controls.ClientId.setValue(null);
    } 
    if(fieldName=='ExperienceId'){
      this.associateexitform.controls.ExperienceId.setValue(null);
    } 
    if(fieldName=='GradeId'){
      this.associateexitform.controls.GradeId.setValue(null);
    } 
    if(fieldName=='DesignationId'){
      this.associateexitform.controls.DesignationId.setValue(null);
    } 
  }

 



  confirmExit(){
    console.log(this.associateexitform);
  }
 
  clear(){

    this.formGroupDirective.resetForm();
    this.createForm;
    

    this.associateexitform.controls["IsBillable"].setValue(-1);
    this.associateexitform.controls["IsCritical"].setValue(-1);

    // this.associateUtilizationReportList = [];

    // this.dataSource = new MatTableDataSource(this.associateUtilizationReportList);
    // this.dataSource.paginator = this.paginator;
    // this.dataSource.sort = this.sort;
    // this.totalRecordsCount = this.associateUtilizationReportList.length;
    
    
  }


}

