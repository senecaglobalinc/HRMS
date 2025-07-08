import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import * as servicePath from '../../../core/service-paths';
import { environment } from '../../../../environments/environment';
import { AssociateLongLeaveData } from '../Models/associate-longleave.model';

@Injectable({
  providedIn: 'root'
})
export class AssociateLongleaveService {
  private resources: any;
  private _serverURL: string;
  private _employeeMicroService: string;

  constructor(private httpClient: HttpClient) {
    this._serverURL = environment.ServerUrl;
    this.resources = servicePath.API.AssociateLongLeave;
    this._employeeMicroService = environment.EmployeeMicroService;
   }
   public CreateAssociateLongLeave(associateResignationData: AssociateLongLeaveData){
    let _url= this._employeeMicroService + this.resources.CreateAssociateLongLeave;
    return this.httpClient.post(_url, associateResignationData);
  }

  public CalculateMaternityPeriod(maternityStartDate: string){
    let _url = this._employeeMicroService + this.resources.CalculateMaternityPeriod + maternityStartDate;
    return this.httpClient.get(_url,{responseType: 'text'});
  }

  public RejoinAssociate(empID:number, reason:string, rejoinedDate: string) {
    var url=this._employeeMicroService+this.resources.RejoinAssociate+empID+"&reason="+reason+"&rejoinedDate="+rejoinedDate;        
    return this.httpClient.get(url);
}
}
