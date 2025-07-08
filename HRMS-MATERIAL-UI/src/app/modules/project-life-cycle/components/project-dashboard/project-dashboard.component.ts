import { Component, OnInit, ViewChild } from '@angular/core';
import { MasterDataService } from '../../../master-layout/services/masterdata.service';
import { ProjectsData } from '../../../master-layout/models/projects.model';
import { Router } from '@angular/router';
import { ProjectCreationService } from '../../services/project-creation.service';
import * as moment from 'moment';
import * as servicePath from '../../../../core/service-paths';
// import { MessageService } from 'primeng/api';
// import { ConfirmationService } from 'primeng/components/common/confirmationservice';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { take } from 'rxjs/operators';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { NavService } from 'src/app/modules/master-layout/services/nav.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatDialog } from '@angular/material/dialog';
import { DateDialogComponent } from '../date-dialog/date-dialog.component';
import { NgxSpinnerService } from 'ngx-spinner';



@Component({
  selector: 'app-project-dashboard',
  templateUrl: './project-dashboard.component.html',
  styleUrls: ['./project-dashboard.component.scss'],
  providers: []
})
export class ProjectDashboardComponent implements OnInit {
  projectstate: string;
  display: boolean;
  ProjectsList: ProjectsData[];
  UserRole: string;
  EmpId: number;
  isDelivaryHead = false;
  hideEdit: boolean = false;;
  PageSize: number;
  PageDropDown: number[] = [];
  dashboard: string;
  roleName : string;
  displayCloseProject = false;
  cols = [];
  showCol = false;
  PrevDate = new Date();
  startDate = new Date();
  submitted = false;
  closeProjectData : FormGroup;
  enddate : any;
  // disablestyles = { 'opacity': '0.6', 'cursor':'not-allowed'}
  disablestyles = { 'opacity': '0.6', 'pointer-events': 'none'}
  dataSource = new MatTableDataSource<ProjectsData>();

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;  
  
  closingProjectData : ProjectsData;
  resources = servicePath.API.PagingConfigValue;
  applyFilter(event: Event) {
    if (event) {
      const filterValue = (event.target as HTMLInputElement).value;
      this.dataSource.filter = filterValue.trim().toLowerCase();
    } else {
      this.dataSource = new MatTableDataSource(this.dataSource.data);
    }
    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
    this.dataSource.sortingDataAccessor = (data: any, sortHeaderId: string): string => {
      if (typeof data[sortHeaderId] === 'string') {
        return data[sortHeaderId].toLocaleLowerCase();
      }
    
      return data[sortHeaderId];
    }
    
  }
  constructor( private route: Router,
    private projectCreationService: ProjectCreationService, private _snackBar: MatSnackBar,
    public navService: NavService, private spinner: NgxSpinnerService, private dialog?: MatDialog) {
    this.PageSize = this.resources.PageSize;
    this.PageDropDown = this.resources.PageDropDown;
    this.PrevDate.setDate(this.PrevDate.getDate() - 1);
    this.navService.currentSearchBoxData.subscribe((responseData) => {
      this.applyFilter(responseData);
    });
  }
  colDH: string[] = [
    'ProjectName',
    'ManagerName',
    'PracticeAreaCode',
    'ClientName',
    'ActualStartDate',
    'ActualEndDate',
    'ProjectState',
    'ProjectDetails',
    'AllocationDetails',
    'Action'
  ];
  colPM: string[] = [
    'ProjectName',
    'PracticeAreaCode',
    'ClientName',
    'ActualStartDate',
    'ActualEndDate',
    'ProjectState',
    'ProjectDetails',
    'AllocationDetails',
    'Action'
  ];
  // colPM = [
  //   { field: 'ProjectName', header: 'Name' },
  //   { field: 'PracticeAreaCode', header: 'Technology' },
  //   { field: 'ClientName', header: 'Client' },
  //   { field: 'ActualStartDate', header: 'Start Date' },
  //   { field: 'ActualEndDate', header: 'End Date' },
  //   { field: 'ProjectState', header: 'Status' }
  // ];

  // colDH = [
  //   { field: 'ProjectName', header: 'Name' },
  //   { field: 'ManagerName', header: 'Manager' },
  //   { field: 'PracticeAreaCode', header: 'Technology' },
  //   { field: 'ClientName', header: 'Client' },
  //   { field: 'ActualStartDate', header: 'Start Date' },
  //   { field: 'ActualEndDate', header: 'End Date' },
  //   { field: 'ProjectState', header: 'Status' }
  // ];

  ngOnInit() {
    this.spinner.show();
    this.CreateCloseProjectForm();
    this.EmpId = JSON.parse(sessionStorage['AssociatePortal_UserInformation']).employeeId;
    this.roleName = JSON.parse(sessionStorage['AssociatePortal_UserInformation']).roleName;
    this.projectCreationService.SetProjectId(0);
    this.projectCreationService.SetSeletedTab(0); 
    this.projectCreationService.SetEditMode(false);
    this.GetProjectsList();
    this.cols=this.colDH;
    this.showCol = true;
    if(this.roleName == 'Program Manager'){
      this.cols = this.colPM;
      this.showCol = false;
    }
    else if(this.roleName == 'Department Head'){

      this.cols = this.colDH;
      this.showCol = true;
    }

  }

  CreateProject(): void {
    this.route.navigate(['/project/addproject/projectDashboard']);
  }
  EditProject(ProjectObject: ProjectsData): void {
    this.projectCreationService.SetSeletedTab(0);
    this.projectCreationService.SetEditMode(true);
    this.projectCreationService.SetProjectId(ProjectObject.ProjectId);
    this.route.navigate(['/project/addproject/projectDashboard']);
  }
  canClose(projectdata : ProjectsData){
    if(projectdata.ProjectState == 'ClosureInitiated' ||projectdata.ProjectState == 'SubmittedForClosureApproval' || projectdata.ProjectState == 'Closed' || projectdata.ProjectState === 'Drafted' || projectdata.ProjectState === 'SubmittedForApproval')
      return this.disablestyles;

    return { 'cursor':'pointer' };
  }
  canViewAllocation(projectdata : ProjectsData){
    if(projectdata.ProjectState =='Execution' )
      return { 'cursor':'pointer' };

    return this.disablestyles;
  }
  CanEdit(projectdata : ProjectsData){
    if(projectdata.ProjectState == 'ClosureInitiated' ||projectdata.ProjectState == 'SubmittedForClosureApproval' || projectdata.ProjectState == 'Closed')
      return this.disablestyles;

    return { 'cursor':'pointer' };
  }

  // DeleteProject(projectId: number,projectName : string): void {
  //   this.confirmationService.confirm({
  //     message: 'Do you want to delete the project?',
  //     accept: () => {
  //       this.projectCreationService.deleteProjectDetails(projectId).subscribe((res: boolean) => {
  //         if (res == true) {
  //           this.messageService.add({ severity: 'success', summary: 'Deletion Success!', detail: 'Project deleted successfully' });
  //           this.GetProjectsList();
  //         }
  //         else
  //           this.messageService.add({ severity: 'error', summary: 'Deletion Failed!', detail: 'Failed to delete the project' });
  //       },
  //         (error: any) => {
  //           this.messageService.add({ severity: 'error', summary: 'Deletion Failed!', detail: 'Failed to delete the project' });
  //         });
  //     },
  //     reject: () => { }
  //   });

  // }

  // after getting all the projects list modify date format else it we display with HH:MM:SS format
  ModifyDateFormat(data: ProjectsData[]): void {
    data.forEach(e => {
      if (e.ActualStartDate != null) {
        e.ActualStartDate = moment(e.ActualStartDate).format('MM/DD/YYYY');
      }
      if (e.ActualEndDate != null) {
        e.ActualEndDate = moment(e.ActualEndDate).format('MM/DD/YYYY');
      }  
    })
    this.ProjectsList = data;

  }
  canShowActions(projectData : ProjectsData){
    let endDate = projectData.ActualEndDate != null ? new Date(projectData.ActualEndDate).getDate() : null;
    let nextDate = new Date().getDate() + 1;
    if(  endDate != null && (endDate === nextDate || endDate < new Date().getDate())){
      
        return false;
    }
    return true;
  }
  CreateCloseProjectForm(){
    this.closeProjectData = new FormGroup({
      'ActualEndDate' : new FormControl(null,Validators.required),
      'Remarks' : new FormControl(null, Validators.required)
    })
  }

  canCloseProject(projectData : ProjectsData){
   
    this.projectCreationService.canCloseProject(projectData.ProjectId).pipe(
      take(1)).subscribe( res =>{
        
       if(res == 0)
        {
          // if (this.closeProjectData.EndDate != null) {
          //   let date = new Date(moment(this.closableRole.EndDate).format("YYYY-MM-DD"));
          //   this.closeRoleData.patchValue({
          //     BillingEndDate: date
          //   })
          // }

          this.displayCloseProject = true;
          const dialogRef = this.dialog.open(DateDialogComponent, {
            data: {BillingEndDate : projectData.ActualEndDate, StartDate: new Date(projectData.ActualStartDate), ProjectName: projectData.ProjectName},
            disableClose: true,
            hasBackdrop: true,
          });

          dialogRef.afterClosed().subscribe(result => {
            
            // projectData.ActualEndDate = moment(result.ActualEndDate).format('MM/DD/YYYY');
            if(result && result.ActualEndDate && result.Remarks){
              this.closeProjectData.value.ActualEndDate = result.ActualEndDate;
              this.closeProjectData.value.Remarks = result.Remarks;
              this.closeProject();
              this.closeProjectData.value.ActualEndDate = result.ActualEndDate;
              this.closeProjectData.value.Remarks = result.Remarks;
            }
            
            
          });
          this.closingProjectData = projectData;
          if(projectData.ActualEndDate != null)
            this.closeProjectData.patchValue({
              'ActualEndDate' : new Date(projectData.ActualEndDate),
              'Remarks' : new String(projectData.Remarks)
            })
          this.startDate = new Date(projectData.ActualStartDate);
        }
        else if(res == -1){
          this._snackBar.open('Project has no Team Lead - Please Assign Team Lead', 'x', {
            duration: 3000,
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });
        }
      else{
         this.display = true
         this._snackBar.open('Project has active Client billing roles, close them to continue.', 'x', {
          duration: 3000,
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
       // this.messageService.add({ severity: 'error', detail: 'Project has active Client billing roles, close them to continue' });
      }
      
    });
  
    
  }
  
  canEdit(projectData : ProjectsData){
    if(projectData.ProjectState == 'Closed'){
      return false;  
    }
    return true;
  }
  // method to get all projets list
  GetProjectsList(): void {
    let currentRole = JSON.parse(sessionStorage['AssociatePortal_UserInformation']).roleName;
    if (currentRole === 'Department Head') {
      this.isDelivaryHead = true;
    } // to display reject and approvaloptions in ui.
    
     this.dashboard = 'ProjectDashboard'; 

    this.UserRole = currentRole;
    this.projectCreationService.GetProjectsList(this.UserRole, this.EmpId, this.dashboard)
     .toPromise().then((res: ProjectsData[]) => {
      this.ModifyDateFormat(res);
      this.dataSource.data = res;
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
        this.dataSource.sortingDataAccessor = (data: any, sortHeaderId: string): string => {
          if (typeof data[sortHeaderId] === 'string') {
            return data[sortHeaderId].toLocaleLowerCase();
          }
        
          return data[sortHeaderId];
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

  ViewProject(ProjectObject: ProjectsData) {
    this.projectstate =ProjectObject.ProjectState;
    if(this.projectstate === "ClosureInitiated" ||
     this.projectstate === "Closed" || this.projectstate === "SubmittedForClosureApproval"){
      this.projectCreationService.SetProjectId(ProjectObject.ProjectId);
      this.route.navigate(["/project/projectclosure/"+ this.dashboard]);
    
   }
   else{
     
    this.projectCreationService.SetProjectId(ProjectObject.ProjectId);
    this.route.navigate(["/project/view/" + this.dashboard]);
   }
    // this.projectCreationService.SetProjectId(ProjectObject.ProjectId);
    // this.route.navigate(['/project/view/' + 'projectDashboard']);
    
  }

  ViewAllocationDetails(ProjectDetails : ProjectsData){
    this.route.navigate(['/reports/resourcereportsbyproject/'+ ProjectDetails.ProjectId]);
  }

  closeProject(){
    this.submitted = true;
    if(this.closeProjectData.value.ActualEndDate != null && this.closeProjectData.value.Remarks){
     this.closingProjectData.ActualEndDate = this.closeProjectData.value.ActualEndDate;
     this.closingProjectData.Remarks = this.closeProjectData.value.Remarks;
     this.projectCreationService.ProjectClosureInitiation(this.closingProjectData).pipe(
      take(1)).subscribe( res=>{
       if(res >= 1){
        this.displayCloseProject = false;
        this._snackBar.open('Project Closure Initiated Successfully', 'x', {
          duration: 3000,
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
        // this.messageService.add({ severity: 'success', summary: 'Close Success!', detail: 'Project closed successfully' });
        this.GetProjectsList();
       }
       else{
        this.displayCloseProject = false;
        this._snackBar.open('Project cannot be closed.', 'x', {
          duration: 3000,
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
        // this.messageService.add({ severity: 'error', detail: 'Project Could not be closed!' });
       }
     },
    (error)=>{
      this.displayCloseProject = false;
      this._snackBar.open(error.error, 'x', {
        duration: 3000,
        horizontalPosition: 'right',
        verticalPosition: 'top',
      });
      // this.messageService.add({ severity: 'error', detail: error.error });

    });
  }
}

  clearData(){
    this.submitted = false;
    this.displayCloseProject = false;
  }


}


