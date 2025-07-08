import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Designation, DesignationData } from '../models/designation.model';
import { BehaviorSubject } from 'rxjs';
import * as servicePath from '../../../service-paths';

@Injectable({
  providedIn: 'root'
})
export class DesignationsService {
  _serviceUrl = environment.AdminMicroService;
  editMode = false;
  editObj = new BehaviorSubject<DesignationData>(new DesignationData());
  designationData = new BehaviorSubject<DesignationData[]>([]);
  resources = servicePath.API.Designation;
  constructor(private httpClient : HttpClient ) { }
  
  getGradesData(){
    return this.httpClient.get(this._serviceUrl +servicePath.API.Grades.list)
  }


  getDesignation(){
     this.httpClient.get(this._serviceUrl +this.resources.list).subscribe((res:DesignationData[])=>{
       this.designationData.next(res);
      });
  }

  createDesignation(designation : DesignationData){
    return this.httpClient.post(this._serviceUrl +this.resources.create , designation)
  }

  editDesignation( designation :DesignationData){
    return this.httpClient.post(this._serviceUrl + this.resources.update , designation)
  }
}
