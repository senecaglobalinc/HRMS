import {Injectable } from '@angular/core';
import {HttpClient } from '@angular/common/http';
import * as servicePath from '../../../core/service-paths';
import {environment } from '../../../../environments/environment';
import { Associate } from '../models/associate.model';

@Injectable({
  providedIn: 'root'
})
export class EditprospectiveassociateService {

  private _roleName: string;
  private _serverURL: string;
  private _hrm: string;
  private _resources:any;
  private _serviceUrl = environment.ServerUrl;

  public notificationCount: number;
  constructor(private httpClient:HttpClient) {
     let loginData = JSON.parse(sessionStorage["AsscoiatePortal_UserInformation"]);
      this._roleName = loginData.roleName;
      this._resources = servicePath.API.PAssociate;
  }  
  getPADetailsById(id:number) {        
      let _url = this._serverURL + this._resources.get+id;

      return new Promise((resolve, reject) => {
          this.httpClient.get(_url);

      });
  }
  UpdatePADetails(newAssociate:Associate) {        
      let _url = this._serverURL + this._resources.update;

        return new Promise((resolve, reject) => {
          this.httpClient.post(_url, newAssociate);
      });
  }
  
}

