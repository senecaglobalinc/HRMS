import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import {
  HttpClient,
  HttpHeaders,
  HttpErrorResponse,
} from '@angular/common/http';
//import 'rxjs/Rx';
import * as environmentInformation from '../../../../environments/environment';
import * as servicePath from '../../../core/service-paths';

@Injectable()
export class EmployeeStatusService {
  private _serverURL: string;
  private _resources: any;
  private EmployeeMicroService: any;
  constructor(private httpClient: HttpClient) {
    this._serverURL = environmentInformation.environment.ServerUrl;
    this._resources = servicePath.API.EmployeeStatus;
    this.EmployeeMicroService =
      environmentInformation.environment.EmployeeMicroService;
  }

  GetAssociateNames() {
    let url = this.EmployeeMicroService + this._resources.GetNames;
    return this.httpClient.get(url);
  }

  GetAssociates() {
    let url = this.EmployeeMicroService + this._resources.GetUsers;
    return this.httpClient.get(url);
  }

  GetResignStatus() {
    let url = this.EmployeeMicroService + this._resources.GetResignStatus;
    return this.httpClient.get(url);
  }

  UpdateEmployeeStatus(empData: any) {
    let url = this.EmployeeMicroService + this._resources.UpdateEmployeeStatus;
    return this.httpClient.post(url, empData);
  }

  MapAssociateId(empData: any) {
    let url = this.EmployeeMicroService + this._resources.MapAssociateId;
    return this.httpClient.post(url, empData);
  }

  GetAssociatesByDepartment(DeptId: number) {
    let url = this.EmployeeMicroService + this._resources.GetAssociatesByDepartment + '?departmentId=' + DeptId;
    return this.httpClient.get(url);
  }
  GetDepartmentHeadByDepartment(departmentId: number) {
    let url = this.EmployeeMicroService + this._resources.GetDepartmentHeadByDepartment + departmentId;
    return this.httpClient.get(url);
  }
}
