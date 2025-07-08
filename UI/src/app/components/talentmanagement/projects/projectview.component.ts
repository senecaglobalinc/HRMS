import { Component, OnInit } from '@angular/core';
import { MasterDataService } from '../../../services/masterdata.service';
import { ProjectsData } from '../../../models/projects.model';
import { Router } from '@angular/router';
//import { ProjectCreationService } from '../services/project-creation.service';
import * as moment from 'moment';
import * as servicePath from '../../../service-paths';
import { ProjectCreationService } from '../../projectLifeCycle/services/project-creation.service';
@Component({
  // selector: 'app-projects',
  // templateUrl: './projects.component.html',
  // styleUrls: ['./projects.component.scss']
  selector: 'app-projectview',
  templateUrl: './projectview.component.html',
  styleUrls: ['./projects.component.scss']
})
export class ProjectViewComponent implements OnInit {
  selectedAssignUserRole:any;
   ProjectsList: ProjectsData[];
   UserRole: string;
   EmpId: number;
   PageSize: number;
   PageDropDown: number[] = [];
   resources = servicePath.API.PagingConfigValue;
  constructor(private masterService: MasterDataService, private route: Router, private ProjectCreationServiceObj: ProjectCreationService) {
    this.PageSize = this.resources.PageSize;
    this.PageDropDown = this.resources.PageDropDown;
  }
 
  cols = [
    { field: 'ProjectName', header: 'Project Name' },
    { field: 'ProjectCode', header: 'Project Code' },
    { field: 'ProjectTypeDescription', header: 'Project Type' },
    //{ field: 'ManagerName', header: 'Program Manager' },
    { field: 'PracticeAreaCode', header: 'Technology' },
    //{ field: 'ClientName', header: 'Client Name' },
    { field: 'DepartmentCode', header: 'Department' },
    //{ field: 'PlannedStartDate', header: 'Planned Start Date' },
    //{ field: 'PlannedEndDate', header: 'Planned End Date' },
    //{ field: 'ActualStartDate', header: 'Actual Start Date' },
    // { field: 'ActualEndDate', header: 'Actual End Date' },
  ];

  ngOnInit() {
    this.GetProjectsList();
  }

   CreateProject(): void {  
     
    this.route.navigate(['/talentmanagement/addproject/0']);
  }
   EditProject(ProjectObject: ProjectsData): void {
  
    this.route.navigate(['/talentmanagement/addproject/', ProjectObject.ProjectId]);
  }

  // after getting all the projects list modify date format else it we display with HH:MM:SS format
   ModifyDateFormat(data: ProjectsData[]): void {
    data.forEach(e => {
      if (e.ActualStartDate != null) {
        e.ActualStartDate = moment(e.ActualStartDate).format('MM/DD/YYYY');
      }
      if (e.ActualEndDate != null) {
        e.ActualEndDate = moment(e.ActualEndDate).format('MM/DD/YYYY');
      }
      if (e.PlannedStartDate != null) {
        e.PlannedStartDate = moment(e.PlannedStartDate).format('MM/DD/YYYY');
      }
      if (e.PlannedEndDate != null) {
        e.PlannedEndDate = moment(e.PlannedEndDate).format('MM/DD/YYYY');
      }
    })
    this.ProjectsList = data;

  }

  // method to get all projets list
   GetProjectsList(): void {
    let currentRole = JSON.parse(sessionStorage['AssociatePortal_UserInformation']).roleName;
    this.UserRole = currentRole;
    this.EmpId = JSON.parse(sessionStorage['AssociatePortal_UserInformation']).employeeId;
    this.masterService.GetProjectsList().subscribe((res: ProjectsData[]) => {
      this.ModifyDateFormat(res);
    });
  }

  //  ViewProject(ProjectObject: ProjectsData) {
  //   this.ProjectCreationServiceObj.ProjectId.next(ProjectObject.ProjectId);
  //   this.route.navigate(['/project/view']);
  // }

}


