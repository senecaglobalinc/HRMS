import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import { KeyFunction } from '../models/key-function.model';
import * as servicePath from '../../../service-paths';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class KeyFunctionService{
  public editMode = false;
  private resources = servicePath.API.KeyFunction;
  public KeyFunctionEdit = new BehaviorSubject<KeyFunction>(new KeyFunction());
  public KeyFunctionList = new BehaviorSubject<KeyFunction[]>([]);
  _serviceUrl = environment.AdminMicroService;
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
