import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import * as servicePath from '../../../core/service-paths';

@Injectable({
  providedIn: 'root'
})
export class AbscondService {
  private _employeeMicroService : string;
  _resources = servicePath.API.AssociateAbscond;
  constructor(private httpClient: HttpClient) { 
    this._employeeMicroService = environment.EmployeeMicroService;
  }
  
  GetAssociateByLead(leadId, DeptId){
    let _url = this._employeeMicroService+ this._resources.GetAssociateByLead+leadId+"/"+DeptId;
    return this.httpClient.get(_url);
  }

  CreateAbscond(params){
    let _url = this._employeeMicroService+ this._resources.CreateAbscond;
    return this.httpClient.post(_url, params)
  }

  GetAssociatesAbscondDashboard(Role, EmpId, RoleDeptId){
    let _url = this._employeeMicroService+ this._resources.GetAssociatesAbscondDashboard + Role + '/' + EmpId + '/' + RoleDeptId;
    return this.httpClient.get(_url)
  }

  AcknowledgeAbscond(params){
    let _url = this._employeeMicroService + this._resources.AcknowledgeAbscond;
    return this.httpClient.post(_url, params)
  }

  ConfirmAbscond(params){
    let _url = this._employeeMicroService + this._resources.ConfirmAbscond;
    return this.httpClient.post(_url, params)
  }
  GetAbscondDetailByAssociateId(associateId){
    let _url =  this._employeeMicroService + this._resources.GetAbscondDetailByAssociateId+associateId;
    return this.httpClient.get(_url)
  }

  AbscondClearance(params){
    let _url = this._employeeMicroService + this._resources.AbscondClearance;
    return this.httpClient.post(_url, params)
  }
  
  GetAbscondSubStatus(associateId: number){
    let _url =  this._employeeMicroService + this._resources.GetAbscondSubStatus+associateId;
    return this.httpClient.get(_url)
  }
}
