import { ProjectResponse } from './../models/projects-report.model';
import { ProjectTypeData } from './../../admin/models/projecttype.model';
import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import * as servicePath from '../../../core/service-paths';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import {ServiceTypeReport, ServiceTypeReportEmp, ServiceTypeDropDown} from '../models/service-typereport.model';

@Injectable({
    providedIn: 'root'
  })

  export class ServiceTypeReportService {
    reportUrl = environment.ReportMicroService;
    adminUrl = environment.AdminMicroService;
    resources = servicePath.API.Reports;
    admin = servicePath.API.ServiceType;
    project = servicePath.API.ProjectType;
    constructor(private httpClient: HttpClient) { }
  
    public GetServiceTypeReportCount(filter: string) {
      var url = this.reportUrl + this.resources.GetServiceTypeReportCount;    
      return this.httpClient.get<ServiceTypeReport[]>(url+filter);
    }
  
    public GetServiceTypeReportEmployee(serviceTypeId: number){
      var url = this.reportUrl + this.resources.GetServiceTypeReportEmployee + serviceTypeId;
      return this.httpClient.get<ServiceTypeReportEmp[]>(url);
    }
    public GetServiceTypeForDropdown(){
        var url = this.adminUrl + this.admin.GetServiceTypeForDropdown;
        return this.httpClient.get<ServiceTypeDropDown[]>(url);
      }
    public GetProjectTypeDropdown(){
      var url = this.adminUrl + this.project.list;
      return this.httpClient.get<ProjectTypeData[]>(url);
    }
    
    public GetServiceTypeReportProject(serviceTypeId: number){
      var url = this.reportUrl + this.resources.GetServiceTypeReportProject + serviceTypeId;
      return this.httpClient.get<ProjectResponse[]>(url);
    }
  
  }
  