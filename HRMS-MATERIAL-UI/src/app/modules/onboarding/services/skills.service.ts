import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import * as servicePath from '../../../core/service-paths';
import { environment } from '../../../../environments/environment';
import { Skill } from '../../../modules/master-layout/models/associate-skills.model';

@Injectable({
  providedIn: 'root'
})
export class SkillsService {
  private _resources: any;
  private _resource: any;
  private skill_workflow_resource: any;
  private _serverURL: string;
  constructor(private _http: HttpClient) {

    this._serverURL = environment.ServerUrl;
    this._resources = servicePath.API.associateSkills;
    this._resource = servicePath.API.SkillSearch;
    this.skill_workflow_resource = servicePath.API.EmployeeSkillWorkFlow;

  }
  public getCompetencyArea(){
    return this._http.get(environment.AdminMicroService + servicePath.API.CompetencyArea.list);
  }

  public getSkillGroupByCompetencyArea(competencyAreaId: number){
    return this._http.get(environment.AdminMicroService + servicePath.API.SkillGroup.getSkillGroupsByCompetenctArea + competencyAreaId);
  }

  public getSkillsBySkillGroup(SkillId: number){
    return this._http.get(environment.AdminMicroService + servicePath.API.Skills.getSkillsBySkillGroup + SkillId);
  }
  
  public GetSkillsBySearchString(skillSearchString: string){
    return this._http.get(environment.AdminMicroService + servicePath.API.Skills.GetSkillsBySearchString + skillSearchString);
  }

  public GetAssociateSkillsById(id: number ,roleName:string) {
    var url = environment.EmployeeMicroService + this._resources.getSkillsById + id +"/"+roleName;
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
    var url = environment.EmployeeMicroService + this._resources.getProficiencyLevels;
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

  public submitForApproval(Id) {
    var url = environment.EmployeeMicroService + this.skill_workflow_resource.Create + Id;
    return this._http.post(url, Id, {responseType: 'text'});
  }
  
  public getSkillSubmittedEmployees(Id) {
    var url = environment.EmployeeMicroService + this.skill_workflow_resource.getSkillSubmittedEmps + Id;
    return this._http.get(url);
  }

  public getNewSkillsByEmployee(Id) {
    var url = environment.EmployeeMicroService + this.skill_workflow_resource.getAllSubmittedSkillsByEmpid + Id;
    return this._http.get(url);
  }

  public UpdateEmpSkillProficienyByRM(SkillObj) {
    var url = environment.EmployeeMicroService + this.skill_workflow_resource.UpdateEmpSkillProficienyByRM;
    return this._http.post(url, SkillObj, {responseType: 'text'});
  }
  

  public updateEmpSkillDetailsByRM(Id) {
    var url = environment.EmployeeMicroService + this.skill_workflow_resource.updateEmpSkillDetailsByRM + Id;
    return this._http.post(url, Id, {responseType: 'text'});
  }

  public ApproveSkillRM(Id) {
    var url = environment.EmployeeMicroService + this.skill_workflow_resource.SkillStatusApprovedByRM + Id;
    return this._http.post(url, Id, {responseType: 'text'});
  }

  public GetEmployeeSkillHistory(EmpId, SkillId) {
    var url = environment.EmployeeMicroService + this.skill_workflow_resource.GetEmployeeSkillHistory + EmpId + '?ID=' + SkillId;
    return this._http.get(url);
  }

  public deleteSkill(id : number){
    var url = environment.EmployeeMicroService + this._resources.deleteSkill + id;
    return this._http.delete(url);
  }
  
}
