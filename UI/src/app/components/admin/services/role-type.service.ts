import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import * as servicePath from '../../../service-paths';
import { Observable } from 'rxjs';
import { Roletype } from '../models/roletype.model';

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
    return this.httpClient.put(this._serviceUrl + this.resources.Update, model);
  }
  DeleteRoleTypes(id) {
    return this.httpClient.delete(this._serviceUrl + this.resources.Delete + '/' + id);
  }
   GetActiveRoleTypes(): Observable<Roletype[]> {
    return this.httpClient.get<Roletype[]>(this._serviceUrl + this.resources.Get+"?isActive=true");
  }
}