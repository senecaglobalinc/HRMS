import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {environment} from '../../../../environments/environment'

import * as servicePath from '../../../core/service-paths'
import {Grade} from '../models/grade.model';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class GradesService {

  editMode = false;
  GradesData = new BehaviorSubject<Grade[]>([]);
  editObj = new BehaviorSubject<Grade>(new Grade());
  // serviceUrl = environment.environment.ServerUrl;
  serviceUrl = environment.AdminMicroService;
  // serviceUrl = "http://sg-srv-vtsapps:2021/admin/api/v1";
  resources = servicePath.API.Grades;
  constructor(private httpclient : HttpClient) { }

  getGradesDetails(){
    this.httpclient.get(this.serviceUrl+this.resources.list).subscribe((res : Grade[])=>{
      this.GradesData.next(res);
    });
 }

 createGrades(createObj){
   if(this.editMode == false )
     return this.httpclient.post(this.serviceUrl+this.resources.create ,createObj);
   else{
       return this.httpclient.post(this.serviceUrl+this.resources.update ,createObj);
   }
  
 }
}
