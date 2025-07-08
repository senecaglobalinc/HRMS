import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import { Seniority } from '../models/seniority.model';
import * as servicePath from '../../../core/service-paths';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SeniorityService {
  public editMode = false;
  private resources = servicePath.API.Seniority;
  public seniorityEdit = new BehaviorSubject<Seniority>(new Seniority());
  public seniorityList = new BehaviorSubject<Seniority[]>([]);
  // private _serviceUrl = environment.ServerUrl;
  _serviceUrl = environment.AdminMicroService;
  // private _serviceUrl = "http://sg-srv-vtsapps:2021/admin/api/v1";
  constructor(private httpClient : HttpClient ) { }
  
  public getSeniorities(){
    this.httpClient.get(this._serviceUrl + this.resources.list)
    .subscribe((res : Seniority[]) => {this.seniorityList.next(res);});
  }

  public createSeniority(seniority : Seniority){
    if(this.editMode == false)
      return this.httpClient.post(this._serviceUrl + this.resources.create, seniority)
    else
      return this.httpClient.post(this._serviceUrl + this.resources.update, seniority)
    }
}
