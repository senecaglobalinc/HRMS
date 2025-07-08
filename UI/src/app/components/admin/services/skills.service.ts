import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import * as servicePath from '../../../service-paths';
import { BehaviorSubject } from 'rxjs';
import { SkillData } from '../../../models/skills.model';

@Injectable({
  providedIn: 'root'
})
export class SkillsService {
  public editMode = false;
  private _serviceUrl = environment.AdminMicroService;
  // _serviceUrl = "http://sg-srv-vtsapps:2021/admin/api/v1";
  private resources = servicePath.API.Skills;
  public skillsEdit = new BehaviorSubject<SkillData>(new SkillData());
  public skillsList = new BehaviorSubject<SkillData[]>([]);
  constructor(private httpClient : HttpClient ) { }

  public getCompetencyAreaList() {
    return this.httpClient.get(this._serviceUrl +servicePath.API.CompetencyArea.list) 
  }

  public getSkillGroupByCompetencyArea(competencyAreaID: number){
    return this.httpClient.get(this._serviceUrl +servicePath.API.SkillGroup.getSkillGroupsByCompetenctArea + competencyAreaID ) 
  }
  
  public getSkills() : void {
    this.httpClient.get(this._serviceUrl + this.resources.list)
    .subscribe((res : SkillData[]) =>{ this.skillsList.next(res);});
  }

  public createSkills(skills : SkillData){
    if(this.editMode ==false)
      return this.httpClient.post(this._serviceUrl + this.resources.create , skills);
    else
      return this.httpClient.post(this._serviceUrl + this.resources.update , skills);
  }
}






