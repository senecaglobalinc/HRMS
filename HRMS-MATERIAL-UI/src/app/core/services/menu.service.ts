import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import * as environmentInformation from '../../../environments/environment';
import * as servicePath from '../service-paths';

@Injectable({
  providedIn: 'root'
})
export class MenuService {
  _serverURL: string;
  _serverURLs: string;
  _roleInstance: any;
  _menuInstance: any;

  constructor(private _http: HttpClient) {
    this._serverURL = environmentInformation.environment.ServerUrl;
    this._serverURLs = environmentInformation.environment.AdminMicroService;
    this._menuInstance = servicePath.API.Menu;
    this._roleInstance= servicePath.API.Role;
  }

  getMenuDetails(roleName: string) {
    let _url = this._serverURL + this._menuInstance.GetMenuDetails + roleName;
    return this._http.get(_url);
  }
  GetUserDetailsByUserName(userName: string) {
    let _url = this._serverURLs + this._roleInstance.GetUserDetailsByUserName + userName;
    return this._http.get(_url);
  }

}
