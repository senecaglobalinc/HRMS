import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import * as servicePath from '../../../core/service-paths';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import {UtilizationReportResponse} from '../models/utilization-report.model';

@Injectable({
    providedIn: 'root'
  })

  export class UtilizationReportService{
    reportUrl = environment.EmployeeMicroService;
    
    resources = servicePath.API.Reports;

    constructor(private httpClient: HttpClient) { }
  
    public GetUtilizationReport() {
      var url = this.reportUrl + this.resources.GetUtilizationReport;    
      return this.httpClient.get<UtilizationReportResponse[]>(url);
    }
  
  }
  