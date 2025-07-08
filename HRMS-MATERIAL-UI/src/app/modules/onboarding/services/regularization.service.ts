import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import * as servicePath from '../../../core/service-paths';
import { AttendanceRegularization, NotPunchInDate } from '../models/regularization.model';


@Injectable({
  providedIn: 'root'
})
export class RegularizationService {
  private _resources: any;
  private leaveResource :  any;

  constructor(private _http: HttpClient) { 
    this._resources = servicePath.API.attendanceRegularization;
    this.leaveResource = servicePath.API.uploadLeaveData
  }

  public GetNotPunchInDate(reqObj:NotPunchInDate) {
    var url = environment.EmployeeMicroService + this._resources.GetNotPunchInDate;
    return this._http.post(url,reqObj);
  }

  public saveAttendanceRegularizationDetails(reqObj){
    var url = environment.EmployeeMicroService + this._resources.SaveAttendanceRegularizationDetails;
    return this._http.post(url,reqObj);
  }

  public GetAllAssociateSubmittedAttendanceRegularization(Id:number,roleName:string){
    var url = environment.EmployeeMicroService + this._resources.GetAllAssociateSubmittedAttendanceRegularization + Id +"/"+ roleName;
    return this._http.get(url);
  }

  public GetAssociateSubmittedAttendanceRegularizationById(associateId : string, roleName:string){
    var url = environment.EmployeeMicroService + this._resources.GetAssociateSubmittedAttendanceRegularization + associateId +"/"+roleName;
    return this._http.get(url);
  }

  public ApproveOrRejectAttendanceRegularizationDetails(reqObj : AttendanceRegularization){
    var url = environment.EmployeeMicroService + this._resources.ApproveOrRejectAttendanceRegularizationDetails;
    return this._http.post(url,reqObj);
  }

  public uploadLeaveData(data:FormData){
    var url = environment.EmployeeMicroService + this.leaveResource.UploadLeaveData;
   
    return this._http.post(url,data);
  }

  public getTemplateFile(){

    var url = environment.EmployeeMicroService + this.leaveResource.GetTemplateFile;

    return this._http.get(url,{ headers: {

      'Content-type': 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'

   } });

  }

}
