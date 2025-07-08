import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import * as servicePath from '../../../service-paths';
import { RoleData, DepartmentDetails } from '../../../models/role.model';
import { Department } from '../models/department.model';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RoleService {
  public editMode = false;
  public roleData = new BehaviorSubject<DepartmentDetails>(new DepartmentDetails());
  public selectedDepartment = new BehaviorSubject<DepartmentDetails>(new DepartmentDetails());
  private serviceUrl = environment.AdminMicroService;
  private resources = servicePath.API.Department;

  constructor(private httpClient : HttpClient) { }

  public getDepartmentList(){
    return this.httpClient.get(this.serviceUrl+this.resources.list)
  }

  public getRoleDetailsByDepartmentId(id){
    return this.httpClient.get(this.serviceUrl+this.resources.GetRoleDetailsByDepartmentID + id)
  }

  public getKRAList(departmentId){
    return this.httpClient.get(this.serviceUrl+servicePath.API.KraAspect.getKraAspects + departmentId )
  }

  public getCurrentFinancialYear(){
    return this.httpClient.get(this.serviceUrl + servicePath.API.Masterdata.getFinancialYears )
  }

  public GetRoleDetailsbyRoleID(roleId : number){
    return this.httpClient.get(this.serviceUrl + servicePath.API.Roles.getRolebyID + roleId )
  }


  public GetRoleSuffixAndPrefix(departmentId : number){   
    return this.httpClient.get(this.serviceUrl + servicePath.API.Roles.getRoleSuffixAndPrefix +  departmentId)

  }

  public createRole(details: RoleData) {
    return this.httpClient.post(this.serviceUrl + servicePath.API.Roles.createRole,  details)
   
  }

  public editRole(details: RoleData) {
    return this.httpClient.post(this.serviceUrl + servicePath.API.Roles.updateRole,  details)
   
  }
   


}
