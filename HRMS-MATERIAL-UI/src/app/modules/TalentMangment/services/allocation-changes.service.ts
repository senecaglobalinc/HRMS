import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { HttpClient } from '@angular/common/http';
import {environment} from '../../../../environments/environment';
import * as servicePath from '../../../core/service-paths';
import { AssociateAllocation } from '../../master-layout/models/associateallocation.model';

@Injectable({
  providedIn: 'root'
})
export class AllocationChangesService {
  private _resources: any;
  private _serverURL: string;
  private _projectMicroService: string;
  constructor(private _httpclient: HttpClient) {
      this._resources = servicePath.API.AssociateAllocation;
      this._projectMicroService = environment.ProjMicroService;
  }

  public UpdateAssociateAllocation(details: AssociateAllocation) {
    let _url = this._projectMicroService + this._resources.UpdateAssociateAllocation;
    return this._httpclient.post(_url, details);
}

public GetCurrentAllocationByEmpIdAndProjectId(employeeId: number, projectId: number) {
  let _url = this._projectMicroService + this._resources.GetCurrentAllocationByEmpIdAndProjectId + employeeId + "&projectId=" + projectId;
  return this._httpclient.get(_url);
}
}
