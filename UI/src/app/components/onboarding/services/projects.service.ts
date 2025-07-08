
import { Injectable } from '@angular/core';
import { ProjectsData } from '../../../models/projects.model';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import * as servicePath from '../../../service-paths';
import { environment } from '../../../../environments/environment';
import 'rxjs/Rx';

@Injectable({
  providedIn: 'root'

})

export class ProjectsService {
  // private resources:any;
  private _resources: any;
  private resource: any;
  private _config: any;
  private _serviceUrl = environment.ServerUrl;
  private _employeeService = environment.EmployeeMicroService;
  private _adminMicroService = environment.AdminMicroService;
  private _projectMicroService = environment.ProjMicroService;
  private _api: any;

  constructor(private httpClient: HttpClient) {
    this._resources = servicePath.API.projects;
    this.resource = servicePath.API.PAssociate;

  }

  // public GetProjectTypes(): Observable<any> {
  //   var url = this._serviceUrl + this._resources.getProjectTypes;
  //   return this.httpClient.get(url);

  // }

  // public GetClientBillingRoles(projectId : number): Observable<any>{
  //  var url = this._serverURL + this._api.AssociateAllocation.GetClientBillingRolesByProjectId + 2;
  //   return this._http.get(url);
  // }
  public getManagersLists() {
    var url = this._adminMicroService + this._resources.getManagers;
    return this.httpClient.get(url);
  }
  public getProgramManagers() {
    var url = this._projectMicroService + this._resources.getProgrammangers;
    return this.httpClient.get(url);
  }

  public getReportingManagers() {
    var url = this._employeeService + this._resources.getEmpList;
    return this.httpClient.get(url);
  }

  public getCustomers() {
    var url = this._adminMicroService + this._resources.getClients;
    return this.httpClient.get(url);
  }
  public getStatus() {
    var url = this._adminMicroService + this._resources.getStatusDetails;
    return this.httpClient.get(url);
  }
  AddProjectDetails(details: ProjectsData) {
    let _url = this._projectMicroService + this._resources.addProject;
    return this.httpClient.post(_url, details);
  }

  updateProjectDetails(details: ProjectsData) {
    let _url = this._projectMicroService + this._resources.updateProject;
    return this.httpClient.post(_url, details);

  }
  public GetProjectDetailsbyID(ProjectId: number) {
    var url = this._projectMicroService + this._resources.getProjectbyID + ProjectId;
    return this.httpClient.get(url);
    // .catch((err) => Util.handleError(err));
  }

  // getById(id: number) {
  //   if (!this._resources.getById) {
  //     throw new Error("getById resource not provided!");
  //   }
  //   var url = this._serviceUrl + this._resources.getById;
  //   return this.httpClient.get(url);

  // }

  // getByCode(code: string) {
  //   if (!this._resources.getByCode) {
  //     throw new Error("getById resource not provided!");
  //   }
  //   var url = this._serviceUrl + this._resources.getByCode;
  //   return this.httpClient.get(url);

  // }

  // update(obj: any) {
  //   if (!this._resources.update) {
  //     // swal("update resource not provided!", "", "error");
  //     return Observable.throw("update resource not provided!");
  //   }
  //   var url = this._serviceUrl + this._resources.update;
  //   return this.httpClient.post(url, obj);

  // }


  // delete(id: number) {
  //   if (!this._resources.delete) {
  //     throw new Error("delete resource not provided!");
  //   }

  //   var url = this._serviceUrl + this._resources.delete;
  //   return this.httpClient.get(url);

  // }
  create(obj: any) {

    var url = this._employeeService + this.resource.create;
    return this.httpClient.post(url, obj);
  }

  list(listname) {
    var url = this._employeeService + listname;

    return this.httpClient.get(url);

  }

  GetList(listname) {
    var url = this._adminMicroService + listname;
    debugger;
    return this.httpClient.get(url);
  }

}


