import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import * as servicePath from '../../../core/service-paths';
import { environment } from '../../../../environments/environment';
import { BehaviorSubject } from 'rxjs';
import { AssociateData } from '../Models/associatedata';
import { ActivityData } from '../Models/activitymodel';
import { AssociateExit } from '../Models/associateExit.model';



@Injectable({
  providedIn: 'root'
})
export class ChecklistService {


  serviceUrl = environment.ServerUrl;
  private _associateExit_adminMicroService = environment.AdminMicroService;
  employeeMicroService =environment.EmployeeMicroService
  projectMicroService = environment.ProjMicroService;
  resources = servicePath.API.AssociateExit;
  _resource = servicePath.API.Activity;
  resource = servicePath.API.projects;
  _resources = servicePath.API.AssociateExitActivity;

  selectedTab = new BehaviorSubject<number>(0);
  Associatestate = new BehaviorSubject<string>(null);
  EditMode = new BehaviorSubject<boolean>(false);
  EmployeeId = new BehaviorSubject<number>(0);
  ProjectId = new BehaviorSubject<number>(0);
  associateList = new BehaviorSubject<AssociateExit[]>([]);



  constructor(private httpClient: HttpClient) { }





  public GetAssociate(){
    return this.Associatestate.asObservable();
  }
 
  public GetEmployeeId() {
    
    return this.EmployeeId.asObservable();
  }

  public GetSelectedTab() {
    return this.selectedTab.asObservable();
  }
  // getAssociatesList(userRole: string, empId: number, dashboard: string) {
  //   const _url =
  //     this.projectMicroService +
  //     this._resources.GetAll +
  //     userRole +
  //     '&employeeId=' +
  //     empId +
  //     '&dashboard=' +
  //     dashboard;
  //   return this.httpClient.get<AssociateData[]>(_url);
  // }

  public getAssociates(){
    return this.httpClient.get(this.employeeMicroService + this.resources.GetAll)
  }



  public SetProjectId(EmployeeId: number) {
    this.EmployeeId.next(EmployeeId);
  }

  public GetByEmployeeId(EmployeeId: number) {
    const url = this.employeeMicroService + this.resources.GetByEmployeeId + EmployeeId;
    return this.httpClient.get(url);
    // getEmployeesByProjectID
    // "getEmployeesByProjectID": "/MasterData/GetEmployeesByDepartmentAndProjectID?departmentId=",

  }
  public GetProjectDetailsbyID(ProjectId: number) {
    const url = this.projectMicroService + this.resource.getProjectbyID + ProjectId;
    return this.httpClient.get(url);
  }
  GetClosureActivitiesByDepartment(departmentId: number){
    const url = this._associateExit_adminMicroService+this._resource.GetClosureActivitiesByDepartment+departmentId;
    return this.httpClient.get(url);
  }
  
  GetExitActivitiesByDepartment(EmployeeId: number, departmentId){
    const url = this._associateExit_adminMicroService+this._resource.GetExitActivitiesByDepartment + '?departmentId=' +departmentId;
   return this.httpClient.get(url);
 }
  getExitDashboard(userRole: string, empId: number, dashboard : string) {
    const url =
     this.employeeMicroService + 
     this.resources.getExitDashboards +
     userRole.replace(/ /g, '') + "/" + 
    //  '&employeeId=' +
     empId + "/" +
    //  '&dashboard=' +
     dashboard;
  return this.httpClient.get<AssociateExit[]>(url);
}

  Create(activityData: ActivityData){
    const url = this._associateExit_adminMicroService+this._resource.Create;
    return this.httpClient.post(url,activityData);
  }

  updateActivityChecklist(activityData: ActivityData){
    const url = this.employeeMicroService+this._resources.updateActivityChecklist;
    return this.httpClient.post(url,activityData, {responseType: 'text'});
  }

  getActivitiesByHRA(EmployeeId: number){
    const url = this.employeeMicroService +this._resources.GetActivitiesByemployeeIdForHRA + '?employeeId=' +EmployeeId;
    return this.httpClient.get(url);
  }
  getActivitiesByEmployeeIdAndDepartmentId(EmployeeId: number,DepartmentId : number){
    const url = this.employeeMicroService +this._resources.GetActivitiesByEmployeeIdAndDepartmentId + '?employeeId=' +EmployeeId + '&departmentId='+DepartmentId;
    return this.httpClient.get(url);
  }
  ExitClearance(SubmitData){
    const url = this.employeeMicroService +this._resources.ExitClearance ;
    return this.httpClient.post(url,SubmitData);
  }

  GetExitClearanceRemarks(employeeId){
    const url = this.employeeMicroService + this._resources.GetClearanceRemarks + '/' + employeeId;
    return this.httpClient.get(url);
  }

  CreateActivityChecklist(empId, hraId){
    const url = this.employeeMicroService +this._resources.CreateActivityChecklist + '/' + empId +'/'+ hraId;
     return this.httpClient.get(url);
    }

  SubmitTermination(NewTermination){
    const url = this.employeeMicroService +this.resources.Create;
     return this.httpClient.get(url,NewTermination);

  }

}