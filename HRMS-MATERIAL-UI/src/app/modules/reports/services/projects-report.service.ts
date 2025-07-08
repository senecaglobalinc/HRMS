import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import * as servicePath from '../../../core/service-paths';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import {ProjectResponse} from '../models/projects-report.model';

@Injectable({
    providedIn: 'root'
  })

  export class ProjectsService {
    reportUrl = environment.ProjMicroService;
    
    resources = servicePath.API.Reports;

    constructor(private httpClient: HttpClient) { }
  
    public GetAllProjects() {
      var url = this.reportUrl + this.resources.GetAllProjectsForReport;    
      return this.httpClient.get<ProjectResponse[]>(url);
    }
  
  }
  