import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import { DepartmentTypeData } from '../models/department-type.model';
import * as servicePath from '../../../core/service-paths';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DepartmentTypeService {


  public editMode = false;
  private resources = servicePath.API.DepartmentType;
  public departmentEdit = new BehaviorSubject<DepartmentTypeData>(new DepartmentTypeData());
  public departmentList = new BehaviorSubject<DepartmentTypeData[]>([]);
  // private _serviceUrl = environment.ServerUrl;
  _serviceUrl = environment.AdminMicroService;
  // private _serviceUrl = "http://sg-srv-vtsapps:2021/admin/api/v1";
  constructor(private httpClient : HttpClient ) { }
  
  public getDepartmentType(){
    this.httpClient.get(this._serviceUrl + this.resources.list)
    .subscribe((res : DepartmentTypeData[]) => {this.departmentList.next(res);});
  }

  createDepartmentType(departmenttype : DepartmentTypeData){
    return this.httpClient.post(this._serviceUrl +this.resources.create , departmenttype)
  }

  editDepartmentType( departmenttype :DepartmentTypeData){
    return this.httpClient.post(this._serviceUrl + this.resources.update , departmenttype)
  }
}

