import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { SkillGroupData } from '../models/skill-group-data.model';
import * as servicePath from '../../../core/service-paths';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class SkillGroupService {
  public editMode = false;
  public skillGroupEdit = new BehaviorSubject<SkillGroupData>(new SkillGroupData());
  public skillGroupList = new BehaviorSubject<SkillGroupData[]>([]);

  _serviceUrl = environment.AdminMicroService;
  private resources = servicePath.API.SkillGroup;

  constructor(private httpClient : HttpClient) { }

  public getCompetencyArea(){
    return this.httpClient.get(this._serviceUrl + servicePath.API.CompetencyArea.list);
  }

  public getSkillGroup(){
    return this.httpClient.get(this._serviceUrl + this.resources.list)
    .subscribe((res :SkillGroupData[]) => {this.skillGroupList.next(res);});
  }

  public createSkillGroup(skillgroup : SkillGroupData){
    if(this.editMode ==false)
      return this.httpClient.post(this._serviceUrl + this.resources.create , skillgroup);
    else
      return this.httpClient.post(this._serviceUrl + this.resources.update , skillgroup);
  }
}
