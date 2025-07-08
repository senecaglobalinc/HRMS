import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import * as servicePath from '../../../service-paths';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { TalentpoolDataCount, TalentPoolReportData } from '../models/talentpool.model';
import { EmployeeReportData } from '../models/employee.model';


@Injectable({
  providedIn: 'root'
})

export class TalentpoolreportService {
  reportUrl = environment.ReportMicroService;
  resources = servicePath.API.Reports;
  userProjectHistory = new BehaviorSubject<TalentPoolReportData[]>([]);
  constructor(private httpClient: HttpClient) { }

  public GetTalentpoolResourceCount(): Observable<Array<TalentpoolDataCount>> {
    var url = this.reportUrl + this.resources.GetTalentpoolResourceCount;
    return this.httpClient.get<TalentpoolDataCount[]>(url);
  }

  public GetEmployeesByTalentPoolProjectId(projectId: number): Observable<Array<EmployeeReportData>> {
    var url = this.reportUrl + this.resources.GetEmployeesByTalentPoolProjectID + projectId;
    return this.httpClient.get<EmployeeReportData[]>(url);
  }

}

