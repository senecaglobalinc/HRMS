import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import { KeyFunction } from '../models/key-function.model';
import * as servicePath from '../../../core/service-paths';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class KeyFunctionService{
  public editMode = false;
  private resources = servicePath.API.KeyFunction;
  public KeyFunctionEdit = new BehaviorSubject<KeyFunction>(new KeyFunction());
  public KeyFunctionList = new BehaviorSubject<KeyFunction[]>([]);
  // private _serviceUrl = environment.ServerUrl;
  _serviceUrl = environment.AdminMicroService;
  // private _serviceUrl = "http://sg-srv-vtsapps:2021/admin/api/v1";
  constructor(private httpClient : HttpClient ) { }
  
  public getKeyFunctions(){
    this.httpClient.get(this._serviceUrl + this.resources.list)
    .subscribe((res : KeyFunction[]) => {this.KeyFunctionList.next(res);});
  }
  public getDepartmentList(){
    return this.httpClient.get(this._serviceUrl +servicePath.API.Department.list) 
  }
  public createKeyFunction(keyFunction : KeyFunction){
    if(this.editMode == false)
      return this.httpClient.post(this._serviceUrl + this.resources.create, keyFunction)
    else
      return this.httpClient.post(this._serviceUrl + this.resources.update, keyFunction)
    }
}
