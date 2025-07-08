import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import * as servicePath from '../../../core/service-paths';
import { environment } from '../../../../environments/environment';
import { AssociateResignationData } from '../Models/associate-resignation.model';

@Injectable({
  providedIn: 'root'
})
export class AssociateResignationService {
  private _resources: any;
//  private _serverURL: string;
  private _employeeMicroService: string;

  

  constructor(private httpClient: HttpClient) {
  //  this._serverURL = environment.ServerUrl;
    this._resources = servicePath.API.AssociateResignation;
    this._employeeMicroService = environment.EmployeeMicroService;
   }

   public GetAssociatesById(resignEmployeeId: number, employeeID: number) {
    let _url = this._employeeMicroService + this._resources.GetAssociatesById + resignEmployeeId  + "&employeeID="+employeeID;
    return this.httpClient.get(_url);
  }

  public CreateAssociateResignation(associateResignationData: AssociateResignationData){
    let _url= this._employeeMicroService + this._resources.CreateAssociateResignation;
    return this.httpClient.post(_url, associateResignationData);
  }

  public CalculateNoticePeriod(resignationDate: string){
    let _url = this._employeeMicroService + this._resources.CalculateNoticePeriod + resignationDate;
    return this.httpClient.get(_url,{responseType: 'text'});
  }

  public RevokeResignation(empID:number,reason:string, revokedDate: string) {
    var url=this._employeeMicroService+this._resources.RevokeResignation+empID+"&reason="+reason+"&revokedDate="+revokedDate;        
    return this.httpClient.get(url);
}
}
