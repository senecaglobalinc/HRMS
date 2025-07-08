import { Injectable } from '@angular/core';
import { Observable } from "rxjs";
import { HttpClient } from '@angular/common/http';
import { ClientBillingRole } from "../models/clientbillingroles.model";
import * as environmentInformation from '../../../../environments/environment';
import * as servicePath from '../../../service-paths';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ClientbillingroleService {
  subscribe(arg0: (res: any) => void): any {
    throw new Error("Method not implemented.");
  }
  private _serverURL: string;
  toGetData = false;
  editMode = false;
  editObj = new BehaviorSubject<ClientBillingRole>(new ClientBillingRole());
  private _resources: any;
  ClientBillingRole = new BehaviorSubject<ClientBillingRole[]>([]);

  constructor(private _http: HttpClient) {
    this._serverURL = environmentInformation.environment.ServerUrl;
    this._resources = servicePath.API.ClientBillingRole;
  }


  GetClientBillingRoles(){
     this._http.get(this._serverURL+this._resources.list )
                          .subscribe((res : ClientBillingRole[]) =>{ this.ClientBillingRole.next(res);});
  }


 public CreateClientBillingRole(createObj: ClientBillingRole): Observable<number> {
   let _url = this._serverURL + this._resources.create;
   if(this.editMode ==false)
   return this._http.post<number>(_url,   createObj);
 
 }

 public UpdateClientBillingRole(createObj: ClientBillingRole): Observable<number> {
   let _url = this._serverURL + this._resources.update;
   if(this.editMode ==false)
   return this._http.post<number>(_url, createObj);
 }
}