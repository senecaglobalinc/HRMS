import { Injectable } from '@angular/core';
import { UtilizationReportFilterData, ReportsFilterData } from '../models/reportsfilter.model';
import { ReportsData } from '../models/reportsdata.model';
import { AllocationDetails } from '../models/resourcereportbyproject.model';
import { environment } from '../../../../environments/environment';
import * as servicePath from '../../../service-paths';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IResourceReports } from '../../reports/interfaces/iresource-reports';

@Injectable({
  providedIn: 'root'
})
export class ResourceReportService implements IResourceReports{

  serviceUrl = environment.ServerUrl;
  reportUrl = environment.ReportMicroService;
  resources = servicePath.API.Reports;
  resourceReportUrl = environment.ProjMicroService;
  constructor(private httpClient: HttpClient) { }

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

}
