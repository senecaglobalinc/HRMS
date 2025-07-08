import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { HttpClient } from '@angular/common/http';

import { Department } from '../models/department.model';
import { BehaviorSubject } from 'rxjs';

import * as servicePath from '../../../core/service-paths';

@Injectable({
  providedIn: 'root',
})
export class DepartmentService {
  private _config: any;
  // private _serviceUrl = environment.ServerUrl;
  // private _serviceUrl = "http://sg-srv-vtsapps:2021/admin/api/v1";
  _serviceUrl = environment.AdminMicroService;
  toGetData = false;
  editMode = false;
  _resources = servicePath.API.Department;
  departmentsEdit = new BehaviorSubject<Department>(new Department());
  Department = new BehaviorSubject<Department[]>([]);
  constructor(private httpClient: HttpClient) {}

  public getDepartmentDetails() {
    var _url = this._serviceUrl + this._resources.list;
    this.httpClient.get(_url).subscribe((res: Department[]) => {
      this.Department.next(res);
    });
  }
  public CreateDepartment(department: Department) {
    let _url = this._serviceUrl + this._resources.create;
    if (this.editMode == false) return this.httpClient.post(_url, department);
    else {
      let _url = this._serviceUrl + this._resources.update;
      return this.httpClient.post(_url, department);
    }
  }
}
