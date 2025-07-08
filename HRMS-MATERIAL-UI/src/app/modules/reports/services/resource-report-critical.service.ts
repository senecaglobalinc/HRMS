import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import * as servicePath from '../../../core/service-paths';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { ReportsData } from '../models/reportsdata.model';

@Injectable({
  providedIn: 'root'
})
export class ResourceReportCriticalService {
  reportUrl = environment.ReportMicroService;
  resources = servicePath.API.Reports;

  constructor(private httpClient: HttpClient) { }
  public GetCriticalResourceReport(): Observable<ReportsData[]> {
    var url = this.reportUrl + this.resources.GetCriticalResourceReport;    
    return this.httpClient.get<ReportsData[]>(url);
  }
}
