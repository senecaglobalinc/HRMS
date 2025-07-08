import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import * as servicePath from '../../../core/service-paths';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ReportsData } from '../models/reportsdata.model';

@Injectable({
  providedIn: 'root'
})
export class ResourceReportNoncriticalService {
  reportUrl = environment.ReportMicroService;
  resources = servicePath.API.Reports;

  constructor(private httpClient: HttpClient) { }
  public GetNonCriticalResourceReport(): Observable<ReportsData[]> {
    var url = this.reportUrl + this.resources.GetNonCriticalResourceReport;    
    return this.httpClient.get<ReportsData[]>(url);
  }
  public GetAssociatesForFutureAllocation(): Observable<ReportsData[]> {
    var url = this.reportUrl + this.resources.GetAssociatesForFutureAllocation;    
    return this.httpClient.get<ReportsData[]>(url);
  }
  public GetNonCriticalBillingReport(): Observable<ReportsData[]> {
    var url = this.reportUrl + this.resources.GetNonCriticalResourceBillingReport;    
    return this.httpClient.get<ReportsData[]>(url);
  }
}
