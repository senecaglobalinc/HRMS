
import { Injectable } from '@angular/core';
import { ReportsFilterData } from '../models/reportsfilter.model';
import { FinanceReportFilterData } from '../models/reportsfilter.model';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import * as servicePath from '../../../service-paths';
import 'rxjs/Rx';
@Injectable({
  providedIn: 'root'
})
export class FinanceReportService {
    serviceUrl = environment.ServerUrl;
    reportUrl = environment.ReportMicroService;
    resources = servicePath.API.Reports;
    constructor(private httpClient: HttpClient) { }

   
   GetProjectsList(){
        let url = this.serviceUrl + this.resources.GetProjectsList;
        return this.httpClient.get(url);
    }

   GetFinanceReport(reportsFilterData: FinanceReportFilterData) {
  //  var url = this.serviceUrl + this.resources.GetFinanceReport;
   var url = this.reportUrl + this.resources.GetFinanceReport;   
    return this.httpClient.post(url, reportsFilterData);
  }
  GetRmgReportDataByMonthYear(monthyear) {
    var url = this.serviceUrl + this.resources.getRmgReportDataByMonthYear+monthyear.Month+'&year='+monthyear.Year;
    return this.httpClient.get(url);
  }
  GetFinanceReportToFreez() {
    var url = this.serviceUrl + this.resources.getFinanceReportToFreez;
    return this.httpClient.get(url);
  }
 }
