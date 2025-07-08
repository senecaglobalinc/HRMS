import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import * as servicePath from '../../../core/service-paths';
import { HttpClient } from '@angular/common/http';
import { Employee } from '../models/assign-manager-to-project.model';

@Injectable({
  providedIn: 'root'
})
export class AssignManagerToProjectService {
    serviceUrl = environment.ServerUrl;
    _employeeMicroService = environment.EmployeeMicroService;
    _projMicroService = environment.ProjMicroService;
    _adminMicroService = environment.AdminMicroService;
    resources = servicePath.API.AssignReportingManager;

  constructor(private httpClient: HttpClient) {
  }
  public GetManagersByProjectId(projectId:number){
    let _url = this._projMicroService + this.resources.GetManagersByProjectId + projectId;
    return this.httpClient.get(_url);
  }
  public AssignReportingManagerToProject(employee:Employee) {
    let _url = this._projMicroService + this.resources.AssignReportingManager;
    return this.httpClient.post(_url, employee);
  }

  public GetProjectsList(userRole : string, empId : number) {
    let _url = this._projMicroService+ servicePath.API.projects.getProjectList + userRole +"&employeeId="+ empId;
    return this.httpClient.get(_url)
    }

}
