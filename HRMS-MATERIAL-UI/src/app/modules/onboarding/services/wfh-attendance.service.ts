import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import * as servicePath from '../../../core/service-paths';

@Injectable({
  providedIn: 'root'
})
export class WfhAttendanceService {
  private _resources: any;
  

  constructor(private _http: HttpClient) {
    this._resources = servicePath.API.wfhAttendance;
   }
  public GetloginStatus(empId:string) {
    var url = environment.EmployeeMicroService + this._resources.GetloginStatus + empId;
    return this._http.get(url);
  }

  public SaveAttendanceDetais(saveReqObj) {
    var url = environment.EmployeeMicroService + this._resources.SaveAttendanceDetails;
    return this._http.post(url,saveReqObj);
  }

  public GetAttendanceDetails(empId:string) {
    var url = environment.EmployeeMicroService + this._resources.GetAttendanceDetails + empId;
    return this._http.get(url);
  }
}
