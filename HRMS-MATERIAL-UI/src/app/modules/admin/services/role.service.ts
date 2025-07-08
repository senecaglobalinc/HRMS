import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import * as servicePath from '../../../core/service-paths';
//import { RoleData, DepartmentDetails } from '../../../models/role.model';
import {RoleData} from '../models/role-data.model'
import { Department } from '../models/department.model';
import { BehaviorSubject } from 'rxjs';
import { DepartmentDetails } from '../../master-layout/models/role.model';

@Injectable({
  providedIn: 'root'
})
export class RoleService {
  public editMode = false;
  public selectedDepartmentname=false;
  public roleData = new BehaviorSubject<RoleData>(new RoleData());
  public selectedDepartment = new BehaviorSubject<DepartmentDetails>(new DepartmentDetails());
  private serviceUrl = environment.AdminMicroService;
  private resources = servicePath.API.Roles;

  constructor(private httpClient : HttpClient) { }

  public getDepartmentList(){
    return this.httpClient.get(this.serviceUrl+servicePath.API.Department.list);
  }

  public getRoleDetailsByDepartmentID(id){
    
    return this.httpClient.get(this.serviceUrl+this.resources.GetRoleDetailsByDepartmentID + id)
  }


  public GetRoleSuffixAndPrefix(departmentId : number){   
    return this.httpClient.get(this.serviceUrl + this.resources.getRoleSuffixAndPrefix +  departmentId)

  }

  public createRole(details: RoleData) {
    return this.httpClient.post(this.serviceUrl + this.resources.createRole,  details)
   
  }

  public editRole(details: RoleData) {
    
    return this.httpClient.post(this.serviceUrl + this.resources.updateRole,  details)
   
  }
   
  GetRolesAndDepartments() {
    let _url = this.serviceUrl + this.resources.GetRolesAndDepartments;
    return this.httpClient.get(_url);
  }


}
