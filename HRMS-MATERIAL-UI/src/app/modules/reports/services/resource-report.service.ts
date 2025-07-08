import { Injectable } from '@angular/core';
import {  ReportsFilterData, UtilizationReportFilterData } from '../models/reportsfilter.model';
import { AllocationDetails } from '../models/resourcereportbyproject.model';
import { environment } from '../../../../environments/environment';
import * as servicePath from '../../../core/service-paths';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IResourceReports } from '../../reports/interfaces/iresource-reports';
import { ReportsData } from '../models/reportsdata.model';
import { NavigationEnd,RouterEvent, Router } from '@angular/router';


@Injectable({
  providedIn: 'root'
})
export class ResourceReportService implements IResourceReports{

  serviceUrl = environment.ServerUrl;
  reportUrl = environment.ReportMicroService;
  resources = servicePath.API.Reports;
  resourceReportUrl = environment.ProjMicroService;
  previousUrl: string = undefined;
  currentUrl: string = undefined;

  constructor(private httpClient: HttpClient, private router: Router) {  this.currentUrl = this.router.url;
    router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {        
        this.previousUrl = this.currentUrl;
        this.currentUrl = event.url;
      };});
      }

  public GetResourceReportByProjectId(projectId: number): Observable<AllocationDetails>{
    var url = this.resourceReportUrl + this.resources.GetResourceReportByProjectId + projectId;
    return this.httpClient.get<AllocationDetails>(url);
  }
  
  public GetUtilizationReportsByTechnology(resourceFilter: ReportsFilterData): Observable<ReportsFilterData> {
    var url = this.serviceUrl + this.resources.GetUtilizationReportsByTechnologyId;
    return this.httpClient.post<ReportsFilterData>(url, resourceFilter);
  }

  public GetUtilizationReportsByMonth(resourceFilter: ReportsFilterData): Observable<ReportsFilterData>{
    var url = this.serviceUrl + this.resources.GetUtilizationReportsByMonth;
    return this.httpClient.post<ReportsFilterData>(url, resourceFilter);
  }

  public ResourceReportByFilters(resourceFilter: UtilizationReportFilterData):Observable<ReportsData[]>{ 
    let url = this.reportUrl + this.resources.GetResourceReports;
    return this.httpClient.post<ReportsData[]>(url, resourceFilter);
  }
  public getPreviousUrl(){
    return this.previousUrl;
  }

}
