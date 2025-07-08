import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import * as servicePath from '../../../core/service-paths';
import { environment } from '../../../../environments/environment';
import { Associate } from '../models/associate.model';

@Injectable({
  providedIn: 'root'
})
export class EmploymentService {
  private _resources: any;
  private _serverURL: string;
  private EmployeeMicroService = environment.EmployeeMicroService;
  constructor(private _httpclient: HttpClient) {
      this._serverURL = environment.ServerUrl;
      this._resources = servicePath.API.employment;
  }

  public GetEmploymentDetails(id: number){
    var url = this.EmployeeMicroService + this._resources.GetEmploymentDetails + id;
    return this._httpclient.get(url);
   
}


public GetProfReferenceDetails(id: number){
  var url = this.EmployeeMicroService + this._resources.GetProfReferenceDetails + id;
  return this._httpclient.get(url);

}

SaveEmploymentDetails(details: Associate) {
  let _url = this.EmployeeMicroService + this._resources.SaveEmployementDetails;
  return this._httpclient.post(_url, details); 
}

}



