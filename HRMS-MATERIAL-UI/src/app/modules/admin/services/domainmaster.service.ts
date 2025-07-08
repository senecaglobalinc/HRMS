import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {environment} from '../../../../environments/environment'
import {DomainMasterData} from '../models/domainmasterdata.model'
import * as servicePath from '../../../core/service-paths'
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DomainMasterService {
  public editMode = false;
  private resources = servicePath.API.Domain;
  public domainEdit = new BehaviorSubject<DomainMasterData>(new DomainMasterData());
  public domainsList = new BehaviorSubject<DomainMasterData[]>([]);
  // private _serviceUrl = environment.ServerUrl;
  _serviceUrl = environment.AdminMicroService;
  // private _serviceUrl = "http://sg-srv-vtsapps:2021/admin/api/v1";
  constructor(private httpClient : HttpClient ) { }
  
  public getDomains(){
    this.httpClient.get(this._serviceUrl + this.resources.list)
    .subscribe((res : DomainMasterData[]) => {this.domainsList.next(res);});
  }

  public createDomain(domain : DomainMasterData){
    if(this.editMode == false)
      return this.httpClient.post(this._serviceUrl + this.resources.create, domain)
    else
      return this.httpClient.post(this._serviceUrl + this.resources.update, domain)
    }

  
}
