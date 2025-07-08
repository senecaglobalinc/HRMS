import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import * as servicePath from '../../../service-paths';
import { environment } from '../../../../environments/environment';
import { Skill } from '../../../models/associate-skills.model';

@Injectable({
  providedIn: 'root'
})
export class SkillsService {
  private _resources: any;
  private _resource: any;
  private _serverURL: string;
  constructor(private _http: HttpClient) {

    this._serverURL = environment.ServerUrl;
    this._resources = servicePath.API.associateSkills;
    this._resource = servicePath.API.SkillSearch;

  }

  public GetAssociateSkillsById(id: number) {
    var url = environment.EmployeeMicroService + this._resources.getSkillsById + id;
    return this._http.get(url);
  }
  public GetAllSkillDetails(id: number) {
    var url = this._serverURL + this._resource.getAllSkillDetails + id;
    return this._http.get(url);
  }
  public GetPracticeArea() {
    var url = this._serverURL + this._resources.getPracticeAreas;
    return this._http.get(url);
  }
  public GetProficiencyLevel() {
    var url = this._serverURL + this._resources.getProficiencyLevels;
    return this._http.get(url);
  }
  public GetSkillByCompetenctAreaAndSkillGroup(SkillGroup: number, CompetencyArea: number) {
    var url = this._serverURL + this._resources.getSkillsByCompetenctAreaAndSkillGroup + SkillGroup + "&competencyAreaID=" + CompetencyArea;
    return this._http.get(url)

  }

  public GetSkillsByCompetenctArea(CompetencyArea: number) {
    var url = this._serverURL + this._resources.getSkillsByCompetenctArea + CompetencyArea;
    return this._http.get(url);
  }
  public DeleteAssociateSkills(Id: number) {
    let _url = this._serverURL + this._resources.DeleteAssociateSkills + Id;
    return this._http.post(_url, Id)
  }
  SaveAssociateSkills(details: Skill) {
    let _url = environment.EmployeeMicroService + this._resources.SaveAssociateSkills;
    return this._http.post(_url, details)
  }
  UpdateAssociateSkills(details: Skill) {
    let _url = environment.EmployeeMicroService + this._resources.UpdateAssociateSkills;
    return this._http.post(_url, details)
  }

  public getSkills() {
    return this._http.get(environment.AdminMicroService + this._resources.getSkillsList)
  }

  public SubmitAssociateSkills(skillsArray) {
    const _url = this._serverURL + this._resources.SubmitAssociateSkills;
    return this._http.post(_url, skillsArray)

  }
}
