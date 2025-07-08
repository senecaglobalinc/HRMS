import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import * as servicePath from '../../../core/service-paths'

@Injectable({
  providedIn: 'root'
})
export class AssociateExitDashboardService {

  _resources = servicePath.API.Resignation;
  _resoureReview = servicePath.API.AssociateExitInterviewReviewInfo
  private _employeeMicroService: string;

  constructor(private httpClient: HttpClient) {
    this._employeeMicroService = environment.EmployeeMicroService;
   }

   getAssociateExitDashbaord(userRole: string, empId: number, dashboard: string){
    const departmentId = JSON.parse(
      sessionStorage.Department
    ).DepartmentId;
    const _url = this._employeeMicroService +this._resources.GetAssociateExitDashbaord + userRole + '/' + empId + '/' + dashboard + '/' + departmentId;
    return this.httpClient.get<any[]>(_url);
   }

   associateExitInterviewReview(fromdate,todate){
      const reqObj = {'fromdate':fromdate,'todate':todate};
      const _url = this._employeeMicroService +this._resoureReview.AssociateExitInterviewReview
      return this.httpClient.post<any[]>(_url,reqObj);
   }
   create(reqObj){
    const _url = this._employeeMicroService +this._resoureReview.Create
    return this.httpClient.post<any[]>(_url,reqObj);
 }
}
