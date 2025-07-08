import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import { CompetencyArea } from '../models/competencyarea.model';
import * as servicePath from '../../../service-paths';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CompetencyAreaService {
  serviceUrl = environment.AdminMicroService;
  editMode = false;
  editObj = new BehaviorSubject<CompetencyArea>(new CompetencyArea());
  resources = servicePath.API.CompetencyArea;
  competencyAreaData = new BehaviorSubject<CompetencyArea[]>([]);
  constructor(private httpClient : HttpClient) { }
  
  GetCompetencyAreaData(){
     this.httpClient.get(this.serviceUrl+this.resources.list)
                          .subscribe((res : CompetencyArea[]) =>{ this.competencyAreaData.next(res);});
  }

  CreateCompetencyArea(createObj:CompetencyArea){
    if(this.editMode ==false)
       return this.httpClient.post(this.serviceUrl+this.resources.create, createObj);
    
    else{ 
     return this.httpClient.post(this.serviceUrl+this.resources.update, createObj);
    }

  }

  
}
