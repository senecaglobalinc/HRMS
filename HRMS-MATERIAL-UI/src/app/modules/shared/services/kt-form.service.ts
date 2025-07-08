import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import * as servicePath from '../../../core/service-paths';
import { ActivityList } from '../../AssociateExit/Models/activitymodel';
import { Activity } from '../../AssociateExit/Models/associateExit.model';
import { GenericType } from '../../master-layout/models/dropdowntype.model';
import { KtForm, KtFormWithSubStatus } from '../models/kt-form.model';

@Injectable({
  providedIn: 'root'
})
export class KtFormService {

  private _adminMicroService : string;
  private _employeeMicroService : string;
  _resources = servicePath.API.TransitionPlan

  constructor(private httpClient: HttpClient) { 
  this._adminMicroService = environment.AdminMicroService;
  this._employeeMicroService = environment.EmployeeMicroService;

  }

  getKtTasks(){
    let _url = this._adminMicroService + servicePath.API.Activity.GetTransitionPlanActivities;
    return this.httpClient.get<Activity[]>(_url);
  }

  getKtPlan(empId,projectId){
    let _url = this._employeeMicroService + this._resources.GetForTeamLead + '?employeeId=' + empId+'&projectId='+ projectId;
    return this.httpClient.get<KtForm>(_url);
  }

  getKtPlansForAssociate(empId){
    let _url = this._employeeMicroService + this._resources.GetForAssociate + '?employeeId=' + empId;
    return this.httpClient.get<KtFormWithSubStatus[]>(_url);
  }

  updateKtTasks(updateKtTasks){
    let _url = this._employeeMicroService + this._resources.Update;
    return this.httpClient.post(_url,updateKtTasks);
  }

  deleteKtTasks(empId,projectId,activityId){
    let _url = this._employeeMicroService + this._resources.DeleteActivity + '?employeeId='+ empId+ '&projectId=' + projectId + '&activityId='+ activityId;
    return this.httpClient.delete(_url);
  }


}
