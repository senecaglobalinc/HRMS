import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import * as servicePath from '../../../service-paths';
import { environment } from '../../../../environments/environment';
import { ProjectTypeData } from '../Models/projecttype.model';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ProjectTypeService {
  editMode = false;
  projectTypeData = new BehaviorSubject<ProjectTypeData[]>([]);
  editObj = new BehaviorSubject<ProjectTypeData>(new ProjectTypeData());
  serviceUrl = environment.AdminMicroService;
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
