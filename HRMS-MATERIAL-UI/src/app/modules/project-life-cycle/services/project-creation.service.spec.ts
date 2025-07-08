import { TestBed, async } from '@angular/core/testing';
import {
  HttpClientTestingModule,
  HttpTestingController
} from '@angular/common/http/testing';
import { ProjectCreationService } from './project-creation.service';
import { ProjectsData } from '../../master-layout/models/projects.model';
import { of, BehaviorSubject } from 'rxjs';
import { inject } from '@angular/core';
import * as servicePath from "../../../core/service-paths";
import { environment } from 'src/environments/environment';

export class MockProjectCreationService {
  selectedTab = new BehaviorSubject<number>(0);
  ProjectState = new BehaviorSubject<string>(null);
  EditMode = new BehaviorSubject<boolean>(false);
  ProjectId = new BehaviorSubject<number>(0);
  projectDummyData: any[] = [{
    "ProjectCode": "SGITPS3",
    "ProjectTypeId": 1,
    "ProjectName": "Testing project 2",
    "DomainId": 2,
    "ProjectStateId": 22,
    "ClientId": 32,
    "ManagerId": 54,
    "PracticeAreaId": 27,
    "ActualStartDate": 62,
    "ActualEndDate": 62,
    "DepartmentId": 21,
    "ProjectId": 234
  }];

  public SaveProject(projectData) {
    this.projectDummyData.push(projectData);

    return of(this.projectDummyData.length);
  }
  public GetProjectId() {
    return of(this.projectDummyData[0].ProjectId);
  }

  public updateProjectDetails(project) {
    let index = this.projectDummyData.findIndex((proData: any) => proData.ProjectCode == project.ProjectCode);
    for (let i = 0; i < this.projectDummyData.length; i++) {
      if (this.projectDummyData[i].ProjectCode == project.ProjectCode) {
        index = i;
        break;
      }
    }
    if (index >= 0) {
      this.projectDummyData.splice(index, 1);
      this.projectDummyData.push(project);

      return of(1);
    }
    return of("no such project to update");
  }

  public SetEditMode(editMode: boolean) {
    this.EditMode.next(editMode);
  }

  public GetProjectState() {
    return this.ProjectState.asObservable();
  }

  public SetProjectState(projectState: string) {
    this.ProjectState.next(projectState);
  }

  public GetEditMode() {
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

  public GetProjectDetailsbyID(ProjectId: number) {
    return of();

  }

  public GetProjectStates() {
    return of();
  }

  deleteProjectDetails(projectId) {
    return of();
  }

  GetProjectsList(userRole: string, empId: number, dashboard: string) {
    return of();
  }
  GetProgramManagers(userRole: string, empId: number) {
    return of();
  }

  public GetDomains() {
    return of();
  }

  public submitForApproval(projectId: number, userRole: string, EmpId: number) {
    return of();
  }

  public ApproveOrRejectByDH(projectId: number, status: string, EmpId: number) {

    return of();
  }

  public canCloseProject(projectId: number) {
    return of();
  }

  public closeProject(projectData: ProjectsData) {
    return of();
  }
}

fdescribe('ProjectCreationService', () => {
  let httpMock: HttpTestingController;

  let projectMockData = [{
    "ProjectCode": "SG123",
    "ProjectTypeId": 1,
    "ProjectName": "Testing project",
    "DomainId": 1,
    "ProjectStateId": 2,
    "ClientId": 3,
    "ManagerId": 5,
    "PracticeAreaId": 7,
    "ActualStartDate": 6,
    "ActualEndDate": 6,
    "DepartmentId": 1

  }, {
    "ProjectCode": "SGITPS3",
    "ProjectTypeId": 1,
    "ProjectName": "Testing project 2",
    "DomainId": 2,
    "ProjectStateId": 22,
    "ClientId": 32,
    "ManagerId": 54,
    "PracticeAreaId": 27,
    "ActualStartDate": 62,
    "ActualEndDate": 62,
    "DepartmentId": 21

  }]

  beforeEach(() => TestBed.configureTestingModule({
    imports: [HttpClientTestingModule]

  })

  );

  it('should be created', () => {
    const service: ProjectCreationService = TestBed.get(ProjectCreationService);
    expect(service).toBeTruthy();
  });

  it('.SaveProject()', () => {
    httpMock = TestBed.get(HttpTestingController)
    const ProjectService = TestBed.get(ProjectCreationService);

    ProjectService.SaveProject(projectMockData[0]).subscribe(res => {
      expect(res).toBeGreaterThan(0);

    })
    
    const resources = servicePath.API.projects.addProject;
    const url = `${environment.ServerUrl + resources}`;
    const testRequest = httpMock.expectOne(url);

    expect(testRequest.request.method).toEqual('POST');
    testRequest.flush(98);

  });

  it('should update project data when valid data is submitted', (() => {
    httpMock = TestBed.get(HttpTestingController)
    const ProjectService = TestBed.get(ProjectCreationService);
    projectMockData[1].ProjectName = "Testing project updated Name";
    ProjectService.updateProjectDetails(projectMockData[1]).subscribe((res: any) => {
      expect(res).toBe(1);
    })

    const resources = servicePath.API.projects.updateProject;
    const url = `${environment.ServerUrl + resources}`;
    const testRequest = httpMock.expectOne(url);

    expect(testRequest.request.method).toEqual('POST');
    testRequest.flush(1);

  }));

});