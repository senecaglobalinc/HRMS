import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import * as servicePath from '../../../core/service-paths';
import { environment } from '../../../../environments/environment';
import { DeliveryHeadTalentRequisitionDetails } from '../../master-layout/models/talentrequisitionhistory.model';

@Injectable({
  providedIn: 'root',
})
export class FinanceHeadService {
  _serverUrl: string;
  _resources: any;
  _employeeId: number;

  constructor(private httpClient: HttpClient) {
    this._serverUrl = environment.ServerUrl;
    this._resources = servicePath.API.FinanceHead;
    this._employeeId = JSON.parse(
      sessionStorage['AssociatePortal_UserInformation']
    ).employeeId;
  }
  public GetPendingRequisitionsForApproval() {
    var url =
      this._serverUrl +
      this._resources.getPendingRequisitionsForApproval +
      this._employeeId;
    return this.httpClient.get(url);
  }

  public RequisitionApprovalByFinance(
    talentRequisitionDetails: DeliveryHeadTalentRequisitionDetails
  ) {
    let _url = this._serverUrl + this._resources.requisitionApprovalByFinance;
    return this.httpClient.post(_url, talentRequisitionDetails);
  }

  public RequisitionRejectionByFinance(
    talentRequisitionDetails: DeliveryHeadTalentRequisitionDetails
  ) {
    let _url = this._serverUrl + this._resources.requisitionRejectionByFinance;

    return this.httpClient.post(_url, talentRequisitionDetails);
  }
}
