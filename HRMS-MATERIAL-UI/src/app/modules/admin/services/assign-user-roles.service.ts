import { Injectable } from '@angular/core';
import * as environment from '../../../../environments/environment';
import * as servicePath from '../../../core/service-paths'
import { BehaviorSubject } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { UserRoles } from '../models/userRoles';


@Injectable({
  providedIn: 'root'
})
export class AssignUserRolesService {
  serviceUrl = environment.environment.AdminMicroService;
  // serviceUrl = environment.environment.ServerUrl;
  resources = servicePath.API.UserNames;
  userRole = new BehaviorSubject<UserRoles>(new UserRoles());
  userRolesList = new BehaviorSubject<UserRoles[]>([]);
  constructor(private httpclient : HttpClient) { }

  public GetUserRoles(id: number) {
    var url = this.serviceUrl + this.resources.getById + id;
    this.httpclient.get(url).subscribe((res : UserRoles[])=>{
      this.userRolesList.next(res);
    });
}
public GetRolesList(){
  return this.userRolesList.asObservable();
}
public GetUserRole(){
  return this.userRole.asObservable();
}
public SetUserRole(userRole){
  this.userRole.next(userRole);
  this.GetUserRole();
}
public GetUserNames(param){
  var url = this.serviceUrl + this.resources.GetUsersBySearchString +param;
  return  this.httpclient.get(url);
}
public SetUserRolesList(data){
  this.userRolesList.next(data);
}

// public GetRoleByDept(id: number){
//     var url = this.serviceUrl + this.resources.getRoleByDept + id;
//     return this.httpclient.get(url)
// }

SaveOrUpdateUserRoles(details) {
    let _url = this.serviceUrl + this.resources.update;
    return this.httpclient.post(_url, details) 
}
}
