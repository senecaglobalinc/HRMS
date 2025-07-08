import {  Injectable } from '@angular/core';
import {HttpClient } from'@angular/common/http';
import * as servicePath from '../../../service-paths';
import {environment } from '../../../../environments/environment';
import { Associate } from '../models/associate.model';

@Injectable({
  providedIn: 'root'
})
export class ProspectiveassociateService {
  private _roleName: string;
  private _hrm: string;
  private _resources:any;
  private _serverURL = environment.ServerUrl;
  private EmployeeMicroService : any;
  public notificationCount: number;
  constructor(private httpClient:HttpClient) {
      this._resources = servicePath.API.PAssociate;   
      this.EmployeeMicroService = environment.EmployeeMicroService;    
  }  
  getPADetailsById(id:number) {        
      let _url = this.EmployeeMicroService + this._resources.get+id;
      return this.httpClient.get(_url);         
  }
  UpdatePADetails(newAssociate:Associate) {        
      let _url = this.EmployeeMicroService + this._resources.update;
        return this.httpClient.post(_url, newAssociate);
          
  }
}

