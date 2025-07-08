import { Injectable } from '@angular/core';
import * as servicePath from '../../../service-paths';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ResouceReleaseService {
  serviceUrl = environment.ServerUrl;
  resources = servicePath.API.ResourceRelease;
  
  constructor(private httpClient: HttpClient) { }
  public GetAssociates() 
  {
    var url = this.serviceUrl + this.resources.getAssociates; 
    return this.httpClient.get(url)
  }
  public GetProjects(empId: number)
  {
    var url = this.serviceUrl + this.resources.getProjectDetails + empId; 
    return this.httpClient.get(url);
  }
}
