import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import * as servicePath from '../../../service-paths';
import { HttpClient } from '@angular/common/http';
import { Employee } from '../models/assignmanagertoproject.model';

@Injectable({
  providedIn: 'root'
})
export class AssignManagerToProjectService {  
    serviceUrl = environment.ServerUrl;
    _adminMicroService = environment.AdminMicroService;
    _projectMicroService = environment.ProjMicroService;
    resources = servicePath.API.AssignReportingManager;

  constructor(private httpClient: HttpClient) { 
  }
  public GetManagersByProjectId(projectId:number){   
    let _url = this._projectMicroService + this.resources.GetManagersByProjectId + projectId;    
    return this.httpClient.get(_url);
  }
  public AssignReportingManagerToProject(employee:Employee) {
    let _url = this._projectMicroService + this.resources.AssignReportingManager;
    return this.httpClient.post(_url, employee);   
  }

  public GetProjectsList(userRole : string, empId : number) {
    let _url = this.serviceUrl + servicePath.API.projects.getProjectList + userRole +"&employeeId="+ empId;
    return this.httpClient.get(_url)
    } 

}
