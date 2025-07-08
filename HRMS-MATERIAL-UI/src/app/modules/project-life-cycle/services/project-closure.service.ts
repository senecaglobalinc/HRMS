import { HttpClient, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import * as servicePath from '../../../core/service-paths';
import { environment } from '../../../../environments/environment';
import { ProjectDetails } from '../../master-layout/models/projects.model';
import { ProjectsData } from '../../master-layout/models/projects.model';
import{ProjectClosureReject} from '../models/projects.model';
import { ProjectClosureReport } from '../models/projects.model';
import { BehaviorSubject } from 'rxjs';
import { ActivityData } from '../models/activities.model';
import {Observable} from 'rxjs';


@Injectable({
  providedIn: 'root'
})
export class ProjectClosureService {

  resources = servicePath.API.Activity;
  resourcesClosure = servicePath.API.ProjectClosure;
  re_sources = servicePath.API.projects;
  adminMicroService = environment.AdminMicroService;
  serviceUrl = environment.ServerUrl;
  projectMicroService = environment.ProjMicroService;
  selectedTab = new BehaviorSubject<number>(0);
  ProjectState = new BehaviorSubject<string>(null);
  EditMode = new BehaviorSubject<boolean>(false);
  ProjectId = new BehaviorSubject<number>(0);

  constructor(private httpClient: HttpClient) { }

  getClosureActivitiesByDepartment(departmentId: number){
    const url = this.adminMicroService+this.resources.GetClosureActivitiesByDepartment+departmentId;
    return this.httpClient.get(url);
  }

  public GetClosureReportByProjectID(ProjectId: number) {
    var url = this.projectMicroService + this.re_sources.getclosurereportbyprojectid + ProjectId;
    return this.httpClient.get(url);
   
  }
 public CreateProjectClosureReport(details: ProjectClosureReport) {
    let _url = this.projectMicroService + this.re_sources.createprojectclosurereport;
    return this.httpClient.post(_url, details);
  }
  public UpdateProjectClosureReport(details: ProjectClosureReport) {
    let _url = this.projectMicroService + this.re_sources.updateprojectclosurereport;
    return this.httpClient.post(_url, details);
  }
  public DownloadProjectClosureFileUpload(FileType: string, projectId: number): any {
    let _url = this.projectMicroService + this.re_sources.download+"Filetype="+FileType+"&projectId="+projectId;
    return this.httpClient.get(_url,{responseType: 'blob'});
  }
  
  public Delete(projectId: number,FileType:string) {
    var url = this.projectMicroService + this.re_sources.delete + FileType + "/"+ projectId ;
    return this.httpClient.delete(url);
  }

  getActivitiesByProjectIdAndDepartmentId(projectId: number,departmentId){
    const url = this.projectMicroService+this.resources.getActivitiesByProjectIdAndDepartmentId+projectId+'&DepartmentId='+departmentId;
    return this.httpClient.get(url);
  }

  CreateActivityChecklist(activityData: ActivityData){
    const url = this.projectMicroService+this.resources.CreateActivityChecklist;
    return this.httpClient.post(url,activityData);
  }

  UpdateActivityChecklist(activityData: ActivityData){
    const url = this.projectMicroService+this.resources.UpdateActivityChecklist;
    return this.httpClient.post(url,activityData);
  }

  GetActivitiesByProjectIdForPM(projectId: number){
    const url = this.projectMicroService+this.resources.GetActivitiesByProjectIdForPM+projectId;
    return this.httpClient.get(url);
  }
  
  SubmitForClosureApproval(projectSubmitData){
    const url = this.projectMicroService + this.resourcesClosure.SubmitForClosureApproval;
    return this.httpClient.post(url,projectSubmitData);
  }
  Reject(details: ProjectClosureReport){
    var url = this.projectMicroService + this.resourcesClosure.reject;
    return this.httpClient.post(url,details);
  }
  ApproveOrRejectClosureByDH(projectSubmitData){
    const url = this.projectMicroService + this.resourcesClosure.ApproveOrRejectClosureByDH;
    return this.httpClient.post(url,projectSubmitData);
  }
}
