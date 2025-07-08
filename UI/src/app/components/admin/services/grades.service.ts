import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Grade } from '../models/grade.model';
import * as environment from '../../../../environments/environment';
import * as servicePath from '../../../service-paths';
import { BehaviorSubject } from 'rxjs';


@Injectable({
  providedIn: 'root'
})
export class GradesService {
  editMode = false;
  GradesData = new BehaviorSubject<Grade[]>([]);
  editObj = new BehaviorSubject<Grade>(new Grade());
  serviceUrl = environment.environment.AdminMicroService;
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
