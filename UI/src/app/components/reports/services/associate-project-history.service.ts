
import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import * as servicePath from '../../../service-paths';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject } from 'rxjs';
import { TalentPoolReportData } from '../models/talentpool.model';

@Injectable({
  providedIn: 'root'
})

export class AssociateProjectHistoryService {
  serviceUrl = environment.ServerUrl;
  resources = servicePath.API.Reports;
  userProjectHistory = new BehaviorSubject<TalentPoolReportData[]>([]);
  constructor(
    private httpClient: HttpClient) { }

  public GetProjectHistoryByEmployee(employeeId: number) {
    var url = this.serviceUrl + this.resources.GetProjectHistoryByEmployee + employeeId;
    return this.httpClient.get(url);
  }

  public SetProjectHistory(projectHistory) {
    this.userProjectHistory.next(projectHistory);
  }

}


