import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import * as servicePath from '../../../core/service-paths';

@Injectable({
  providedIn: 'root'
})
export class PmApprovalService {

  private _employeeMicroService : string;
  _resources = servicePath.API.Resignation;

  constructor(private httpClient: HttpClient) {
    this._employeeMicroService = environment.EmployeeMicroService;
   }

   public submitPMApproval(AssociateApprovalDetails){
    let _url = this._employeeMicroService + this._resources.PMApproval;
    return this.httpClient.post(_url,AssociateApprovalDetails);
  }

  public withdrawRequest(employeeId, resignationRecommendation){
    let _url = this._employeeMicroService + this._resources.WithdrawRequest + employeeId + '&resignationRecommendation='+resignationRecommendation;
    return this.httpClient.get(_url);
  }

  public ApproveOrRejectRevoke(submittedData){
    
    let _url = this._employeeMicroService + this._resources.ApproveOrRejectRevoke;
    return this.httpClient.post(_url,submittedData);
  }

  public ReviewByPM(AcceptByPMDetails){
    let _url = this._employeeMicroService + this._resources.ReviewByPM;
    return this.httpClient.post(_url,AcceptByPMDetails);
  }

  public ReviewReminderNotification(EmpId){
    let _url = this._employeeMicroService + this._resources.ReviewReminderNotification + EmpId;
    return this.httpClient.get(_url);
  }

}
