import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import * as environmentInformation from '../../environments/environment';
import * as servicePath from '../service-paths';

@Injectable({
  providedIn: 'root'
})
export class MenuService {
  _serverURL: string;
  _roleInstance: any;
  _menuInstance: any;

  constructor(private _http: HttpClient) {
    this._serverURL = environmentInformation.environment.ServerUrl;
    this._menuInstance = servicePath.API.Menu;
  }

  getMenuDetails(roleName: string) {
    let _url = this._serverURL + this._menuInstance.GetMenuDetails + roleName;
    return this._http.get(_url);
  }

}
