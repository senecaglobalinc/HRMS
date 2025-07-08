import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import * as servicePath from '../../../core/service-paths';
import { DepartmentDetails } from "../../master-layout/models/role.model";
import { EmployeeDetails } from "../../onboarding/models/changeRM.model";
import { NonDeliveryAssociates } from "../../onboarding/models/changeRM.model"

@Injectable({
  providedIn: 'root'
})
export class AssignRmForNonDeliveryService {

  private employeeServiceUrl = environment.EmployeeMicroService;
  private adminServiceUrl = environment.AdminMicroService;
  private resources = servicePath.API.ChangeRmForNonDelivery;

  constructor(private httpClient : HttpClient) { }

  public getDepartments(){
    let url = this.adminServiceUrl + this.resources.GetAllDepartment;
    return this.httpClient.get<DepartmentDetails[]>(url)

  }

  public getEmpListByDeptId(departmentId:number){
    let url = this.employeeServiceUrl + this.resources.GetAssociateRMDetailsByDepartmentId + departmentId;
    return this.httpClient.get<EmployeeDetails[]>(url)

  }

  public getNonDeliveryAssociates(){
    let url = this.employeeServiceUrl + this.resources.GetNonDeleveryAssociates;
    return this.httpClient.get<NonDeliveryAssociates[]>(url)
  }

  public assignRmByDeptOrByAssociate(updateRmObj){
    let url = this.employeeServiceUrl + this.resources.UpdateAssociateRMDetails;
    return this.httpClient.post<NonDeliveryAssociates[]>(url,updateRmObj)
  }
}
