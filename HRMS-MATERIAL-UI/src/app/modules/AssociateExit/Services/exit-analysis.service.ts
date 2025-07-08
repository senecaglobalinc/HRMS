import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import * as servicePath from '../../../core/service-paths';
import { environment } from '../../../../environments/environment';
import { NavigationEnd,RouterEvent, Router } from '@angular/router';


@Injectable({
  providedIn: 'root'
})
export class ExitAnalysisService {
  serviceUrl = environment.ServerUrl;
associateMicroService =environment.EmployeeMicroService
  _resource = servicePath.API.Activity;
  _resources = servicePath.API.AssociateExitAnalysis;
  resource = servicePath.API.AssociateExit;
  _associateTransitionPlan = servicePath.API.TransitionPlan
  projects=[]
  previousUrl: string = undefined;
  currentUrl: string = undefined;

  constructor(private httpClient: HttpClient, private router: Router) {  this.currentUrl = this.router.url;
    router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {        
        this.previousUrl = this.currentUrl;
        this.currentUrl = event.url;
      };
    }); }

  GetAssociateExitAnalysis() {
    let _url = this.associateMicroService + this._resources.GetAssociateExitAnalysis;
    return this.httpClient.get(_url);
  }

  GetAssociateExitReport() {
    let _url = this.associateMicroService + this._resources.GetExitEmployeeReport;
    return this.httpClient.get(_url);
  }

  getAssociateExitAnalysis(fromDate: string,toDate : string) {
    let _url = this.associateMicroService + this._resources.GetAssociateExitAnalysis + '?fromDate=' + fromDate + '&toDate='+ toDate;
    return this.httpClient.get(_url);
  }


  public CreateExitAnalysis(causeForm){
    let _url = this.associateMicroService + this._resources.CreateExitAnalysis;
    return this.httpClient.post(_url,causeForm);
  }

  public GetAssociateTransitionPlan(EmpId){
    let _url = this.associateMicroService + this._associateTransitionPlan.GetForAssociate+'?employeeId='+EmpId
    return this.httpClient.get(_url);
  }

  public SetAssociateProjects(projects){
    this.projects = projects
  }
  public GetAssociateProjects(){
    return this.projects
  }
  public getPreviousUrl(){
    return this.previousUrl;
  }
  public CreateAssociateAbscond(SubmitData){
    let _url = this.associateMicroService + this.resource.Create;
    return this.httpClient.post(_url,SubmitData);
  }
}
