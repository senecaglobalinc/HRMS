import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import * as servicePath from '../../../core/service-paths';
import { environment } from '../../../../environments/environment';
import { DeliveryHeadTalentRequisitionDetails } from '../../master-layout/models/talentrequisitionhistory.model';
import { ProjectDetails } from '../../master-layout/models/projects.model';

@Injectable({
  providedIn: 'root',
})
export class DeliveryHeadService {
  private _resources: any;
  private _serverURL: string;
  private _projectMicroService: string;
  private _employeeId: number;
  constructor(private httpClient: HttpClient) {
    this._serverURL = environment.ServerUrl;
    this._projectMicroService = environment.ProjMicroService;
    this._resources = servicePath.API.deliveryHead;
    this._employeeId = JSON.parse(
      sessionStorage['AssociatePortal_UserInformation']
    ).employeeId;
  }

  GetProjectsList(userRole: string, empId: number, dashboard: string) {
    const _url =
      this._projectMicroService +
      this._resources.getProjectList +
      userRole +
      '&employeeId=' +
      empId +
      '&dashboard=' +
      dashboard;
    return this.httpClient.get<ProjectDetails[]>(_url);
  }

  public GetApproverList() {
    var url = this._serverURL + this._resources.getApproverList;
    return this.httpClient.get(url);
  }

  ApproveOrRejectByDH(projectId: number, status: string, employeeId: number) {
    let _url =
      this._projectMicroService +
      this._resources.ApproveOrRejectByDH +
      projectId +
      '/' +
      status +
      '/' +
      employeeId;
    return this.httpClient.get(_url);
  }

  public GetPendingRequisitionsForApproval() {
    var url =
      this._serverURL +
      this._resources.getPendingRequisitionsForApproval +
      this._employeeId;
    return this.httpClient.get(url);
  }
  public GetRolePositionDetailsByTRID(trID: number) {
    var url =
      this._serverURL + this._resources.getRolePositionDetailsByTRID + trID;
    return this.httpClient.get(url);
  }
  public GetTrRoleEmployeeList(talentRequisitionId: number, roleId: number) {
    var url =
      this._serverURL +
      this._resources.getTrRoleEmployeeList +
      talentRequisitionId +
      '&roleId=' +
      roleId;
    return this.httpClient.get(url);
  }
  public MarkedAsApprove(
    selectedApprovers: DeliveryHeadTalentRequisitionDetails
  ) {
    let _url = this._serverURL + this._resources.ApproveTalentRequisition;
    return this.httpClient.post(_url, selectedApprovers);
  }

  public RejectTalentRequisition(
    talentRequisitionDetails: DeliveryHeadTalentRequisitionDetails
  ) {
    let _url = this._serverURL + this._resources.rejectTalentRequisition;
    return this.httpClient.post(_url, talentRequisitionDetails);
  }
}
