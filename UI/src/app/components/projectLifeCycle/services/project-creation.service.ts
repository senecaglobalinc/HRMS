import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import * as servicePath from '../../../service-paths';
import { environment } from '../../../../environments/environment';
import { ProjectDetails } from '../../../models/projects.model';
import { ProjectsData } from '../../../models/projects.model';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ProjectCreationService {
  serviceUrl = environment.ServerUrl;
  adminMicroService = environment.AdminMicroService;
  projectMicroService = environment.ProjMicroService;
  resources = servicePath.API.projects;
  selectedTab = new BehaviorSubject<number>(0);
  ProjectState = new BehaviorSubject<string>(null);
  EditMode = new BehaviorSubject<boolean>(false);
  ProjectId = new BehaviorSubject<number>(0);
  constructor(private httpClient: HttpClient) { }

  public SaveProject(newProject) {
    const url = this.projectMicroService + this.resources.addProject;
    return this.httpClient.post(url, newProject);
  }

  public SetEditMode(editMode : boolean){
    this.EditMode.next(editMode);
  }

  public GetProjectState(){
    return this.ProjectState.asObservable();
  }

  public SetProjectState(projectState : string){
    this.ProjectState.next(projectState);
  }

  public GetEditMode(){
    return this.EditMode.asObservable();
  }

  public SetSeletedTab(tabIndex: number) {
    this.selectedTab.next(tabIndex);
  }

  public GetSelectedTab() {
    return this.selectedTab.asObservable();
  }

  public SetProjectId(projectId: number) {
    this.ProjectId.next(projectId);
  }

  public GetProjectId() {
    return this.ProjectId.asObservable();
  }

  public GetProjectDetailsbyID(ProjectId: number) {
    const url = this.projectMicroService + this.resources.getProjectbyID + ProjectId;
    return this.httpClient.get(url);
  }

  public GetProjectStates(){
    const url = this.adminMicroService + this.resources.GetProjectsStatuses ;
    return this.httpClient.get(url);
  }
  updateProjectDetails(project: ProjectsData) {
    const url = this.projectMicroService + this.resources.updateProject;
    return this.httpClient.post(url, project);
  }

  deleteProjectDetails(projectId) {
    const url = this.projectMicroService + this.resources.deleteProject + projectId;
    return this.httpClient.post(url, projectId);
  }

  GetProjectsList(userRole: string, empId: number, dashboard : string) {
    const _url = this.projectMicroService + this.resources.getProjectList + userRole + "&employeeId=" + empId + "&dashboard=" + dashboard;
    return this.httpClient.get<ProjectDetails[]>(_url);
  }
  GetProgramManagers(userRole: string, empId: number) {
    const _url = this.adminMicroService + this.resources.getManagers + userRole + "&employeeId=" + empId;
    return this.httpClient.get<ProjectDetails[]>(_url);
  }

  public GetDomains(){
    return this.httpClient.get(this.adminMicroService + servicePath.API.Domain.list)
  }

  public submitForApproval(projectId : number, userRole : string, EmpId : number){
    const url = this.projectMicroService + this.resources.submitForApproval + projectId + "/" + userRole + "/" + EmpId;
    return this.httpClient.get(url);
  }
  
  public ApproveOrRejectByDH(projectId : number, status : string, EmpId : number){
    return this.httpClient.get(this.projectMicroService + this.resources.approveOrRejectByDH + projectId + "/" + status + "/" + EmpId);

  }

  public canCloseProject(projectId : number){
    const url = this.projectMicroService + this.resources.canCloseProject + "?projectId=" + projectId;
    return this.httpClient.get(url);
  }

  public closeProject(projectData : ProjectsData){
    const url = this.projectMicroService + this.resources.closeProject ;
    return this.httpClient.post(url,projectData);
  }
}
