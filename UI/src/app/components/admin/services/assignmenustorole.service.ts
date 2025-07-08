import { Injectable, Inject } from '@angular/core';
import { MenuRoles, Menus } from '../Models/menu-roles.model';
import { Observable } from 'rxjs';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { IAssignMenusToRole } from "../../../Interfaces/IAssignMenuToRole";
import * as servicePath from '../../../service-paths';
import * as environmentInformation from '../../../../environments/environment';
import { environment } from '../../../../environments/environment';



@Injectable({
  providedIn: 'root'
})
export class AssignmenustoroleService implements IAssignMenusToRole {
  private _resources: any;
  private _employeeId: number;
  private _adminMicroService: string;
  constructor(private _http: HttpClient) {
    this._resources = servicePath.API.AssignMenusRole;
    this._adminMicroService = environment.AdminMicroService;
  }

  public getSourceMenus(RoleId: number): Observable<Menus[]> {
      let _url = this._adminMicroService + this._resources.getSourceMenuRoles + RoleId;
      return this._http.get<Menus[]>(_url);
  }

  public getTargetMenus(RoleId: number): Observable<Menus[]> {
      let _url = this._adminMicroService + this._resources.getTargetMenuRoles + RoleId;
      return this._http.get<Menus[]>(_url);
  }

  public addTargetMenuRoles(TargetMenuRoles: MenuRoles): Observable<boolean> {
      let _url = this._adminMicroService + this._resources.addTargetMenuRoles;
      return this._http.post<boolean>(_url, TargetMenuRoles);
  }
}
