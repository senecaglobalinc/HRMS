import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import * as servicePath from '../../../core/service-paths';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { GenericType } from '../../../modules/master-layout/models/dropdowntype.model';
import { AssociateExitReportFilter, AssociateExitReportRequestForPm, AssociateExitReportResponse, ChartData } from '../models/associate-exit-report.model';

@Injectable({
    providedIn: 'root'
  })

  export class AssociateExitReportService{
    reportUrl = environment.EmployeeMicroService;
    
    resources = servicePath.API.Reports;

    constructor(private httpClient: HttpClient) { }
  
    public GetAssociateExitReport() {
      var url = this.reportUrl + this.resources.GetAssociateExitReport;    
      return this.httpClient.get<AssociateExitReportResponse[]>(url);
    }

    public GetAssociateExitGridReport(filter: AssociateExitReportFilter) {
      var url = this.reportUrl + this.resources.GetAssociateExitReport;    
      return this.httpClient.post<AssociateExitReportResponse[]>(url, filter);
    }

    public GetAssociateExitReportTypes() {
      var url = this.reportUrl + this.resources.GetAssociateExitReportTypes;    
      return this.httpClient.get<GenericType[]>(url);
    }

    public GetAssociateExitChartReport(filter: AssociateExitReportFilter) {
      var url = this.reportUrl + this.resources.GetAssociateExitChartReport;    
      return this.httpClient.post<ChartData[]>(url,filter);
    } 
    
    public GetAssociateExitReportForPM(exitRequestObj:AssociateExitReportRequestForPm){
      var url = this.reportUrl + this.resources.GetAssociateExitReport;    
      return this.httpClient.post<AssociateExitReportResponse[]>(url,exitRequestObj);
    }
  }
  