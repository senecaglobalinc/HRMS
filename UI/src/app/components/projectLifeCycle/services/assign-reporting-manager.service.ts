import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import * as servicePath from '../../../service-paths';
import { HttpClient } from '@angular/common/http';
import { Employee } from '../models/assignmanagertoproject.model';

@Injectable({
  providedIn: 'root'
})
export class AssignReportingManagerService {
  serviceUrl = environment.ServerUrl;
  projectMicroService = environment.ProjMicroService;
  employeeMicroService = environment.EmployeeMicroService;
  resources = servicePath.API.AssignReportingManager;
  constructor(private httpClient: HttpClient) { }

  public AssignReportingManager(employee: Employee, isDelivery: boolean) {
    let _url = this.projectMicroService + this.resources.UpdateReportingManagerToAssociate + isDelivery;
    return this.httpClient.post(_url, employee);
  }
  public GetManagerName(projectId: number, employeeId: number) {
    var url = this.projectMicroService + this.resources.getManagerName + projectId + "/" + employeeId;
    return this.httpClient.get(url)
  }

  public GetAllocatedAssociates() {
    var url = this.projectMicroService + this.resources.getAllocatedAssociates;
    return this.httpClient.get(url)
  }

  GetEmployeesAndManagers(suggestionString: string) {
    let url = this.employeeMicroService + this.resources.getEmployeesAndManagers + suggestionString;
    return this.httpClient.get(url);
  }

  public GetProjects(empId: number) {
    var url = this.projectMicroService + this.resources.getProjectDetails + empId;
    return this.httpClient.get(url);
  }

  GetAllLeadsManagers(suggestionString: string) {
    let url = this.projectMicroService + this.resources.getAllLeadsManagers + suggestionString;
    return this.httpClient.get(url);
  }
}
