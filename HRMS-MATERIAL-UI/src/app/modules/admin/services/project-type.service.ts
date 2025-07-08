import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import * as servicePath from '../../../core/service-paths';
import { environment } from '../../../../environments/environment';
import { ProjectTypeData } from '../models/projecttype.model';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ProjectTypeService {
  editMode = false;
  projectTypeData = new BehaviorSubject<ProjectTypeData[]>([]);
  editObj = new BehaviorSubject<ProjectTypeData>(new ProjectTypeData());
  // serviceUrl = environment.ServerUrl;
  serviceUrl = environment.AdminMicroService;
  // serviceUrl = "http://sg-srv-vtsapps:2021/admin/api/v1";
  resources = servicePath.API.ProjectType;
  constructor( private httpClient : HttpClient) { }

  getProjectTypeData(){
    this.httpClient.get(this.serviceUrl+this.resources.list)
    .subscribe((res : ProjectTypeData[]) =>{
      this.projectTypeData.next(res);
    });
  }
  createProjectType(createObj : ProjectTypeData){

    if(this.editMode == false)
      return this.httpClient.post(this.serviceUrl+this.resources.create, createObj);
    else{
      return this.httpClient.post(this.serviceUrl+this.resources.update, createObj);
    }
  }
}
