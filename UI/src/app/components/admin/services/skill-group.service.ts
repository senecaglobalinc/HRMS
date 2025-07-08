import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import * as servicePath from '../../../service-paths';
import { BehaviorSubject } from 'rxjs';
import { SkillGroupData } from '../../../models/skillgroup.model';

@Injectable({
  providedIn: 'root'
})
export class SkillGroupService {
  public editMode = false;
  private _serviceUrl = environment.AdminMicroService;
  private resources = servicePath.API.SkillGroup;
  public skillgroupEdit = new BehaviorSubject<SkillGroupData>(new SkillGroupData());
  public skillGroupList = new BehaviorSubject<SkillGroupData[]>([]);
  constructor(private httpClient : HttpClient ) { }

  public getCompetencyAreaList(){
    return this.httpClient.get(this._serviceUrl +servicePath.API.CompetencyArea.list) 
  }
  
  public getSkillGroup(){
    this.httpClient.get(this._serviceUrl + this.resources.list)
    .subscribe((res : SkillGroupData[]) =>{ this.skillGroupList.next(res);});
  }

  public createSkillGroup(skillgroup : SkillGroupData){
    if(this.editMode ==false)
      return this.httpClient.post(this._serviceUrl + this.resources.create , skillgroup);
    else
      return this.httpClient.post(this._serviceUrl + this.resources.update , skillgroup);
  }
}


