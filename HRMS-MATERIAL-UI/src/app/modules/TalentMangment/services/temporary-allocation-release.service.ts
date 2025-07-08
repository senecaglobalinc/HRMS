import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import * as servicePath from '../../../core/service-paths';
import { ResourceRelease } from '../models/resourcerelease.model';


@Injectable({
  providedIn: 'root'
})
export class TemporaryAllocationReleaseService {
  private _projectMicroService: string;
  private _resources: any;
  private _serverURL: string;
  constructor(private _httpclient: HttpClient) {
      this._serverURL = environment.ServerUrl;
      this._resources = servicePath.API.TemporaryAllocationRelease;
      this._projectMicroService = environment.ProjMicroService;
  }
  public GetEmployeesAllocations() {
      let url = this._projectMicroService + this._resources.GetEmployeesAllocations;
      return this._httpclient.get(url);
  }
  public GetAssociatesForAllocation() {
    let url = this._projectMicroService + this._resources.GetAssociatesForAllocation;
    return this._httpclient.get(url);
}
  public TemporaryReleaseAssociate(releaseResource: ResourceRelease) {
      let _url = this._projectMicroService + this._resources.TemporaryReleaseAssociate;
      return this._httpclient.post(_url, releaseResource); 
  }

  public GetAssociatesToRelease(employeeId: number, roleName: string) {
      let url = this._projectMicroService + this._resources.GetAssociatesToRelease + employeeId + "/" + roleName;
      return this._httpclient.get(url);
  }
  public GetAssociatePrimaryProject(employeeId: number){
    let url = this._serverURL + this._resources.GetAssociatePrimaryProject + employeeId;
    return this._httpclient.get(url);
  }
}
