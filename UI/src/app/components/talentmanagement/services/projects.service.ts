// import { Injectable } from '@angular/core';
// import { Http } from '@angular/http';
// import 'rxjs/Rx';
// import { Observable } from 'rxjs/internal/Observable';
// import { ProjectsData } from '../../../models/projects.model';
// import { HttpClient } from '@angular/common/http';
// import { environment } from '../../../../environments/environment';
// import * as servicePath from '../../../service-paths';

// @Injectable({
//   providedIn: 'root'
// })
// export class ProjectsService {
//   _serverURL = environment.ServerUrl;
//   _resources = servicePath.API.projects;
//   constructor(private _http: HttpClient) { 

//     // let _config = JSON.parse(sessionStorage["AssociatePortal_Configuration"]);
//     //                                        this._serverURL = _config.ServerUrl;
//     //                                        this._resources = _config.API.projects;
//     //                                        this._api = _config.API;
//   }
  
//   public GetProjectTypes(): Observable<any> {
//     var url = this._serverURL + this._resources.getProjectTypes;
//     return this._http.get(url   );

//   }    

//   // public GetClientBillingRoles(projectId : number): Observable<any>{
//   //  var url = this._serverURL + this._api.AssociateAllocation.GetClientBillingRolesByProjectId + 2;
//   //   return this._http.get(url   );
//   // }
//   public getManagersLists() {
//     var url = this._serverURL + this._resources.getManagers;
//     return this._http.get(url   );
//   }
//   public getProgramManagers() {
//     var url = this._serverURL + this._resources.getProgrammangers;
//     return this._http.get(url   );
//   }

//   public getReportingManagers() {
//     var url = this._serverURL + this._resources.getEmpList;
//     return this._http.get(url   );
//   }

//   public getCustomers() {
//     var url = this._serverURL + this._resources.getClients;
//     return this._http.get(url   );
//   }

//   // public getRolesList(DepartmentId: number) {
//   //   var url = this._serverURL + this._api.TalentRequisition.getRolesList + DepartmentId;
//   //   return this._http.get(url   );
//   // }

//   public getStatus() {
//     var url = this._serverURL + this._resources.getStatusDetails;
//     return this._http.get(url   );
//   }
//   AddProjectDetails(details: ProjectsData) {
//     let _url = this._serverURL + this._resources.addProject;
//     return this._http.post(_url, details);
   
//   }
 
//   updateProjectDetails(details: ProjectsData) {
//     let _url = this._serverURL + this._resources.updateProject;
//     return this._http.post (_url, details);
    
//   }
//   public GetProjectDetailsbyID(ProjectId: number){
//     var url = this._serverURL + this._resources.getProjectbyID + ProjectId;
//     return this._http.get(url);
//     // .catch((err) => Util.handleError(err));
//   }
// }
