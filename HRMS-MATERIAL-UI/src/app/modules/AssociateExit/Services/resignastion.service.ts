import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, BehaviorSubject, Subject } from 'rxjs';
import { runInThisContext } from 'vm';
import { environment } from '../../../../environments/environment';
import * as servicePath from '../../../core/service-paths';
import { GenericType } from '../../master-layout/models/dropdowntype.model';
import { Associate } from '../../onboarding/models/associate.model';

@Injectable({
  providedIn: 'root'
})
export class ResignastionService {

  private _adminMicroService : string;
  private _employeeMicroService : string;
  _resources = servicePath.API.Resignation;
  resource = servicePath.API.AssociateExitInterview;

  private resigReasonDtls = new Subject<any>();


  constructor(private httpClient: HttpClient) {
    this._adminMicroService = environment.AdminMicroService;
    this._employeeMicroService = environment.EmployeeMicroService;
   }

  public getResignationReason() {
    let _url = this._adminMicroService + this._resources.Reason;
    return this.httpClient.get<GenericType[]>(_url);
  }

  public getEmpDetails(empId: number){
    let _url = this._employeeMicroService + "/Employee/GetById/" + empId;
    return this.httpClient.get<Associate>(_url);
  }

  public submitResignation(newAssociateResignation){
    let _url = this._employeeMicroService + this._resources.Create;
    return this.httpClient.post(_url,newAssociateResignation);
  }

  public getExitDetailsById(empId : number){
    let userRole = JSON.parse(
      sessionStorage.AssociatePortal_UserInformation
    ).roleName;
    let isDecryptReq = environment.IsDecryptionRequired[userRole]? environment.IsDecryptionRequired[userRole]: false;
    let _url = this._employeeMicroService + this._resources.GetExitDetailsById + '/' + empId + '?isDecryptReq='+isDecryptReq;
    return this.httpClient.get(_url);
  }

  public getAllAssociateExit(){
    let _url = this._employeeMicroService + this._resources.GetAll;
    return this.httpClient.get(_url);
  }

  public revokeResignation(revokeResignationData){
    let _url = this._employeeMicroService + this._resources.Revoke;
    return this.httpClient.post(_url,revokeResignationData);
  }
  public CreateExitFeedback(exitFeedback){
    let _url = this._employeeMicroService + this.resource.CreateExitFeedback;
    return this.httpClient.post(_url,exitFeedback);
  }

  public getExitInterview(employeeId){
    let _url = this._employeeMicroService + this.resource.GetExitInterview + '/' + employeeId;
    return this.httpClient.get(_url);
  }

  setResigReasonDtls(resig_reason_dtls: any){
    this.resigReasonDtls.next(resig_reason_dtls);
  }
  getResigReasonDtls(){
    return this.resigReasonDtls;
  }

  public GetResignationSubStatus(employeeId){
    let _url = this._employeeMicroService + this._resources.GetResignationSubStatus + employeeId;
    return this.httpClient.get(_url);
  }
}
