import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import * as servicePath from '../../../core/service-paths';
import { environment } from '../../../../environments/environment';
import { ResourceRelease } from '../models/resourcerelease.model';



@Injectable({
  providedIn: 'root'
})
export class ResourceReleaseService {

  private _resources: any;
  private _serverURL: string;
  private _projectMicroService: string;

  constructor(private _httpclient: HttpClient) {
      this._serverURL = environment.ServerUrl;
    this._resources = servicePath.API.ResourceRelease;
    this._projectMicroService = environment.ProjMicroService;
  }

  public GetEmployeesByProjectID(projectId:number) {
    var url = this._serverURL + this._resources.GetEmployeesByProjectID + projectId;
    return this._httpclient.get(url);
  }

  public GetTalentPools() {
      var url = this._serverURL + this._resources.getTalentPools;
      return this._httpclient.get(url);
  }

  public GetAssociates() {
      var url = this._serverURL + this._resources.getAssociates;
      return this._httpclient.get(url);
  }

  public GetProjects(empId: number) {
      var url = this._serverURL + this._resources.getProjectDetails + empId;
      return this._httpclient.get(url);
  }

  public ReleaseAssociate(releaseResource: ResourceRelease) {
      let _url = this._serverURL + this._resources.releaseAssociate;
      return this._httpclient.post(_url, releaseResource); 
  }

  public GetAssociateTalentPool(empId: number) {
      var url = this._projectMicroService + this._resources.getAssociateTalentPool + empId;
      return this._httpclient.get(url);
  }
  public GetAssociateReleaseToProjects(empId: number, projectId: number, roleName: string) {
    var url = this._projectMicroService + this._resources.getAssociateTalentPool + empId + "/" + projectId + "/" + roleName;
    return this._httpclient.get(url);
}
  public GetAssociateProjectsForRelease(empId: number) {
    var url = this._projectMicroService + this._resources.GetAssociateProjectsForRelease + empId;
  }
}
