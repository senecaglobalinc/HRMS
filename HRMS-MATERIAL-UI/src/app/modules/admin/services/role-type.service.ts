import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {environment} from '../../../../environments/environment'
import { Observable } from "rxjs";
import * as servicePath from '../../../core/service-paths'
import {Roletype} from '../models/roletype.model';
import { GenericType } from '../../master-layout/models/dropdowntype.model';

@Injectable({
  providedIn: 'root'
})
export class RoleTypeService {
  _serviceUrl = environment.AdminMicroService;
  resources = servicePath.API.RoleType;
  constructor(private httpClient: HttpClient) { }

  GetRoleTypes(): Observable<Roletype[]> {
    return this.httpClient.get<Roletype[]>(this._serviceUrl + this.resources.Get);
  }
  CreateRoleTypes(model: Roletype) {
    return this.httpClient.post(this._serviceUrl + this.resources.Create, model);
  }
  UpdateRoleTypes(model: Roletype) {
    return this.httpClient.post(this._serviceUrl + this.resources.Update, model);
  }
  DeleteRoleTypes(id) {
    return this.httpClient.delete(this._serviceUrl + this.resources.Delete + '/' + id);
  }
     GetActiveRoleTypes(): Observable<Roletype[]> {
    return this.httpClient.get<Roletype[]>(this._serviceUrl + this.resources.Get+"?isActive=true");
  }
  GetGradeRoleTypes(financialYearId:number, departmentId:number, roleTypeId:number): Observable<Roletype[]> {
    return this.httpClient.get<Roletype[]>(this._serviceUrl + this.resources.GetGradesByDepartment + financialYearId + "&departmentId=" + departmentId + "&roleTypeId="  + roleTypeId);
  }

  GetRoleTypesForDropdown(financialYearId:number, departmentId:number): Observable<GenericType[]> {
    return this.httpClient.get<GenericType[]>(this._serviceUrl + this.resources.GetRoleTypesForDropdown + financialYearId + "&departmentId=" + departmentId);
  }

}
