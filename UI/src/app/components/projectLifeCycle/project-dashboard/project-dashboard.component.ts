import { Component, OnInit } from '@angular/core';
import { MasterDataService } from '../../../services/masterdata.service';
import { ProjectsData } from '../../../models/projects.model';
import { Router } from '@angular/router';
import { ProjectCreationService } from '../services/project-creation.service';
import * as moment from 'moment';
import * as servicePath from '../../../service-paths';
import { MessageService } from 'primeng/api';
import { ConfirmationService } from 'primeng/components/common/confirmationservice';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { take } from 'rxjs/operators';
@Component({
  selector: 'app-project-dashboard',
  templateUrl: './project-dashboard.component.html',
  styleUrls: ['./project-dashboard.component.scss'],
  providers: [MessageService, ConfirmationService]
})
export class ProjectDashboardComponent implements OnInit {
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
  PrevDate = new Date();
  startDate = new Date();
  submitted = false;
  closeProjectData : FormGroup;
  // disablestyles = { 'opacity': '0.6', 'cursor':'not-allowed'}
  disablestyles = { 'opacity': '0.6', 'pointer-events': 'none'}
  
  
  closingProjectData : ProjectsData;
  resources = servicePath.API.PagingConfigValue;
  constructor( private route: Router, private messageService: MessageService,
    private projectCreationService: ProjectCreationService, private confirmationService: ConfirmationService) {
    this.PageSize = this.resources.PageSize;
    this.PageDropDown = this.resources.PageDropDown;
    this.PrevDate.setDate(this.PrevDate.getDate() - 1);
  }

  colPM = [
    { field: 'ProjectName', header: 'Name' },
    { field: 'PracticeAreaCode', header: 'Technology' },
    { field: 'ClientName', header: 'Client' },
    { field: 'ActualStartDate', header: 'Start Date' },
    { field: 'ActualEndDate', header: 'End Date' },
    { field: 'ProjectState', header: 'Status' }
  ];

  colDH = [
    { field: 'ProjectName', header: 'Name' },
    { field: 'ManagerName', header: 'Manager' },
    { field: 'PracticeAreaCode', header: 'Technology' },
    { field: 'ClientName', header: 'Client' },
    { field: 'ActualStartDate', header: 'Start Date' },
    { field: 'ActualEndDate', header: 'End Date' },
    { field: 'ProjectState', header: 'Status' }
  ];

  ngOnInit() {
    this.CreateCloseProjectForm();
    this.EmpId = JSON.parse(sessionStorage['AssociatePortal_UserInformation']).employeeId;
    this.roleName = JSON.parse(sessionStorage['AssociatePortal_UserInformation']).roleName;
    this.projectCreationService.SetProjectId(0);
    this.projectCreationService.SetSeletedTab(0); 
    this.projectCreationService.SetEditMode(false);
    this.GetProjectsList();
    if(this.roleName == 'Program Manager')
      this.cols = this.colPM;
    else if(this.roleName == 'Department Head')
      this.cols = this.colDH;
    
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
    if(projectdata.ProjectState == 'Drafted')
      return this.disablestyles;
   
    return { 'cursor':'pointer' };
  }
  canViewAllocation(projectdata : ProjectsData){
    if(projectdata.ProjectState =='Execution' )
      return { 'cursor':'pointer' };

    return this.disablestyles;
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
      'ActualEndDate' : new FormControl(null,Validators.required)
    })
  }

  canCloseProject(projectData : ProjectsData){
   
    this.projectCreationService.canCloseProject(projectData.ProjectId).pipe(
      take(1)).subscribe( res =>{
       if(res == 0)
        {
          this.displayCloseProject = true;
          this.closingProjectData = projectData;
          if(projectData.ActualEndDate != null)
            this.closeProjectData.patchValue({
              'ActualEndDate' : new Date(projectData.ActualEndDate)
            })
          this.startDate = new Date(projectData.ActualStartDate);
        }
      else{
         this.display = true
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
    this.projectCreationService.GetProjectsList(this.UserRole, this.EmpId, this.dashboard).subscribe((res: ProjectsData[]) => {
      this.ModifyDateFormat(res);
    });
  }

  ViewProject(ProjectObject: ProjectsData) {
    this.projectCreationService.SetProjectId(ProjectObject.ProjectId);
    this.route.navigate(['/project/view/' + 'projectDashboard']);
  }

  ViewAllocationDetails(ProjectDetails : ProjectsData){
    this.route.navigate(['/reports/resourcereportsbyproject/'+ ProjectDetails.ProjectId]);
  }

  closeProject(){
    this.submitted = true;
    if(this.closeProjectData.value.ActualEndDate != null){
     this.closingProjectData.ActualEndDate = this.closeProjectData.value.ActualEndDate;
     this.projectCreationService.closeProject(this.closingProjectData).pipe(
      take(1)).subscribe( res=>{
       if(res == 1){
        this.displayCloseProject = false;
        this.messageService.add({ severity: 'success', summary: 'Close Success!', detail: 'Project closed successfully' });
        this.GetProjectsList();
       }
       else{
        this.displayCloseProject = false;
        this.messageService.add({ severity: 'error', detail: 'Project Could not be closed!' });
       }
     },
    (error)=>{
      this.displayCloseProject = false;
      this.messageService.add({ severity: 'error', detail: error.error });

    });
  }
}

  clearData(){
    this.submitted = false;
    this.displayCloseProject = false;
  }


}


