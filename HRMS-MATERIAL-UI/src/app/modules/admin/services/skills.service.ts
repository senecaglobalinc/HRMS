import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {environment} from '../../../../environments/environment';
import * as servicePath from '../../../core/service-paths';
import { BehaviorSubject } from 'rxjs';
import { SkillData } from '../models/skill-data.model';

@Injectable({
  providedIn: 'root'
})
export class SkillsService {
  public editMode = false;
  private _serverURL: string;
  private _resource: any;

  _serviceUrl = environment.AdminMicroService;
  private resources = servicePath.API.Skills;
  employeeUrl = environment.EmployeeMicroService;
  
  public skillsEdit = new BehaviorSubject<SkillData>(new SkillData());
  public skillsList = new BehaviorSubject<SkillData[]>([]);
  

  constructor(private httpClient : HttpClient) {
    this._serverURL = environment.ServerUrl;
    this._resource = servicePath.API.SkillSearch;


   }

  public getCompetencyArea(){
    return this.httpClient.get(this._serviceUrl + servicePath.API.CompetencyArea.list);
  }

  public getSkillGroupByCompetencyArea(competencyAreaId: number){
    return this.httpClient.get(this._serviceUrl + servicePath.API.SkillGroup.getSkillGroupsByCompetenctArea + competencyAreaId);
  }

  public getSkills(){
    return this.httpClient.get(this._serviceUrl + this.resources.list)
    .subscribe((res : SkillData[]) =>{ this.skillsList.next(res);});
  }

  public GetAllSkillDetails(EmployeeId: number) {
    const url = this.employeeUrl + this._resource.GetAllSkillDetails + EmployeeId;
    return this.httpClient.get(url);
  }


  // getActivitiesByHRA(EmployeeId: number){
  //   const url = this.associateMicroService +this._resources.GetActivitiesByemployeeIdForHRA + '?employeeId=' +EmployeeId;
  //   return this.httpClient.get(url);
  // }


  public createSkills(skills : SkillData){
    if(this.editMode ==false)
      return this.httpClient.post(this._serviceUrl + this.resources.create , skills);
    else
      return this.httpClient.post(this._serviceUrl + this.resources.update , skills);
  }

  
}