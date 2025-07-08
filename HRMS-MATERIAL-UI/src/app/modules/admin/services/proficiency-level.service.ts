import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import * as servicePath from '../../../core/service-paths';
import { environment } from '../../../../environments/environment';
import { ProficiencyLevel } from '../models/proficiencyLevel.model';
import { BehaviorSubject } from 'rxjs';


@Injectable({
  providedIn: 'root'
})
export class ProficiencyLevelService {
  proficiencyLevelData = new BehaviorSubject<ProficiencyLevel[]>([]);
  editObj =  new BehaviorSubject<ProficiencyLevel>(new ProficiencyLevel());
  editMode = false;
  // serviceUrl = environment.ServerUrl;
  serviceUrl = environment.AdminMicroService;
  // serviceUrl = "http://sg-srv-vtsapps:2021/admin/api/v1";
  resources = servicePath.API.ProficiencyLevel;
  constructor( private httpClient : HttpClient) { }
  getProficiencyLevelData(){
    this.httpClient.get( this.serviceUrl+this.resources.list)
    .subscribe((res : ProficiencyLevel[]) =>{
      this.proficiencyLevelData.next(res);
    });
  }
  createProficiencyLevelData(creatingObj){
     if(this.editMode == false)
        return this.httpClient.post( this.serviceUrl+this.resources.create,creatingObj);
    else{
      return this.httpClient.post(this.serviceUrl+this.resources.update,creatingObj);
    }
  }
}
